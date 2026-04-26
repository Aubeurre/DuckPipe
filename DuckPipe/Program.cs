using System.Diagnostics;
using DuckPipe.Core.Config;
using DuckPipe.Core.Services;

namespace DuckPipe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        public static readonly string CurrentVersion = "2.0.4";

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

                string latestVersion = (await client
                    .GetStringAsync("https://raw.githubusercontent.com/Aubeurre/DuckPipe/master/version.txt"))
                    .Trim();

                int CurrentVersionInt =
                    int.Parse(CurrentVersion.Split('.')[0]) * 10000 +
                    int.Parse(CurrentVersion.Split('.')[1]) * 100 +
                    int.Parse(CurrentVersion.Split('.')[2]);

                int latestVersionInt =
                    int.Parse(latestVersion.Split('.')[0]) * 10000 +
                    int.Parse(latestVersion.Split('.')[1]) * 100 +
                    int.Parse(latestVersion.Split('.')[2]);

                if (latestVersionInt <= CurrentVersionInt)
                    return;

                DialogResult result = MessageBox.Show(
                    $"Une nouvelle version ({latestVersion}) est disponible.\nVoulez-vous l’installer maintenant ?",
                    "Mise à jour DuckPipe",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

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

                if (!File.Exists(tempPath))
                {
                    MessageBox.Show("Erreur lors du téléchargement de la mise à jour.");
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempPath,
                    Arguments = "/VERYSILENT",
                    UseShellExecute = true,
                    Verb = "runas"
                });

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogService.EchoErrorLog($"Erreur update : {ex.Message}");
            }
        }

    }
}
