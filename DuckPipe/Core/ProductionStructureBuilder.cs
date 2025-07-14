using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DuckPipe.Core
{
    public class ProductionStructureBuilder
    {
        public string name { get; set; }
        public string created { get; set; }
        public string version { get; set; }

        public void CreateProductionStructure(string prodName, string rootPath)
        {
            string prodPath = Path.Combine(rootPath, prodName);
            Directory.CreateDirectory(prodPath);

            string[] folders = new string[]
            {
                "Assets/Characters",
                "Assets/Props",
                "Assets/Environments",
                "Assets/Template",
                "Shots/Template",
                "Renders",
                "IO/In",
                "IO/Out",
                "Dev",
                "RnD",
            };

            foreach (string folder in folders)
            {
                string fullPath = Path.Combine(prodPath, folder);
                Directory.CreateDirectory(fullPath);
            }

            name = prodName;
            created = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            version = "1.0";

            // Copie du fichier JSON original dans le dossier de production pour modification future
            string assetStructurePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "AssetStructure.json");
            File.Copy(assetStructurePath, Path.Combine(prodPath, "Dev", "AssetStructure.json"), overwrite: true);

            string toolPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools");
            string targetToolPath = Path.Combine(prodPath, "Dev");

            foreach (string dirPath in Directory.GetDirectories(toolPath, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(toolPath, dirPath);
                string targetDir = Path.Combine(targetToolPath, relativePath);
                Directory.CreateDirectory(targetDir);
            }

            foreach (string filePath in Directory.GetFiles(toolPath, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(toolPath, filePath);
                string targetFilePath = Path.Combine(targetToolPath, relativePath);
                File.Copy(filePath, targetFilePath, overwrite: true);
            }

            // Sauvegarde du fichier config de production
            string configJson = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(Path.Combine(prodPath, "config.json"), configJson);
        }
    }
}