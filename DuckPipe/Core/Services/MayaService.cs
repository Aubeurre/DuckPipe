using System;
using System.IO;

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
