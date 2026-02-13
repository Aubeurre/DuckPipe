using DuckPipe.Core.Builders;
using DuckPipe.Core.Config;
using DuckPipe.Core.Manager;
using DuckPipe.Core.Manipulators;
using DuckPipe.Core.Services;
using DuckPipe.Core.Services.Softwares;
using DuckPipe.Core.Utils;
using DuckPipe.Forms;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace DuckPipe.Core.Manipulator
{
    public static class NodeManip
    {
        // faudra revoir car tout ca gere pas un node mais un workfile
        // un node est un ensemble de workfiles
        public class NodeContext
        {
            public string FileName { get; set; }
            public string File { get; set; }
            public string Extension { get; set; }
            public string WorkFolder { get; set; }
            public string NodeRoot { get; set; }
            public string RelativeWorkPath { get; set; }
            public string NodeType { get; set; }
            public string Department { get; set; }
            public string RootPath { get; set; }
            public string ProdName { get; set; }
        }

        public static NodeContext ExtractNodeContext(string nodePath)
        {
            // Normalisation slashes
            nodePath = nodePath?.Replace('/', '\\') ?? "";

            var ctx = new NodeContext
            {
                FileName = Path.GetFileNameWithoutExtension(nodePath),
                File = Path.GetFileName(nodePath),
                Extension = Path.GetExtension(nodePath),
                WorkFolder = Path.GetDirectoryName(nodePath),
                RootPath = ProductionService.GetProductionRootPath()
            };

            string[] nodeParts = nodePath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);

            ctx.NodeRoot = nodeParts[0];
            ctx.RelativeWorkPath = (nodeParts.Length > 1) ? nodeParts[1] : "";

            string relativeToRoot = nodePath.Replace(ctx.RootPath, "").TrimStart('\\');
            string[] segments = relativeToRoot.Split('\\', StringSplitOptions.RemoveEmptyEntries);

            ctx.ProdName = (segments.Length >= 1) ? segments[0] : "Unknown";
            ctx.NodeType = (segments.Length >= 3) ? segments[2] : "Unknown";
            ctx.Department = (segments.Length >= 6) ? segments[5] : "Unknown";

            return ctx;
        }

        public static JsonDocument LoadNodeJson(string nodeJsonPath)
        {
            if (!File.Exists(nodeJsonPath))
                throw new FileNotFoundException($"Fichier non trouve : {nodeJsonPath}");

            string jsonText = File.ReadAllText(nodeJsonPath);
            return JsonDocument.Parse(jsonText);
        }

        public static string SetEnvVariables(string path)
        {
            string rootPath = ProductionService.GetProductionRootPath();
            if (!Directory.Exists(rootPath))
            {
                LogService.EchoErrorLog($"Le chemin defini dans DUCKPIPE_ROOT est invalide :\n{rootPath}");
                return null;
            }
            return path.Replace(rootPath ?? "", "${DUCKPIPE_ROOT}\\");
        }

        public static string ReplaceEnvVariables(string path)
        {
            string rootPath = ProductionService.GetProductionRootPath();

            if (!Directory.Exists(rootPath))
            {
                LogService.EchoErrorLog($"Le chemin defini dans DUCKPIPE_ROOT est invalide :\n{rootPath}");
                return null;
            }

            return path.Replace("${DUCKPIPE_ROOT}", rootPath ?? "");

        }

        public static int GetLastWorkVersion(string nodePath)
        {
            if (!File.Exists(nodePath))
            {
                LogService.EchoLog("Fichier introuvable.");
                return 0;
            }

            string fileName = Path.GetFileNameWithoutExtension(nodePath);
            string extension = Path.GetExtension(nodePath);

            string workFolder = Path.GetDirectoryName(nodePath);
            string incrementalsDir = Path.Combine(workFolder, "incrementals");

            string[] existingFiles = Directory.GetFiles(incrementalsDir);

            int maxVersion = 0;
            string pattern = Regex.Escape(fileName) + @"_v(\d{3})" + Regex.Escape(extension) + "$";

            foreach (string file in existingFiles)
            {
                string fileOnly = Path.GetFileName(file);
                Match match = Regex.Match(fileOnly, pattern);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int version))
                {
                    if (version > maxVersion)
                        maxVersion = version;
                }
            }
            return maxVersion;
        }

        public static void MarkDownstreamDepartmentsOutdated(string nodePath, string deptPublished)
        {
            var ctx = ExtractNodeContext(nodePath);

            string configPath = Path.Combine(ctx.RootPath, ctx.ProdName, "Dev", "DangerZone", "config.json");
            string configJson = File.ReadAllText(configPath);
            using var configDoc = JsonDocument.Parse(configJson);

            if (!configDoc.RootElement.TryGetProperty("departments", out var allDepartments))
                return;

            if (!allDepartments.TryGetProperty(deptPublished, out var deptInfo))
                return;

            if (!deptInfo.TryGetProperty("downstream", out var downstreamList))
                return;

            string jsonPath = Path.Combine(ctx.NodeRoot, "node.json");
            if (!File.Exists(jsonPath))
                return;

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { WriteIndented = true };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var updatedNodeData = new Dictionary<string, object>();

            var workfileDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(
                root.GetProperty("workfile").GetRawText()
            );

            bool updated = false;
            var keys = workfileDict.Keys.ToList();

            foreach (var key in keys)
            {
                var data = workfileDict[key];

                if (!data.ContainsKey("department"))
                    continue;

                string departmentName = data["department"]?.ToString();
                string fileName = Path.GetFileName(key);
                foreach (var downstream in downstreamList.EnumerateArray())
                {
                    if (downstream.GetString().ToLower() == departmentName.ToLower() && fileName != ctx.FileName)
                    {
                        data["status"] = "outDated";
                        updated = true;
                    }
                }
            }

            updatedNodeData["workfile"] = workfileDict;

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name == "workfile") continue;
                updatedNodeData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
            }

            if (updated)
            {
                string updatedJson = JsonSerializer.Serialize(updatedNodeData, options);
                File.WriteAllText(jsonPath, updatedJson);
            }
        }

        public static List<AllPbPath> GetAllImages(string path)
        {
            var result = new List<AllPbPath>();

            string folder = Path.Combine(path, "dlv");

            if (!Directory.Exists(folder))
                return result;

            var validExtensions = new[] { ".png", ".jpg" };

            foreach (var file in Directory.GetFiles(folder))
            {
                if (validExtensions.Contains(Path.GetExtension(file).ToLower()))
                {
                    result.Add(new AllPbPath
                    {
                        Name = Path.GetFileName(file),
                        FullPath = file,
                        Modified = File.GetLastWriteTime(file)
                    });
                }
            }

            return result;
        }

        #region NOTES COMMITS
        public static void AddNote(string nodePath, AssetManagerForm form)
        {
            var ctx = ExtractNodeContext(nodePath);

            int versionName = GetLastWorkVersion(nodePath);
            var msgPopup = new MessageBoxPopup();
            if (msgPopup.ShowDialog() == DialogResult.OK)
            {
                AddPublishLog(ctx.WorkFolder, ctx.FileName, versionName, msgPopup.CommitMessage);
            }
            form.RefreshTab(ctx.NodeRoot);
        }

        public static void AddPublishLog(string workPath, string fileName, int version, string message)
        {
            string changelogPath = Path.Combine(workPath, $"{fileName}_changelog.json");

            if (!File.Exists(changelogPath))
            {
                File.WriteAllText(changelogPath, "[]");
            }

            List<CommitEntry> entries = new();

            string json = File.ReadAllText(changelogPath).Trim();
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    entries = JsonSerializer.Deserialize<List<CommitEntry>>(json) ?? new();
                }
                catch (Exception ex)
                {
                    LogService.EchoErrorLog($"Erreur lors de la lecture du changelog :\n{ex.Message}");
                    entries = new();
                }
            }

            entries.Add(new CommitEntry
            {
                Version = version.ToString("D3"),
                Timestamp = DateTime.Now,
                User = ProductionService.GetUserName(),
                Message = message
            });

            string updatedJson = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(changelogPath, updatedJson);
        }

        private static void UpdateNodeMetadata(string nodePath, int version, string deptName)
        {
            var ctx = ExtractNodeContext(nodePath);
            string jsonPath = Path.Combine(ctx.NodeRoot, "node.json");

            if (!File.Exists(jsonPath))
                return;

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { WriteIndented = true };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var updatedNodeData = new Dictionary<string, object>();

            var workfileDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(
                root.GetProperty("workfile").GetRawText()
            );

            bool updated = false;

            var keys = workfileDict.Keys.ToList();
            foreach (var key in keys)
            {
                if (Path.GetFileName(key) == ctx.File)
                {
                    var data = workfileDict[key];
                    data["version"] = $"v{version:D3}";
                    data["lastModified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data["status"] = "upToDate";
                    updated = true;
                }
            }

            updatedNodeData["workfile"] = workfileDict;

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name == "workfile") continue;

                updatedNodeData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
            }

            if (updated)
            {
                string updatedJson = JsonSerializer.Serialize(updatedNodeData, options);
                File.WriteAllText(jsonPath, updatedJson);
            }
        }

        public static List<CommitEntry> GetNodeCommits(string nodePath)
        {
            var ctx = ExtractNodeContext(nodePath);
            string changelogPath = Path.Combine(ctx.WorkFolder, $"{ctx.FileName}_changelog.json");
            var commits = new List<CommitEntry>();

            if (!File.Exists(changelogPath))
                return commits;

            string json = File.ReadAllText(changelogPath);
            using var doc = JsonDocument.Parse(json);

            foreach (var entry in doc.RootElement.EnumerateArray())
            {
                string version = entry.TryGetProperty("Version", out var v) ? v.GetString() : "???";
                DateTime timestamp = entry.TryGetProperty("Timestamp", out var t) && t.TryGetDateTime(out var dt)
                    ? dt
                    : DateTime.MinValue; string user = entry.TryGetProperty("User", out var u) ? u.GetString() ?? "(inconnu)" : "(inconnu)";
                string message = entry.TryGetProperty("Message", out var m) ? m.GetString() ?? "" : "";
                message = message.Replace("\u000B", Environment.NewLine);

                commits.Add(new CommitEntry
                {
                    Version = version,
                    Timestamp = timestamp,
                    User = user,
                    Message = message
                });
            }

            return commits;
        }
        #endregion
        public static List<string> GetAllRefs(string nodePath)
        {
            var ctx = ExtractNodeContext(nodePath);

            string jsonPath = Path.Combine(ctx.NodeRoot, "node.json");
            var jsonNode = JsonHelper.ParseJson(jsonPath);

            List<string> allRefs = new List<string>();

            if (jsonNode?["nodeInfos"] is JsonObject nodeInfos)
            {
                if (nodeInfos["refAssets"] is JsonArray refs)
                {
                    foreach (var r in refs)
                    {
                        string refPath = r?.GetValue<string>() ?? "";
                        if (!string.IsNullOrEmpty(refPath))
                        {
                            // Remplacer les variables d'environnement si besoin
                            refPath = ReplaceEnvVariables(refPath);
                            allRefs.Add(refPath);
                        }
                    }
                }
            }

            return allRefs;
        }


        public static void AddRef(string nodePath, AssetManagerForm form)
        {
            var ctx = ExtractNodeContext(nodePath);
            string prodPath = Path.Combine(ctx.RootPath, ctx.ProdName);

            // Récupère tous les nodes disponibles dans la prod
            var nodesDict = GetAllNodesInProduction(prodPath);
            var allNodes = nodesDict
                .SelectMany(typeEntry => typeEntry.Value.Select(nodeEntry => $"{typeEntry.Key}/{nodeEntry.Key}"))
                .ToList();

            using (var popup = new AddreferencesPopup(allNodes, nodePath))
            {
                if (popup.ShowDialog() != DialogResult.OK)
                    return;

                string nodeName = popup.NodeName;       // ex "Props/Chair"
                string[] parts = nodeName.Split('/');
                string assetType = parts[0];
                string assetName = parts[1];

                string PublishPath = Path.Combine(prodPath, "Assets", nodeName, "dlv");
                string refAsset = SetEnvVariables(PublishPath).Replace("\\", "/");

                LogService.EchoSuccessLog($"Reference ajoutee avec succès.\n{refAsset}");

                // --- JSON ---
                string jsonPath = Path.Combine(ctx.NodeRoot, "node.json");
                var json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions { WriteIndented = true };

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var updatedNodeData = new Dictionary<string, object>();

                // Copier workfile tel quel
                updatedNodeData["workfile"] = JsonSerializer.Deserialize<object>(
                    root.GetProperty("workfile").GetRawText()
                );

                // --- nodeInfos ---
                Dictionary<string, object> nodeInfos;

                if (root.TryGetProperty("nodeInfos", out var nodeInfosElement))
                {
                    nodeInfos = JsonSerializer.Deserialize<Dictionary<string, object>>(nodeInfosElement.GetRawText())!;
                }
                else
                {
                    nodeInfos = new Dictionary<string, object>();
                }

                // Ajouter ou créer refAssets
                List<string> refList;
                if (nodeInfos.ContainsKey("refAssets"))
                {
                    refList = JsonSerializer.Deserialize<List<string>>(nodeInfos["refAssets"].ToString()) ?? new List<string>();
                }
                else
                {
                    refList = new List<string>();
                }

                if (!refList.Contains(refAsset))
                    refList.Add(refAsset);

                nodeInfos["refAssets"] = refList;
                updatedNodeData["nodeInfos"] = nodeInfos;

                // Copier les autres propriétés
                foreach (var prop in root.EnumerateObject())
                {
                    if (prop.Name is "workfile" or "nodeInfos")
                        continue;

                    updatedNodeData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
                }

                // Écriture finale
                string updatedJson = JsonSerializer.Serialize(updatedNodeData, options);
                File.WriteAllText(jsonPath, updatedJson);
            }
        }

        public static void RemoveRef(string itemText, string nodePath)
        {
            // Extrait le chemin complet depuis la liste
            string refPath;
            int parenIndex = itemText.IndexOf('(');
            if (parenIndex >= 0)
                refPath = itemText.Substring(parenIndex + 1).TrimEnd(')');
            else
                refPath = itemText; // fallback si pas de parenthèses

            // Résoudre variables d'environnement
            refPath = SetEnvVariables(refPath).Replace("\\\\", "\\");
            refPath = SetEnvVariables(refPath).Replace("\\/", "/");

            var ctx = ExtractNodeContext(nodePath);
            string jsonPath = Path.Combine(ctx.NodeRoot, "node.json");
            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { WriteIndented = true };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var updatedNodeData = new Dictionary<string, object>();

            // Copier workfile tel quel
            updatedNodeData["workfile"] = JsonSerializer.Deserialize<object>(
                root.GetProperty("workfile").GetRawText()
            );

            // --- nodeInfos ---
            Dictionary<string, object> nodeInfos;
            if (root.TryGetProperty("nodeInfos", out var nodeInfosElement))
            {
                nodeInfos = JsonSerializer.Deserialize<Dictionary<string, object>>(nodeInfosElement.GetRawText())!;
            }
            else
            {
                nodeInfos = new Dictionary<string, object>();
            }

            // Supprimer refAssets si présent
            if (nodeInfos.ContainsKey("refAssets"))
            {
                var refList = JsonSerializer.Deserialize<List<string>>(nodeInfos["refAssets"].ToString()) ?? new List<string>();
                if (refList.Contains(refPath))
                {
                    refList.Remove(refPath);
                    LogService.EchoSuccessLog($"Référence supprimée avec succès.\n{refPath}");
                }
                nodeInfos["refAssets"] = refList;
            }

            updatedNodeData["nodeInfos"] = nodeInfos;

            // Copier les autres propriétés
            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name is "workfile" or "nodeInfos")
                    continue;

                updatedNodeData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
            }

            // Écriture finale
            string updatedJson = JsonSerializer.Serialize(updatedNodeData, options);
            File.WriteAllText(jsonPath, updatedJson);
        }


        #region TEMP FILE
        public static string GetTempPath(string nodePath)
        {
            string prodPath = ProductionService.GetProductionRootPath();
            string tempNodelPath = nodePath.Replace(prodPath, $"{UserConfig.GetLocalBasePath()}\\");

            return tempNodelPath;
        }

        public static void EnsureDependences(string nodePath, AssetManagerForm form)
        {
            // on check si les fichiers sont a jour
            var ctx = ExtractNodeContext(nodePath);

            // on copy les fichiers de dlv en local pour les refs
            string dlvPath = Path.Combine(ctx.NodeRoot, "dlv");
            string tempDlvPath = GetTempPath(dlvPath);
            List<string> changedFiles = new List<string>();
            changedFiles = ProdFilesManip.SyncFolder(dlvPath, changedFiles, toLocal: true);
            LogService.EchoSuccessLog($"Fichiers DLV copie en local : {changedFiles.ToString()}");

            // on copy les fichiers de la prod en local
            ProdFilesManip.EnsureLocalProductionFiles(ctx.ProdName);
            LogService.EchoInfoLog($"EnsureLocalProductionFiles OK");

            // on copy les dependences des refs du node en local
            changedFiles = new List<string>();

            foreach (var refPath in GetAllRefs(nodePath))
            {
                if (Directory.Exists(refPath))
                    changedFiles = ProdFilesManip.SyncFolder(refPath, changedFiles, toLocal: true);

                // Si Environment sync sous refs
                if (refPath.Contains("Environments"))
                {
                    string envPath = Path.GetDirectoryName(refPath);
                    foreach (var underPath in GetAllRefs(envPath))
                    {
                        if (Directory.Exists(underPath))
                            changedFiles = ProdFilesManip.SyncFolder(underPath, changedFiles, toLocal: true);
                    }
                }
            }

            ProdFilesManip.ReturnChanges(changedFiles);

        }

        public static void GrabbNode(string nodePath, AssetManagerForm form)
        {
            var ctx = ExtractNodeContext(nodePath);
            // grabb
            string tempNodelPath = GetTempPath(nodePath);
            string tempDirPath = Path.GetDirectoryName(tempNodelPath)!;
            string nodejsonPath = Path.Combine(ctx.NodeRoot, "node.json");
            string tempNodejsonPath = GetTempPath(nodejsonPath);

            Directory.CreateDirectory(tempDirPath);
            File.Copy(nodePath, tempNodelPath, true);
            File.Copy(nodejsonPath, tempNodejsonPath, true);
            LogService.EchoInfoLog($"Fichier copié en local :\n{tempNodelPath}");

            EnsureDependences(nodePath, form);
            LogService.EchoSuccessLog($"Node grabbed : {ctx.File}");

        }

        public static void UngrabbNode(string nodePath)
        {
            string tempNodelPath = GetTempPath(nodePath);
            string tempDirPath = Path.GetDirectoryName(tempNodelPath)!;

            // if (Directory.Exists(tempDirPath))
            //    Directory.Delete(tempDirPath, true);
            LogService.EchoSuccessLog($"Node ungrabbed :\n{tempNodelPath}");
        }
        #endregion


        #region EXECUTE TASK ON ASSET
        public static void LaunchNode(string filePath, AssetManagerForm form)
        {
            string LocalFile = GetTempPath(filePath);
            string fileToOpen = File.Exists(LocalFile) ? LocalFile : filePath;

            // Chemin temporaire pour le .bat
            string tempBat = Path.Combine(Path.GetTempPath(), $"DuckPipe_Launch_{Guid.NewGuid()}.bat");

            string batContent = $@"
@echo off
set PROD_ROOT={form.GetSelectedProductionPath()}
set STUDIO_TOOLS={Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(ProductionService.GetProductionRootPath())), "SHARED_TOOLS")}
start """" ""{fileToOpen}""
";

            File.WriteAllText(tempBat, batContent);

            // Lancer le .bat
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = tempBat,
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process proc = Process.Start(psi);

            // On peut supprimer le .bat
            Task.Run(() =>
            {
                proc.WaitForExit();
                try
                {
                    File.Delete(tempBat);
                }
                catch {}
            });
        }

        public static void InitNode(string nodePath, AssetManagerForm form)
        {
            if (LockNodeFileManager.IsLockedByUser(nodePath))
            {
                var ctx = ExtractNodeContext(nodePath);
                string LocalFile = GetTempPath(nodePath);

                // on regarde si il existe un template
                string TmplPath = string.Empty;

                if (ctx.Department == "Anim" || ctx.Department == "Light" || ctx.Department == "CfxShot" || ctx.Department == "Layout" && nodePath.Contains("Shots"))
                {
                    TmplPath = Path.Combine(ctx.RootPath, ctx.ProdName, "Shots", "Template");
                }
                else if (ctx.Department == "Rig" || ctx.Department == "Surf" || ctx.Department == "Cfx" || ctx.Department == "Groom" || ctx.Department == "Facial" || ctx.Department == "Model" && nodePath.Contains("Assets"))
                {
                    TmplPath = Path.Combine(ctx.RootPath, ctx.ProdName, "Assets", "Template");
                }
                else
                {
                    TmplPath = Path.Combine(ctx.RootPath, ctx.ProdName, "Assets", "Template");
                }

                if (!string.IsNullOrEmpty(TmplPath))
                {
                    string templateName = $"{ctx.NodeType}_{ctx.Department}_template{ctx.Extension}";
                    string templateFile = Path.Combine(TmplPath, $"{templateName}");

                    if (!File.Exists(templateFile))
                    {
                        // si pas de template on cree un fichier de base
                        if (ctx.Extension == ".ma")
                        {
                            MayaService.CreateBasicMaFile(templateFile, templateName);
                            LogService.EchoInfoLog($"No template found for {templateName}.\nA basic scene has been created.");
                        }
                        else if (ctx.Extension == ".blend")
                        {
                            BlenderService.CreateBasicBlendFile(templateFile);
                            LogService.EchoInfoLog($"No template found for {templateName}.\nA basic scene has been created.");
                        }
                        else if (ctx.Extension == ".hip" || ctx.Extension == ".hipnc")
                        {
                            HoudiniService.CreateBasicHoudiniFile(templateFile);
                            LogService.EchoInfoLog($"No template found for {templateName}.\nA basic Houdini scene has been created.");
                        }
                    }
                }

                //  verix du stub avant exec ---
                string stubPath = Path.ChangeExtension(LocalFile, $".stub{ctx.Extension}");
                if (File.Exists(stubPath))
                {
                    LogService.EchoLog($"Stub detected. Rebuilding base scene for {ctx.Department}...");

                    if (ctx.Extension == ".ma")
                        MayaService.CreateBasicMaFile(LocalFile, $"{ctx.NodeType}_{ctx.Department}");
                    else if (ctx.Extension == ".blend")
                        BlenderService.CreateBasicBlendFile(LocalFile);
                    else if (ctx.Extension == ".hip" || ctx.Extension == ".hipnc")
                        HoudiniService.CreateBasicHoudiniFile(LocalFile);
                }

                // lancer le py d'ouverture du node selon le department
                string pyPath = Path.Combine(ctx.RootPath, ctx.ProdName, "Dev", "Pythons", $"{ctx.NodeType}_{ctx.Department}_exec.py");
                if (ctx.Extension == ".ma")
                {
                    MayaService.CreateBasicMaFile(LocalFile, $"{ctx.FileName}_{ctx.Department}");
                    MayaService.ExecuteMayaBatchScript(LocalFile, pyPath, nodePath);
                }
                else if (ctx.Extension == ".blend")
                {
                    BlenderService.CreateBasicBlendFile(LocalFile);
                    BlenderService.ExecuteBlenderBatchScript(LocalFile, pyPath, nodePath);
                }
                else if (ctx.Extension == ".hip" || ctx.Extension == ".hipnc")
                {
                    HoudiniService.CreateBasicHoudiniFile(LocalFile);
                    HoudiniService.ExecuteHoudiniBatchScript(LocalFile, pyPath, nodePath);
                }

            }
            else
            {
                LogService.EchoLog($"Please Grab Node First");
            }
        }

        public static void PublishNode(string nodePath, AssetManagerForm form)
        {
            VersionNode(nodePath, form);

            var ctx = ExtractNodeContext(nodePath);

            string publishFolder = Path.Combine(ctx.NodeRoot, "dlv");
            Directory.CreateDirectory(publishFolder);


            string LocalFile = GetTempPath(nodePath);
            string localPublishFolder = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(LocalFile)).FullName).FullName, "dlv");
            Directory.CreateDirectory(localPublishFolder);

            string publishedFileName = $"{ctx.FileName}_OK{ctx.Extension}";
            string publishedFilePath = Path.Combine(publishFolder, publishedFileName);
            string localPublishedFilePath = Path.Combine(localPublishFolder, publishedFileName);


            ProdFilesManip.SafeCopyLargeFile(LocalFile, localPublishedFilePath);

            // on lance la scene de publish pour lancer des scripts dedans
            string pyPath = Path.Combine(ctx.RootPath, ctx.ProdName, "Dev", "Pythons", $"{ctx.NodeType}_{ctx.Department}_publish.py");
            if (ctx.Extension == ".ma")
            {
                MayaService.ExecuteMayaBatchScript(localPublishedFilePath, pyPath, nodePath);
            }
            else if (ctx.Extension == ".blend")
            {
                BlenderService.ExecuteBlenderBatchScript(localPublishedFilePath, pyPath, nodePath);
            }
            else if (ctx.Extension == ".hipnc" || ctx.Extension == ".hip")
            {
                HoudiniService.ExecuteHoudiniBatchScript(localPublishedFilePath, pyPath, nodePath);
            }

            // File.Copy(localPublishedFilePath, publishedFilePath, overwrite: true); // faut passer ca dans le pyton plutot

            AddNote(nodePath, form);
            MarkDownstreamDepartmentsOutdated(nodePath, ctx.Department);
            form.RefreshTab(ctx.NodeRoot);
            LogService.EchoSuccessLog($"Node publie : {publishedFileName}");

        }

        public static void VersionNode(string nodePath, AssetManagerForm form)
        {
            if (LockNodeFileManager.IsLockedByUser(nodePath))
            {

                string LocalFile = GetTempPath(nodePath);
                if (!File.Exists(nodePath))
                {
                    LogService.EchoLog("Fichier Local introuvable.");
                    return;
                }

                var ctx = ExtractNodeContext(nodePath);
                string incrementalsDir = Path.Combine(ctx.WorkFolder, "incrementals");

                Directory.CreateDirectory(incrementalsDir);

                int newVersion = GetLastWorkVersion(nodePath) + 1;
                string versionedFileName = $"{ctx.FileName}_v{newVersion:D3}{ctx.Extension}";
                string destinationPath = Path.Combine(incrementalsDir, versionedFileName);
                ProdFilesManip.SafeCopyLargeFile(LocalFile, nodePath);
                ProdFilesManip.SafeCopyNasToNas(nodePath, destinationPath);
                LogService.EchoSuccessLog($"Version enregistree : {versionedFileName}");

                UpdateNodeMetadata(nodePath, newVersion, ctx.Department);

                form.RefreshTab(ctx.NodeRoot);
            }
            else
            {
                LogService.EchoLog($"Please Grab Node First");
            }
        }
        #endregion


        #region EDIT ASSET METADATA FROM UI
        public static void SaveTaskDataFromUI(string jsonPath, FlowLayoutPanel flp)
        {
            var taskData = new Dictionary<string, (string status, string user, string startDate, string dueDate)>();

            // Dictionnaires temporaires pour stocker les valeurs par type
            var users = new Dictionary<string, string>();
            var startDates = new Dictionary<string, string>();
            var dueDates = new Dictionary<string, string>();
            var statuses = new Dictionary<string, string>();

            foreach (Control ctrl in flp.Controls)
            {
                // On descend dans les panneaux et sous-panneaux
                foreach (Control inner in ctrl.Controls)
                {
                    if (inner is TableLayoutPanel mainTable)
                    {
                        foreach (Control sub in mainTable.Controls)
                        {
                            if (sub is TableLayoutPanel innerTable)
                            {
                                foreach (Control input in innerTable.Controls)
                                {
                                    if (input.Tag is not string dept) continue;

                                    switch (input)
                                    {
                                        case ComboBox cb when cb.Name == "cbUser":
                                            users[dept] = cb.SelectedItem?.ToString() ?? "";
                                            break;

                                        case DateTimePicker dtp when dtp.Name == "dtpStartDate":
                                            startDates[dept] = dtp.Value.ToString("dd-MM-yyyy");
                                            break;

                                        case DateTimePicker dtp when dtp.Name == "dtpDueDate":
                                            dueDates[dept] = dtp.Value.ToString("dd-MM-yyyy");
                                            break;

                                        case ComboBox cb when cb.Name == "cbStatus":
                                            statuses[dept] = cb.SelectedItem?.ToString() ?? "";
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Combine les donnees dans le dictionnaire final
            var allDepartments = new HashSet<string>(users.Keys
                .Concat(startDates.Keys)
                .Concat(dueDates.Keys)
                .Concat(statuses.Keys));

            foreach (var dept in allDepartments)
            {
                taskData[dept] = (
                    statuses.GetValueOrDefault(dept, ""),
                    users.GetValueOrDefault(dept, ""),
                    startDates.GetValueOrDefault(dept, ""),
                    dueDates.GetValueOrDefault(dept, "")
                );
            }

            // Écriture dans le JSON
            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            using var stream = new FileStream(jsonPath, FileMode.Create, FileAccess.Write);
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

            writer.WriteStartObject();

            foreach (var property in root.EnumerateObject())
            {
                if (property.Name == "Tasks")
                {
                    writer.WritePropertyName("Tasks");
                    writer.WriteStartObject();

                    foreach (var kvp in taskData)
                    {
                        writer.WritePropertyName(kvp.Key);
                        writer.WriteStartObject();
                        writer.WriteString("status", kvp.Value.status);
                        writer.WriteString("user", kvp.Value.user);
                        writer.WriteString("startDate", kvp.Value.startDate);
                        writer.WriteString("dueDate", kvp.Value.dueDate);
                        writer.WriteEndObject();
                    }

                    writer.WriteEndObject();
                }
                else
                {
                    property.WriteTo(writer); 
                }
            }

            writer.WriteEndObject();
        }
        #endregion


        #region SHELUDE PARSE INFOS

        public static List<string> GetAllShotPaths(string prodPath)
        {
            var shotPaths = new List<string>();
            string sequencesRoot = Path.Combine(prodPath, "Shots", "Sequences");

            if (!Directory.Exists(sequencesRoot))
                return shotPaths; // aucune séquence trouvée

            foreach (string seqDir in Directory.GetDirectories(sequencesRoot))
            {
                string shotRoot = Path.Combine(seqDir, "Shots");
                if (!Directory.Exists(shotRoot))
                    continue; // pas de shots dans cette séquence

                foreach (string shotDir in Directory.GetDirectories(shotRoot))
                {
                    string nodeJsonPath = Path.Combine(shotDir, "Node.json");
                    if (File.Exists(nodeJsonPath))
                    {
                        shotPaths.Add(shotDir);
                    }
                }
            }

            return shotPaths;
        }

        public static Dictionary<string, Dictionary<string, TaskData>> ParseNodesFromPaths(List<string> nodePaths)
        {
            var result = new Dictionary<string, Dictionary<string, TaskData>>();

            foreach (var path in nodePaths)
            {
                string nodeName;

                // Shot
                if (path.Contains("\\Shots\\Sequences\\" ))
                {
                    string[] parts = path.Split('\\');
                    if (parts.Length == 8)
                    {
                        var sequenceName = parts[5];
                        var shotName = Path.GetFileName(path);
                        nodeName = $"{sequenceName}_{shotName}";

                    }
                    else
                    {
                        nodeName = Path.GetFileName(path);
                    }
                }
                else
                {
                    nodeName = Path.GetFileName(path);
                }
                string jsonPath = Path.Combine(path, "Node.json");

                if (!File.Exists(jsonPath))
                    continue;

                string json = File.ReadAllText(jsonPath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("Tasks", out var tasksElement))
                    continue;

                var taskDict = new Dictionary<string, TaskData>();

                foreach (var task in tasksElement.EnumerateObject())
                {
                    string dept = task.Name;
                    var taskData = new TaskData();

                    if (task.Value.TryGetProperty("status", out var status)) taskData.Status = status.GetString() ?? "";
                    if (task.Value.TryGetProperty("user", out var user)) taskData.User = user.GetString() ?? "";
                    if (task.Value.TryGetProperty("startDate", out var startDate)) taskData.StartDate = startDate.GetString() ?? "";
                    if (task.Value.TryGetProperty("dueDate", out var dueDate)) taskData.DueDate = dueDate.GetString() ?? "";
                    taskData.NodePath = path;
                    taskData.Department = dept;

                    taskDict[dept] = taskData;
                }

                result[nodeName] = taskDict;
            }

            return result;
        }

        public static Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>> GetAllNodesInProduction(string prodPath)
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>>();

            var nodeTypes = new Dictionary<string, string>
    {
        { "Characters", "Assets\\Characters" },
        { "Props", "Assets\\Props" },
        { "Environments", "Assets\\Environments" },
        { "Sequences", "Shots\\Sequences" },
    };

            // 
            foreach (var kvp in nodeTypes)
            {
                string typeName = kvp.Key;
                string relativePath = kvp.Value;
                string fullPath = Path.Combine(prodPath, relativePath);

                if (!Directory.Exists(fullPath))
                    continue;

                var nodePaths = Directory.GetDirectories(fullPath).ToList();
                var parsedNodes = ParseNodesFromPaths(nodePaths);
                result[typeName] = parsedNodes;
            }

            // shots
            var shotPaths = GetAllShotPaths(prodPath);
            var parsedShots = ParseNodesFromPaths(shotPaths);
            result["Shots"] = parsedShots;

            return result;
        }
        #endregion


        #region Edit Node Metadata
        public static void UpdateTaskDates(string nodePath, string department, string startDate, string dueDate)
        {
            string jsonPath = Path.Combine(nodePath, "node.json");
            if (!File.Exists(jsonPath)) return;

            var json = File.ReadAllText(jsonPath);
            var nodeData = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                           ?? new Dictionary<string, object>();

            if (!nodeData.ContainsKey("Tasks"))
                nodeData["Tasks"] = new Dictionary<string, Dictionary<string, string>>();

            var tasksDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(
                nodeData["Tasks"].ToString()
            ) ?? new Dictionary<string, Dictionary<string, string>>();

            if (!tasksDict.ContainsKey(department))
                tasksDict[department] = new Dictionary<string, string>
        {
            { "status", "" },
            { "user", "" },
            { "startDate", startDate },
            { "dueDate", dueDate }
        };
            else
            {
                tasksDict[department]["startDate"] = startDate;
                tasksDict[department]["dueDate"] = dueDate;
            }

            nodeData["Tasks"] = tasksDict;

            string updatedJson = JsonSerializer.Serialize(nodeData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, updatedJson);
        }

        #endregion
    }
}