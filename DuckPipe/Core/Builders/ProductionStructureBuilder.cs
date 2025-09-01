using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DuckPipe.Core.Services;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DuckPipe.Core
{
    public class ProductionStructureBuilder
    {
        public string name { get; set; }
        public string created { get; set; }
        public string version { get; set; }
        public string deliveryDay { get; set; }
        public Dictionary<string, DepartmentStructure> departments { get; set; } = new();

        public void CreateProductionStructure(string prodName, string rootPath)
        {
            string prodPath = Path.Combine(rootPath, prodName);
            Directory.CreateDirectory(prodPath);

            CreateDefaultFolders(prodPath);
            CopyNodeStructure(prodPath);
            CopyTools(prodPath);
            CreateDefaultTemplateScene(prodPath);
            SaveProductionConfig(prodPath);
        }

        private void CreateDefaultTemplateScene(string prodPath)
        {
            // Implementation for creating a default template scene can be added here.
            // This could involve copying a predefined scene file into the appropriate directory.
            string templateScenePath = Path.Combine(prodPath, "Shots", "Templates", "studioCamera.ma");
            if (!File.Exists(templateScenePath))
                MayaService.CreateBasicMaFile(templateScenePath, "studioCamera.ma");
        }

        private void CreateDefaultFolders(string prodPath)
        {
            string[] folders =
            {
                "Assets/Characters",
                "Assets/Props",
                "Assets/Environments",
                "Assets/Templates",
                "Shots/Templates",
                "Shots/Sequences",
                "Renders",
                "IO/In",
                "IO/Out",
                "Dev/DangerZone",
                "RnD",
                "Preprod/Concept/Characters/Work",
                "Preprod/Concept/Props/Work",
                "Preprod/Concept/Environments/Work",
                "Preprod/Concept/Characters/dlv",
                "Preprod/Concept/Props/dlv",
                "Preprod/Concept/Environments/dlv",
                "Preprod/Board/dlv",
                "Preprod/Board/Work",
            };

            foreach (string folder in folders)
                Directory.CreateDirectory(Path.Combine(prodPath, folder));
        }

        private void CopyNodeStructure(string prodPath)
        {
            string nodeStructurePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "NodeStructure.json");
            string targetPath = Path.Combine(prodPath, "Dev", "DangerZone", "NodeStructure.json");
            File.Copy(nodeStructurePath, targetPath, overwrite: false);
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
                ["Rig"] = new DepartmentStructure { downstream = new() },
                ["Layout"] = new DepartmentStructure { downstream = new() { "Anim", "Lighting", "Comp", "Cfx" } },
                ["Anim"] = new DepartmentStructure { downstream = new() { "Lighting", "Comp", "Cfx" } },
                ["Cfx"] = new DepartmentStructure { downstream = new() { "Comp" } },
                ["Lighting"] = new DepartmentStructure { downstream = new() { "Comp" } }
            };
        }

        private void SaveProductionConfig(string prodPath)
        {
            InitializeDefaultDepartments();

            name = Path.GetFileName(prodPath);
            created = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            version = "1.0";
            deliveryDay = DateTime.Now.AddDays(365).ToString("yyyy-MM-dd");

            var config = new
            {
                name,
                created,
                deliveryDay,
                version,
                departments,
                status = new Dictionary<string, string>
        {
            { "not_started", "icons/NEW.png" },
            { "outDated", "icons/OLD.png" },
            { "upToDate", "icons/APP.png" },
            { "wip", "icons/WIP.png" },
            { "pendingReview", "icons/PR.png" },
            { "rtk", "icons/RTK.png" },
            { "omit", "icons/OMIT.png" }
        },
                color = new Dictionary<string, Color>
            {
                { "RIG", Color.Teal },
                { "MODELING", Color.SteelBlue },
                { "FACIAL", Color.IndianRed },
                { "CFX", Color.Goldenrod },
                { "ANIM", Color.MediumPurple },
                { "LIGHTING", Color.Gold },
                { "GROOM", Color.MediumSpringGreen },
                { "SURF", Color.LightSeaGreen },
                { "LAYOUT", Color.DimGray },
                { "ART", Color.OliveDrab }
        },
            Users = new List<string>()
                {
                    ProductionService.GetUserName().ToString(),
                }
            };

            string configPath = Path.Combine(prodPath, "Dev", "DangerZone", "config.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Directory.Exists(configPath))
                File.WriteAllText(configPath, JsonSerializer.Serialize(config, options));
        }

        public void Check(string prodName, string prodPath)
        {
            string fullPath = Path.Combine(prodPath, prodName);
            CreateDefaultFolders(fullPath);
            CopyTools(fullPath);
            CreateDefaultTemplateScene(fullPath);
            SaveProductionConfig(fullPath);
            CopyNodeStructure(fullPath);
        }
    }

    public class DepartmentStructure
    {
        public List<string> downstream { get; set; } = new();
    }
}
