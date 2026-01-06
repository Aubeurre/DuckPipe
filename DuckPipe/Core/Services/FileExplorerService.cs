using System.Diagnostics;
using System.Windows.Forms;

namespace DuckPipe.Core.Services
{
    public static class FileExplorerService
    {
        public static void OpenInExplorer(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path) || File.Exists(path))
            {
                string argument = Directory.Exists(path) ? path : $"/select,\"{path}\"";
                Process.Start("explorer.exe", argument);
            }
            else
            {
                LogService.EchoErrorLog("Le chemin n'existe pas ou plus.");
            }
        }

        public static void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                LogService.EchoErrorLog($"Impossible d'ouvrir : {filePath}");
            }
        }
    }
}
