using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace DuckPipe.Core
{
    public static class AssetManip
    {
        public static JsonDocument LoadAssetJson(string assetJsonPath)
        {
            if (!File.Exists(assetJsonPath))
                throw new FileNotFoundException($"Fichier non trouvé : {assetJsonPath}");

            string jsonText = File.ReadAllText(assetJsonPath);
            return JsonDocument.Parse(jsonText);
        }

        public static void OpenWorkFile(string assetJsonPath, string department)
        {
            using var doc = LoadAssetJson(assetJsonPath);

            if (!doc.RootElement.TryGetProperty("departments", out JsonElement departments)) return;
            if (!departments.TryGetProperty(department, out JsonElement dept)) return;

            string workPath = ReplaceEnvVariables(dept.GetProperty("workPath").GetString());
            string workFile = dept.GetProperty("workFile").GetString();
            string software = dept.GetProperty("software").GetString();

            string filePath = Path.Combine(workPath, workFile);
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Fichier introuvable : {filePath}");
                return;
            }

            LaunchSoftware(filePath);
        }

        public static void LaunchSoftware(string filePath)
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
    }
}
