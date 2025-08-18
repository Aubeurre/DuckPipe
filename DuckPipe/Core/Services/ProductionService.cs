using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DuckPipe.Core.Model;

namespace DuckPipe.Core.Services
{
    internal class ProductionService
    {

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

            using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
            if (configDoc.RootElement.TryGetProperty("Users", out JsonElement userArray))
            {
                foreach (var user in userArray.EnumerateArray())
                {
                    prodUser.Add(user.GetString() ?? "");
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

    }
}
