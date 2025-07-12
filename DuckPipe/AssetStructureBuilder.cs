using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Windows.Forms.Design.AxImporter;

namespace DuckPipe
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
            if (structure.Name == "Props")
                CreateAssetPropsJson(rootPath, assetPath, structure, assetName);
            else if (structure.Name == "Characters")
                CreateAssetCharactersJson(rootPath, assetPath, structure, assetName);
                AddCostume(assetPath, "Body");

            foreach (var kvp in structure.Structure)
            {
                CreateNode(Path.Combine(assetPath, kvp.Key), kvp.Value, assetName);
            }
        }

        private static void CreateNode(string currentPath, AssetNode node, string assetName)
        {
            Directory.CreateDirectory(currentPath);

            // Créer les fichiers du nœud
            if (node.Files != null)
            {
                foreach (var file in node.Files)
                {
                    string fileName = file;
                    if (file.Contains("assetnamepipeplaceholder"))
                    {
                        fileName = file.Replace("assetnamepipeplaceholder", assetName.ToLower());
                    }
                    string filePath = Path.Combine(currentPath, fileName);
                    File.Create(filePath).Dispose();
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

        private static void CreateAssetCharactersJson(string rootPath, string assetPath, AssetStructure structure, string assetName)
        {
            Directory.CreateDirectory(assetPath);

            var departmentsData = new Dictionary<string, object>();

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var child in workNode.Children)
                {
                    if (child.Key == "_files") continue;

                    string deptName = child.Key;

                    string incrementalPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName, "incrementals");
                    string workPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName);
                    string publishPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "dlv");

                    string baseFileName = $"{assetName}_{deptName.ToLower()}";

                    departmentsData[deptName] = new
                    {
                        status = "not_started",
                        version = "v001",
                        workPath = workPath,
                        incrementalPath = incrementalPath,
                        workFile = baseFileName + ".ma",
                        publishName = baseFileName + "_pub.ma",
                        publishPath = publishPath,
                        software = "unknown",
                        user = "",
                        lastModified = ""
                    };
                }
            }

            var assetJson = new
            {
                name = assetName,
                type = structure.Name,
                created = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                status = "wip",
                version = 1,
                departments = departmentsData,
                costumes = new List<object>()
            };

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string jsonString = JsonSerializer.Serialize(assetJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
        }

        private static void CreateAssetPropsJson(string rootPath, string assetPath, AssetStructure structure, string assetName)
        {
            Directory.CreateDirectory(assetPath);

            var departmentsData = new Dictionary<string, object>();

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var child in workNode.Children)
                {
                    if (child.Key == "_files") continue;

                    string deptName = child.Key;

                    string incrementalPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName, "incrementals");
                    string workPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "Work", deptName);
                    string publishPath = Path.Combine(assetPath.Replace(rootPath, "${DUCKPIPE_ROOT}"), "dlv");

                    string baseFileName = $"{assetName}_{deptName.ToLower()}";

                    departmentsData[deptName] = new
                    {
                        status = "not_started",
                        version = "v001",
                        workPath = workPath,
                        incrementalPath = incrementalPath,
                        workFile = baseFileName + ".ma",
                        publishName = baseFileName + "_pub.ma",
                        publishPath = publishPath,
                        software = "unknown",
                        user = "",
                        lastModified = ""
                    };
                }
            }

            var assetJson = new
            {
                name = assetName,
                type = structure.Name,
                created = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                status = "wip",
                version = 1,
                departments = departmentsData,
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
    }
}
