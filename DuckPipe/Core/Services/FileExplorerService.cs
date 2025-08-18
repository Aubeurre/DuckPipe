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
                MessageBox.Show("Le chemin n'existe pas ou plus.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show($"Impossible d'ouvrir : {filePath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
