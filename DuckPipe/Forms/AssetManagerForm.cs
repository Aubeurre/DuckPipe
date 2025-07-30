using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Forms;
using DuckPipe.Core;
// using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WinFormsListView = System.Windows.Forms.ListView;
using static DuckPipe.Core.AssetManip;
using System;

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

            // config
            var assetStructures = AssetStructureBuilder.LoadAssetStructures(ctx.ProductionPath);
            if (assetStructures != null && assetStructures.TryGetValue(ctx.AssetType, out var structure))
            {
                DisplayPipelineDepartments(fullPath);
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
            lbDescription.Text = "";
            btnEditAsset.Visible = false;
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();
        }

        public void RefreshRightPanel(string assetPath)
        {
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();
            DisplayPipelineDepartments(assetPath);
        }

        private void DisplayPipelineDepartments(string assetPath)
        {
            flpPipelineStatus.SuspendLayout();
            flpPipelineStatus.Controls.Clear();
            flpAssetInspect.Controls.Clear();
            flpDeptButton.Controls.Clear();

            string jsonPath = Path.Combine(assetPath, "asset.json");
            string json = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(json);
            string[] parts = jsonPath.Split('\\');

            var ctx = ExtractAssetContext(assetPath);

            // maj labels
            lblAssetName.Text = $"{ctx.AssetName} |";
            lblAssetType.Text = ctx.AssetType;

            // maj bouton edit asset
            btnEditAsset.Visible = true;
            btnEditAsset.Click += (s, e) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = jsonPath,
                    UseShellExecute = true
                });
            };

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
                var departmentPanel = BuildDepartmentPanel(jsonPath, deptName);
                flpPipelineStatus.Controls.Add(departmentPanel);
            }

            if (jsonPath.Contains("\\Shots\\") && parts.Length > 8)
            {
                DisplayPlayBlastPanel(assetPath);
            }
            flpPipelineStatus.ResumeLayout();
            }

        private Panel BuildDepartmentPanel(string jsonPath, string deptName)
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
            AddAllWorkFilesToDepartementPanel(listView, info.Item1, info.Item2);

            return departmentPanel;
        }

        private void AddActionButtons(string assetPath)
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

        private void DisplayCommitsPanel(string assetPath)
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

        private void DisplayPlayBlastPanel(string assetPath)
        {
            flpPipelineStatus.SuspendLayout();

            Panel pbpPnel = new()
            {
                AutoSize = false,
                Size = new Size(flpPipelineStatus.ClientSize.Width - 30, 100),
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

            flpPipelineStatus.Controls.Add(pbpPnel);
            flpPipelineStatus.ResumeLayout();
        }

        private void AddAllWorkFilesToDepartementPanel(ListView listView, string assetJsonPath, string department)
        {
            listView.Items.Clear();

            // asset.json
            JsonDocument doc = AssetManip.LoadAssetJson(assetJsonPath);
            if (!doc.RootElement.TryGetProperty("workfile", out JsonElement workfiles)) return;

            // config.json
            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string configPath = Path.Combine(rootPath, selectedProd, "config.json");

            Dictionary<string, string> statusIcons = new();

            using var configDoc = JsonDocument.Parse(File.ReadAllText(configPath));
            if (configDoc.RootElement.TryGetProperty("status", out JsonElement statusElement))
            {
                foreach (var kv in statusElement.EnumerateObject())
                {
                    statusIcons[kv.Name] = kv.Value.GetString() ?? "";
                }
            }

            // icônes
            ImageList statusImageList = new ImageList();
            statusImageList.ImageSize = new Size(16, 16);

            foreach (var pair in statusIcons)
            {
                string statusKey = pair.Key;
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pair.Value);

                if (File.Exists(iconPath))
                {
                    statusImageList.Images.Add(statusKey, Image.FromFile(iconPath));
                }
            }

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
            DisplayCommitsPanel(assetPath);
            AddActionButtons(assetPath);
        }

        private void userSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserConfig.OpenConfigFile();
        }

        private void assetSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rootPath = GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string path = Path.Combine(rootPath, selectedProd, "Dev", "AssetStructure.json");

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
            string path = Path.Combine(rootPath, selectedProd, "config.json");

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
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

        private void checkFoldersStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rootPath = GetProductionRootPath();
            string ProdName = cbProdList.SelectedItem?.ToString();

            var productionConfig = new ProductionStructureBuilder { name = ProdName };
            productionConfig.Check(ProdName, rootPath);
        }


    }
}