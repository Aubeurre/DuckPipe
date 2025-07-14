using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        public static void VersionAsset(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                MessageBox.Show("Fichier introuvable.");
                return;
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

            int newVersion = maxVersion + 1;
            string versionedFileName = $"{fileName}_v{newVersion:D3}{extension}";
            string destinationPath = Path.Combine(incrementalsDir, versionedFileName);

            File.Copy(assetPath, destinationPath);
            MessageBox.Show($"Version enregistrée : {versionedFileName}", "Succès");
        }

        public static void PublishAsset(string assetPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string extension = Path.GetExtension(assetPath);
            string workFolder = Path.GetDirectoryName(assetPath);


            string[] assetParts = assetPath.Split(new[] { "\\Work\\" }, StringSplitOptions.None);
            if (assetParts.Length < 2)
            {
                MessageBox.Show("Chemin d’asset invalide.");
                return;
            }
            string assetRoot = assetParts[0]; 
            string relativeWorkPath = assetParts[1]; 

            string publishFolder = Path.Combine(assetRoot, "dlv");
            Directory.CreateDirectory(publishFolder);

            string publishedFileName = $"{fileName}_OK{extension}";
            string publishedFilePath = Path.Combine(publishFolder, publishedFileName);

            File.Copy(assetPath, publishedFilePath, overwrite: true);
            MessageBox.Show($"Asset publié : {publishedFileName}", "Succès");

            // Extraction de prodName et assetType (ex: PROD/TEST, Props)
            string rootPath = AssetManagerForm.GetProductionRootPath(); // Ex: D:\ICHIGO\PROD\TEST
            string relativeToRoot = assetPath.Replace(rootPath, "").TrimStart('\\');
            string[] segments = relativeToRoot.Split('\\');

            string assetType = segments[2]; 
            string deptName = segments[5]; 

            string batPath = Path.Combine(rootPath, "Dev", assetType, $"{deptName}.bat");
            MessageBox.Show($"bat path : {batPath}");

            if (File.Exists(batPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = batPath,
                    Arguments = $"\"{publishedFilePath}\"",
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
        }

    }
}
