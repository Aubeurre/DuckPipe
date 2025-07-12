using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace DuckPipe
{
    public class ProductionConfig
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
            string assetStructurePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AssetStructure.json");
            File.Copy(assetStructurePath, Path.Combine(prodPath, "AssetStructure.json"), overwrite: true);

            // Sauvegarde du fichier config de production
            string configJson = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(Path.Combine(prodPath, "config.json"), configJson);
        }
    }
}