using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace DuckPipe.Core.Services.Softwares
{
    internal class BlenderService
    {
        public static void CreateBasicBlendFile(string filePath)
        {
            string blenderPath = GetBlenderPath();

            if (!File.Exists(blenderPath))
            {
                MessageBox.Show("Blender introuvable !");
                return;
            }

            // Blender : lancer en batch et sauvegarder un nouveau fichier
            string args = $"-b -noaudio --python-expr \"import bpy; bpy.ops.wm.save_as_mainfile(filepath=r'{filePath}')\"";

            RunBlender(blenderPath, args);
            Console.WriteLine($"Fichier .blend cree : {filePath}");
        }

        public static void AddReference(string blendFilePath, string assetPath, string dataBlock = "Object")
        {
            string blenderPath = GetBlenderPath();

            if (!File.Exists(blenderPath))
            {
                MessageBox.Show("Blender introuvable !");
                return;
            }

            if (!File.Exists(blendFilePath))
            {
                // on force sa creation
                MessageBox.Show("Le fichier .blend cible est introuvable !");
                CreateBasicBlendFile(blendFilePath);
            }

            if (!File.Exists(assetPath))
            {
                MessageBox.Show("L’asset .blend source est introuvable !");
                return;
            }

            // Script inline Python pour Blender
            string pyCommand =
                "import bpy;" +
                $"bpy.ops.wm.open_mainfile(filepath=r'{blendFilePath}');" +
                $"bpy.ops.wm.append(filename='Cube', directory=r'{assetPath}/Object/');" +
                $"bpy.ops.wm.save_mainfile(filepath=r'{blendFilePath}')";

            string args = $"-b -noaudio --python-expr \"{pyCommand}\"";

            RunBlender(blenderPath, args);
            Console.WriteLine($"Reference ajoutee dans {blendFilePath} depuis {assetPath}");
        }

        public static string GetBlenderPath()
        {
            // userconfig.json BlenderLocation
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string userconfigpath = Path.Combine(userFolder, ".duckpipe", "user_config.json");

            using var configDoc = JsonDocument.Parse(File.ReadAllText(userconfigpath));
            string blenderPath = configDoc.RootElement.GetProperty("BlenderLocation").GetString() ?? @"C:\Program Files\Blender Foundation\Blender 4.4\blender.exe";

            return blenderPath;
        }

        public static void ExecuteBlenderBatchScript(string blendPath, string pyPath)
        {
            string blenderPath = GetBlenderPath();

            if (!File.Exists(blenderPath) || !File.Exists(pyPath))
            {
                MessageBox.Show("blender.exe ou script Python introuvable !");
                return;
            }

            string args = $"-b \"{blendPath}\" -P \"{pyPath}\"";

            RunBlender(blenderPath, args);
        }

        private static void RunBlender(string blenderExe, string args)
        {
            var psi = new ProcessStartInfo
            {
                FileName = blenderExe,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            using (var p = new Process { StartInfo = psi })
            {
                p.OutputDataReceived += (s, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
                p.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
            }

           MessageBox.Show($"Blender Batch termine.\n\n--- STDOUT ---\n{stdout}\nArguments:{args}\n--- STDERR ---\n{stderr}", "Blender Batch");
        }

        public static string PathIntoBlenderFormat(string path)
        {
            // pas de variable d'env dans blender, on va devoir faire manuellement...
            string root = ProductionService.GetProductionRootPath();
            if (path.StartsWith(root))
            {
                path = "$PROD_ROOT\\" + path.Substring(root.Length);
            }
            return path.Replace("\\", "/");
        }
    }
}
