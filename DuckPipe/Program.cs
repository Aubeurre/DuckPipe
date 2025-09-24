using System.Diagnostics;
using DuckPipe.Core.Config;

namespace DuckPipe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        public static readonly string CurrentVersion = "1.9.1"; //fix

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            UserConfig.LoadOrCreate();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            // check async
            CheckForUpdatesAsync().GetAwaiter().GetResult();

            Application.Run(new AssetManagerForm());
        }


        public static async Task CheckForUpdatesAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();

                string latestVersion = (await client.GetStringAsync("https://raw.githubusercontent.com/Aubeurre/DuckPipe/master/version.txt"))
                                       .Trim();

                if (latestVersion == CurrentVersion)
                    return;

                DialogResult result = MessageBox.Show(
                    $"Une nouvelle version ({latestVersion}) est disponible.\nVoulez-vous l’installer maintenant ?",
                    "Mise à jour disponible",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                    return;

                string tempPath = Path.Combine(Path.GetTempPath(), "DuckPipeSetup.exe");
                string setupUrl = "https://github.com/Aubeurre/DuckPipe/releases/latest/download/DuckPipeSetup.exe";

                using (var response = await client.GetAsync(setupUrl))
                {
                    response.EnsureSuccessStatusCode();
                    await using var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await response.Content.CopyToAsync(fs);
                }
                string exePath = Application.ExecutablePath;

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempPath,
                    Arguments = $"/VERYSILENT /NORESTART /RESTART=\"{exePath}\"",
                    UseShellExecute = true
                });

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification des mises à jour : " + ex.Message,
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
