using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

namespace DuckPipe.Core.Manipulator
{
    public class AllPbPath
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public DateTime Modified { get; set; }
    }

    public static class ShotManip
    {
        public static List<AllPbPath> GetAllPlayblats(string path)
        {
            var result = new List<AllPbPath>();

            string playblastFolder = Path.Combine(path, "blast");

            if (!Directory.Exists(playblastFolder))
                return result;

            var validExtensions = new[] { ".mp4", ".mov", ".png", ".jpg" };

            foreach (var file in Directory.GetFiles(playblastFolder))
            {
                if (validExtensions.Contains(Path.GetExtension(file).ToLower()))
                {
                    result.Add(new AllPbPath
                    {
                        Name = Path.GetFileName(file),
                        FullPath = file,
                        Modified = File.GetLastWriteTime(file)
                    });
                }
            }

            return result;
        }
        public static string GenerateThumbnail(string videoPath)
        {
            string videoDir = Path.GetDirectoryName(videoPath)!;
            string videoName = Path.GetFileNameWithoutExtension(videoPath);
            string thumbPath = Path.Combine(videoDir, "thmb", $"{videoName}_thumb.jpg");

            Directory.CreateDirectory(Path.Combine(videoDir, "thmb"));

            if (File.Exists(thumbPath))
                return thumbPath;

            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{videoPath}\" -ss 00:00:01 -vframes 1 \"{thumbPath}\" -y",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(ffmpeg);
            process?.WaitForExit();

            return File.Exists(thumbPath) ? thumbPath : "";
        }
    }
}
