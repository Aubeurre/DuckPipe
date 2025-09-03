using System.Diagnostics;
using DuckPipe.Core.Config;

namespace DuckPipe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            UserConfig.LoadOrCreate();
            Application.Run(new AssetManagerForm()); 
            CheckForUpdates();
        }
        public static readonly string CurrentVersion = "1.5.0"; //pre
        public static async Task CheckForUpdates()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string latestVersion = await client.GetStringAsync("https://raw.githubusercontent.com/Aubeurre/DuckPipe/master/version.txt");

                if (latestVersion.Trim() != CurrentVersion)
                {
                    DialogResult result = MessageBox.Show(
                        $"Une nouvelle version ({latestVersion}) est disponible. Voulez-vous l’ouvrir sur GitHub ?",
                        "Mise à jour disponible",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://github.com/Aubeurre/DuckPipe/releases",
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de vérification de mise à jour : " + ex.Message);
            }
        }
    }
}