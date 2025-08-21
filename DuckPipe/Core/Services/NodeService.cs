using System.Diagnostics;
using System.Text.Json;
using DuckPipe.Core.Model;

namespace DuckPipe.Core.Services
{
    public static class NodeService
    {
        public static NodeContext? ExtractNodeContext(string nodePath)
        {
            try
            {
                string rootPath = ProductionService.GetProductionRootPath();
                if (!nodePath.StartsWith(rootPath))
                    return null;

                string relativePath = nodePath.Replace(rootPath, "").Trim('\\');
                string[] parts = relativePath.Split('\\');

                string prodName = parts[0];
                string firstFolder = parts.ElementAtOrDefault(1);

                var ctx = new NodeContext
                {
                    RootPath = rootPath,
                    ProductionName = prodName,
                    ProductionPath = Path.Combine(rootPath, prodName),
                };

                if (firstFolder == "Assets")
                {
                    if (parts.Length == 4)
                    {
                        ctx.NodeType = parts[2];
                        ctx.NodeName = parts[3];
                        ctx.NodeRoot = Path.Combine(rootPath, prodName, "Assets", ctx.NodeType, ctx.NodeName);
                    }
                    else return null;
                }
                else if (firstFolder == "Preprod")
                {
                    if (parts.Length == 4)
                    {
                        ctx.NodeType = parts[2];
                        ctx.NodeName = parts[3];
                        ctx.NodeRoot = Path.Combine(rootPath, prodName, "Preprod", ctx.NodeType, ctx.NodeName);
                    }
                    else return null;
                }
                else if (firstFolder == "Shots")
                {
                    if (parts.Length == 4)
                    {
                        ctx.SequenceName = parts[3];
                        ctx.NodeType = "Sequences";
                        ctx.NodeName = $"{parts[3]}";
                        ctx.NodeRoot = Path.Combine(rootPath, prodName, "Shots", "Sequences", ctx.SequenceName);
                    }
                    else if (parts[4] == "Shots" && parts.Length == 6)
                    {
                        ctx.SequenceName = parts[3];
                        ctx.NodeType = "Shots";
                        ctx.NodeName = $"{parts[3]}_{parts[5]}";
                        ctx.NodeRoot = Path.Combine(rootPath, prodName, "Shots", "Sequences", ctx.SequenceName, "Shots", ctx.NodeName);
                    }
                    else return null;
                }
                else return null;

                return ctx;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ExtractNodeContext] Erreur : {ex.Message}");
                return null;
            }
        }

        public static void createNode(string selectedProd, string newItemName, string newItemType, string seqName, string Description, string rangeIn, string rangeOut)
        {
                if (string.IsNullOrEmpty(newItemName))
                {
                    MessageBox.Show("Le nom de l'node ne peut pas etre vide.");
                    return;
                }

                if (string.IsNullOrEmpty(selectedProd))
                {
                    MessageBox.Show("Aucune production selectionnee.");
                    return;
                }

                string rootPath = ProductionService.GetProductionRootPath();
                string prodPath = Path.Combine(rootPath, selectedProd);
                if (newItemType == "Props" || newItemType == "Characters" || newItemType == "Environments")
                {
                    string baseNodeFolder = Path.Combine(prodPath, "Assets", newItemType);
                    string nodePath = Path.Combine(baseNodeFolder, newItemName);

                    if (Directory.Exists(nodePath))
                    {
                        MessageBox.Show("Cet node existe déjà.");
                        return;
                    }

                    // Charger structures
                    var nodeStructures = NodeStructureBuilder.LoadNodeStructures(prodPath);
                    if (nodeStructures == null || !nodeStructures.TryGetValue(newItemType, out var nodeStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }

                    // arborescence des fichiers/dossiers
                    NodeStructureBuilder.CreateNodeStructure(rootPath, nodePath, nodeStructure, newItemName, Description, rangeIn, rangeOut);

                    MessageBox.Show($"Node '{newItemName}' ({newItemType}) créé dans :\n{nodePath}");
                }

                if (newItemType == "Sequences")
                {
                    string baseSeqFolder = Path.Combine(prodPath, "Shots", newItemType);
                    string seqPath = Path.Combine(baseSeqFolder, newItemName);

                    if (Directory.Exists(seqPath))
                    {
                        MessageBox.Show("Cette Sequence existe déjà.");
                        return;
                    }

                    // Charger structures
                    var nodeStructures = NodeStructureBuilder.LoadNodeStructures(prodPath);
                    if (nodeStructures == null || !nodeStructures.TryGetValue(newItemType, out var nodeStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }
                    NodeStructureBuilder.CreateNodeStructure(rootPath, seqPath, nodeStructure, newItemName, Description, rangeIn, rangeOut);

                }

                if (newItemType == "Shots")
                {
                    string baseSeqFolder = Path.Combine(prodPath, "Shots", "Sequences", seqName, newItemType);
                    string seqPath = Path.Combine(baseSeqFolder, newItemName);

                    if (Directory.Exists(seqPath))
                    {
                        MessageBox.Show("Cette Sequence existe déjà.");
                        return;
                    }

                    // Charger structures
                    var nodeStructures = NodeStructureBuilder.LoadNodeStructures(prodPath);
                    if (nodeStructures == null || !nodeStructures.TryGetValue(newItemType, out var nodeStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }
                    NodeStructureBuilder.CreateNodeStructure(rootPath, seqPath, nodeStructure, newItemName, Description, rangeIn, rangeOut);

                }
        }

        private static string nodeFile = "allNodes.json";

        public static int CountByType(string prodPath, string type)
        {
            string jsonPath = Path.Combine(prodPath, "Dev", "DangerZone", nodeFile);
            string json = File.ReadAllText(jsonPath);

            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                      .EnumerateObject()
                      .Count(element => element.Value.TryGetProperty("Type", out var t) &&
                                        t.GetString()?.Equals(type, StringComparison.OrdinalIgnoreCase) == true);
        }
    }
}
