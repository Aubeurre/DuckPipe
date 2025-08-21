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
        public string ProdBasePath { get; set; } = @"I:\PROD\";
        public string userTempFolder { get; set; } = Path.Combine(Path.GetTempPath(), "DuckPipe");

        private static UserConfig? _instance;

        public static string GetDefaultConfigPath()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userFolder, ".duckpipe", "user_config.json");
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

        public static UserConfig Get()
        {
            if (_instance == null)
                LoadOrCreate();
            return _instance!;
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
