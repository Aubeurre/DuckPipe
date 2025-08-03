using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace DuckPipe.Core
{
    public static class AssetManip
    {
        public class AssetContext
        {
            public string FileName { get; set; }
            public string File { get; set; }
            public string Extension { get; set; }
            public string WorkFolder { get; set; }
            public string AssetRoot { get; set; }
            public string RelativeWorkPath { get; set; }
            public string AssetType { get; set; }
            public string Department { get; set; }
            public string RootPath { get; set; }
            public string ProdName { get; set; }
        }

        private static AssetContext ExtractAssetContext(string assetPath)
        {
            var ctx = new AssetContext
            {
                FileName = Path.GetFileNameWithoutExtension(assetPath),
                File = Path.GetFileName(assetPath),
                Extension = Path.GetExtension(assetPath),
                WorkFolder = Path.GetDirectoryName(assetPath),
                RootPath = AssetManagerForm.GetProductionRootPath()
            };

            string[] assetParts = assetPath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);
            ctx.AssetRoot = assetParts[0];
            ctx.RelativeWorkPath = assetParts[1];

            string relativeToRoot = assetPath.Replace(ctx.RootPath, "").TrimStart('\\');
            string[] segments = relativeToRoot.Split('\\');
            ctx.ProdName = segments.Length > 2 ? segments[0] : "Unknown";
            ctx.AssetType = segments.Length > 2 ? segments[2] : "Unknown";
            ctx.Department = segments.Length > 5 ? segments[5] : "Unknown";

            return ctx;
        }

        public static JsonDocument LoadAssetJson(string assetJsonPath)
        {
            if (!File.Exists(assetJsonPath))
                throw new FileNotFoundException($"Fichier non trouvé : {assetJsonPath}");

            string jsonText = File.ReadAllText(assetJsonPath);
            return JsonDocument.Parse(jsonText);
        }

        public static void LaunchAsset(string filePath, AssetManagerForm form)
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

        public static int GetLastWorkVersion(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                MessageBox.Show("Fichier introuvable.");
                return 0;
            }

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string extension = Path.GetExtension(assetPath);

            string workFolder = Path.GetDirectoryName(assetPath);
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

        public static void VersionAsset(string assetPath, AssetManagerForm form)
        {
            if (LockAssetDepartment.IsLockedByUser(assetPath))
            {

                string LocalFile = GetTempPath(assetPath);
                if (!File.Exists(assetPath))
                {
                    MessageBox.Show("Fichier Local introuvable.");
                    return;
                }

                var ctx = ExtractAssetContext(assetPath);
                string incrementalsDir = Path.Combine(ctx.WorkFolder, "incrementals");

                Directory.CreateDirectory(incrementalsDir);

                int newVersion = GetLastWorkVersion(assetPath) + 1;
                string versionedFileName = $"{ctx.FileName}_v{newVersion:D3}{ctx.Extension}";
                string destinationPath = Path.Combine(incrementalsDir, versionedFileName);

                File.Copy(LocalFile, destinationPath);
                File.Copy(LocalFile, assetPath, true);
                MessageBox.Show($"Version enregistrée : {versionedFileName}", "Succès");
                form.WorkTabRefreshPanel(ctx.AssetRoot);
            }
            else
            {
                MessageBox.Show($"Please Grab Asset First");
            }
        }

        public static void AddNote(string assetPath, AssetManagerForm form)
        {
            var ctx = ExtractAssetContext(assetPath);

            int versionName = GetLastWorkVersion(assetPath);
            var msgPopup = new MessageBoxPopup();
            if (msgPopup.ShowDialog() == DialogResult.OK)
            {
                AddPublishLog(ctx.WorkFolder, ctx.FileName, versionName, msgPopup.CommitMessage);
            }
            UpdateAssetMetadata(assetPath, versionName, ctx.Department);
            form.WorkTabRefreshPanel(ctx.AssetRoot);
        }

        public static void PublishAsset(string assetPath, AssetManagerForm form)
        {
            if (LockAssetDepartment.IsLockedByUser(assetPath))
            {
                VersionAsset(assetPath, form);

                var ctx = ExtractAssetContext(assetPath);

                string publishFolder = Path.Combine(ctx.AssetRoot, "dlv");
                Directory.CreateDirectory(publishFolder);

                string publishedFileName = $"{ctx.FileName}_OK{ctx.Extension}";
                string publishedFilePath = Path.Combine(publishFolder, publishedFileName);

                File.Copy(assetPath, publishedFilePath, overwrite: true);
                MessageBox.Show($"Asset publié : {publishedFileName}", "Succès");

                string batPath = Path.Combine(ctx.RootPath, "Dev", "Batches", "Publish", ctx.AssetType, $"{ctx.Department}.bat");
                if (File.Exists(batPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batPath,
                        Arguments = $"\"{publishedFilePath}\"",
                        UseShellExecute = true
                    });
                }
                AddNote(assetPath, form);
                MarkDownstreamDepartmentsOutdated(assetPath, ctx.Department);
                form.WorkTabRefreshPanel(ctx.AssetRoot);
            }
            else
            {
                MessageBox.Show($"Please Grab Asset First");
            }
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

        public static void ExecAsset(string assetPath, AssetManagerForm form)
        {
            if (LockAssetDepartment.IsLockedByUser(assetPath))
            {
                var ctx = ExtractAssetContext(assetPath);

                string LocalFile = GetTempPath(assetPath);
                string publishFolder = Path.Combine(ctx.AssetRoot, "dlv");
                string publishedFileName = $"{ctx.FileName}_OK{ctx.Extension}";
                string publishedFilePath = Path.Combine(publishFolder, publishedFileName);

                string batPath = Path.Combine(ctx.RootPath, "Dev", "Batches", "Exec", ctx.AssetType, $"{ctx.Department}.bat");
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
                MessageBox.Show($"Please Grab Asset First");
            }
        }

        private static void UpdateAssetMetadata(string assetPath, int version, string deptName)
        {
            var ctx = ExtractAssetContext(assetPath);
            string jsonPath = Path.Combine(ctx.AssetRoot, "asset.json");

            if (!File.Exists(jsonPath))
                return;

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { WriteIndented = true };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var updatedAssetData = new Dictionary<string, object>();

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
                    data["user"] = Environment.UserName;
                    updated = true;
                }
            }

            updatedAssetData["workfile"] = workfileDict;

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name == "workfile") continue;

                updatedAssetData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
            }

            if (updated)
            {
                string updatedJson = JsonSerializer.Serialize(updatedAssetData, options);
                File.WriteAllText(jsonPath, updatedJson);
            }
        }

        public static List<CommitEntry> GetAssetCommits(string assetPath)
        {
            var ctx = ExtractAssetContext(assetPath);
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

        public static void MarkDownstreamDepartmentsOutdated(string assetPath, string deptPublished)
        {
            var ctx = ExtractAssetContext(assetPath);

            string configPath = Path.Combine(ctx.RootPath, ctx.ProdName, "config.json");
            string configJson = File.ReadAllText(configPath);
            using var configDoc = JsonDocument.Parse(configJson);

            if (!configDoc.RootElement.TryGetProperty("departments", out var allDepartments))
                return;

            if (!allDepartments.TryGetProperty(deptPublished, out var deptInfo))
                return;

            if (!deptInfo.TryGetProperty("downstream", out var downstreamList))
                return;

            string jsonPath = Path.Combine(ctx.AssetRoot, "asset.json");
            if (!File.Exists(jsonPath))
                return;

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { WriteIndented = true };

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var updatedAssetData = new Dictionary<string, object>();

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

            updatedAssetData["workfile"] = workfileDict;

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name == "workfile") continue;
                updatedAssetData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
            }

            if (updated)
            {
                string updatedJson = JsonSerializer.Serialize(updatedAssetData, options);
                File.WriteAllText(jsonPath, updatedJson);
            }
        }

        public static string GetTempPath(string assetPath)
        {
            string tempAssetlPath = assetPath.Replace(UserConfig.Get().ProdBasePath, UserConfig.Get().userTempFolder);

            return tempAssetlPath;
        }

        public static void CopyAssetToTemp(string assetPath)
        {
            string tempAssetlPath = GetTempPath(assetPath);
            string tempDirPath = Path.GetDirectoryName(tempAssetlPath)!;

            if (Directory.Exists(tempDirPath))
                Directory.Delete(tempDirPath, true);

            Directory.CreateDirectory(tempDirPath);
            File.Copy(assetPath, tempAssetlPath, true);
        }

        public static void DeleteTemp(string assetPath)
        {
            string tempAssetlPath = GetTempPath(assetPath);
            string tempDirPath = Path.GetDirectoryName(tempAssetlPath)!;

            if (Directory.Exists(tempDirPath))
                Directory.Delete(tempDirPath, true);
        }

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
                    property.WriteTo(writer); // Conserve les autres champs
                }
            }

            writer.WriteEndObject();
        }
        #endregion



        public class TaskData
        {
            public string Status { get; set; }
            public string User { get; set; }
            public string StartDate { get; set; }
            public string DueDate { get; set; }
        }

        public static Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>> GetAllAssetsInProduction(string prodPath)
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>>();

            var assetTypes = new Dictionary<string, string>
    {
        { "Characters", "Assets\\Characters" },
        { "Props", "Assets\\Props" },
        { "Environments", "Assets\\Environments" },
    };
            foreach (var kvp in assetTypes)
            {
                string typeName = kvp.Key;
                string relativePath = kvp.Value;

                string fullPath = Path.Combine(prodPath, relativePath);

                var typeDict = new Dictionary<string, Dictionary<string, TaskData>>();

                foreach (string dir in Directory.GetDirectories(fullPath))
                {
                    string assetName = Path.GetFileName(dir);
                    string AssetJsonPath = Path.Combine(dir, "Asset.json");
                    string json = File.ReadAllText(AssetJsonPath);
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

                        taskDict[dept] = taskData;
                    }

                    typeDict[assetName] = taskDict;
                }

                result[typeName] = typeDict;
            }

            return result;
        }

    }
}