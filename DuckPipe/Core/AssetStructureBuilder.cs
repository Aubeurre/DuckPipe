using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using DuckPipe.Utils;
using static System.Windows.Forms.Design.AxImporter;

namespace DuckPipe.Core
{
    public class AssetTypeDefinition
    {
        public string Name { get; set; }
        public List<string> Folders { get; set; }
        public List<string> Files { get; set; }
    }

    public class AssetStructure
    {
        public string Name { get; set; }

        [JsonPropertyName("structure")]
        public Dictionary<string, AssetNode> Structure { get; set; } = new();
    }

    public class AssetNode
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Children { get; set; }

        [JsonPropertyName("_files")]
        public List<string> Files { get; set; } = new();
    }

    public static class AssetStructureBuilder
    {
        public static void CreateAssetStructure(string rootPath, string assetPath, AssetStructure structure, string assetName)
        {

            if (structure.Name == "Characters" || structure.Name == "Props" || structure.Name == "Environments")
            {
                Directory.CreateDirectory(assetPath);
                CreateAssetJson(rootPath, assetPath, structure, assetName);

                foreach (var kvp in structure.Structure)
                {
                    CreateNode(Path.Combine(assetPath, kvp.Key), kvp.Value, assetName);
                }
            }

            if (structure.Name == "Shots")
            {
                string cleanPath = Path.Combine(Path.GetDirectoryName(assetPath), $"P{assetName}");
                Directory.CreateDirectory(cleanPath);
                CreateShotJson(rootPath, cleanPath, structure, assetName);

                foreach (var kvp in structure.Structure)
                {
                    CreateNode(Path.Combine(cleanPath, kvp.Key), kvp.Value, assetName);
                }
            }

            if (structure.Name == "Sequences")
            {
                string cleanPath = Path.Combine(Path.GetDirectoryName(assetPath), $"S{assetName}");
                Directory.CreateDirectory(cleanPath);
                CreateSeqJson(rootPath, cleanPath, structure, assetName);

                foreach (var kvp in structure.Structure)
                {
                    CreateNode(Path.Combine(cleanPath, kvp.Key), kvp.Value, assetName);
                }
            }

            if (structure.Name == "Characters")
                AddCostume(assetPath, "Body");
        }

        private static void CreateNode(string currentPath, AssetNode node, string assetName)
        {
            Directory.CreateDirectory(currentPath);

            // Créer les fichiers
            if (node.Files != null)
            {

                string[] parts = currentPath.Split('\\');
                string sequenceFolder = parts[6];
                string sequenceName = sequenceFolder.StartsWith("S") ? sequenceFolder.Substring(1) : sequenceFolder;
                foreach (var file in node.Files)
                {
                    string fileName = file;
                    if (file.Contains("{assetnamepipeplaceholder}"))
                    {
                        fileName = file.Replace("{assetnamepipeplaceholder}", assetName.ToLower())
                            .Replace("{seqnamepipeplaceholder}", sequenceName.ToLower());
                    }
                    string filePath = Path.Combine(currentPath, fileName);

                    if (filePath.EndsWith(".ma", StringComparison.OrdinalIgnoreCase))
                    {
                        CreateMayaScene.CreateBasicMaFile(filePath, Path.GetFileNameWithoutExtension(fileName));
                    }
                    else
                    {
                        File.Create(filePath).Dispose();
                    }
                }
            }

            // Créer les sous-dossiers récursivement
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    if (child.Key == "_files") continue;

                    var childNode = JsonSerializer.Deserialize<AssetNode>(child.Value.GetRawText());
                    CreateNode(Path.Combine(currentPath, child.Key), childNode, assetName);
                }
            }
        }

        private static void CreateAssetJson(string rootPath, string assetPath, AssetStructure structure, string assetName)
        {
            var workfileData = new Dictionary<string, object>();

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var department in workNode.Children)
                {
                    if (department.Key == "_files") continue;

                    var deptNode = JsonSerializer.Deserialize<AssetNode>(department.Value.GetRawText());
                    if (deptNode == null) continue;

                    string deptName = department.Key;
                    string deptUpper = deptName.ToUpper();
                    string deptLower = deptName.ToLower();

                    string workPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName);
                    string incrementalPath = Path.Combine(workPath, "incrementals");
                    string publishPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "dlv");

                    if (deptNode.Files != null)
                    {
                        foreach (var fileTemplate in deptNode.Files)
                        {
                            string workFile = fileTemplate.Replace("{assetnamepipeplaceholder}", assetName.ToLower());
                            string extension = Path.GetExtension(workFile);
                            string baseName = Path.GetFileNameWithoutExtension(workFile);
                            string publishFile = $"{baseName}_OK{extension}";

                            workfileData[workFile] = new
                            {
                                department = deptUpper,
                                status = "not_started",
                                version = "v001",
                                workPath = workPath,
                                publishPath = publishPath,
                                incrementalPath = incrementalPath,
                                workFile = workFile,
                                publishName = publishFile,
                                user = "",
                                lastModified = ""
                            };
                        }
                    }
                }
            }

            var assetJson = new
            {
                workfile = workfileData,
            };

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string jsonString = JsonSerializer.Serialize(assetJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
        }

        private static void CreateShotJson(string rootPath, string assetPath, AssetStructure structure, string assetName)
        {
            Directory.CreateDirectory(assetPath);

            var workfileData = new Dictionary<string, object>();

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var department in workNode.Children)
                {
                    if (department.Key == "_files") continue;

                    var deptNode = JsonSerializer.Deserialize<AssetNode>(department.Value.GetRawText());
                    if (deptNode == null) continue;

                    string deptName = department.Key;
                    string deptUpper = deptName.ToUpper();
                    string deptLower = deptName.ToLower();
                    string sequenceFolder = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(assetPath)));

                    string sequenceName = sequenceFolder.StartsWith("S") ? sequenceFolder.Substring(1) : sequenceFolder;


                    string workPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName);
                    string incrementalPath = Path.Combine(workPath, "incrementals");
                    string publishPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "dlv");

                    if (deptNode.Files != null)
                    {
                        foreach (var fileTemplate in deptNode.Files)
                        {
                            string workFile = fileTemplate
                                .Replace("{assetnamepipeplaceholder}", assetName.ToLower())
                                .Replace("{seqnamepipeplaceholder}", sequenceName.ToLower());
                            string extension = Path.GetExtension(workFile);
                            string baseName = Path.GetFileNameWithoutExtension(workFile);
                            string publishFile = $"{baseName}_OK{extension}";

                            workfileData[workFile] = new
                            {
                                department = deptUpper,
                                status = "not_started",
                                version = "v001",
                                workPath = workPath,
                                publishPath = publishPath,
                                incrementalPath = incrementalPath,
                                workFile = workFile,
                                publishName = publishFile,
                                user = "",
                                lastModified = ""
                            };
                        }
                    }
                }
            }

            var assetJson = new
            {
                workfile = workfileData,
            };

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string jsonString = JsonSerializer.Serialize(assetJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
        }

        private static void CreateSeqJson(string rootPath, string assetPath, AssetStructure structure, string assetName)
        {
            Directory.CreateDirectory(assetPath);

            var workfileData = new Dictionary<string, object>();

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var department in workNode.Children)
                {
                    if (department.Key == "_files") continue;

                    var deptNode = JsonSerializer.Deserialize<AssetNode>(department.Value.GetRawText());
                    if (deptNode == null) continue;

                    string deptName = department.Key;
                    string deptUpper = deptName.ToUpper();
                    string deptLower = deptName.ToLower();
                    string sequenceName = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(assetPath)));

                    string workPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName);
                    string incrementalPath = Path.Combine(workPath, "incrementals");
                    string publishPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "dlv");

                    if (deptNode.Files != null)
                    {
                        foreach (var fileTemplate in deptNode.Files)
                        {
                            string workFile = fileTemplate
                                .Replace("{assetnamepipeplaceholder}", assetName.ToLower())
                                .Replace("{seqnamepipeplaceholder}", sequenceName.ToLower());
                            string extension = Path.GetExtension(workFile);
                            string baseName = Path.GetFileNameWithoutExtension(workFile);
                            string publishFile = $"{baseName}_OK{extension}";

                            workfileData[workFile] = new
                            {
                                department = deptUpper,
                                status = "not_started",
                                version = "v001",
                                workPath = workPath,
                                publishPath = publishPath,
                                incrementalPath = incrementalPath,
                                workFile = workFile,
                                publishName = publishFile,
                                user = "",
                                lastModified = ""
                            };
                        }
                    }
                }
            }

            var assetJson = new
            {
                workfile = workfileData,
            };

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string jsonString = JsonSerializer.Serialize(assetJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
        }

        public static void AddCostume(string assetPath, string costumeName)
        {
            // Charger et mettre à jour le JSON
            string assetJsonPath = Path.Combine(assetPath, "asset.json");
            if (File.Exists(assetJsonPath))
            {
                var jsonText = File.ReadAllText(assetJsonPath);
                var options = new JsonSerializerOptions { WriteIndented = true };
                var docObj = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonText, options);

                var fullJson = JsonDocument.Parse(jsonText);
                var rootObj = fullJson.RootElement;

                List<string> updatedCostumes = new List<string>();
                if (rootObj.TryGetProperty("costumes", out JsonElement currentCostumes) && currentCostumes.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in currentCostumes.EnumerateArray())
                        updatedCostumes.Add(item.ToString());
                }

                updatedCostumes.Add(costumeName);

                // Recréer l'objet JSON complet avec les costumes mis à jour
                var newJson = new Dictionary<string, object>();

                foreach (var prop in rootObj.EnumerateObject())
                {
                    if (prop.Name == "costumes")
                        newJson["costumes"] = updatedCostumes;
                    else
                        newJson[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
                }

                // Écriture dans le fichier
                File.WriteAllText(assetJsonPath, JsonSerializer.Serialize(newJson, options));
            }
        }

        public static Dictionary<string, AssetStructure> LoadAssetStructures(string prodPath)
        {
            string assetStructPath = Path.Combine(prodPath, "Dev", "AssetStructure.json");

            if (!File.Exists(assetStructPath))
            {
                MessageBox.Show($"Fichier AssetStructure.json manquant dans :\n{prodPath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(assetStructPath);
                var rawData = JsonSerializer.Deserialize<Dictionary<string, AssetNode>>(json);

                Dictionary<string, AssetStructure> structures = new();

                foreach (var entry in rawData)
                {
                    structures[entry.Key] = new AssetStructure
                    {
                        Name = entry.Key,
                        Structure = entry.Value.Children.ToDictionary(
                            child => child.Key,
                            child => JsonSerializer.Deserialize<AssetNode>(child.Value.GetRawText())
                        )
                    };
                }

                return structures;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la lecture de AssetStructure.json :\n{ex.Message}");
                return null;
            }
        }
    }
}
