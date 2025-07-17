
using System.Diagnostics;
using System.Text.Json;
using DuckPipe.Core;

namespace DuckPipe
{
    public partial class FileSelectionPopup : Form
    {
        private string assetJsonPath;
        private string department;

        public FileSelectionPopup(string assetJsonPath, string department)
        {
            InitializeComponent();
            this.assetJsonPath = assetJsonPath;
            this.department = department;

            LoadFileOptions();
        }
        public string SelectedFilePath { get; private set; }

        private void LoadFileOptions()
        {
            JsonDocument doc = AssetManip.LoadAssetJson(assetJsonPath);

            if (!doc.RootElement.TryGetProperty("departments", out JsonElement departments)) return;
            if (!departments.TryGetProperty(department, out JsonElement dept)) return;

            string workPath = AssetManip.ReplaceEnvVariables(dept.GetProperty("workPath").GetString());

            if (Directory.Exists(workPath))
            {
                string[] files = Directory.GetFiles(workPath);
                foreach (var file in files)
                {
                    if (!file.EndsWith(".json"))
                    {
                        listBoxFiles.Items.Add(Path.GetFileName(file));
                        lbDepartementName.Text = workPath;
                    }
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un fichier.");
            }

            string selectedFile = listBoxFiles.SelectedItem.ToString();

            JsonDocument doc = AssetManip.LoadAssetJson(assetJsonPath);
            string workPath = lbDepartementName.Text;

            string fullPath = Path.Combine(workPath, selectedFile);

            string software = doc.RootElement
                .GetProperty("departments")
                .GetProperty(department)
                .GetProperty("software")
                .GetString();
            Debug.WriteLine($"filePath = {fullPath}");

            if (File.Exists(fullPath))
            {
                SelectedFilePath = Path.Combine(workPath, selectedFile);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Le fichier n'existe pas.");
            }
        }

        private void FileSelectionPopup_Load(object sender, EventArgs e)
        {

        }

    }
}