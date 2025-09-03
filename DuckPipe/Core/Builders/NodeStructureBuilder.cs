using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using DuckPipe.Core.Builders;
using DuckPipe.Core.Manipulator;
using DuckPipe.Core.Services;
using DuckPipe.Forms.Builder.Tabs;
using static System.Windows.Forms.Design.AxImporter;

namespace DuckPipe.Core
{
    public class NodeTypeDefinition
    {
        public string Name { get; set; }
        public List<string> Folders { get; set; }
        public List<string> Files { get; set; }
    }

    public class NodeStructure
    {
        public string Name { get; set; }

        [JsonPropertyName("structure")]
        public Dictionary<string, NodeNode> Structure { get; set; } = new();
    }

    public class NodeNode
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Children { get; set; }

        [JsonPropertyName("_files")]
        public List<string> Files { get; set; } = new();
    }

    public static class NodeStructureBuilder
    {
        public static void CreateNodeStructure(string rootPath, string nodePath, NodeStructure structure, string nodeName, string Description, string rangeIn, string rangeOut)
        {
            string relativePath = Path.GetRelativePath(rootPath, nodePath);
            string firstFolder = relativePath.Split(Path.DirectorySeparatorChar)[0];
            string prodPath = Path.Combine(rootPath, firstFolder);

            if (structure.Name == "Characters" || structure.Name == "Props" || structure.Name == "Environments")
            {
                Directory.CreateDirectory(nodePath);
                CreateNodeJson(rootPath, nodePath, structure, nodeName, Description);

                foreach (var kvp in structure.Structure)
                {
                    CreateNode(Path.Combine(nodePath, kvp.Key), kvp.Value, nodeName);
                }

                if (structure.Name == "Characters")
                    AddCostume(nodePath, "Body");

                AddNodeIntoGlobalJson(prodPath, nodePath, nodeName, structure.Name);
            }

            if (structure.Name == "Shots")
            {
                string onlyShotName = nodeName.Split('-')[1];
                string onlyShotNum = nodeName.Split('P')[1];
                string cleanPath = Path.Combine(Path.GetDirectoryName(nodePath), onlyShotName);
                Directory.CreateDirectory(cleanPath);
                string jsonPath = CreateShotJson(rootPath, cleanPath, structure, nodeName, Description, rangeIn, rangeOut);

                foreach (var kvp in structure.Structure)
                {
                    CreateNode(Path.Combine(cleanPath, kvp.Key), kvp.Value, nodeName);
                }
                AddNodeIntoShot(jsonPath, "", "Characters");
                AddNodeIntoShot(jsonPath, "", "Props");
                AddNodeIntoShot(jsonPath, "", "Environments");
                AddNodeIntoGlobalJson(prodPath, nodePath, nodeName, structure.Name);
            }

            if (structure.Name == "Sequences")
            {
                string cleanPath = Path.Combine(Path.GetDirectoryName(nodePath), nodeName);
                Directory.CreateDirectory(cleanPath);
                CreateSeqJson(rootPath, cleanPath, structure, nodeName, Description);

                foreach (var kvp in structure.Structure)
                {
                    CreateNode(Path.Combine(cleanPath, kvp.Key), kvp.Value, nodeName);
                }
                AddNodeIntoGlobalJson(prodPath, nodePath, nodeName, structure.Name);
            }

        }

        private static void CreateNode(string currentPath, NodeNode node, string nodeName)
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
                    if (file.Contains("{nodenamepipeplaceholder}"))
                    {
                        fileName = file.Replace("{nodenamepipeplaceholder}", nodeName.ToLower())
                            .Replace("{seqnamepipeplaceholder}", sequenceName.ToLower());
                    }
                    string filePath = Path.Combine(currentPath, fileName);

                    if (filePath.EndsWith(".ma", StringComparison.OrdinalIgnoreCase))
                    {
                        MayaService.CreateBasicMaFile(filePath, Path.GetFileNameWithoutExtension(fileName));
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

                    var childNode = JsonSerializer.Deserialize<NodeNode>(child.Value.GetRawText());
                    CreateNode(Path.Combine(currentPath, child.Key), childNode, nodeName);
                }
            }
        }

        private static void CreateNodeJson(string rootPath, string nodePath, NodeStructure structure, string nodeName, string Description)
        {
            var workfileData = new Dictionary<string, object>();
            var taskData = new Dictionary<string, object>();
            string relativePath = Path.GetRelativePath(rootPath, nodePath);
            string firstFolder = relativePath.Split(Path.DirectorySeparatorChar)[0];
            string prodPath = Path.Combine(rootPath, firstFolder);

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var department in workNode.Children)
                {
                    if (department.Key == "_files") continue;

                    var deptNode = JsonSerializer.Deserialize<NodeNode>(department.Value.GetRawText());
                    if (deptNode == null) continue;

                    string deptName = department.Key;
                    string deptUpper = deptName.ToUpper();
                    string deptLower = deptName.ToLower();

                    string workPath = Path.Combine(NodeManip.SetEnvVariables(nodePath), "Work", deptName);
                    string incrementalPath = Path.Combine(workPath, "incrementals");
                    string publishPath = Path.Combine(NodeManip.SetEnvVariables(nodePath), "dlv");

                    if (deptNode.Files != null)
                    {
                        foreach (var fileTemplate in deptNode.Files)
                        {
                            string workFile = fileTemplate.Replace("{nodenamepipeplaceholder}", nodeName.ToLower());
                            string extension = Path.GetExtension(workFile);
                            string baseName = Path.GetFileNameWithoutExtension(workFile);
                            string publishFile = $"{baseName}_OK{extension}";

                            List<string> refNodes = WorkFileBuilder.CreateBasicReferencesStructure(nodeName, structure.Name, deptUpper, publishPath, prodPath);

                            workfileData[workFile] = new
                            {
                                department = deptUpper,
                                status = "not_started",
                                version = "v000",
                                workPath = workPath,
                                publishPath = publishPath,
                                incrementalPath = incrementalPath,
                                workFile = workFile,
                                publishName = publishFile,
                                lastModified = "",
                                refNodes = refNodes
                            };
                        }

                        taskData[deptUpper] = new
                        {
                            status = "not_started",
                            startDate = DateTime.Today,
                            dueDate = DateTime.Today
                        };
                    }
                }
                taskData["ART"] = new
                {
                    status = "not_started",
                    startDate = DateTime.Today,
                    dueDate = DateTime.Today
                };
            }

            var nodeJson = new
            {
                workfile = workfileData,
                nodeInfos = new
                {
                    status = "not_started",
                    description = Description,
                },
                Tasks = taskData
            };

            string jsonPath = Path.Combine(nodePath, "node.json");
            string jsonString = JsonSerializer.Serialize(nodeJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
        }

        private static string CreateShotJson(string rootPath, string nodePath, NodeStructure structure, string nodeName, string Description, string rangeIn, string rangeOut)
        {
            Directory.CreateDirectory(nodePath);

            var workfileData = new Dictionary<string, object>();
            var taskData = new Dictionary<string, object>();
            string relativePath = Path.GetRelativePath(rootPath, nodePath);
            string firstFolder = relativePath.Split(Path.DirectorySeparatorChar)[0];
            string prodPath = Path.Combine(rootPath, firstFolder);

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var department in workNode.Children)
                {
                    if (department.Key == "_files") continue;

                    var deptNode = JsonSerializer.Deserialize<NodeNode>(department.Value.GetRawText());
                    if (deptNode == null) continue;

                    string deptName = department.Key;
                    string deptUpper = deptName.ToUpper();
                    string deptLower = deptName.ToLower();
                    string sequenceFolder = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(nodePath)));
                    string seqDlvPath = Path.Combine(sequenceFolder, "dlv");

                    string sequenceName = sequenceFolder.StartsWith("S") ? sequenceFolder.Substring(1) : sequenceFolder;


                    string workPath = Path.Combine(NodeManip.SetEnvVariables(nodePath), "Work", deptName);
                    string incrementalPath = Path.Combine(workPath, "incrementals");
                    string publishPath = Path.Combine(NodeManip.SetEnvVariables(nodePath), "dlv");


                    List<string> refNodes = WorkFileBuilder.CreateShotBasicReferencesStructure(nodeName, deptUpper, publishPath, prodPath, seqDlvPath);
                    if (deptNode.Files != null)
                    {
                        foreach (var fileTemplate in deptNode.Files)
                        {
                            string workFile = fileTemplate
                                .Replace("{nodenamepipeplaceholder}", nodeName.ToLower())
                                .Replace("{seqnamepipeplaceholder}", sequenceName.ToLower());
                            string extension = Path.GetExtension(workFile);
                            string baseName = Path.GetFileNameWithoutExtension(workFile);
                            string publishFile = $"{baseName}_OK{extension}";

                            workfileData[workFile] = new
                            {
                                department = deptUpper,
                                status = "not_started",
                                version = "v000",
                                workPath = workPath,
                                publishPath = publishPath,
                                incrementalPath = incrementalPath,
                                workFile = workFile,
                                publishName = publishFile,
                                lastModified = "",
                                refNodes = refNodes
                            };
                        }

                        taskData[deptUpper] = new
                        {
                            status = "not_started",
                            startDate = DateTime.Today,
                            dueDate = DateTime.Today
                        };
                    }
                }
            }

            var nodeJson = new
            {
                workfile = workfileData,
                nodeInfos = new
                {
                    inFrame = rangeIn,
                    outFrame = rangeOut,
                    status = "not_started",
                    description = Description
                },
                Tasks = taskData
            };

            string jsonPath = Path.Combine(nodePath, "node.json");
            string jsonString = JsonSerializer.Serialize(nodeJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
            return jsonPath;
        }

        private static void CreateSeqJson(string rootPath, string nodePath, NodeStructure structure, string nodeName, string Description)
        {
            Directory.CreateDirectory(nodePath);

            var workfileData = new Dictionary<string, object>();
            var taskData = new Dictionary<string, object>();
            string relativePath = Path.GetRelativePath(rootPath, nodePath);
            string firstFolder = relativePath.Split(Path.DirectorySeparatorChar)[0];
            string prodPath = Path.Combine(rootPath, firstFolder);

            if (structure.Structure.TryGetValue("Work", out var workNode) && workNode.Children != null)
            {
                foreach (var department in workNode.Children)
                {
                    if (department.Key == "_files") continue;

                    var deptNode = JsonSerializer.Deserialize<NodeNode>(department.Value.GetRawText());
                    if (deptNode == null) continue;

                    string deptName = department.Key;
                    string deptUpper = deptName.ToUpper();
                    string deptLower = deptName.ToLower();
                    string sequenceName = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(nodePath)));

                    string workPath = Path.Combine(NodeManip.SetEnvVariables(nodePath), "Work", deptName);
                    string incrementalPath = Path.Combine(workPath, "incrementals");
                    string publishPath = Path.Combine(NodeManip.SetEnvVariables(nodePath), "dlv");

                    if (deptNode.Files != null)
                    {
                        foreach (var fileTemplate in deptNode.Files)
                        {
                            string workFile = fileTemplate
                                .Replace("{nodenamepipeplaceholder}", nodeName.ToLower())
                                .Replace("{seqnamepipeplaceholder}", sequenceName.ToLower());
                            string extension = Path.GetExtension(workFile);
                            string baseName = Path.GetFileNameWithoutExtension(workFile);
                            string publishFile = $"{baseName}_OK{extension}";

                            List<string> refNodes = WorkFileBuilder.CreateSequenceBasicReferencesStructure(nodeName, deptUpper, publishPath, prodPath);

                            workfileData[workFile] = new
                            {
                                department = deptUpper,
                                status = "not_started",
                                version = "v000",
                                workPath = workPath,
                                publishPath = publishPath,
                                incrementalPath = incrementalPath,
                                workFile = workFile,
                                publishName = publishFile,
                                lastModified = "",
                                refNodes = refNodes
                            };
                        }

                        taskData[deptUpper] = new
                        {
                            status = "not_started",
                            startDate = DateTime.Today,
                            dueDate = DateTime.Today
                        };
                    }
                }
            }

            var nodeJson = new
            {
                workfile = workfileData,
                nodeInfos = new
                {
                    status = "not_started",
                    description = Description
                },
                Tasks = taskData
            };

            string jsonPath = Path.Combine(nodePath, "node.json");
            string jsonString = JsonSerializer.Serialize(nodeJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, jsonString);
        }

        public static void AddCostume(string nodePath, string costumeName)
        {
            // Charger et mettre à jour le JSON
            string nodeJsonPath = Path.Combine(nodePath, "node.json");
            if (File.Exists(nodeJsonPath))
            {
                var jsonText = File.ReadAllText(nodeJsonPath);
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
                File.WriteAllText(nodeJsonPath, JsonSerializer.Serialize(newJson, options));
            }
        }

        public static void AddNodeIntoShot(string nodeJsonPath, string nodeName, string nodeType)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            Dictionary<string, object> newJson = new();

            // Lire le JSON
            string jsonText = File.ReadAllText(nodeJsonPath);
            var doc = JsonDocument.Parse(jsonText);
            var root = doc.RootElement;

            // Copie
            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name != "nodes")
                {
                    newJson[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
                }
            }

            // Section nodes 
            Dictionary<string, List<string>> nodesDict = new();

            if (root.TryGetProperty("nodes", out var nodesProp) && nodesProp.ValueKind == JsonValueKind.Object)
            {
                foreach (var typeEntry in nodesProp.EnumerateObject())
                {
                    var entries = typeEntry.Value.EnumerateArray()
                                             .Select(v => v.GetString() ?? "")
                                             .ToList();
                    nodesDict[typeEntry.Name] = entries;
                }
            }

            // Ajouter
            nodeType = nodeType.ToLower();
            if (!nodesDict.ContainsKey(nodeType))
                nodesDict[nodeType] = new List<string>();

            if (!nodesDict[nodeType].Contains(nodeName))
                nodesDict[nodeType].Add(nodeName);

            // Reinjecter
            newJson["nodes"] = nodesDict;

            // Sauvegarde
            string newJsonString = JsonSerializer.Serialize(newJson, options);
            File.WriteAllText(nodeJsonPath, newJsonString);
        }

        public static Dictionary<string, NodeStructure> LoadNodeStructures(string prodPath)
        {
            string nodeStructPath = Path.Combine(prodPath, "Dev", "DangerZone", "NodeStructure.json");

            if (!File.Exists(nodeStructPath))
            {
                MessageBox.Show($"Fichier NodeStructure.json manquant dans :\n{prodPath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(nodeStructPath);
                var rawData = JsonSerializer.Deserialize<Dictionary<string, NodeNode>>(json);

                Dictionary<string, NodeStructure> structures = new();

                foreach (var entry in rawData)
                {
                    structures[entry.Key] = new NodeStructure
                    {
                        Name = entry.Key,
                        Structure = entry.Value.Children.ToDictionary(
                            child => child.Key,
                            child => JsonSerializer.Deserialize<NodeNode>(child.Value.GetRawText())
                        )
                    };
                }

                return structures;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la lecture de NodeStructure.json :\n{ex.Message}");
                return null;
            }
        }

        public class NodeInfo
        {
            public string Path { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }
        }

        public static void AddNodeIntoGlobalJson(string prodPath, string nodePath, string nodeName, string nodeType)
        {

            string globaljsonPath = Path.Combine(NodeManip.ReplaceEnvVariables(prodPath), "Dev", "DangerZone", "allNodes.json");

            Dictionary<string, NodeInfo> allNodes;

            if (File.Exists(globaljsonPath))
            {
                string json = File.ReadAllText(globaljsonPath);
                allNodes = JsonSerializer.Deserialize<Dictionary<string, NodeInfo>>(json) ?? new Dictionary<string, NodeInfo>();
            }
            else
            {
                allNodes = new Dictionary<string, NodeInfo>();
            }

            allNodes[nodeName] = new NodeInfo
            {
                Path = nodePath,
                Type = nodeType,
                Status = "NotStarted"
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string newJson = JsonSerializer.Serialize(allNodes, options);
            File.WriteAllText(globaljsonPath, newJson);
        }
    }
}