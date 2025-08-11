using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using DuckPipe.Core;
using DuckPipe.Forms;
using static DuckPipe.Core.AssetManip;
// using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WinFormsListView = System.Windows.Forms.ListView;
// using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DuckPipe
{
    public partial class AssetManagerForm : Form
    {
        private string selectedAssetPath = null!;
        private EventHandler? saveClickHandler;


        #region MAIN PROC
        public AssetManagerForm()
        {
            InitializeComponent();
            LoadProductionList();
            WorkTabClearPanel();
            AssetTabClearPanel();
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
            public string SequenceName { get; set; }
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

                string prodName = parts[0];
                string firstFolder = parts.ElementAtOrDefault(1);

                var ctx = new AssetContext
                {
                    RootPath = rootPath,
                    ProductionName = prodName,
                    ProductionPath = Path.Combine(rootPath, prodName),
                };

                if (firstFolder == "Assets")
                {
                    if (parts.Length == 4)
                    {
                        ctx.AssetType = parts[2];
                        ctx.AssetName = parts[3];
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Assets", ctx.AssetType, ctx.AssetName);
                    }
                    else return null;

                }

                else if (firstFolder == "Preprod")
                {
                    if (parts.Length == 4)
                    {
                        ctx.AssetType = parts[2];
                        ctx.AssetName = parts[3];
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Preprod", ctx.AssetType, ctx.AssetName);
                    }
                    else return null;

                }

                else if (firstFolder == "Shots")
                {
                    if (parts.Length == 4)
                    {

                        ctx.SequenceName = parts[3];
                        ctx.AssetType = "Sequences";
                        ctx.AssetName = $"{parts[3]}";
                        ctx.AssetRoot = Path.Combine(rootPath, prodName, "Shots", "Sequences", ctx.SequenceName);
                    }
                    else if (parts[4] == "Shots")
                    {
                        if (parts.Length == 6)
                        {

                            ctx.SequenceName = parts[3];
                            ctx.AssetType = "Shots";
                            ctx.AssetName = $"{parts[3]}_{parts[5]}";
                            ctx.AssetRoot = Path.Combine(rootPath, prodName, "Shots", "Sequences", ctx.SequenceName, "Shots", ctx.AssetName);
                        }
                        else return null;

                    }
                    else return null;

                }
                else return null;

                return ctx;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ExtractAssetContext] Erreur : {ex.Message}");
                return null;
            }
        }

        public static string GetProductionRootPath()
        {
            string envPath = UserConfig.Get().ProdBasePath;

            if (!Directory.Exists(envPath))
            {
                MessageBox.Show($"Le chemin défini dans DUCKPIPE_ROOT est invalide :\n{envPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return envPath;

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

        private Dictionary<string, string> LoadStatusIcons()
        {
            Dictionary<string, string> statusIcons = new();

            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();

            string configPath = Path.Combine(rootPath, selectedProd, "Dev", "DangerZone", "config.json");

            using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
            if (configDoc.RootElement.TryGetProperty("status", out JsonElement statusElement))
            {
                foreach (var kv in statusElement.EnumerateObject())
                {
                    statusIcons[kv.Name] = kv.Value.GetString() ?? "";
                }
            }

            return statusIcons;
        }

        private List<string> LoadProdUsers()
        {
            List<string> prodUser = new();

            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();

            string configPath = Path.Combine(rootPath, selectedProd, "Dev", "DangerZone", "config.json");

            using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
            if (configDoc.RootElement.TryGetProperty("Users", out JsonElement userArray))
            {
                foreach (var user in userArray.EnumerateArray())
                {
                    prodUser.Add(user.GetString() ?? "");
                }
            }

            return prodUser;
        }

        private ImageList LoadStatusIconsIntoImageList(out Dictionary<string, string> statusIcons)
        {
            statusIcons = LoadStatusIcons();

            ImageList statusImageList = new ImageList
            {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit
            };

            foreach (var pair in statusIcons)
            {
                string statusKey = pair.Key;
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pair.Value);

                if (File.Exists(iconPath))
                {
                    statusImageList.Images.Add(statusKey, Image.FromFile(iconPath));
                }
            }

            return statusImageList;
        }
        #endregion


        #region TAB BTN
        private void btnTab1_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 0;
        }

        private void btnTab2_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 1;
        }

        private void btnTab3_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 2;
            DisplayShelude();
        }

        private void btnTab4_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 3;
            DisplayTimeLogs();
            DisplayStats();
        }
        #endregion


        #region  MENU HEADER
        private void userSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserConfig.OpenConfigFile();
        }

        private void assetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string path = Path.Combine(rootPath, selectedProd, "Dev", "DangerZone", "AssetStructure.json");

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string docPath = Path.Combine(Application.StartupPath, "Docs", "index.html");
            MessageBox.Show(docPath);
            if (File.Exists(docPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = docPath,
                    UseShellExecute = true
                });
            }
        }

        private void prodSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string path = Path.Combine(rootPath, selectedProd, "Dev", "DangerZone", "config.json");

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        private void checkFoldersStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rootPath = GetProductionRootPath();
            string ProdName = cbProdList.SelectedItem?.ToString();

            var productionConfig = new ProductionStructureBuilder { name = ProdName };
            productionConfig.Check(ProdName, rootPath);
        }
        #endregion


        #region  LEFT ZONE / LIST ASSET / PROD SELECTION
        #region  LIST ASSET
        private void tvAssetList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = GetFullPathFromNode(e.Node);

            if (string.IsNullOrEmpty(fullPath) || !Directory.Exists(fullPath))
            {
                WorkTabClearPanel();
                AssetTabClearPanel();
                return;
            }

            var ctx = ExtractAssetContext(fullPath);
            if (ctx == null)
            {
                MessageBox.Show("No ctx");
                WorkTabClearPanel();
                AssetTabClearPanel();
                return;
            }

            if (Path.GetFileName(fullPath).Equals(ctx.AssetType, StringComparison.OrdinalIgnoreCase))
            {
                WorkTabClearPanel();
                AssetTabClearPanel();
                Debug.WriteLine("Sélection d'un groupe d'assets.");
                return;
            }

            // config
            var assetStructures = AssetStructureBuilder.LoadAssetStructures(ctx.ProductionPath);
            if (assetStructures != null && assetStructures.TryGetValue(ctx.AssetType, out var structure))
            {
                WorkTabDisplayDepartments(fullPath);
                AssetTabDisplayDepartments(fullPath);
                WorkTabDisplayHeader(fullPath);
                AssetTabDisplayHeader(fullPath);
            }
            else
            {
                if (fullPath.Contains("\\Concept\\"))
                {
                    AssetTabDisplayHeader(fullPath);
                    flpAssetTask.SuspendLayout();
                    flpAssetTask.Controls.Clear();
                    AssetTabDisplayImagePanel(fullPath);
                    flpAssetTask.ResumeLayout();
                }
            }
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
        { "Shots", 5 },
        { "Renders", 1 },
        { "IO", 100 },
        { "Dev", 100 },
        { "RnD", 100 },
        { "Preprod", 100 },
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
                if (!dir.Contains("Work"))
                {
                    TreeNode node = new TreeNode(Path.GetFileName(dir));
                    node.Tag = dir;
                    parentNode.Nodes.Add(node);

                    AddDirectoriesToTreeWithDepth(dir, node, currentDepth + 1, maxDepth);
                }
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

        private void viewInExplorerToolStripMenuItem_Click(object sender, EventArgs e)

        {
            if (tvAssetList.SelectedNode == null)
                return;

            string selectedPath = tvAssetList.SelectedNode.Tag as string;
            if (string.IsNullOrEmpty(selectedPath))
                return;

            if (Directory.Exists(selectedPath) || File.Exists(selectedPath))
            {
                string argument = Directory.Exists(selectedPath) ? selectedPath : $"/select,\"{selectedPath}\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else
            {
                MessageBox.Show("Le chemin n'existe pas ou plus.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region  PRODLIST / CREATE BUTTONS
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
            using (var form = new CreateAssetPopup(this))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                string newItemName = form.AssetName.Trim();
                string newItemType = form.AssetType;
                string seqName = form.SeqName.Trim();
                string Description = form.Description.Trim();
                string rangeIn = form.rangeIn.Trim();
                string rangeOut = form.rangeOut.Trim();

                if (string.IsNullOrEmpty(newItemName))
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
                if (newItemType == "Props" || newItemType == "Characters" || newItemType == "Environments")
                {
                    string baseAssetFolder = Path.Combine(prodPath, "Assets", newItemType);
                    string assetPath = Path.Combine(baseAssetFolder, newItemName);

                    if (Directory.Exists(assetPath))
                    {
                        MessageBox.Show("Cet asset existe déjà.");
                        return;
                    }

                    // Charger structures
                    var assetStructures = AssetStructureBuilder.LoadAssetStructures(prodPath);
                    if (assetStructures == null || !assetStructures.TryGetValue(newItemType, out var assetStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }

                    // arborescence des fichiers/dossiers
                    AssetStructureBuilder.CreateAssetStructure(rootPath, assetPath, assetStructure, newItemName, Description, rangeIn, rangeOut);

                    MessageBox.Show($"Asset '{newItemName}' ({newItemType}) créé dans :\n{assetPath}");
                }

                if (newItemType == "Sequences")
                {
                    string baseSeqFolder = Path.Combine(prodPath, "Shots", newItemType);
                    string seqPath = Path.Combine(baseSeqFolder, newItemName);

                    if (Directory.Exists(seqPath))
                    {
                        MessageBox.Show("Cette Sequence existe déjà.");
                        return;
                    }

                    // Charger structures
                    var assetStructures = AssetStructureBuilder.LoadAssetStructures(prodPath);
                    if (assetStructures == null || !assetStructures.TryGetValue(newItemType, out var assetStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }
                    AssetStructureBuilder.CreateAssetStructure(rootPath, seqPath, assetStructure, newItemName, Description, rangeIn, rangeOut);

                }

                if (newItemType == "Shots")
                {
                    string baseSeqFolder = Path.Combine(prodPath, "Shots", "Sequences", seqName, newItemType);
                    string seqPath = Path.Combine(baseSeqFolder, newItemName);

                    if (Directory.Exists(seqPath))
                    {
                        MessageBox.Show("Cette Sequence existe déjà.");
                        return;
                    }

                    // Charger structures
                    var assetStructures = AssetStructureBuilder.LoadAssetStructures(prodPath);
                    if (assetStructures == null || !assetStructures.TryGetValue(newItemType, out var assetStructure))
                    {
                        MessageBox.Show($"Structure introuvable pour le type : {newItemType}");
                        return;
                    }
                    AssetStructureBuilder.CreateAssetStructure(rootPath, seqPath, assetStructure, newItemName, Description, rangeIn, rangeOut);

                }

                // Recharger  TreeView
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
        #endregion
        #endregion


        #region  WORK PANNEL
        private void WorkTabClearPanel()
        {
            lblAssetName.Text = "";
            lblAssetType.Text = "";
            lbDescription.Text = "";
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();
        }

        public void WorkTabRefreshPanel(string assetPath)
        {
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();
            WorkTabDisplayHeader(assetPath);
            WorkTabDisplayDepartments(assetPath);
        }

        private void WorkTabDisplayHeader(string assetPath)
        {
            string jsonPath = Path.Combine(assetPath, "asset.json");
            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);
            string[] parts = jsonPath.Split('\\');

            var ctx = ExtractAssetContext(assetPath);

            // maj labels
            lblAssetName.Text = $"{ctx.AssetName} |";
            lblAssetType.Text = ctx.AssetType;

            // maj Description
            if (doc.RootElement.TryGetProperty("assetInfos", out JsonElement assetInfos))
            {
                if (assetInfos.TryGetProperty("description", out JsonElement descriptionElement))
                {
                    if (jsonPath.Contains("\\Shots\\") && parts.Length > 8)
                    {
                        string rangeIn = "";
                        string rangeOut = "";

                        if (assetInfos.TryGetProperty("inFrame", out JsonElement inFrameElement))
                            rangeIn = inFrameElement.GetRawText().Replace("\"", "");

                        if (assetInfos.TryGetProperty("outFrame", out JsonElement outFrameElement))
                            rangeOut = outFrameElement.GetRawText().Replace("\"", "");

                        lbDescription.Text = $"[ {rangeIn} - {rangeOut} ] {descriptionElement.GetString()}";
                    }
                    else
                    {
                        lbDescription.Text = descriptionElement.GetString();
                    }
                }
            }
        }

        private void WorkTabDisplayDepartments(string assetPath)
        {
            flpPipelineStatus.SuspendLayout();
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);
            string[] parts = jsonPath.Split('\\');


            // maj tasks
            if (!doc.RootElement.TryGetProperty("workfile", out JsonElement workfiles))
                return;

            var departmentMap = new Dictionary<string, (string status, string version)>();

            foreach (JsonProperty fileEntry in workfiles.EnumerateObject())
            {
                JsonElement fileData = fileEntry.Value;

                if (!fileData.TryGetProperty("department", out JsonElement deptProp))
                    continue;

                string dept = deptProp.GetString() ?? "Unknown";
                string status = fileData.GetProperty("status").GetString() ?? "not_started";
                string version = fileData.GetProperty("version").GetString() ?? "v001";

                departmentMap[dept] = (status, version);
            }

            foreach (var kvp in departmentMap)
            {
                string deptName = kvp.Key;
                var departmentPanel = WorkTabBuildWorkDepartmentsPanel(jsonPath, deptName);
                flpPipelineStatus.Controls.Add(departmentPanel);
            }
            flpPipelineStatus.ResumeLayout();
        }

        private Panel WorkTabBuildWorkDepartmentsPanel(string jsonPath, string deptName)
        {

            var departmentPanel = new RoundedPanel
            {
                AutoSize = false,
                Size = new Size(flpPipelineStatus.ClientSize.Width - 30, 80),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(8),
                Margin = new Padding(5),
                Location = new Point(20, 20),
                BorderColor = Color.FromArgb(90, 90, 90),
                BorderRadius = 5,
                BorderThickness = 0,
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

            // ListView
            WinFormsListView listView = new WinFormsListView()
            {
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.None,
            };
            listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;

            // Ajoute les colonnes
            listView.Columns.Add("Fichier", flpPipelineStatus.ClientSize.Width - 180);
            listView.Columns.Add("User", 70);
            listView.Columns.Add("Version", 50);

            listView.MouseClick += (s, e) =>
            {
                if (listView.FocusedItem != null)
                {
                    string assetPath = listView.FocusedItem.Tag?.ToString();
                    if (!string.IsNullOrEmpty(assetPath))
                        OnAssetItemSelected(assetPath);
                }
            };

            // Ajout du contexte à .Tag
            listView.Tag = Tuple.Create(jsonPath, deptName);

            // Panel contenant la list
            Panel listContainer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(3),
            };

            listContainer.Controls.Add(listView);
            departmentPanel.Controls.Add(listContainer);

            // Récupère les infos
            var info = (Tuple<string, string>)listView.Tag;
            WorkTabFillDepartementPanel(listView, info.Item1, info.Item2);

            return departmentPanel;
        }

        private void WorkTabAddActionButtons(string assetPath)
        {
            flpDeptButton.Controls.Clear();
            void AddButton(string label, Action<string, AssetManagerForm> onValidSelection, AssetManagerForm form)
            {
                var btn = new Button
                {
                    Text = label,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(105, 105, 105),
                    ForeColor = Color.White,
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.LightGray;

                btn.Click += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(selectedAssetPath))
                    {
                        onValidSelection(selectedAssetPath, form);
                    }
                    else
                    {
                        MessageBox.Show("Aucun asset sélectionné dans l’arborescence.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                flpDeptButton.Controls.Add(btn);
            }

            // on check le lock pour voir quelles actions l'user peut faire.
            // on ajoutera un SuperUser plus tard
            string workFolderPath = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string FileExt = Path.GetExtension(assetPath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            string lockedByUser = "";
            if (!File.Exists(lockFile))
            {
                AddButton("Grab", LockAssetDepartment.TryLockFile, this); //if no lock file
            }
            else
            {
                lockedByUser = File.ReadAllText(lockFile);
                if (lockedByUser == Environment.UserName)
                {
                    AddButton("Ungrab", LockAssetDepartment.UnlockFile, this); //if user is grabbed
                }
            }
            AddButton("Run", AssetManip.LaunchAsset, this);
            if (File.Exists(lockFile))
            {
                // AddButton("Exec", AssetManip.ExecAsset, this); // A voir avec les scripts, pour initialiser la scene
                AddButton("Increment", AssetManip.VersionAsset, this); //if user is grabbed
                AddButton("Publish", AssetManip.PublishAsset, this); //if user is grabbed
            }
            AddButton("Add Note", AssetManip.AddNote, this);
        }

        private void WorkTabDisplayCommitsPanel(string assetPath)
        {
            flpAssetInspect.SuspendLayout();
            flpAssetInspect.Controls.Clear();

            var commits = AssetManip.GetAssetCommits(assetPath);
            commits.Reverse();

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

        private void WorkTabFillDepartementPanel(ListView listView, string assetJsonPath, string department)
        {
            listView.Items.Clear();

            // asset.json
            JsonDocument doc = AssetManip.LoadAssetJson(assetJsonPath);
            if (!doc.RootElement.TryGetProperty("workfile", out JsonElement workfiles)) return;

            // config.json
            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string configPath = Path.Combine(rootPath, selectedProd, "Dev", "DangerZone", "config.json");


            // icônes
            ImageList statusImageList = LoadStatusIconsIntoImageList(out Dictionary<string, string> statusIcons);

            listView.SmallImageList = statusImageList;

            // workfiles
            foreach (JsonProperty fileEntry in workfiles.EnumerateObject())
            {
                JsonElement fileData = fileEntry.Value;

                if (!fileData.TryGetProperty("department", out JsonElement deptProp)) continue;
                string dept = deptProp.GetString() ?? "";
                if (dept != department) continue;

                string workPath = AssetManip.ReplaceEnvVariables(fileData.GetProperty("workPath").GetString());
                string status = fileData.GetProperty("status").GetString() ?? "not_started";
                string version = fileData.GetProperty("version").GetString() ?? "v000";
                string fileName = fileData.GetProperty("workFile").GetString() ?? "";
                string file = Path.Combine(workPath, fileName);

                if (!file.EndsWith(".json") && !file.EndsWith(".lock"))
                {
                    string userLocked = LockAssetDepartment.GetuserLocked(file);
                    if (userLocked != "")
                    {
                        userLocked = $"🔒 {userLocked}";
                    }

                    ListViewItem item = new ListViewItem($" |  {fileName}");
                    item.SubItems.Add(userLocked);
                    item.SubItems.Add(version);

                    if (statusImageList.Images.ContainsKey(status))
                    {
                        item.ImageKey = status;
                    }

                    item.Tag = file;
                    listView.Items.Add(item);
                }
            }
        }

        public void OnAssetItemSelected(string assetPath)
        {
            selectedAssetPath = assetPath as string;
            WorkTabDisplayCommitsPanel(assetPath);
            WorkTabAddActionButtons(assetPath);
        }
        #endregion


        #region  ASSET PANNEL
        private void AssetTabClearPanel()
        {
            lblAssetName2.Text = "";
            lblAssetType2.Text = "";
            lbDescription2.Text = "";
            btnEditAsset.Visible = false;
            flpAssetTask.Controls.Clear();
        }

        private void AssetTabDisplayHeader(string assetPath)
        {

            var ctx = ExtractAssetContext(assetPath);

            // maj labels
            lblAssetName2.Text = $"{ctx.AssetName} |";
            lblAssetType2.Text = ctx.AssetType;

            // maj bouton edit asset
            btnEditAsset.Visible = true;

            // maj Description
            string jsonPath = Path.Combine(assetPath, "asset.json");
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                using var doc = JsonDocument.Parse(json);
                string[] parts = jsonPath.Split('\\');

                if (doc.RootElement.TryGetProperty("assetInfos", out JsonElement assetInfos))
                {
                    if (assetInfos.TryGetProperty("description", out JsonElement descriptionElement))
                    {
                        if (jsonPath.Contains("\\Shots\\") && parts.Length > 8)
                        {
                            string rangeIn = "";
                            string rangeOut = "";

                            if (assetInfos.TryGetProperty("inFrame", out JsonElement inFrameElement))
                                rangeIn = inFrameElement.GetRawText().Replace("\"", "");

                            if (assetInfos.TryGetProperty("outFrame", out JsonElement outFrameElement))
                                rangeOut = outFrameElement.GetRawText().Replace("\"", "");

                            lbDescription2.Text = $"[ {rangeIn} - {rangeOut} ] {descriptionElement.GetString()}";
                        }
                        else
                        {
                            lbDescription2.Text = descriptionElement.GetString();
                        }
                    }
                }
            }
        }

        private void AssetTabDisplayDepartments(string assetPath)
        {
            flpAssetTask.SuspendLayout();
            flpAssetTask.Controls.Clear();

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string[] parts = jsonPath.Split('\\');

            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);


            if (jsonPath.Contains("\\Shots\\") && parts.Length > 8)
            {
                AssetTabDisplayPlayBlastPanel(assetPath);
            }
            MessageBox.Show(assetPath);

            // maj tasks
            if (!doc.RootElement.TryGetProperty("Tasks", out JsonElement tasks))
                // flpAssetTask.ResumeLayout();
                return;

            var tasksMap = new Dictionary<string, (string status, string user, string startDate, string dueDate)>();

            foreach (JsonProperty fileEntry in tasks.EnumerateObject())
            {
                JsonElement fileData = fileEntry.Value;

                string dept = fileEntry.Name;
                string status = fileData.GetProperty("status").GetString() ?? "not_started";
                string user = fileData.GetProperty("user").GetString() ?? "";
                string startDate = fileData.GetProperty("startDate").GetString() ?? "";
                string dueDate = fileData.GetProperty("dueDate").GetString() ?? "";

                tasksMap[dept] = (status, user, startDate, dueDate);
            }

            foreach (var kvp in tasksMap)
            {
                var taskPanel = AssetTabDisplayPanel(jsonPath, kvp);
                flpAssetTask.Controls.Add(taskPanel);
            }

            flpAssetTask.ResumeLayout();

            // on refresh le bouton pour garder flpAssetTask plein
            if (saveClickHandler != null)
                btnEditAsset.Click -= saveClickHandler;
            saveClickHandler = (s, e) => AssetManip.SaveTaskDataFromUI(jsonPath, flpAssetTask);
            btnEditAsset.Click += saveClickHandler;
        }

        private Panel AssetTabDisplayPanel(string assetJsonPath, KeyValuePair<string, (string status, string user, string startDate, string dueDate)> taskData)
        {
            string deptName = taskData.Key;
            var (status, user, startDate, dueDate) = taskData.Value;

            ImageList statusImageList = LoadStatusIconsIntoImageList(out Dictionary<string, string> statusIcons);
            List<String> Users = LoadProdUsers();

            var departmentPanel = new RoundedPanel
            {
                AutoSize = false,
                Size = new Size(flpAssetTask.ClientSize.Width - 30, 80),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(8),
                Margin = new Padding(5),
                BorderColor = Color.FromArgb(90, 90, 90),
                BorderRadius = 5,
                BorderThickness = 0,
            };

            // Main Tablayout
            var MainTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
            };
            MainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            MainTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            MainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            departmentPanel.Controls.Add(MainTable);

            // label département
            Label lblDept = new Label
            {
                Text = $" - {deptName}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
            };

            // BIG Tablayout
            var bigTable = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(60, 60, 60),
                Padding = new Padding(3),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
            };
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            bigTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            bigTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Label User
            Label lblUser = new Label
            {
                Text = "Assigned to:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            // ComboBox de User
            var cbUser = new IconComboBox
            {
                Anchor = AnchorStyles.Left,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Tag = deptName,
                Name = "cbUser",
            };
            cbUser.Items.AddRange(Users.ToArray());
            cbUser.SelectedItem = user;

            // Label In
            Label lblIn = new Label
            {
                Text = "Start Date:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            // TextBox startDate
            DateTimePicker dtpStart = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy", // ou "dd/MM/yyyy"
                Value = DateTime.TryParse(startDate, out var parsedstartDate) ? parsedstartDate : DateTime.Today,
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(60, 60, 60),
                CalendarTitleBackColor = Color.FromArgb(90, 90, 90),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray,
                Tag = deptName,
                Name = "dtpStartDate"
            };
            dtpStart.BackColor = Color.FromArgb(60, 60, 60);
            dtpStart.ForeColor = Color.White;
            dtpStart.Font = new Font("Segoe UI", 9);
            // dtpStart.BorderStyle = BorderStyle.None;

            // Label Out
            Label lblOut = new Label
            {
                Text = "Due Date:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            // TextBox dueDate
            DateTimePicker dtpOut = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy", // ou "dd/MM/yyyy"
                Value = DateTime.TryParse(dueDate, out var parsedOutDate) ? parsedOutDate : DateTime.Today,
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(60, 60, 60),
                CalendarTitleBackColor = Color.FromArgb(90, 90, 90),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray,
                Tag = deptName,
                Name = "dtpDueDate"
            };
            dtpStart.BackColor = Color.FromArgb(60, 60, 60);
            dtpStart.ForeColor = Color.White;
            dtpStart.Font = new Font("Segoe UI", 9);

            // Label Out
            Label lblStatus = new Label
            {
                Text = "Status:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            // ComboBox de statut
            var cbStatus = new IconComboBox
            {
                Anchor = AnchorStyles.Left,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Tag = deptName,
                Name = "cbStatus",
            };

            cbStatus.Items.AddRange(statusIcons.Keys.ToArray());
            cbStatus.IconMap = statusIcons.ToDictionary(kv => kv.Key, kv => Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kv.Value)));
            cbStatus.SelectedItem = status;

            // add into panel
            MainTable.Controls.Add(lblDept, 0, 0);
            MainTable.Controls.Add(bigTable, 1, 0);

            bigTable.Controls.Add(lblUser, 0, 0);
            bigTable.Controls.Add(lblStatus, 3, 0);
            bigTable.Controls.Add(lblIn, 1, 0);
            bigTable.Controls.Add(lblOut, 2, 0);

            bigTable.Controls.Add(cbUser, 0, 1);
            bigTable.Controls.Add(cbStatus, 3, 1);
            bigTable.Controls.Add(dtpStart, 1, 1);
            bigTable.Controls.Add(dtpOut, 2, 1);

            return departmentPanel;
        }

        private void AssetTabDisplayPlayBlastPanel(string assetPath)
        {
            flpAssetTask.SuspendLayout();

            Panel pbpPnel = new()
            {
                AutoSize = false,
                Size = new Size(flpAssetTask.ClientSize.Width - 30, 100),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(5),
                Margin = new Padding(5)
            };

            // Label
            pbpPnel.Controls.Add(new Label
            {
                Text = " - Playblasts",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(5, 5),
                ForeColor = Color.White,
            });

            var playblasts = ShotManip.GetAllPlayblats(assetPath);

            int offsetX = 10;
            foreach (var playblast in playblasts)
            {
                var image = new PictureBox
                {
                    ImageLocation = ShotManip.GenerateThumbnail(playblast.FullPath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 60),
                    Cursor = Cursors.Hand,
                    Tag = playblast.FullPath,
                    Location = new Point(offsetX, 30)
                };

                image.DoubleClick += (s, e) =>
                {
                    MessageBox.Show($"Ouverture de: {Path.GetFileName(playblast.FullPath)}, soyez patient");
                    Process.Start("explorer", $"\"{playblast.FullPath}\"");
                };

                pbpPnel.Controls.Add(image);
                offsetX += 170;
            }

            flpAssetTask.Controls.Add(pbpPnel);
            flpAssetTask.ResumeLayout();
        }

        private void AssetTabDisplayImagePanel(string assetPath)
        {
            flpAssetTask.SuspendLayout();

            Panel pbpPnel = new()
            {
                AutoSize = false,
                Size = new Size(flpAssetTask.ClientSize.Width - 30, 100),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(5),
                Margin = new Padding(5)
            };

            // Label
            pbpPnel.Controls.Add(new Label
            {
                Text = " - Arts",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(5, 5),
                ForeColor = Color.White,
            });

            var images = AssetManip.GetAllImages(assetPath);

            int offsetX = 10;
            foreach (var image in images)
            {
                MessageBox.Show(image.FullPath);
                var thumb = new PictureBox
                {
                    ImageLocation = image.FullPath,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 60),
                    Cursor = Cursors.Hand,
                    Tag = image.FullPath,
                    Location = new Point(offsetX, 30)
                };

                thumb.DoubleClick += (s, e) =>
                {
                    MessageBox.Show($"Ouverture de: {Path.GetFileName(image.FullPath)}, soyez patient");
                    Process.Start("explorer", $"\"{image.FullPath}\"");
                };

                pbpPnel.Controls.Add(thumb);
                offsetX += 170;
            }

            flpAssetTask.Controls.Add(pbpPnel);
            flpAssetTask.ResumeLayout();
        }
        #endregion


        #region SHELUDE
        private Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>> allAssets;
        private Dictionary<string, Color> taskColors;
        private DateTime startingDate;
        private int todayOffset;
        private TableLayoutPanel mainTable;
        private Control timelineHeader;

        void DisplayShelude()
        {
            pnlShelude.Controls.Clear();

            string rootPath = AssetManagerForm.GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedProd)) return;

            string prodPath = Path.Combine(rootPath, selectedProd);
            allAssets = GetAllAssetsInProduction(prodPath);

            using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(prodPath, "Dev", "DangerZone", "config.json")));
            startingDate = DateTime.Parse(doc.RootElement.GetProperty("created").GetString());
            DateTime endDate = DateTime.Parse(doc.RootElement.GetProperty("deliveryDay").GetString());

            int totalDays = (int)(endDate - startingDate).TotalDays;
            todayOffset = (int)(DateTime.Today - startingDate).TotalDays;

            taskColors = GetTaskColorsFromConfig(doc);

            mainTable = CreateMainTable(totalDays);
            timelineHeader = CreateTimelineHeader(startingDate, totalDays);
            mainTable.Controls.Add(timelineHeader, 1, 0);

            AddAssetRows(mainTable, allAssets, taskColors, startingDate, todayOffset, timelineHeader.Width);

            var scrollWrapper = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            scrollWrapper.Controls.Add(mainTable);
            pnlShelude.Controls.Add(scrollWrapper);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mainTable == null) return;

            ClearTableLayout(mainTable);

            AddAssetRows(mainTable, allAssets, taskColors, startingDate, todayOffset, timelineHeader.Width);
        }

        private void ClearTableLayout(TableLayoutPanel tableLayout)
        {
            for (int i = tableLayout.Controls.Count - 1; i >= 0; i--)
            {
                var control = tableLayout.Controls[i];
                var position = tableLayout.GetRow(control);
                if (position > 0)
                {
                    tableLayout.Controls.RemoveAt(i);
                    control.Dispose();
                }
            }

            for (int i = tableLayout.RowStyles.Count - 1; i >= 1; i--)
            {
                tableLayout.RowStyles.RemoveAt(i);
            }

            tableLayout.RowCount = 1;
        }

        private TableLayoutPanel CreateMainTable(int totalDays)
        {
            var mainTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(40, 40, 40),
                Padding = new Padding(0)
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, totalDays * 15));

            var cbFilter = new IconComboBox
            {
                Anchor = AnchorStyles.Left,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Name = "sheludefILTER",
                Tag = mainTable
            };
            List<String> Users = LoadProdUsers();
            cbFilter.Items.AddRange(new string[] { "All", "Characters", "Props", "Environments", "Sequences", "Shots" });
            cbFilter.Items.AddRange(Users.ToArray());
            cbFilter.SelectedIndex = 0;
            mainTable.Controls.Add(cbFilter, 0, 0);
            cbFilter.SelectedIndexChanged += cbFilter_SelectedIndexChanged;

            return mainTable;
        }

        private FlowLayoutPanel CreateTimelineHeader(DateTime startDate, int totalDays)
        {
            var timelineHeader = new FlowLayoutPanel
            {
                Height = 45,
                Width = totalDays * 15,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.FromArgb(40, 40, 40),
            };

            DateTime currentDate = startDate;
            int remainingDays = totalDays;
            int monthIndex = 0;

            while (remainingDays > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                int startDay = (currentDate.Day == 1) ? 1 : currentDate.Day;
                int remainingDaysInMonth = daysInMonth - startDay + 1;
                int daysToDisplay = Math.Min(remainingDaysInMonth, remainingDays);

                Color bgColor = (monthIndex % 2 == 0) ? Color.FromArgb(50, 50, 50) : Color.FromArgb(30, 30, 30);

                var monthPanel = new FlowLayoutPanel
                {
                    Height = 45,
                    Width = daysToDisplay * 15,
                    FlowDirection = FlowDirection.TopDown,
                    Margin = new Padding(0),
                    BackColor = bgColor,
                    WrapContents = false
                };

                monthPanel.Controls.Add(new Label
                {
                    Text = currentDate.ToString("MMMM"),
                    Height = 15,
                    Width = daysToDisplay * 15,
                    Margin = new Padding(0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 7, FontStyle.Bold),
                    ForeColor = Color.White
                });

                var dayPanel = new FlowLayoutPanel
                {
                    Height = 15,
                    Width = daysToDisplay * 15,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(0),
                    WrapContents = false
                };

                for (int d = 0; d < daysToDisplay; d++)
                {
                    int dayNumber = currentDate.Day + d;
                    dayPanel.Controls.Add(new Label
                    {
                        Text = dayNumber.ToString(),
                        Width = 15,
                        Height = 15,
                        Font = new Font("Segoe UI", 6),
                        BackColor = bgColor,
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(0)
                    });
                }

                monthPanel.Controls.Add(dayPanel);
                timelineHeader.Controls.Add(monthPanel);

                remainingDays -= daysToDisplay;
                currentDate = currentDate.AddDays(daysToDisplay);
                monthIndex++;
            }

            return timelineHeader;
        }

        private void AddAssetRows(TableLayoutPanel mainTable, Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>> allAssets, Dictionary<string, Color> taskColors, DateTime startingDate, int todayOffset, int timelineWidth)
        {
            ComboBox cbFilter = mainTable.GetControlFromPosition(0, 0) as ComboBox;
            string selectedValue = cbFilter.SelectedItem?.ToString();

            foreach (var typeEntry in allAssets)
            {
                if (selectedValue == "All" || typeEntry.Key == selectedValue)
                {
                    foreach (var assetEntry in typeEntry.Value)
                    {
                        string assetName = assetEntry.Key;
                        var tasks = assetEntry.Value;

                        mainTable.RowCount += 1;
                        int currentRow = mainTable.RowCount - 1;

                        var lblAsset = new Label
                        {
                            Text = assetName,
                            Width = 150,
                            Height = 15,
                            Font = new Font("Segoe UI", 8),
                            ForeColor = Color.White,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Dock = DockStyle.Top,
                            BackColor = Color.FromArgb(60, 60, 60),
                            Padding = new Padding(0)
                        };

                        mainTable.Controls.Add(lblAsset, 0, currentRow);

                        var taskLine = CreateTaskLine(tasks, taskColors, startingDate, todayOffset, timelineWidth);
                        mainTable.Controls.Add(taskLine, 1, currentRow);
                    }
                }

            }
        }

        private Panel CreateTaskLine(Dictionary<string, TaskData> tasks, Dictionary<string, Color> taskColors, DateTime startingDate, int todayOffset, int width)
        {
            var taskLine = new Panel
            {
                Width = width,
                Height = tasks.Count * 14,
                BackColor = Color.FromArgb(60, 60, 60),
                AutoScroll = true,
                Padding = new Padding(0)
            };

            // Ligne "Today"
            var taskTodayLine = new Panel
            {
                Width = 2,
                Height = taskLine.Height,
                BackColor = Color.Red,
                Location = new Point(todayOffset * 15, 0),
                Margin = new Padding(0)
            };
            taskLine.Controls.Add(taskTodayLine);
            taskTodayLine.BringToFront();

            int taskIndex = 0;
            foreach (var taskEntry in tasks)
            {
                DateTime start = DateTime.Parse(taskEntry.Value.StartDate);
                DateTime end = DateTime.Parse(taskEntry.Value.DueDate);

                int offsetDays = (int)(start - startingDate).TotalDays + 1;
                int durationDays = Math.Max(1, (int)(end - start).TotalDays + 1);

                int leftMargin = offsetDays * 15;
                int blockWidth = durationDays * 15;

                Color blockColor = taskColors.TryGetValue(taskEntry.Key.ToUpper(), out var color)
                    ? color
                    : Color.FromArgb(70, 70, 70);

                var block = new Panel
                {
                    Width = blockWidth,
                    Height = 10,
                    BackColor = blockColor,
                    Margin = new Padding(0),
                    Location = new Point(leftMargin, taskIndex * 14)
                };

                var lblTask = new Label
                {
                    Text = $"{taskEntry.Key}: {taskEntry.Value.User} ({taskEntry.Value.StartDate} → {taskEntry.Value.DueDate})",
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 6),
                };

                block.Controls.Add(lblTask);

                var tooltip = new ToolTip();
                tooltip.SetToolTip(lblTask, lblTask.Text);

                taskLine.Controls.Add(block);

                taskIndex++;
            }

            return taskLine;
        }

        public static Dictionary<string, Color> GetTaskColorsFromConfig(JsonDocument doc)
        {
            var colors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

            if (!doc.RootElement.TryGetProperty("color", out var colorSection))
                return colors;

            foreach (var item in colorSection.EnumerateObject())
            {
                var r = item.Value.GetProperty("R").GetInt32();
                var g = item.Value.GetProperty("G").GetInt32();
                var b = item.Value.GetProperty("B").GetInt32();
                var a = item.Value.GetProperty("A").GetInt32();
                colors[item.Name.ToUpper()] = Color.FromArgb(a, r, g, b);
            }

            return colors;
        }
        #endregion

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        #region STATS TAB / TIMELOGS
        private void btnAddTimelog_Click(object sender, EventArgs e)
        {
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string rootPath = GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, selectedProd);

            var assetsDict = GetAllAssetsInProduction(prodPath);
            var allAssets = assetsDict.SelectMany(typeEntry => typeEntry.Value.Select(assetEntry => $"{typeEntry.Key}/{assetEntry.Key}")).ToList(); //CRADE DE FOU

            using (var form = new TimeLogAddPopup(allAssets))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var log = new TimeLog(
                        assetName: form.AssetName,
                        department: form.Department,
                        artist: Environment.UserName,
                        hours: form.Hours,
                        date: DateTime.Now
                    );

                    TimeLogManager.Add(log, prodPath);
                    MessageBox.Show("TimeLog ajouté !");
                }
            }
        }

        private void EmptyTimeLogs()
        {
            for (int i = tblpnlTimeLogs.RowCount - 1; i > 0; i--)
            {
                for (int j = 0; j < tblpnlTimeLogs.ColumnCount; j++)
                {
                    var control = tblpnlTimeLogs.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        tblpnlTimeLogs.Controls.Remove(control);
                        control.Dispose();
                    }
                }
                tblpnlTimeLogs.RowStyles.RemoveAt(i);
                tblpnlTimeLogs.RowCount--;
            }
        }

        private void DisplayTimeLogs()
        {
            EmptyTimeLogs();

            string selectedProd = cbProdList.SelectedItem?.ToString();
            string rootPath = GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, selectedProd);
            var logs = TimeLogManager.GetAll(prodPath);

            int row = 1;
            foreach (var log in logs)
            {
                tblpnlTimeLogs.RowCount++;
                tblpnlTimeLogs.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                // AssetName
                var lblAsset = new Label
                {
                    Text = log.AssetName,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true
                };
                tblpnlTimeLogs.Controls.Add(lblAsset, 0, row);

                // Department
                var lblDept = new Label
                {
                    Text = log.Department,
                    ForeColor = Color.White,
                    AutoSize = true
                };
                tblpnlTimeLogs.Controls.Add(lblDept, 1, row);

                // Artist
                var lblArtist = new Label
                {
                    Text = log.Artist,
                    ForeColor = Color.White,
                    AutoSize = true
                };
                tblpnlTimeLogs.Controls.Add(lblArtist, 2, row);

                // Hours
                var lblHours = new Label
                {
                    Text = log.Hours.ToString(),
                    ForeColor = Color.White,
                    AutoSize = true
                };
                tblpnlTimeLogs.Controls.Add(lblHours, 3, row);

                // Date
                var lblDate = new Label
                {
                    Text = log.Date.ToString("yyyy-MM-dd"),
                    ForeColor = Color.White,
                    AutoSize = true
                };
                tblpnlTimeLogs.Controls.Add(lblDate, 4, row);

                row++;
            }
        }

        private void DisplayStats()
        {
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string rootPath = GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, selectedProd);
            lblTotalProjectHouresLogged.Text = $"Total Logged Hours: {TimeLogStats.GetTotalHours(prodPath).ToString()}";
            lblTotalProjectAssets.Text = $"Total Assets: {TimeLogStats.GetTotalAssets(prodPath).ToString()}";
            lblTotalProjectShots.Text = $"Total Shots: {TimeLogStats.GetTotalShots(prodPath).ToString()}";
        }
        #endregion

        private void tblpnlTimeLogs_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}