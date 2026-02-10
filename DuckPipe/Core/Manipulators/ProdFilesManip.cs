using DuckPipe.Core.Builders;
using DuckPipe.Core.Config;
using DuckPipe.Core.Manipulator;
using DuckPipe.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DuckPipe.Core.Manipulators
{
    public class ProdFilesManip
    {
        #region PATH HELPERS

        // Calcule le chemin local ou serveur correspondant, en gérant "PROD"
        public static string MapPath(string sourcePath, bool toLocal)
        {
            string serverBase = UserConfig.GetServerBasePath().Replace("\\", "/");
            string localBase = UserConfig.GetLocalBasePath().Replace("\\", "/");
            string path = sourcePath.Replace("\\", "/");

            if (toLocal)
            {
                if (!path.StartsWith(serverBase, StringComparison.OrdinalIgnoreCase))
                {
                    serverBase = serverBase.Replace("/PROD", "", StringComparison.OrdinalIgnoreCase);
                    localBase = localBase.Replace("/PROD", "", StringComparison.OrdinalIgnoreCase);
                }
                path = path.Replace(serverBase, localBase);
            }
            else
            {
                if (!path.StartsWith(localBase, StringComparison.OrdinalIgnoreCase))
                {
                    localBase = localBase.Replace("/PROD", "", StringComparison.OrdinalIgnoreCase);
                    serverBase = serverBase.Replace("/PROD", "", StringComparison.OrdinalIgnoreCase);
                }
                path = path.Replace(localBase, serverBase);
            }

            return Path.GetFullPath(path).Replace("\\\\", "\\").Replace("//", "/");
        }

        #endregion

        #region FILE COMPARISON

        private static bool OLDFilesDiffer(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
                return true;

            try
            {
                using (FileStream fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs1.Length != fs2.Length)
                        return true;

                    int b1, b2;
                    do
                    {
                        b1 = fs1.ReadByte();
                        b2 = fs2.ReadByte();
                    } while (b1 == b2 && b1 != -1);

                    return b1 != b2;
                }
            }
            catch
            {
                return true; // si erreur ou verrouillage, considérer différent
            }
        }
        private static bool FilesDiffer(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
                return true;

            try
            {
                var f1 = new FileInfo(file1);
                var f2 = new FileInfo(file2);

                return f1.Length != f2.Length ||
                       f1.LastWriteTimeUtc != f2.LastWriteTimeUtc;
            }
            catch
            {
                return true;
            }
        }

        #endregion

        #region COPY METHODS

        private static void CopyFileSafe(string sourceFile, string destFile)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
                File.Copy(sourceFile, destFile, true);
                Console.WriteLine($"[COPY] {sourceFile} -> {destFile}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[WARN] Impossible de copier : {sourceFile} -> {destFile} ({ex.Message})");
            }
        }

        #endregion

        #region FOLDER SYNC

        public static List<string> SyncFolder(string folderPath, List<string> changedFiles, bool toLocal)
        {

            LogService.EchoInfoLog($" Verification de :{folderPath}"); 
            foreach (string eachFile in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
            {
                string targetFile = MapPath(eachFile, toLocal);

                if (!File.Exists(targetFile) || FilesDiffer(eachFile, targetFile))
                {
                    CopyFileSafe(eachFile, targetFile);
                    changedFiles.Add(targetFile);
                }
            }

            return changedFiles;
        }

        #endregion

        #region PUBLIC METHODS

        // Serveur -> Local
        public static void EnsureLocalProductionFiles(string prodName)
        {
            List<string> changedFiles = new List<string>();
            string serverPath = UserConfig.GetServerBasePath();

            // 1. Dev
            string devPath = Path.Combine(serverPath, prodName, "Dev");
            if (Directory.Exists(devPath))
                changedFiles = SyncFolder(devPath, changedFiles, toLocal: true);

            // 2. Assets Template
            string assetsPath = Path.Combine(serverPath, prodName, "Assets", "Template");
            if (Directory.Exists(assetsPath))
                changedFiles = SyncFolder(assetsPath, changedFiles, toLocal: true);

            // 3. Shots Template
            string shotsPath = Path.Combine(serverPath, prodName, "Shots", "Template");
            if (Directory.Exists(shotsPath))
                changedFiles = SyncFolder(shotsPath, changedFiles, toLocal: true);

            // 4. Shared Tools StudioLib
            string sharedToolsPath = Path.Combine(serverPath.Replace("\\PROD", ""), "SHARED_TOOLS", "StudioLib");
            if (Directory.Exists(sharedToolsPath))
                changedFiles = SyncFolder(sharedToolsPath, changedFiles, toLocal: true);


            // disable ca for now, too long to process with many grabbed nodes puis a quoi ca sert che plus
            //5. for each grabbed in prod, dependencies
            //var prodGrabbed = NodeService.GetAllGrabbedInProd(prodName, UserConfig.GetUserName());
            //foreach (var grabbed in prodGrabbed)
            //{
            //    LogService.EchoLog($"Vérification des dépendances pour le node grabbé : {grabbed}");
            //    // on veut le root du node et pas le workfolder
            //    string nodeRoot = NodeManip.ExtractNodeContext(grabbed).NodeRoot;

            //    foreach (var refPath in NodeManip.GetAllRefs(nodeRoot))
            //    {
            //        LogService.EchoLog($"  - Référence : {refPath}");
            //        if (Directory.Exists(refPath))
            //            changedFiles = ProdFilesManip.SyncFolder(refPath, changedFiles, toLocal: true);
            //    }
            //}

            ReturnChanges(changedFiles);
        }

        // Local -> Serveur
        public static void EnsureServerProductionFiles(string prodName)
        {
            List<string> changedFiles = new List<string>();
            string localPath = UserConfig.GetLocalBasePath();

            // 4. Shared Tools StudioLib
            string sharedToolsPath = Path.Combine(localPath.Replace("\\PROD", ""), "SHARED_TOOLS", "StudioLib");
            if (Directory.Exists(sharedToolsPath))
                changedFiles = SyncFolder(sharedToolsPath, changedFiles, toLocal: false);

            ReturnChanges(changedFiles);
        }

        #endregion

        #region UTILITY

        internal static void ReturnChanges(List<string> changedFiles)
        {
            if (changedFiles.Count == 0)
                LogService.EchoInfoLog("Aucun fichier mis à jour !");
            else
                LogService.EchoInfoLog("Fichiers mis à jour :\n" + string.Join("\n", changedFiles));
        }

        #endregion
    }
}
