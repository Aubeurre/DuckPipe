using DuckPipe.Core.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DuckPipe.Core.Services.Softwares
{
    internal class HoudiniService
    {
        public static void CreateBasicHoudiniFile(string filePath)
        {
            string houdiniPath = GetHoudiniPath();
            LogService.EchoLog(houdiniPath);
            // Vérifie d’abord la présence réelle de houdini.exe et de hython.exe
            string hythonPath = Path.Combine(Path.GetDirectoryName(houdiniPath), "hython.exe");
            LogService.EchoLog(hythonPath);
            bool houdiniExists = File.Exists(houdiniPath);
            bool hythonExists = File.Exists(hythonPath);
            LogService.EchoLog(houdiniExists.ToString());

            if (!houdiniExists || !hythonExists)
            {
                // Si aucun exécutable valide trouvé → créer un stub
                string stubPath = Path.ChangeExtension(filePath, ".stub.hipnc");
                File.WriteAllText(stubPath, "// Placeholder pour .hipnc (Houdini non installé)");
                Console.WriteLine($"[DuckPipe] Houdini introuvable → stub créé : {stubPath}");
                return;
            }

            // Si Houdini est bien présent → création réelle du fichier
            string tmpPy = Path.GetTempFileName() + ".py";
            File.WriteAllText(tmpPy, @"
import hou
hou.hipFile.clear(suppress_save_prompt=True)
hou.hipFile.save(r'" + filePath.Replace("\\", "/") + @"')
print('Houdini: Fichier .hipnc créé ->', r'" + filePath.Replace("\\", "/") + @"')
");

            RunHoudiniPython(hythonPath, tmpPy);
            File.Delete(tmpPy);
        }


        public static void ExecuteHoudiniBatchScript(string hipPath, string pyPath, string serverPath)
        {
            string houdiniPath = GetHoudiniPath();
            string hythonPath = Path.Combine(Path.GetDirectoryName(houdiniPath), "hython.exe");

            if (!File.Exists(hythonPath))
            {
                LogService.EchoLog("hython.exe introuvable !");
                return;
            }

            if (!File.Exists(pyPath))
            {
                LogService.EchoLog($"Script Python introuvable : {pyPath}");
                return;
            }

            string args = $"\"{pyPath}\" \"{hipPath}\" \"{serverPath}\"";
            RunHoudiniPython(hythonPath, args);
        }


        private static void RunHoudiniPython(string hythonExe, string args)
        {
            var psi = new ProcessStartInfo
            {
                FileName = hythonExe,
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

            LogService.EchoSuccessLog($"Houdini Batch terminé.\n--- STDOUT ---\n{stdout}");
            if (stderr.Length > 0)
                LogService.EchoErrorLog($"--- STDERR ---\n{stderr}");
        }

        public static string GetHoudiniPath()
        {
            // user_config.json -> HoudiniLocation
            string houdiniPath = UserConfig.Instance.HoudiniLocation
                ?? @"C:\Program Files\Side Effects Software\Houdini 20.5\bin\houdini.exe";

            return houdiniPath;
        }

        public static string PathIntoHoudiniFormat(string path)
        {
            string root = ProductionService.GetProductionRootPath();
            if (path.StartsWith(root))
            {
                path = "$PROD_ROOT/" + path.Substring(root.Length).TrimStart('\\', '/');
            }
            return path.Replace("\\", "/");
        }
    }
}
