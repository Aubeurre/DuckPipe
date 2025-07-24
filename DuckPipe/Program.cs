using DuckPipe.Core;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckPipe
{
    internal static class Program
    {
        public static IStorageProvider Storage { get; private set; }

        [STAThread]
        static async Task Main()
        {
            ApplicationConfiguration.Initialize();
            UserConfig.LoadOrCreate();

            Storage = await CreateStorageProviderAsync();

            Application.Run(new AssetManagerForm());

        }

        private static async Task<IStorageProvider> CreateStorageProviderAsync()
        {
            const string nasUrl = "https://100.107.223.82:5001a";
            const string nasUser = "AlexBis";
            const string nasPassword = "Alexine_041016";

            try
            {
                var session = await SynologySession.CreateAsync(nasUrl, nasUser, nasPassword);
                var fileStation = new SynologyFileStation(session);
                return new SynologyStorageProvider(fileStation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connexion au NAS échouée : {ex.Message}\nPassage en mode local.",
                                "Mode local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new LocalStorageProvider();
            }
        }
    }
}
