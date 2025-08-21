using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DuckPipe.Core.Model;
using DuckPipe.Core.Services;
using DuckPipe.Core.Manager;
using DuckPipe.Core.Config;

namespace DuckPipe.Core.Manipulator
{
    public static class NodeManip
    {
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

        private static NodeContext ExtractNodeContext(string nodePath)
        {
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
            ctx.RelativeWorkPath = nodeParts[1];

            string relativeToRoot = nodePath.Replace(ctx.RootPath, "").TrimStart('\\');
            string[] segments = relativeToRoot.Split('\\');
            ctx.ProdName = segments.Length > 2 ? segments[0] : "Unknown";
            ctx.NodeType = segments.Length > 2 ? segments[2] : "Unknown";
            ctx.Department = segments.Length > 5 ? segments[5] : "Unknown";

            return ctx;
        }

        public static JsonDocument LoadNodeJson(string nodeJsonPath)
        {
            if (!File.Exists(nodeJsonPath))
                throw new FileNotFoundException($"Fichier non trouvé : {nodeJsonPath}");

            string jsonText = File.ReadAllText(nodeJsonPath);
            return JsonDocument.Parse(jsonText);
        }

        public static string ReplaceEnvVariables(string path)
        {
            string envPath = UserConfig.Get().ProdBasePath;

            if (!Directory.Exists(envPath))
            {
                MessageBox.Show($"Le chemin défini dans DUCKPIPE_ROOT est invalide :\n{envPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return path.Replace("${DUCKPIPE_ROOT}", envPath ?? "");

        }

        public static int GetLastWorkVersion(string nodePath)
        {
            if (!File.Exists(nodePath))
            {
                MessageBox.Show("Fichier introuvable.");
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
            UpdateNodeMetadata(nodePath, versionName, ctx.Department);
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
                    MessageBox.Show($"Erreur lors de la lecture du changelog :\n{ex.Message}");
                    entries = new();
                }
            }

            entries.Add(new CommitEntry
            {
                Version = version.ToString("D3"),
                Timestamp = DateTime.Now,
                User = Environment.UserName,
                Message = message
            });

            string updatedJson = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(changelogPath, updatedJson);
        }

        public class CommitEntry
        {
            public string Version { get; set; }
            public DateTime Timestamp { get; set; }
            public string User { get; set; }
            public string Message { get; set; }
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


        #region TEMP FILE
        public static string GetTempPath(string nodePath)
        {
            string tempNodelPath = nodePath.Replace(UserConfig.Get().ProdBasePath, UserConfig.Get().userTempFolder);

            return tempNodelPath;
        }

        public static void CopyNodeToTemp(string nodePath)
        {
            string tempNodelPath = GetTempPath(nodePath);
            string tempDirPath = Path.GetDirectoryName(tempNodelPath)!;

            if (Directory.Exists(tempDirPath))
                Directory.Delete(tempDirPath, true);

            Directory.CreateDirectory(tempDirPath);
            File.Copy(nodePath, tempNodelPath, true);
        }

        public static void DeleteTemp(string nodePath)
        {
            string tempNodelPath = GetTempPath(nodePath);
            string tempDirPath = Path.GetDirectoryName(tempNodelPath)!;

            if (Directory.Exists(tempDirPath))
                Directory.Delete(tempDirPath, true);
        }
        #endregion


        #region EXECUTE TASK ON ASSET
        public static void LaunchNode(string filePath, AssetManagerForm form)
        {
            string LocalFile = GetTempPath(filePath);
            if (File.Exists(LocalFile))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = LocalFile,
                    UseShellExecute = true
                });
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }

        }

        public static void ExecNode(string nodePath, AssetManagerForm form)
        {
            if (LockNodeFileManager.IsLockedByUser(nodePath))
            {
                var ctx = ExtractNodeContext(nodePath);

                string LocalFile = GetTempPath(nodePath);
                string publishFolder = Path.Combine(ctx.NodeRoot, "dlv");
                string publishedFileName = $"{ctx.FileName}_OK{ctx.Extension}";
                string publishedFilePath = Path.Combine(publishFolder, publishedFileName);

                string batPath = Path.Combine(ctx.RootPath, "Dev", "Batches", "Exec", ctx.NodeType, $"{ctx.Department}.bat");
                if (File.Exists(batPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batPath,
                        Arguments = $"\"{publishedFilePath}\"",
                        UseShellExecute = true
                    });
                }
            }
            else
            {
                MessageBox.Show($"Please Grab Node First");
            }
        }

        public static void PublishNode(string nodePath, AssetManagerForm form)
        {
            if (LockNodeFileManager.IsLockedByUser(nodePath))
            {
                VersionNode(nodePath, form);

                var ctx = ExtractNodeContext(nodePath);

                string publishFolder = Path.Combine(ctx.NodeRoot, "dlv");
                Directory.CreateDirectory(publishFolder);

                string publishedFileName = $"{ctx.FileName}_OK{ctx.Extension}";
                string publishedFilePath = Path.Combine(publishFolder, publishedFileName);

                File.Copy(nodePath, publishedFilePath, overwrite: true);
                MessageBox.Show($"Node publié : {publishedFileName}", "Succès");

                string batPath = Path.Combine(ctx.RootPath, "Dev", "Batches", "Publish", ctx.NodeType, $"{ctx.Department}.bat");
                if (File.Exists(batPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batPath,
                        Arguments = $"\"{publishedFilePath}\"",
                        UseShellExecute = true
                    });
                }
                AddNote(nodePath, form);
                MarkDownstreamDepartmentsOutdated(nodePath, ctx.Department);
                form.RefreshTab(ctx.NodeRoot);
            }
            else
            {
                MessageBox.Show($"Please Grab Node First");
            }
        }

        public static void VersionNode(string nodePath, AssetManagerForm form)
        {
            if (LockNodeFileManager.IsLockedByUser(nodePath))
            {

                string LocalFile = GetTempPath(nodePath);
                if (!File.Exists(nodePath))
                {
                    MessageBox.Show("Fichier Local introuvable.");
                    return;
                }

                var ctx = ExtractNodeContext(nodePath);
                string incrementalsDir = Path.Combine(ctx.WorkFolder, "incrementals");

                Directory.CreateDirectory(incrementalsDir);

                int newVersion = GetLastWorkVersion(nodePath) + 1;
                string versionedFileName = $"{ctx.FileName}_v{newVersion:D3}{ctx.Extension}";
                string destinationPath = Path.Combine(incrementalsDir, versionedFileName);

                File.Copy(LocalFile, destinationPath);
                File.Copy(LocalFile, nodePath, true);
                MessageBox.Show($"Version enregistrée : {versionedFileName}", "Succès");

                UpdateNodeMetadata(nodePath, newVersion, ctx.Department);

                form.RefreshTab(ctx.NodeRoot);
            }
            else
            {
                MessageBox.Show($"Please Grab Node First");
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

            // Combine les données dans le dictionnaire final
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

            foreach (string seqDir in Directory.GetDirectories(sequencesRoot))
            {
                string shotRoot = Path.Combine(seqDir, "Shots");
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

        public static Dictionary<string, Dictionary<string, Models.TaskData>> ParseNodesFromPaths(List<string> nodePaths)
        {
            var result = new Dictionary<string, Dictionary<string, Models.TaskData>>();

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

                var taskDict = new Dictionary<string, Models.TaskData>();

                foreach (var task in tasksElement.EnumerateObject())
                {
                    string dept = task.Name;
                    var taskData = new Models.TaskData();

                    if (task.Value.TryGetProperty("status", out var status)) taskData.Status = status.GetString() ?? "";
                    if (task.Value.TryGetProperty("user", out var user)) taskData.User = user.GetString() ?? "";
                    if (task.Value.TryGetProperty("startDate", out var startDate)) taskData.StartDate = startDate.GetString() ?? "";
                    if (task.Value.TryGetProperty("dueDate", out var dueDate)) taskData.DueDate = dueDate.GetString() ?? "";

                    taskDict[dept] = taskData;
                }

                result[nodeName] = taskDict;
            }

            return result;
        }

        public static Dictionary<string, Dictionary<string, Dictionary<string, Models.TaskData>>> GetAllNodesInProduction(string prodPath)
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, Models.TaskData>>>();

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

    }
}