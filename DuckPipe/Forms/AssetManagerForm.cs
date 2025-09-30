using System.ComponentModel;
using System.Diagnostics;
using DuckPipe.Core;
using DuckPipe.Forms;
using static DuckPipe.Core.Manipulator.NodeManip;
// using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WinFormsListView = System.Windows.Forms.ListView;
// using static System.Runtime.InteropServices.JavaScript.JSType;
using DuckPipe.Core.Services;
using DuckPipe.Core.Utils;
using DuckPipe.Forms.Builder.NodeTab;
using DuckPipe.Forms.Builder.Shared;
using DuckPipe.Forms.Builder.Tabs;
using DuckPipe.Core.Config;
using DuckPipe.Core.Builders;
using DuckPipe.Core.Manipulators;

namespace DuckPipe
{
    public partial class AssetManagerForm : Form
    {
        private string selectedNodePath = null!;

        #region MAIN PROC  - CLEANED -
        public AssetManagerForm()
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            LoadProductionList();
            WorkTabBuilder.InitWorkTab(flpPipelineStatus, flpNodeInspect, flpDeptButton, lblNodeName, lblNodeType, lbDescription, this);
            NodeTabBuilder.InitNodeTab(flpNodeTask, lblNodeName2, lblNodeType2, lbDescription2, cbbNodeStatus, btnEditNode, this);
            StatsChartTabBuilder.InitStatsTab(tblpnlTimeLogs, flpAllDeptTimeLogsGraphs, cbbGraphList, lblTotalProjectHouresLogged, lblTotalProjectNodes, lblTotalProjectShots, this);
            ScheduleTabBuilder.InitTimelineTab(pnlShelude, this);
            WorkTabBuilder.ClearPanel(WorkTabBuilder.GetContext("", ""));
            NodeTabBuilder.ClearPanel(NodeTabBuilder.GetContext("", ""));

            this.Text = $"DuckPipe v{Program.CurrentVersion} . Running on {ProductionService.GetProductionRootPath()}";
        }

        private void LoadProductionList()
        {
            cbProdList.Items.Clear();

            var productions = ProductionService.GetProductionList();
            foreach (string prodName in productions)
            {
                // on check si le userName contenu dans userconfig est aussi dans la config de la prod
                string UserName = ProductionService.GetUserName();
                string rootPath = ProductionService.GetProductionRootPath();
                string prodPath = Path.Combine(rootPath, prodName);
                var prodUsers = ProductionService.LoadProdUsers(prodPath);
                if (prodUsers.Count > 0 && !prodUsers.Contains(UserName))
                    // continue; // skip this production if user is not authorized
                    cbProdList.Items.Add(prodName); // TEMPORAIREMENT ON AFFICHE TOUTES LES PRODS
                else
                {
                    cbProdList.Items.Add(prodName);
                }
            }

            if (cbProdList.Items.Count > 0)
                cbProdList.SelectedIndex = 0;
        }

        public string GetSelectedProductionPath()
        {
            string rootPath = ProductionService.GetProductionRootPath();
            string selectedProd = cbProdList.SelectedItem?.ToString() ?? string.Empty;
            return Path.Combine(rootPath, selectedProd);
        }
        #endregion


        #region TAB BTN  - CLEANED -
        private void btnTab1_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 1;
        }

        private void btnTab2_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 0;
        }

        private void btnTab3_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 2;
            string selectedProd = cbProdList.SelectedItem?.ToString() ?? string.Empty;
            ScheduleTabBuilder.DisplaySchedule(ScheduleTabBuilder.GetContext(selectedProd));
        }

        private void btnTab4_Click(object sender, EventArgs e)
        {
            tabCtrlMain.SelectedIndex = 3;
            string selectedProd = cbProdList.SelectedItem?.ToString() ?? string.Empty;
            StatsChartTabBuilder.DisplayTimeLogs(StatsChartTabBuilder.GetContext(selectedProd));
            StatsChartTabBuilder.DisplayStats(StatsChartTabBuilder.GetContext(selectedProd));
        }
        #endregion


        #region  MENU HEADER  - CLEANED -
        private void userSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserConfig.OpenConfigFile();
        }

        private void nodeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string prodPath = GetSelectedProductionPath();
            string path = Path.Combine(prodPath, "Dev", "DangerZone", "NodeStructure.json");

            FileExplorerService.OpenFile(path);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string docPath = Path.Combine(Application.StartupPath, "Docs", "index.html");
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
            string prodPath = GetSelectedProductionPath();
            string configJsonPath = ProductionService.getConfigJsonPath(prodPath);

            Process.Start(new ProcessStartInfo
            {
                FileName = configJsonPath,
                UseShellExecute = true
            });
        }

        private void checkFoldersStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ProdName = cbProdList.SelectedItem?.ToString();
            ProdFilesManip.EnsureServerProductionFiles(ProdName);
        }


        private void ensureLocalStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ProdName = cbProdList.SelectedItem?.ToString();
            ProdFilesManip.EnsureLocalProductionFiles(ProdName);
        }
        #endregion


        #region  LEFT ZONE / LIST ASSET / PROD SELECTION  - CLEANED -
        private void tvNodeList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string selectedProd = cbProdList.SelectedItem?.ToString();
            string productionPath = GetSelectedProductionPath();

            // ON CHECK SI LA SELECTION EST DANS UN ASSET VALIDE
            string nodePath = TreeViewBuilder.GetPathFromNode(e.Node);

            // si l'nodePath est vide ou n'existe pas, on clear les panels
            if (string.IsNullOrEmpty(nodePath) || !Directory.Exists(nodePath))
            {
                WorkTabBuilder.ClearPanel(WorkTabBuilder.GetContext(nodePath, selectedProd));
                NodeTabBuilder.ClearPanel(NodeTabBuilder.GetContext(nodePath, selectedProd));
                return;
            }

            var ctx = NodeService.ExtractNodeContext(nodePath);

            // si ctx est null, on clear les panels car l'nodePath n'est pas un node valide
            if (ctx == null)
            {
                WorkTabBuilder.ClearPanel(WorkTabBuilder.GetContext(nodePath, selectedProd));
                NodeTabBuilder.ClearPanel(NodeTabBuilder.GetContext(nodePath, selectedProd));
                return;
            }

            // si l'nodePath n'est pas un node valide, on clear les panels car il n'est pas dans la structure attendue
            if (Path.GetFileName(nodePath).Equals(ctx.NodeType, StringComparison.OrdinalIgnoreCase))
            {
                WorkTabBuilder.ClearPanel(WorkTabBuilder.GetContext(nodePath, selectedProd));
                NodeTabBuilder.ClearPanel(NodeTabBuilder.GetContext(nodePath, selectedProd));
                return;
            }

            // ON CHARGE LES PANELS DE TRAVAIL ET D'ASSET
            var nodeStructures = NodeStructureBuilder.LoadNodeStructures(productionPath);

            if (nodeStructures != null && nodeStructures.TryGetValue(ctx.NodeType, out var structure))
            {
                string jsonPath = Path.Combine(nodePath, "node.json");
                var jsonNode = JsonHelper.ParseJson(jsonPath);

                WorkTabBuilder.DisplayDepartments(WorkTabBuilder.GetContext(nodePath, selectedProd));
                NodeTabBuilder.DisplayDepartments(NodeTabBuilder.GetContext(nodePath, selectedProd));
                WorkTabBuilder.DisplayHeader(jsonNode, ctx, WorkTabBuilder.GetContext(nodePath, selectedProd));
                NodeTabBuilder.DisplayHeader(NodeTabBuilder.GetContext(nodePath, selectedProd));
            }
            else
            {
                if (nodePath.Contains("\\Concept\\"))
                {
                    NodeTabBuilder.DisplayHeader(NodeTabBuilder.GetContext(nodePath, selectedProd));
                    flpNodeTask.SuspendLayout();
                    flpNodeTask.Controls.Clear();
                    NodeTabBuilder.DisplayImagePanel(NodeTabBuilder.GetContext(nodePath, selectedProd));
                    flpNodeTask.ResumeLayout();
                }
            }
        }

        private void LoadTreeViewFromFolder(string rootPath, string prodName)
        {
            tvNodeList.Nodes.Clear();
            var rootNode = TreeViewBuilder.GetFolderHierarchy(rootPath, prodName);
            if (rootNode != null) tvNodeList.Nodes.Add(rootNode);
            rootNode?.Expand();
        }

        private void contextMenuTree_Opening(object sender, CancelEventArgs e)
        {
            if (tvNodeList.SelectedNode == null)
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
            string selectedPath = tvNodeList.SelectedNode?.Tag as string;
            FileExplorerService.OpenInExplorer(selectedPath);
        }

        private void cbProdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProd = cbProdList.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedProd))
            {
                string rootPath = ProductionService.GetProductionRootPath();
                LoadTreeViewFromFolder(rootPath, selectedProd);
                // ProdFilesManip.EnsureLocalProductionFiles(selectedProd); pas foufou car si usersettings mal defini ca fout le bronx
            }
        }

        private void btCreateAsset_Click(object sender, EventArgs e)
        {
            // Ouvre le formulaire de création d'asset

            string prodPath = GetSelectedProductionPath();
            if (!ProductionService.CheckIfOnServer(prodPath))
            {
                MessageBox.Show("This action an only be done on Server connection");
                return;
            }

            using (var form = new CreateAssetPopup(this))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                string newItemName = form.NodeName.Trim();
                string newItemType = form.NodeType;
                string Description = form.Description.Trim();

                string selectedProd = cbProdList.SelectedItem?.ToString();
                string rootPath = ProductionService.GetProductionRootPath();


                NodeService.CreateAsset(prodPath, newItemName, newItemType, Description);

                // Recharger  TreeView
                LoadTreeViewFromFolder(rootPath, selectedProd);
            }
        }

        private void btCreateSeq_Click(object sender, EventArgs e)
        {
            // Ouvre le formulaire de création de séquence

            string prodPath = GetSelectedProductionPath();
            if (!ProductionService.CheckIfOnServer(prodPath))
            {
                MessageBox.Show("This action an only be done on Server connection");
                return;
            }

            using (var form = new CreateSequencePopup(this))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                string newSeqName = $"S{form.NodeName.Trim()}";
                string Description = form.Description.Trim();
                string selectedProd = cbProdList.SelectedItem?.ToString();
                string rootPath = ProductionService.GetProductionRootPath();
                NodeService.CreateSequence(prodPath, newSeqName, Description);

                foreach (var shot in form.GetShots())
                {
                    NodeService.CreateShot(prodPath, newSeqName, shot.Name, shot.Description, shot.RangeIn, shot.RangeOut);
                }

                // Recharger  TreeView
                LoadTreeViewFromFolder(rootPath, selectedProd);
            }

        }

        private void btCreateShot_Click(object sender, EventArgs e)
        {
            // Ouvre le formulaire de création de séquence

            string prodPath = GetSelectedProductionPath();
            if (!ProductionService.CheckIfOnServer(prodPath))
            {
                MessageBox.Show("This action an only be done on Server connection");
                return;
            }

            using (var form = new CreateShotPopup(this))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                string newShotName = $"P{form.NodeName.Trim()}";
                string seqName = form.HostName;
                string Description = form.Description.Trim();
                string rangeIn = form.RangeIn.Trim();
                string rangeOut = form.RangeOut.Trim();
                string selectedProd = cbProdList.SelectedItem?.ToString();
                string rootPath = ProductionService.GetProductionRootPath();
                NodeService.CreateShot(prodPath, seqName, newShotName, Description, rangeIn, rangeOut);
                // Recharger  TreeView
                LoadTreeViewFromFolder(rootPath, selectedProd);
            }

        }

        private void btnCreateProduction_Click(object sender, EventArgs e)
        {
            // Ouvre le formulaire de création de production

            string prodPath = GetSelectedProductionPath();
            if (!ProductionService.CheckIfOnServer(prodPath))
            {
                MessageBox.Show("This action an only be done on Server connection");
                return;
            }

            using (var form = new CreateProductionPopup())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string prodName = form.ProductionName;
                    var prodStructure = form.ProductionStructure;

                    string rootPath = ProductionService.GetProductionRootPath();
                    var productionConfig = new ProductionStructureBuilder { name = prodName };

                    productionConfig.CreateProductionStructure(prodName, rootPath, prodStructure);

                    MessageBox.Show($"Production '{prodName}' créée !");

                    LoadProductionList();
                    cbProdList.SelectedItem = prodName;
                }
            }
        }
        #endregion


        #region  WORK PANNEL - CLEANED -
        public void RefreshTab(string nodePath)
        {
            string selectedProd = cbProdList.SelectedItem?.ToString();
            WorkTabBuilder.RefreshTab(WorkTabBuilder.GetContext(nodePath, selectedProd));
        }
        #endregion


        #region  ASSET PANNEL - CLEANED -
        private void cbbNodeStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbNodeStatus.Tag == null)
                return;

            string nodePath = cbbNodeStatus.Tag.ToString();
            string selectedProd = cbProdList.SelectedItem?.ToString();

            NodeTabBuilder.cbbNodeStatus_SelectedIndexChanged(NodeTabBuilder.GetContext(nodePath, selectedProd));
        }
        #endregion


        #region STATS TAB / TIMELOGS - CLEANED -
        private void btnAddTimelog_Click(object sender, EventArgs e)
        {
            string prodPath = GetSelectedProductionPath();
            if (!ProductionService.CheckIfOnServer(prodPath))
            {
                MessageBox.Show("This action an only be done on Server connection");
                return;
            }

            var nodesDict = GetAllNodesInProduction(prodPath);
            var allNodes = nodesDict.SelectMany(typeEntry => typeEntry.Value.Select(nodeEntry => $"{typeEntry.Key}/{nodeEntry.Key}")).ToList(); //CRADE DE FOU

            using (var form = new TimeLogAddPopup(allNodes))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var log = new TimeLogData
                    {
                        NodeName = form.NodeName,
                        Department = form.Department,
                        Artist = ProductionService.GetUserName(),
                        Hours = form.Hours,
                        Date = DateTime.Now
                    };

                    TimeLogManager.Add(log, prodPath);
                    MessageBox.Show("TimeLog ajouté !");
                }
            }
        }

        private void cbbGraphList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProd = cbProdList.SelectedItem?.ToString() ?? string.Empty;
            flpAllDeptTimeLogsGraphs.Controls.Clear();
            if (cbbGraphList.SelectedIndex == 1)
            { StatsChartTabBuilder.DisplayDeptHourChart(StatsChartTabBuilder.GetContext(selectedProd)); }
            if (cbbGraphList.SelectedIndex == 4)
            { StatsChartTabBuilder.DisplaNodeHourChart(StatsChartTabBuilder.GetContext(selectedProd), "Characters"); }
            if (cbbGraphList.SelectedIndex == 5)
            { StatsChartTabBuilder.DisplaNodeHourChart(StatsChartTabBuilder.GetContext(selectedProd), "Props"); }
            if (cbbGraphList.SelectedIndex == 6)
            { StatsChartTabBuilder.DisplaNodeHourChart(StatsChartTabBuilder.GetContext(selectedProd), "Environments"); }
            if (cbbGraphList.SelectedIndex == 7)
            { StatsChartTabBuilder.DisplaNodeHourChart(StatsChartTabBuilder.GetContext(selectedProd), "Sequences"); }
            if (cbbGraphList.SelectedIndex == 8)
            { StatsChartTabBuilder.DisplaNodeHourChart(StatsChartTabBuilder.GetContext(selectedProd), "Shots"); }

        }
        #endregion

        private void AssetManagerForm_Load(object sender, EventArgs e)
        {

        }
    }
}