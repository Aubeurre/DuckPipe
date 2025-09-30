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
        public string LocalBasePath { get; set; } = @"D:\ICHIGO\PROD\";  //Path.Combine(Path.GetTempPath(), "DuckPipe");
        public string MayaLocation { get; set; } = @"C:\Program Files\Autodesk\Maya2025\";
        public string BlenderLocation { get; set; } = @"C:\Program Files\Blender Foundation\Blender 4.5\blender.exe";
        public string PhotoshopLocation { get; set; } = @"None";
        public string Nuke { get; set; } = @"None";
        public string painter { get; set; } = @"None";

        private static UserConfig? _instance;
        private static DateTime _lastWriteTime;

        public static void LoadOrCreate()
        {
            string path = GetDefaultConfigPath();

            if (!File.Exists(path))
            {
                var config = new UserConfig();
                config.Save();
                _instance = config;
                _lastWriteTime = File.GetLastWriteTime(path);
            }
            else
            {
                try
                {
                    var json = File.ReadAllText(path);
                    _instance = JsonSerializer.Deserialize<UserConfig>(json) ?? new UserConfig();
                    _lastWriteTime = File.GetLastWriteTime(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UserConfig] Erreur de lecture : {ex.Message}");
                    _instance = new UserConfig();
                }
            }
        }

        public static void Reload()
        {
            LoadOrCreate();
        }

        public static UserConfig Instance
        {
            get
            {
                string path = GetDefaultConfigPath();
                var currentWriteTime = File.GetLastWriteTime(path);
                if (currentWriteTime != _lastWriteTime)
                {
                    LoadOrCreate();
                }
                return _instance!;
            }
        }

        public static string GetDefaultConfigPath()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userFolder, ".duckpipe", "user_config.json");
        }

        public static string GetLocalBasePath()
        {

            return UserConfig.Instance.LocalBasePath;
        }

        public static string GetServerBasePath()
        {

            return UserConfig.Instance.ServerBasePath;
        }

        public static string GetUserName()
        {

            return UserConfig.Instance.User;
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
            _lastWriteTime = File.GetLastWriteTime(path); // 🔥 Ajout
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
