using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DuckPipe.Core.Config;

namespace DuckPipe.Core.Services
{
    internal class ProductionService
    {
        public static string GetUserName()
        {
            return UserConfig.Get().User;
        }

        public static string GetProductionRootPath()
        {
            string envPath = UserConfig.Get().ProdBasePath;

            if (!Directory.Exists(envPath))
            {
                MessageBox.Show($"Le chemin défini dans DUCKPIPE_ROOT est invalide :\n{envPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return envPath;
        }

        public static Dictionary<string, string> LoadStatusIcons(string prodPath)
        {
            Dictionary<string, string> statusIcons = new();
            string configPath = Path.Combine(prodPath, "Dev", "DangerZone", "config.json");

            using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
            if (configDoc.RootElement.TryGetProperty("status", out JsonElement statusElement))
            {
                foreach (var kv in statusElement.EnumerateObject())
                {
                    statusIcons[kv.Name] = kv.Value.GetString() ?? "";
                }
            }

            return statusIcons;
        }

        public static List<string> LoadProdUsers(string prodPath)
        {
            List<string> prodUser = new();
            string configPath = Path.Combine(prodPath, "Dev", "DangerZone", "config.json");

            if (File.Exists(configPath) == true)
            {
                using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (configDoc.RootElement.TryGetProperty("Users", out JsonElement userArray))
                {
                    foreach (var user in userArray.EnumerateArray())
                    {
                        prodUser.Add(user.GetString() ?? "");
                    }
                }
            }
            return prodUser;
        }

        public static List<string> GetProductionList()
        {
            string rootPath = GetProductionRootPath();

            if (!Directory.Exists(rootPath))
                return new List<string>();

            return Directory.GetDirectories(rootPath)
                            .Select(Path.GetFileName)
                            .ToList();
        }

        public static string getConfigJsonPath(string prodPath)
        {
            return Path.Combine(prodPath, "Dev", "DangerZone", "config.json");
        }

        public static ImageList LoadStatusIconsIntoImageList(out Dictionary<string, string> statusIcons, string prodPath)
        {
            statusIcons = ProductionService.LoadStatusIcons(prodPath);

            ImageList statusImageList = new ImageList
            {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit
            };

            foreach (var pair in statusIcons)
            {
                string statusKey = pair.Key;
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pair.Value);

                if (File.Exists(iconPath))
                {
                    statusImageList.Images.Add(statusKey, Image.FromFile(iconPath));
                }
            }

            return statusImageList;
        }

        public static Dictionary<string, Color> GetTaskColorsFromConfig(string prodPath, string Department)
        {
            MessageBox.Show($"Récupération de la couleur pour le département : {Department}");
            // ouvre le config.json et lit l'extension principale pour le département donné
            string configPath = Path.Combine(prodPath, "Dev", "DangerZone", "config.json");

            if (File.Exists(configPath) == true)
            {
                using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (configDoc.RootElement.TryGetProperty("departments", out JsonElement deptElement))
                {
                    foreach (var dept in deptElement.EnumerateObject())
                    {
                        if (dept.Name.Equals(Department, StringComparison.OrdinalIgnoreCase))
                        {
                            if (dept.Value.TryGetProperty("color", out JsonElement extElement))
                            {
                                var r = extElement.GetProperty("R").GetInt32();
                                var g = extElement.GetProperty("G").GetInt32();
                                var b = extElement.GetProperty("B").GetInt32();
                                var a = extElement.GetProperty("A").GetInt32();
                                return new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
                                {
                                    { Department.ToUpper(), Color.FromArgb(a, r, g, b) }
                                };
                            }
                        }
                    }
                }
                return new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

            }
            return new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
        }

        public static string GetFileMainExt(string prodPath, string Department)
        {
            // ouvre le config.json et lit l'extension principale pour le département donné
            string configPath = Path.Combine(prodPath, "Dev", "DangerZone", "config.json");

            if (File.Exists(configPath) == true)
            {
                using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (configDoc.RootElement.TryGetProperty("departments", out JsonElement deptElement))
                {
                    foreach (var dept in deptElement.EnumerateObject())
                    {
                        if (dept.Name.Equals(Department, StringComparison.OrdinalIgnoreCase))
                        {
                            if (dept.Value.TryGetProperty("mainFileExt", out JsonElement extElement))
                            {
                                return extElement.GetString() ?? "";
                            }
                        }
                    }
                }
                return "";

            }
            
            return "";
        }
    }
}
