using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DuckPipe.Core
{
    public class ProductionStructureBuilder
    {
        public string name { get; set; }
        public string created { get; set; }
        public string version { get; set; }
        public Dictionary<string, DepartmentStructure> departments { get; set; } = new();

        public void CreateProductionStructure(string prodName, string rootPath)
        {
            string prodPath = Path.Combine(rootPath, prodName);
            Directory.CreateDirectory(prodPath);

            CreateDefaultFolders(prodPath);
            CopyAssetStructure(prodPath);
            CopyTools(prodPath);

            name = prodName;
            created = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            version = "1.0";

            InitializeDefaultDepartments();
            SaveProductionConfig(prodPath);
        }

        private void CreateDefaultFolders(string prodPath)
        {
            string[] folders =
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
                Directory.CreateDirectory(Path.Combine(prodPath, folder));
        }

        private void CopyAssetStructure(string prodPath)
        {
            string assetStructurePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "AssetStructure.json");
            string targetPath = Path.Combine(prodPath, "Dev", "AssetStructure.json");
            File.Copy(assetStructurePath, targetPath, overwrite: true);
        }

        private void CopyTools(string prodPath)
        {
            string sourceToolPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools");
            string targetToolPath = Path.Combine(prodPath, "Dev");

            foreach (string dirPath in Directory.GetDirectories(sourceToolPath, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceToolPath, dirPath);
                Directory.CreateDirectory(Path.Combine(targetToolPath, relativePath));
            }

            foreach (string filePath in Directory.GetFiles(sourceToolPath, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceToolPath, filePath);
                File.Copy(filePath, Path.Combine(targetToolPath, relativePath), overwrite: true);
            }
        }

        private void InitializeDefaultDepartments()
        {
            departments = new Dictionary<string, DepartmentStructure>
            {
                ["Modeling"] = new DepartmentStructure { downstream = new() { "Facial", "Cfx", "Groom", "Surf", "Rig" } },
                ["Facial"] = new DepartmentStructure { downstream = new() { "Rig" } },
                ["Cfx"] = new DepartmentStructure { downstream = new() { "Rig" } },
                ["Groom"] = new DepartmentStructure { downstream = new() { "Rig" } },
                ["Surf"] = new DepartmentStructure { downstream = new() { "Rig" } },
                ["Rig"] = new DepartmentStructure { downstream = new() }
            };
        }

        private void SaveProductionConfig(string prodPath)
        {
            var config = new
            {
                name,
                created,
                version,
                departments
            };

            string configPath = Path.Combine(prodPath, "config.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(configPath, JsonSerializer.Serialize(config, options));
        }
    }

    public class DepartmentStructure
    {
        public List<string> downstream { get; set; } = new();
    }
}
