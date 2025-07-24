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

        private async Task CreateDefaultFolders(string prodPath)
        {
            string[] folders =
            {
                "Assets/Characters",
                "Assets/Props",
                "Assets/Environments",
                "Assets/Template",
                "Shots/Template",
                "Shots/Sequences",
                "Renders",
                "IO/In",
                "IO/Out",
                "Dev",
                "RnD",
                "Preprod/Concept/Characters",
                "Preprod/Concept/Props",
                "Preprod/Concept/Environments",
                "Preprod/Board",
            };

            foreach (string folder in folders)
                await Program.Storage.CreateFolderAsync(Path.Combine(prodPath, folder));
        }

        private async Task CopyAssetStructure(string prodPath)
        {
            string assetStructurePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "AssetStructure.json");
            string targetPath = Path.Combine(prodPath, "Dev", "AssetStructure.json");
            await Program.Storage.CopyFileAsync(assetStructurePath, targetPath);
        }

        private async Task CopyTools(string prodPath)
        {
            string sourceToolPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools");
            string targetToolPath = Path.Combine(prodPath, "Dev");

            foreach (string dirPath in Directory.GetDirectories(sourceToolPath, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceToolPath, dirPath);
                await Program.Storage.CreateFolderAsync(Path.Combine(targetToolPath, relativePath));
            }

            foreach (string filePath in Directory.GetFiles(sourceToolPath, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceToolPath, filePath);
                await Program.Storage.CopyFileAsync(filePath, Path.Combine(targetToolPath, relativePath));

            }
        }

        private async Task InitializeDefaultDepartments()
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

        private async Task SaveProductionConfig(string prodPath)
        {
            var config = new
            {
                name,
                created,
                version,
                departments
            };

            string configPath = Path.Combine(prodPath, "config.json").Replace('\\', '/');
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(config, options);
            byte[] content = System.Text.Encoding.UTF8.GetBytes(json);
            await Program.Storage.CreateFileAsync(configPath, content);
        }

    }

    public class DepartmentStructure
    {
        public List<string> downstream { get; set; } = new();
    }
}
