using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DuckPipe.Core
{
    public static class AssetManip
    {
        public class AssetContext
        {
            public string FileName { get; set; }
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

        public static void GrabAsset(string assetPath)
        {
            // LockAssetDepartment.TryLockFile(assetPath);
        }
        
        public static void LaunchAsset(string filePath)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });

        }

        public static string ReplaceEnvVariables(string path)
        {
            string envPath = Environment.GetEnvironmentVariable("DUCKPIPE_ROOT");

            if (string.IsNullOrEmpty(envPath))
            {
                envPath = @"D:\ICHIGO\PROD";
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

        public static void VersionAsset(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                MessageBox.Show("Fichier introuvable.");
                return;
            }

            var ctx = ExtractAssetContext(assetPath);
            string incrementalsDir = Path.Combine(ctx.WorkFolder, "incrementals");

            Directory.CreateDirectory(incrementalsDir);

            int newVersion = GetLastWorkVersion(assetPath) + 1;
            string versionedFileName = $"{ctx.FileName}_v{newVersion:D3}{ctx.Extension}";
            string destinationPath = Path.Combine(incrementalsDir, versionedFileName);

            File.Copy(assetPath, destinationPath);
            MessageBox.Show($"Version enregistrée : {versionedFileName}", "Succès");
        }

        public static void PublishAsset(string assetPath)
        {
            VersionAsset(assetPath);

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

            int versionName = GetLastWorkVersion(assetPath);
            var msgPopup = new MessageBoxPopup();
            if (msgPopup.ShowDialog() == DialogResult.OK)
            {
                AddPublishLog(ctx.WorkFolder, versionName, msgPopup.CommitMessage);
            }
            UpdateAssetMetadata(assetPath, versionName, ctx.Department);
            MarkDownstreamDepartmentsOutdated(assetPath, ctx.Department);
        }

        public static void AddPublishLog(string assetPath, int version, string message)
        {
            string changelogPath = Path.Combine(assetPath, "changelog.json");

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

        public static void ExecAsset(string assetPath)
        {
            var ctx = ExtractAssetContext(assetPath);

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

        private static void UpdateAssetMetadata(string assetPath, int version, string deptName)
        {
            var ctx = ExtractAssetContext(assetPath);
            string jsonPath = Path.Combine(ctx.AssetRoot, "asset.json");

            string json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { WriteIndented = true };

            var assetData = JsonSerializer.Deserialize<Dictionary<string, object>>(json, options);

            var deptDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(
                JsonSerializer.Serialize(assetData["departments"])
            );

            deptDict[deptName]["version"] = $"v{version.ToString("D3")}";
            deptDict[deptName]["lastModified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            deptDict[deptName]["status"] = "upToDate";

            assetData["departments"] = deptDict;

            string updatedJson = JsonSerializer.Serialize(assetData, options);
            File.WriteAllText(jsonPath, updatedJson);
        }

        public static List<CommitEntry> GetAssetCommits(string assetPath)
        {
            var ctx = ExtractAssetContext(assetPath);
            string changelogPath = Path.Combine(ctx.WorkFolder, "changelog.json");
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
                    : DateTime.MinValue; string user = entry.TryGetProperty("user", out var u) ? u.GetString() ?? "(inconnu)" : "(inconnu)";
                string message = entry.TryGetProperty("Message", out var m) ? m.GetString() ?? "" : "";

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
            MessageBox.Show($"{configPath}");

            var configJson = File.ReadAllText(configPath);
            using var configDoc = JsonDocument.Parse(configJson);
            var root = configDoc.RootElement;

            if (!root.TryGetProperty("departments", out var departments))
                return;

            if (!departments.TryGetProperty(deptPublished, out var deptInfo))
                return;

            if (!deptInfo.TryGetProperty("downstream", out var downstreamList))
                return;

            string assetJsonPath = Path.Combine(ctx.AssetRoot, "asset.json");

            var assetJson = File.ReadAllText(assetJsonPath);
            var assetDoc = JsonDocument.Parse(assetJson);
            var assetObj = JsonSerializer.Deserialize<Dictionary<string, object>>(assetJson);

            var departmentsDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(
                assetDoc.RootElement.GetProperty("departments").GetRawText()
            );

            foreach (var downstreamDept in downstreamList.EnumerateArray())
            {
                var name = downstreamDept.GetString();
                if (departmentsDict.ContainsKey(name))
                {
                    departmentsDict[name]["status"] = "outDated";
                }
            }

            var newData = new Dictionary<string, object>
            {
                ["departments"] = departmentsDict
            };

            File.WriteAllText(assetJsonPath, JsonSerializer.Serialize(newData, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
