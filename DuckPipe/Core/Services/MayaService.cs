using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DuckPipe.Core.Services
{
    internal class MayaService
    {
        public static void CreateBasicMaFile(string filePath, string sceneName)
        {
            string[] maLines = new string[]
            {
                "//Maya ASCII 2023 scene",
                "//Name: " + sceneName,
                "//Codeset: UTF-8",
                "requires maya \"2023\";",
                "// End of file"
            };

            File.WriteAllLines(filePath, maLines);
            Console.WriteLine($"Fichier .ma créé : {filePath}");
        }

        public static void AddReference(string maFilePath, string referencePath)
        {
            if (!File.Exists(maFilePath))
            {
                Console.WriteLine($"Le fichier {maFilePath} n'existe pas.");
                return;
            }
            string referenceLine = $"file -r -ns \"ref_{Path.GetFileNameWithoutExtension(referencePath)}\" -type \"mayaAscii\" \"{referencePath}\";";
            using (StreamWriter sw = File.AppendText(maFilePath))
            {
                sw.WriteLine(referenceLine);
            }
            Console.WriteLine($"Référence ajoutée dans {maFilePath} : {referencePath}");
        }

        public static string GetMayaPath()
        {
            // on regarde dans userconfig.json MayaLocation

            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string userconfigpath = Path.Combine(userFolder, ".duckpipe", "user_config.json");
            using var configDoc = JsonDocument.Parse(File.ReadAllText(userconfigpath));
            string mayaPath = configDoc.RootElement.GetProperty("MayaLocation").GetString() ?? @"C:\Program Files\Autodesk\Maya2023\";

            return mayaPath;
        }

        public static void ExecuteMayaPublish(string publishedFilePath, string RootPath, string ProdName, string NodeType, string Department)
        {
            string pyPath = Path.Combine(RootPath, ProdName, "Dev", "Pythons", $"{NodeType}_{Department}_publish.py");
            string pyPathEsc = pyPath.Replace("\\", "/");
            string publishedEsc = publishedFilePath.Replace("\\", "/");
            string pythonInline =
                $"import sys; sys.argv=['']; sys.argv.append(r'{publishedEsc}'); "
              + $"ns={{}}; exec(open(r'{pyPathEsc}').read(), ns); ns['main'](r'{publishedEsc}')"; string args = $"-file \"{publishedFilePath}\" -command \"python(\\\"{pythonInline}\\\")\"";


            string mayabatch = Path.Combine(GetMayaPath(), "bin", "mayabatch.exe"); 

            if (File.Exists(pyPath) && File.Exists(mayabatch))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = mayabatch,
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

                MessageBox.Show($"Maya Batch terminé.\n\n--- STDOUT ---\n{stdout}\n--- STDERR ---\n{stderr}", "Maya Batch");

            }
            else
            {
                MessageBox.Show("mayabatch.exe ou script Python introuvable !");
            }

        }

        public static string PathIntoMayaFormat(string path)
        {
            // Convertit les backslashes en slashes pour Maya
            // converti la racine du path en variable d'environnement
            string root = ProductionService.GetProductionRootPath();
            if (path.StartsWith(root))
            {
                path = "$PROD_ROOT\\" + path.Substring(root.Length);
            }
            return path.Replace("\\", "/");
        }
    }
}
