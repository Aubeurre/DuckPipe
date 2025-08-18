using System.Diagnostics;
using System.Text.Json;
using DuckPipe.Core.Model;

namespace DuckPipe.Core.Services
{
    public static class AssetService
    {
        public static AssetContext? ExtractAssetContext(string assetPath)
        {
            try
            {
                string rootPath = ProductionService.GetProductionRootPath();
                if (!assetPath.StartsWith(rootPath))
                    return null;

                string relativePath = assetPath.Replace(rootPath, "").Trim('\\');
                string[] parts = relativePath.Split('\\');

                string prodName = parts[0];
                string firstFolder = parts.ElementAtOrDefault(1);

                var ctx = new AssetContext
                {
                    RootPath = rootPath,
                    ProductionName = prodName,
                    ProductionPath = Path.Combine(rootPath, prodName),
                };

                if (firstFolder == "Assets")
                {
                    if (parts.Length == 4)
                    {
                        ctx.AssetType = parts[2];
                        ctx.AssetName = parts[3];
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Assets", ctx.AssetType, ctx.AssetName);
                    }
                    else return null;
                }
                else if (firstFolder == "Preprod")
                {
                    if (parts.Length == 4)
                    {
                        ctx.AssetType = parts[2];
                        ctx.AssetName = parts[3];
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Preprod", ctx.AssetType, ctx.AssetName);
                    }
                    else return null;
                }
                else if (firstFolder == "Shots")
                {
                    if (parts.Length == 4)
                    {
                        ctx.SequenceName = parts[3];
                        ctx.AssetType = "Sequences";
                        ctx.AssetName = $"{parts[3]}";
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Shots", "Sequences", ctx.SequenceName);
                    }
                    else if (parts[4] == "Shots" && parts.Length == 6)
                    {
                        ctx.SequenceName = parts[3];
                        ctx.AssetType = "Shots";
                        ctx.AssetName = $"{parts[3]}_{parts[5]}";
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Shots", "Sequences", ctx.SequenceName, "Shots", ctx.AssetName);
                    }
                    else return null;
                }
                else return null;

                return ctx;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ExtractAssetContext] Erreur : {ex.Message}");
                return null;
            }
        }

        public static void createAsset(string selectedProd, string newItemName, string newItemType, string seqName, string Description, string rangeIn, string rangeOut)
        {
                if (string.IsNullOrEmpty(newItemName))
                {
                    MessageBox.Show("Le nom de l'asset ne peut pas etre vide.");
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
                    string baseAssetFolder = Path.Combine(prodPath, "Assets", newItemType);
                    string assetPath = Path.Combine(baseAssetFolder, newItemName);

                    if (Directory.Exists(assetPath))
                    {
                        MessageBox.Show("Cet asset existe déjà.");
                        return;
                    }

                    // Charger structures
                    var assetStructures = AssetStructureBuilder.LoadAssetStructures(prodPath);
                    if (assetStructures == null || !assetStructures.TryGetValue(newItemType, out var assetStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }

                    // arborescence des fichiers/dossiers
                    AssetStructureBuilder.CreateAssetStructure(rootPath, assetPath, assetStructure, newItemName, Description, rangeIn, rangeOut);

                    MessageBox.Show($"Asset '{newItemName}' ({newItemType}) créé dans :\n{assetPath}");
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
                    var assetStructures = AssetStructureBuilder.LoadAssetStructures(prodPath);
                    if (assetStructures == null || !assetStructures.TryGetValue(newItemType, out var assetStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }
                    AssetStructureBuilder.CreateAssetStructure(rootPath, seqPath, assetStructure, newItemName, Description, rangeIn, rangeOut);

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
                    var assetStructures = AssetStructureBuilder.LoadAssetStructures(prodPath);
                    if (assetStructures == null || !assetStructures.TryGetValue(newItemType, out var assetStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }
                    AssetStructureBuilder.CreateAssetStructure(rootPath, seqPath, assetStructure, newItemName, Description, rangeIn, rangeOut);

                }
        }
    }
}
