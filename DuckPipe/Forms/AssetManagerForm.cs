using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using DuckPipe.Core;

namespace DuckPipe
{
    public partial class AssetManagerForm : Form
    {
        public AssetManagerForm()
        {
            InitializeComponent();
            LoadProductionList();
            ClearRightPanel();
        }

        private void tvAssetList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = GetFullPathFromNode(e.Node);
            if (string.IsNullOrEmpty(fullPath) || !Directory.Exists(fullPath))
            {
                ClearRightPanel();
                Debug.WriteLine("Sélection invalide 1.");
                return;
            }
            string assetType = DetectAssetType(fullPath);
            if (Path.GetFileName(fullPath) == assetType)
            {
                ClearRightPanel();
                Debug.WriteLine("Sélection d'un groupe d'assets ignorée.");
                return;
            }

            //  MISE À JOUR LABELS
            lblAssetName.Text = Path.GetFileName(fullPath);
            lblAssetType.Text = assetType;

            // Charger la structure depuis le config
            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string configPath = Path.Combine(rootPath, selectedProd, "config.json");

            if (!File.Exists(configPath))
            {
                ClearRightPanel();
                Debug.WriteLine("Fichier config non trouvé.");
                return;
            }

            var assetStructures = LoadAssetStructures(Path.Combine(rootPath, selectedProd));

            if (assetStructures != null && assetStructures.TryGetValue(assetType, out var structure))
            {
                DisplayPipelineDepartments(fullPath);
            }

        }

        public static string GetProductionRootPath()
        {
            string envPath = Environment.GetEnvironmentVariable("DUCKPIPE_ROOT");

            if (string.IsNullOrEmpty(envPath))
            {
                envPath = @"D:\ICHIGO\PROD";
            }

            if (!Directory.Exists(envPath))
            {
                MessageBox.Show($"Le chemin défini dans DUCKPIPE_ROOT est invalide :\n{envPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return envPath;
        }

        private void cbProdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProd = cbProdList.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedProd))
                return;

            string rootPath = GetProductionRootPath();
            LoadTreeViewFromFolder(rootPath, selectedProd);
        }

        private void btCreateAsset_Click(object sender, EventArgs e)
        {
            using (var form = new CreateAssetPopup())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                string assetName = form.AssetName.Trim();
                string assetType = form.AssetType;

                if (string.IsNullOrEmpty(assetName))
                {
                    MessageBox.Show("Le nom de l'asset ne peut pas être vide.");
                    return;
                }

                string selectedProd = cbProdList.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedProd))
                {
                    MessageBox.Show("Aucune production sélectionnée.");
                    return;
                }

                string rootPath = GetProductionRootPath();
                string prodPath = Path.Combine(rootPath, selectedProd);
                string baseAssetFolder = Path.Combine(prodPath, "Assets", assetType);
                string assetPath = Path.Combine(baseAssetFolder, assetName);

                if (Directory.Exists(assetPath))
                {
                    MessageBox.Show("Cet asset existe déjà.");
                    return;
                }

                // Charger les structures
                var assetStructures = LoadAssetStructures(prodPath);
                if (assetStructures == null || !assetStructures.TryGetValue(assetType, out var assetStructure))
                {
                    MessageBox.Show($"Structure introuvable pour le type : {assetType}");
                    return;
                }

                // Créer l’arborescence des fichiers/dossiers
                AssetStructureBuilder.CreateAssetStructure(rootPath, assetPath, assetStructure, assetName);

                MessageBox.Show($"Asset '{assetName}' ({assetType}) créé dans :\n{assetPath}");

                // Recharger le TreeView
                LoadTreeViewFromFolder(rootPath, selectedProd);
            }
        }

        private void btnCreateProduction_Click(object sender, EventArgs e)
        {
            using (var form = new CreateProductionPopup())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string prodName = form.ProductionName;

                    string rootPath = GetProductionRootPath();
                    var productionConfig = new ProductionStructureBuilder { name = prodName };

                    productionConfig.CreateProductionStructure(prodName, rootPath);

                    MessageBox.Show($"Production '{prodName}' créée !");

                    LoadProductionList();
                    cbProdList.SelectedItem = prodName;
                }
            }
        }

        private void LoadProductionList()
        {
            string rootPath = GetProductionRootPath();

            if (!Directory.Exists(rootPath))
                return;

            string[] productions = Directory.GetDirectories(rootPath);

            cbProdList.Items.Clear(); // Vide le ComboBox
            foreach (string prodPath in productions)
            {
                string prodName = Path.GetFileName(prodPath);
                cbProdList.Items.Add(prodName);
            }

            if (cbProdList.Items.Count > 0)
                cbProdList.SelectedIndex = 0; // Sélectionne la 1ère prod par défaut
        }

        private void LoadTreeViewFromFolder(string rootPath, string prodName)
        {
            string prodPath = Path.Combine(rootPath, prodName);

            if (!Directory.Exists(prodPath)) return;

            tvAssetList.Nodes.Clear();

            TreeNode rootNode = new TreeNode(prodName);
            rootNode.Tag = prodPath;
            tvAssetList.Nodes.Add(rootNode);

            // Config de profondeur par dossier
            var folderDepthLimits = new Dictionary<string, int>()
    {
        { "Assets", 3 },
        { "Sequences", 5 },
        { "Renders", 1 },
        { "IO", 100 },
        { "Dev", 100 },
        { "RnD", 100 },
    };

            foreach (string dir in Directory.GetDirectories(prodPath))
            {
                string folderName = Path.GetFileName(dir);
                TreeNode node = new TreeNode(folderName);
                node.Tag = dir;
                rootNode.Nodes.Add(node);

                int maxDepth = 1;
                if (folderDepthLimits.TryGetValue(folderName, out int depth))
                    maxDepth = depth;

                AddDirectoriesToTreeWithDepth(dir, node, 1, maxDepth);
            }

            rootNode.Expand();
        }

        private void AddDirectoriesToTreeWithDepth(string folderPath, TreeNode parentNode, int currentDepth, int maxDepth)
        {
            if (currentDepth >= maxDepth)
                return;

            foreach (string dir in Directory.GetDirectories(folderPath))
            {
                TreeNode node = new TreeNode(Path.GetFileName(dir));
                node.Tag = dir;
                parentNode.Nodes.Add(node);

                AddDirectoriesToTreeWithDepth(dir, node, currentDepth + 1, maxDepth);
            }
        }

        private void contextMenuTree_Opening(object sender, CancelEventArgs e)
        {
            if (tvAssetList.SelectedNode == null)
            {
                e.Cancel = true;
            }
        }

        private void tsmiRename_Click(object sender, EventArgs e)
        {
            // TODO: 
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            // TODO: 
        }

        private string GetFullPathFromNode(TreeNode node)
        {
            // Construire récursivement le chemin complet selon ta structure
            // Exemple simple :
            List<string> parts = new();
            TreeNode current = node;
            while (current != null)
            {
                parts.Insert(0, current.Text);
                current = current.Parent;
            }
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string rootPath = GetProductionRootPath();
            return Path.Combine(rootPath, Path.Combine(parts.ToArray()));

        }

        private string DetectAssetType(string path)
        {
            // Exemple : récupérer le segment après "Assets"
            var parts = path.Split(Path.DirectorySeparatorChar);
            int assetsIndex = Array.IndexOf(parts, "Assets");
            if (assetsIndex >= 0 && assetsIndex + 1 < parts.Length)
                return parts[assetsIndex + 1];
            return "";
        }

        private void ClearRightPanel()
        {
            lblAssetName.Text = "";
            lblAssetType.Text = "";
            flpPipelineStatus.Controls.Clear();
        }

        private Dictionary<string, AssetStructure> LoadAssetStructures(string prodPath)
        {
            string assetStructPath = Path.Combine(prodPath, "Dev", "AssetStructure.json");

            if (!File.Exists(assetStructPath))
            {
                MessageBox.Show($"Fichier AssetStructure.json manquant dans :\n{prodPath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(assetStructPath);
                var rawData = JsonSerializer.Deserialize<Dictionary<string, AssetNode>>(json);

                Dictionary<string, AssetStructure> structures = new();

                foreach (var entry in rawData)
                {
                    structures[entry.Key] = new AssetStructure
                    {
                        Name = entry.Key,
                        Structure = entry.Value.Children.ToDictionary(
                            child => child.Key,
                            child => JsonSerializer.Deserialize<AssetNode>(child.Value.GetRawText())
                        )
                    };
                }

                return structures;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la lecture de AssetStructure.json :\n{ex.Message}");
                return null;
            }
        }

        private void DisplayPipelineDepartments(string assetPath)
        {
            flpPipelineStatus.SuspendLayout();
            flpPipelineStatus.Controls.Clear();

            flpPipelineStatus.AutoScroll = true;
            flpPipelineStatus.FlowDirection = FlowDirection.TopDown;
            flpPipelineStatus.WrapContents = false;
            flpPipelineStatus.Dock = DockStyle.Fill;

            string jsonPath = Path.Combine(assetPath, "asset.json");
            if (!File.Exists(jsonPath))
            {
                Debug.WriteLine("asset.json introuvable pour : " + assetPath);
                return;
            }

            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("departments", out JsonElement departments))
                return;

            foreach (JsonProperty dept in departments.EnumerateObject())
            {
                string deptName = dept.Name;
                string status = dept.Value.GetProperty("status").GetString();
                string version = dept.Value.GetProperty("version").GetString();

                Panel departmentPanel = new Panel
                {
                    Size = new Size(flpPipelineStatus.ClientSize.Width - 30, 60),
                    BackColor = Color.LightGray,
                    Padding = new Padding(3),
                    Margin = new Padding(3)
                };

                // Label département
                Label lblDepartment = new Label
                {
                    Text = $" - {deptName}",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(5, 5)
                };
                departmentPanel.Controls.Add(lblDepartment);

                // Panel boutons (gauche)
                FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    Location = new Point(5, 30),
                    AutoSize = true
                };

                Button btnRun = new Button { Text = "Run" };
                Button btnExec = new Button { Text = "Exec" };
                Button btnIncrement = new Button { Text = "Increment" };
                Button btnPublish = new Button { Text = "Publish" };
                Button btnEdit = new Button { Text = "Edit" };

                btnRun.Click += (s, e) =>
                {
                    var popup = new FileSelectionPopup(jsonPath, deptName);
                    if (popup.ShowDialog() == DialogResult.OK)
                        AssetManip.LaunchAsset(popup.SelectedFilePath);
                };
                btnIncrement.Click += (s, e) =>
                {
                    var popup = new FileSelectionPopup(jsonPath, deptName);
                    if (popup.ShowDialog() == DialogResult.OK)
                        AssetManip.VersionAsset(popup.SelectedFilePath);
                };
                btnPublish.Click += (s, e) =>
                {
                    var popup = new FileSelectionPopup(jsonPath, deptName);
                    if (popup.ShowDialog() == DialogResult.OK)
                        AssetManip.PublishAsset(popup.SelectedFilePath);
                };

                buttonPanel.Controls.AddRange(new Control[]
                {
            btnRun, btnExec, btnIncrement, btnPublish, btnEdit
                });

                departmentPanel.Controls.Add(buttonPanel);

                // Panel statut/version (droite)
                FlowLayoutPanel rightPanel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    Location = new Point(departmentPanel.Width - 100, 5),
                    FlowDirection = FlowDirection.LeftToRight
                };

                Color statusColor = status switch
                {
                    "not_started" => Color.Gray,
                    "outDated" => Color.Orange,
                    "upToDate" => Color.Green,
                    _ => Color.Red
                };

                rightPanel.Controls.Add(new Button
                {
                    Width = 16,
                    Height = 16,
                    BackColor = statusColor,
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false,
                    Margin = new Padding(0, 0, 4, 0)
                });

                rightPanel.Controls.Add(new Label
                {
                    Text = version,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8),
                    TextAlign = ContentAlignment.MiddleLeft
                });

                departmentPanel.Controls.Add(rightPanel);

                flpPipelineStatus.Controls.Add(departmentPanel);
            }

            flpPipelineStatus.ResumeLayout();
        }
    }
}