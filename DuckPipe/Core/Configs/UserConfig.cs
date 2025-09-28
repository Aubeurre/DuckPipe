using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DuckPipe.Core.Config
{
    public class UserConfig
    {
        public string User { get; set; } = Environment.UserName;
        public string ServerBasePath { get; set; } = @"I:\PROD\";
        public string LocalBasePath { get; set; } = @"D:\ICHIGO\";  //Path.Combine(Path.GetTempPath(), "DuckPipe");
        public string MayaLocation { get; set; } = @"C:\Program Files\Autodesk\Maya2023\";
        public string BlenderLocation { get; set; } = @"C:\Program Files\Blender Foundation\Blender 4.4\blender.exe";

        private static UserConfig? _instance;

        public static string GetDefaultConfigPath()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userFolder, ".duckpipe", "user_config.json");
        }

        public static string GetLocalBasePath()
        {
            string localPath = "";
            string configPath = GetDefaultConfigPath();

            if (File.Exists(configPath) == true)
            {
                using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (configDoc.RootElement.TryGetProperty("LocalBasePath", out JsonElement localPathElement))
                {
                    localPath = localPathElement.GetString() ?? "";
                }
            }
            return localPath;
        }

        public static string GetServerBasePath()
        {
            string ServerPath = "";
            string configPath = GetDefaultConfigPath();

            if (File.Exists(configPath) == true)
            {
                using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (configDoc.RootElement.TryGetProperty("ServerBasePath", out JsonElement serverlPathElement))
                {
                    ServerPath = serverlPathElement.GetString() ?? "";
                }
            }
            return ServerPath;
        }

        public static string GetUserName()
        {
            string userName = "";
            string configPath = GetDefaultConfigPath();

            if (File.Exists(configPath) == true)
            {
                using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
                if (configDoc.RootElement.TryGetProperty("User", out JsonElement userNameElement))
                {
                    userName = userNameElement.GetString() ?? "";
                }
            }
            return userName;
        }

        public static void LoadOrCreate()
        {
            string path = GetDefaultConfigPath();
            if (!File.Exists(path))
            {
                var config = new UserConfig();
                config.Save();
                _instance = config;
            }
            else
            {
                try
                {
                    var json = File.ReadAllText(path);
                    _instance = JsonSerializer.Deserialize<UserConfig>(json) ?? new UserConfig();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UserConfig] Erreur de lecture : {ex.Message}");
                    _instance = new UserConfig();
                }
            }
        }


        public void Save()
        {
            string path = GetDefaultConfigPath();
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            File.WriteAllText(path, json);
        }

        public static void OpenConfigFile()
        {
            string path = GetDefaultConfigPath();
            if (!File.Exists(path))
            {
                var config = new UserConfig();
                config.Save();
                _instance = config;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }
    }
}
