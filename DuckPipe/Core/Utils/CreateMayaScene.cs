using System;
using System.IO;

namespace DuckPipe.Core.Utils
{
    internal class CreateMayaScene
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
    }
}
