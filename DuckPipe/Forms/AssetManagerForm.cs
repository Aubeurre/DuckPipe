using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Forms;
using DuckPipe.Core;
using static DuckPipe.Core.AssetManip;

namespace DuckPipe
{
    public partial class AssetManagerForm : Form
    {
        private string selectedAssetPath = null!;
        public AssetManagerForm()
        {
            InitializeComponent();
            LoadProductionList();
            ClearRightPanel();
        }
        public class AssetContext
        {
            public string RootPath { get; set; }
            public string ProductionName { get; set; }
            public string ProductionPath { get; set; }
            public string AssetType { get; set; }
            public string AssetName { get; set; }
            public string? Department { get; set; }
            public string AssetRoot { get; set; }
        }

        public static AssetContext? ExtractAssetContext(string assetPath)
        {
            try
            {
                string rootPath = AssetManagerForm.GetProductionRootPath();
                if (!assetPath.StartsWith(rootPath))
                    return null;

                string relativePath = assetPath.Replace(rootPath, "").Trim('\\');
                string[] parts = relativePath.Split('\\');

                int assetsIndex = Array.IndexOf(parts, "Assets");
                if (assetsIndex < 0 || assetsIndex + 2 >= parts.Length)
                    return null;

                string prodName = parts[0];
                string assetType = parts[assetsIndex + 1];
                string assetName = parts[assetsIndex + 2];

                var ctx = new AssetContext
                {
                    RootPath = rootPath,
                    ProductionName = prodName,
                    ProductionPath = Path.Combine(rootPath, prodName),
                    AssetType = assetType,
                    AssetName = assetName,
                    AssetRoot = Path.Combine(rootPath, prodName, "Assets", assetType, assetName)
                };

                if (parts.Length > assetsIndex + 4)
                    ctx.Department = parts[assetsIndex + 4];

                return ctx;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ExtractAssetContext] Erreur : {ex.Message}");
                return null;
            }
        }

        private void tvAssetList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = GetFullPathFromNode(e.Node);

            if (string.IsNullOrEmpty(fullPath) || !Directory.Exists(fullPath))
            {
                ClearRightPanel();
                return;
            }

            var ctx = ExtractAssetContext(fullPath);
            if (ctx == null)
            {
                ClearRightPanel();
                return;
            }

            if (Path.GetFileName(fullPath).Equals(ctx.AssetType, StringComparison.OrdinalIgnoreCase))
            {
                ClearRightPanel();
                Debug.WriteLine("Sélection d'un groupe d'assets.");
                return;
            }

            // maj labels
            lblAssetName.Text = ctx.AssetName;
            lblAssetType.Text = ctx.AssetType;

            // config
            var assetStructures = LoadAssetStructures(ctx.ProductionPath);
            if (assetStructures != null && assetStructures.TryGetValue(ctx.AssetType, out var structure))
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
            if (!string.IsNullOrEmpty(selectedProd))
            {
                string rootPath = GetProductionRootPath();
                LoadTreeViewFromFolder(rootPath, selectedProd);
            }
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
                    MessageBox.Show("Le nom de l'asset ne peut pas etre vide.");
                    return;
                }

                string selectedProd = cbProdList.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedProd))
                {
                    MessageBox.Show("Aucune production selectionnee.");
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

        private void ClearRightPanel()
        {
            lblAssetName.Text = "";
            lblAssetType.Text = "";
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();
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
            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("departments", out JsonElement departments))
                return;

            foreach (JsonProperty dept in departments.EnumerateObject())
            {
                string deptName = dept.Name;
                string status = dept.Value.GetProperty("status").GetString();
                string version = dept.Value.GetProperty("version").ToString();

                var departmentPanel = BuildDepartmentPanel(jsonPath, deptName, status, version);
                flpPipelineStatus.Controls.Add(departmentPanel);
            }
            AddActionButtons();
            flpPipelineStatus.ResumeLayout();
        }

        private Panel BuildDepartmentPanel(string jsonPath, string deptName, string status, string version)
        {
            Panel departmentPanel = new()
            {
                AutoSize = false,
                Size = new Size(flpPipelineStatus.ClientSize.Width - 30, 80),
                BackColor = Color.FromArgb(80,80,80),
                Padding = new Padding(5),
                Margin = new Padding(5)
            };

            // Label
            departmentPanel.Controls.Add(new Label
            {
                Text = $" - {deptName}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(5, 5),
                ForeColor = Color.White,
            });

            // TreeView
            TreeView tvWorkFiles = new TreeView
            {
                AutoSize = false,
                Width = 500,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Left,
                Tag = new Tuple<string, string>(jsonPath, deptName),
            };
            tvWorkFiles.NodeMouseClick += OnAssetNodeClick;

            Panel treeContainer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(3),
            };

            treeContainer.Controls.Add(tvWorkFiles);
            departmentPanel.Controls.Add(treeContainer);
            var info = (Tuple<string, string>)tvWorkFiles.Tag;
            AddAllWorkFilesToDepartementPanel(tvWorkFiles, info.Item1, info.Item2);

            // Statut version 
            var rightPanel = BuildStatusVersionPanel(status, version);
            rightPanel.Location = new Point(departmentPanel.Width - 100, 5);
            departmentPanel.Controls.Add(rightPanel);

            return departmentPanel;
        }

        private void AddActionButtons()
        {
            void AddButton(string label, Action<string> onValidSelection)
            {
                var btn = new Button 
                { 
                    Text = label, 
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(105, 105, 105),
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.LightGray;
                btn.Click += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(selectedAssetPath))
                    {
                        onValidSelection(selectedAssetPath);
                    }
                    else
                    {
                        MessageBox.Show("Aucun asset sélectionné dans l’arborescence.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                flpDeptButton.Controls.Add(btn);
            }

            AddButton("Run", AssetManip.LaunchAsset);
            AddButton("Exec", AssetManip.ExecAsset);
            AddButton("Increment", AssetManip.VersionAsset);
            AddButton("Publish", AssetManip.PublishAsset);
        }

        private FlowLayoutPanel BuildStatusVersionPanel(string status, string version)
        {
            Color statusColor = status switch
            {
                "not_started" => Color.Gray,
                "outDated" => Color.Orange,
                "upToDate" => Color.Green,
                _ => Color.Red
            };

            var panel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            panel.Controls.Add(new Button
            {
                Width = 16,
                Height = 16,
                BackColor = statusColor,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Margin = new Padding(0, 0, 4, 0)
            });

            panel.Controls.Add(new Label
            {
                Text = version,
                AutoSize = true,
                Font = new Font("Segoe UI", 8),
                TextAlign = ContentAlignment.MiddleLeft
            });

            return panel;
        }

        private void DisplayCommitsPanel(string assetPath)
        {
            flpAssetInspect.SuspendLayout();
            flpAssetInspect.Controls.Clear();

            var commits = AssetManip.GetAssetCommits(assetPath);

            foreach (var commit in commits)
            {
                var panel = new Panel
                {
                    Width = flpAssetInspect.ClientSize.Width - 10,
                    AutoSize = false,
                    BackColor = Color.LightGray,
                    Padding = new Padding(5),
                    Margin = new Padding(3)
                };

                var lblHeader = new Label
                {
                    Text = $"v{commit.Version} - {commit.Timestamp:dd/MM/yyyy HH:mm} \n - {commit.User}",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                var lblMessage = new Label
                {
                    Text = commit.Message,
                    AutoSize = true,
                    MaximumSize = new Size(panel.Width - 10, 0),
                    Font = new Font("Segoe UI", 9)
                };

                panel.Controls.Add(lblHeader);
                panel.Controls.Add(lblMessage);
                lblMessage.Location = new Point(0, lblHeader.Bottom + 4);

                flpAssetInspect.Controls.Add(panel);
            }

            flpAssetInspect.ResumeLayout();
        }

        private void AddAllWorkFilesToDepartementPanel(TreeView tv, string assetJsonPath, string department)
        {
            tv.Nodes.Clear();

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
                        string fileName = Path.GetFileName(file);
                        TreeNode node = new TreeNode(fileName)
                        {
                            ForeColor = Color.White
                        };
                        node.Tag = file;
                        tv.Nodes.Add(node);
                    }
                }
            }
        }

        private void OnAssetNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node?.Tag is string path)
            {
                selectedAssetPath = e.Node.Tag as string;
                MessageBox.Show(selectedAssetPath);
                DisplayCommitsPanel(path);
            }
        }
    }
}