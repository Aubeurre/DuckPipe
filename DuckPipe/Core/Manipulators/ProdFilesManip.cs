using DuckPipe.Core.Config;
using DuckPipe.Core.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core.Manipulators
{
    public class ProdFilesManip
    {
        #region LOCAL METHODS
        internal static void ReturnChanges(List<string> ChangedFileListe)
        {
            if (ChangedFileListe.Count == 0)
                MessageBox.Show("Local Production is Up to date !");
            else;
            MessageBox.Show("Updated files:\n" + string.Join("\n", ChangedFileListe));

        }

        public static List<string> runOnFolder(string folderPath, List<string> ChangedFileListe)
        {

            List<string> FileListe = new List<string>();
            foreach (string File in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                FileListe.Add(File);
            }

            foreach (string eachFile in FileListe)
            {
                string localFilePath = eachFile.Replace(UserConfig.GetServerBasePath(), UserConfig.GetLocalBasePath());

                if (!localFilePath.Contains("\\PROD", StringComparison.OrdinalIgnoreCase))
                {
                    localFilePath = eachFile.Replace(
                        UserConfig.GetServerBasePath().Replace("\\PROD", "", StringComparison.OrdinalIgnoreCase),
                        UserConfig.GetLocalBasePath().Replace("\\PROD", "", StringComparison.OrdinalIgnoreCase)
                    );

                }
                if (!File.Exists(localFilePath))
                {
                    copyProductionFilesToLocal(eachFile);
                    ChangedFileListe.Add(localFilePath);
                }
                else
                {
                    if (checkDifferencesBetweenFiles(eachFile))
                    {
                        copyProductionFilesToLocal(eachFile);
                        ChangedFileListe.Add(localFilePath);
                    }
                }
            }
            return ChangedFileListe;
        }

        public static void EnsureLocalProductionFiles(string prodName)
        {
            List<string> ChangedFileListe = new List<string>();

            string serverPath = UserConfig.GetServerBasePath();

            // === 1. Dev files ===
            string devPath = Path.Combine(serverPath, prodName, "Dev");
            if (Directory.Exists(devPath))
                ChangedFileListe = runOnFolder(devPath, ChangedFileListe);

            // === 2. Asset Template ===
            string assetTemplatePath = Path.Combine(serverPath, prodName, "Assets", "Template");
            if (Directory.Exists(assetTemplatePath))
                ChangedFileListe = runOnFolder(assetTemplatePath, ChangedFileListe);

            // === 3. Shots Template ===
            string shotsTemplatePath = Path.Combine(serverPath, prodName, "Shots", "Template");
            if (Directory.Exists(shotsTemplatePath))
                ChangedFileListe = runOnFolder(shotsTemplatePath, ChangedFileListe);

            // === 4. Shared Tools ===
            string StudioLibPath = Path.Combine(serverPath.Replace("\\PROD", ""), "SHARED_TOOLS");
            if (Directory.Exists(StudioLibPath))
                ChangedFileListe = runOnFolder(StudioLibPath, ChangedFileListe);

            ReturnChanges(ChangedFileListe);
        }


        internal static void copyProductionFilesToLocal(string serverFilePath)
        {
            try
            {
                string localFilePath = serverFilePath.Replace(UserConfig.GetServerBasePath(), UserConfig.GetLocalBasePath());
                if (!localFilePath.Contains("\\PROD", StringComparison.OrdinalIgnoreCase))
                {
                    localFilePath = serverFilePath.Replace(
                        UserConfig.GetServerBasePath().Replace("\\PROD", "", StringComparison.OrdinalIgnoreCase),
                        UserConfig.GetLocalBasePath().Replace("\\PROD", "", StringComparison.OrdinalIgnoreCase)
                    );

                }
                Directory.CreateDirectory(Path.GetDirectoryName(localFilePath)!);
                File.Copy(serverFilePath, localFilePath, true);
                Console.WriteLine($"copie : {serverFilePath} -> {localFilePath}");
            }
            catch (IOException ex)
            {
                // Fichier verrouillé par un autre process
                Console.WriteLine($"[WARN] Impossible de copier : {serverFilePath} ({ex.Message})");
            }
        }


        internal static bool checkDifferencesBetweenFiles(string serverFilePath)
        {
            string localFilePath = serverFilePath.Replace(UserConfig.GetServerBasePath(), UserConfig.GetLocalBasePath());
            if (!localFilePath.Contains("\\PROD", StringComparison.OrdinalIgnoreCase))
            {
                localFilePath = serverFilePath.Replace(
                    UserConfig.GetServerBasePath().Replace("\\PROD", "", StringComparison.OrdinalIgnoreCase),
                    UserConfig.GetLocalBasePath().Replace("\\PROD", "", StringComparison.OrdinalIgnoreCase)
                );

            }

            // Si un des fichiers n'existe pas → considéré différent
            if (!File.Exists(serverFilePath) || !File.Exists(localFilePath))
                return true;

            try
            {
                // Ouvre les fichiers en lecture, avec partage autorisé
                using (FileStream fs1 = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream fs2 = new FileStream(serverFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // Compare taille d'abord
                    if (fs1.Length != fs2.Length)
                        return true;

                    int byte1, byte2;
                    do
                    {
                        byte1 = fs1.ReadByte();
                        byte2 = fs2.ReadByte();
                    }
                    while (byte1 == byte2 && byte1 != -1);

                    return (byte1 != byte2);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[WARN] Fichier verrouillé ou inaccessible : {serverFilePath} ({ex.Message})");
                // On le considère différent pour forcer une recopie plus tard
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erreur inattendue sur {serverFilePath} ({ex.Message})");
                return true;
            }
        }

        #endregion

        #region SERVER METHODS
        public static void EnsureServerProductionFiles(string prodName)
        {
            string serverPath = UserConfig.GetServerBasePath();
            string fullPath = Path.Combine(serverPath, prodName);
            // only create if not exist already
            // from duckpipe to prod
            // not updating existing, to avoid overwriting prod customs
            var productionConfig = new ProductionStructureBuilder { name = prodName };
            productionConfig.CreateDefaultFolders(fullPath);
            productionConfig.CopyTools(fullPath);
            productionConfig.CreateDefaultTemplateScene(fullPath);
        }
        #endregion
    }
}
