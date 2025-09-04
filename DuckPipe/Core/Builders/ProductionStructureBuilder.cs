using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DuckPipe.Core.Services;
using DuckPipe.Core.Services.Softwares;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static DuckPipe.CreateProductionPopup;

namespace DuckPipe.Core
{
    public class ProductionStructureBuilder
    {
        public string name { get; set; }
        public string created { get; set; }
        public string version { get; set; }
        public string deliveryDay { get; set; }
        public Dictionary<string, DepartmentStructure> departments { get; set; } = new();

        public void CreateProductionStructure(string prodName, string rootPath, Dictionary<string, Dictionary<string, DeptInfo>> prodStructure)
        {
            string prodPath = Path.Combine(rootPath, prodName);
            Directory.CreateDirectory(prodPath);

            CreateDefaultFolders(prodPath);
            SaveProductionConfig(prodPath, prodStructure);
            BuildNodeStructure(prodPath, prodStructure);
            CreateDefaultTemplateScene(prodPath);
            CopyTools(prodPath);
        }

        private static void BuildNodeStructure(string prodPath, Dictionary<string, Dictionary<string, DeptInfo>> prodStructure)
        {
            var nodeStructure = new Dictionary<string, object>();

            foreach (var category in prodStructure)
            {
                var categoryObj = new Dictionary<string, object>
                {
                    ["dlv"] = new Dictionary<string, object>()
                };

                var worksDict = new Dictionary<string, object>();

                foreach (var work in category.Value)
                {
                    string deptName = work.Key.Split("|")[0];
                    if (string.IsNullOrEmpty(deptName)) continue;

                    var tools = work.Value.Works ?? new List<string>();

                    var incrementals = new List<string>();
                    var finals = new List<string>();

                    foreach (var tool in tools)
                    {
                        string toolLower = tool.ToLower();
                        incrementals.Add($"{{nodenamepipeplaceholder}}_{deptName.ToLower()}_v000.{toolLower}");
                        finals.Add($"{{nodenamepipeplaceholder}}_{deptName.ToLower()}.{toolLower}");
                    }

                    var workNode = new Dictionary<string, object>
                    {
                        ["incrementals"] = new Dictionary<string, object> { ["_files"] = incrementals },
                        ["_files"] = finals
                    };

                    worksDict[deptName] = workNode;
                }

                if (worksDict.Any())
                    categoryObj["Work"] = worksDict;

                nodeStructure[category.Key] = categoryObj;
            }


            string configDir = Path.Combine(prodPath, "Dev", "DangerZone");
            string configPath = Path.Combine(configDir, "NodeStructure.json");

            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Directory.Exists(configPath))
                File.WriteAllText(configPath, JsonSerializer.Serialize(nodeStructure, options));
        }


        private void CreateDefaultTemplateScene(string prodPath)
        {
            // Implementation for creating a default template scene can be added here.

            // STUDIO CAM FOR LAYOUT
            string templateScenePath = Path.Combine(prodPath, "Shots", "Templates",
                $"studioCamera{ProductionService.GetFileMainExt(prodPath, "Layout")}");
            if (ProductionService.GetFileMainExt(prodPath, "Layout") == ".ma" && !File.Exists(templateScenePath))
                MayaService.CreateBasicMaFile(templateScenePath, "studioCamera.ma");
            if (ProductionService.GetFileMainExt(prodPath, "Layout") == ".blend" && !File.Exists(templateScenePath))
                BlenderService.CreateBasicBlendFile(templateScenePath);

            // STUDIO LIGHT FOR LIGHT
            templateScenePath = Path.Combine(prodPath, "Shots", "Templates",
                $"studioLight{ProductionService.GetFileMainExt(prodPath, "Light")}");
            if (ProductionService.GetFileMainExt(prodPath, "Light") == ".ma" && !File.Exists(templateScenePath))
                MayaService.CreateBasicMaFile(templateScenePath, "studioLight.ma");
            if (ProductionService.GetFileMainExt(prodPath, "Light") == ".blend" && !File.Exists(templateScenePath))
                BlenderService.CreateBasicBlendFile(templateScenePath);
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

        private void InitializeDefaultDepartments(Dictionary<string, Dictionary<string, DeptInfo>> prodStructure)
        {
            foreach (var category in prodStructure)
            {
                foreach (var work in category.Value)
                {
                    string deptName = work.Key;
                    string deptColor = work.Value.ColorHex;
                    Color color = ColorTranslator.FromHtml(deptColor);

                    // on prend le premier work si dispo
                    string tool = (work.Value.Works != null && work.Value.Works.Count > 0)
                        ? work.Value.Works[0]
                        : "";

                    if (!departments.ContainsKey(deptName))
                    {
                        departments[deptName] = new DepartmentStructure
                        {
                            downstream = new List<string>(),
                            mainFileExt = $".{tool}",
                            color = color
                        };
                    }
                }
            }
        }

        private void SaveProductionConfig(string prodPath, Dictionary<string, Dictionary<string, DeptInfo>> prodStructure)
        {
            InitializeDefaultDepartments(prodStructure);

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
        }
    }

    public class DepartmentStructure
    {
        public List<string> downstream { get; set; } = new();
        public string mainFileExt { get; set; } = ".ma";
        public Color color { get; set; } = Color.Gray;
    }
}
