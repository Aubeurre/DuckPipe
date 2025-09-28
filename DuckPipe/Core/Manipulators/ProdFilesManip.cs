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


        public static void EnsureLocalProductionFiles(string prodName)
        {
            List<string> ChangedFileListe = new List<string>();

            // files to check (hardcoded for now)
            string serverPath = UserConfig.GetServerBasePath();
            List<string> FileListe = new List<string>();
            // all file from Dev
            if (!Directory.Exists(Path.Combine(serverPath, prodName, "Dev")))
            {
                return;
            }
            foreach (string File in Directory.GetFiles(Path.Combine(serverPath, prodName, "Dev"), "*", SearchOption.AllDirectories))
            {
                FileListe.Add(File);
            }

            foreach (string eachFile in FileListe)
            {
                string localFilePath = eachFile.Replace(UserConfig.GetServerBasePath(), UserConfig.GetLocalBasePath());
                if (!File.Exists(localFilePath))
                {
                    copyProductionFilesToLocal(localFilePath);
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
            ReturnChanges(ChangedFileListe);

        }


        internal static void copyProductionFilesToLocal(string serverFilePath)
        {
            // copy production files from server to local
            string localFilePath = serverFilePath.Replace(UserConfig.GetServerBasePath(), UserConfig.GetLocalBasePath());
            Directory.CreateDirectory(Path.GetDirectoryName(localFilePath)!);
            File.Copy(serverFilePath, localFilePath, true);
        }


        internal static bool checkDifferencesBetweenFiles(string serverFilePath)
        {
            string localFilePath = serverFilePath.Replace(UserConfig.GetServerBasePath(), UserConfig.GetLocalBasePath());
            // compare both item to check if they are different or not

            {
                // Open the two files.
                FileStream fs1;
                FileStream fs2;

                fs1 = new FileStream(localFilePath, FileMode.Open);
                fs2 = new FileStream(serverFilePath, FileMode.Open);

                if (fs1.Length != fs2.Length)
                {
                    fs1.Close();
                    fs2.Close();
                    return true;
                }

                // Read and compare byte
                int file1byte;
                int file2byte;
                do
                {
                    // Read one byte from each file.
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                }
                while ((file1byte == file2byte) && (file1byte != -1));

                // Close the files.
                fs1.Close();
                fs2.Close();
                return ((file1byte - file2byte) != 0);

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
