using System.Text.Json;

namespace DuckPipe
{
    partial class AssetManagerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetManagerForm));
            tvAssetList = new TreeView();
            contextMenuTree = new ContextMenuStrip(components);
            tsmiRename = new ToolStripMenuItem();
            tsmiDelete = new ToolStripMenuItem();
            viewInExplorerToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            btnCreateProduction = new Button();
            btCreateAsset = new Button();
            cbProdList = new ComboBox();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            userSettingsToolStripMenuItem = new ToolStripMenuItem();
            assetSettingsToolStripMenuItem = new ToolStripMenuItem();
            prodSettingsToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            checkFoldersStructureToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            plAssetTaskInfo = new Panel();
            flpAssetInspect = new FlowLayoutPanel();
            lblAssetType = new Label();
            lblAssetName = new Label();
            pnlPipelineStatus = new Panel();
            flpPipelineStatus = new FlowLayoutPanel();
            pnlDeptBtn = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            flpDeptButton = new FlowLayoutPanel();
            pnlTopRight = new Panel();
            flpAssetDescription = new FlowLayoutPanel();
            lbDescription = new Label();
            splitMain = new SplitContainer();
            tablpanTabBtn = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnTab2 = new Button();
            btnTab1 = new Button();
            tabCtrlMain = new CustomTabControl();
            tabPWork = new TabPage();
            splitContainer3 = new SplitContainer();
            tabPAsset = new TabPage();
            flpAssetTask = new FlowLayoutPanel();
            panel2 = new Panel();
            btnEditAsset = new Button();
            lblAssetType2 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            lblAssetName2 = new Label();
            lbDescription2 = new Label();
            button1 = new Button();
            tabPShelude = new TabPage();
            contextMenuTree.SuspendLayout();
            panel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            plAssetTaskInfo.SuspendLayout();
            pnlPipelineStatus.SuspendLayout();
            pnlDeptBtn.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlTopRight.SuspendLayout();
            flpAssetDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tablpanTabBtn.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tabCtrlMain.SuspendLayout();
            tabPWork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            tabPAsset.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tvAssetList
            // 
            tvAssetList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvAssetList.BackColor = Color.FromArgb(30, 30, 30);
            tvAssetList.BorderStyle = BorderStyle.None;
            tvAssetList.ContextMenuStrip = contextMenuTree;
            tvAssetList.ForeColor = Color.White;
            tvAssetList.LineColor = Color.White;
            tvAssetList.Location = new Point(12, 65);
            tvAssetList.Name = "tvAssetList";
            tvAssetList.Size = new Size(192, 467);
            tvAssetList.TabIndex = 0;
            tvAssetList.AfterSelect += tvAssetList_AfterSelect;
            // 
            // contextMenuTree
            // 
            contextMenuTree.Items.AddRange(new ToolStripItem[] { tsmiRename, tsmiDelete, viewInExplorerToolStripMenuItem });
            contextMenuTree.Name = "contextMenuTree";
            contextMenuTree.Size = new Size(158, 70);
            contextMenuTree.Opening += contextMenuTree_Opening;
            // 
            // tsmiRename
            // 
            tsmiRename.Name = "tsmiRename";
            tsmiRename.Size = new Size(157, 22);
            tsmiRename.Text = "Rename";
            tsmiRename.Click += tsmiRename_Click;
            // 
            // tsmiDelete
            // 
            tsmiDelete.Name = "tsmiDelete";
            tsmiDelete.Size = new Size(157, 22);
            tsmiDelete.Text = "Delete";
            tsmiDelete.Click += tsmiDelete_Click;
            // 
            // viewInExplorerToolStripMenuItem
            // 
            viewInExplorerToolStripMenuItem.Name = "viewInExplorerToolStripMenuItem";
            viewInExplorerToolStripMenuItem.Size = new Size(157, 22);
            viewInExplorerToolStripMenuItem.Text = "View in explorer";
            viewInExplorerToolStripMenuItem.Click += viewInExplorerToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 45, 45);
            panel1.Controls.Add(btnCreateProduction);
            panel1.Controls.Add(btCreateAsset);
            panel1.Controls.Add(cbProdList);
            panel1.Controls.Add(tvAssetList);
            panel1.Controls.Add(menuStrip1);
            panel1.Dock = DockStyle.Fill;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(215, 569);
            panel1.TabIndex = 2;
            // 
            // btnCreateProduction
            // 
            btnCreateProduction.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCreateProduction.BackColor = Color.FromArgb(80, 80, 80);
            btnCreateProduction.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnCreateProduction.FlatAppearance.BorderSize = 0;
            btnCreateProduction.FlatStyle = FlatStyle.Flat;
            btnCreateProduction.ForeColor = Color.White;
            btnCreateProduction.Location = new Point(179, 34);
            btnCreateProduction.Name = "btnCreateProduction";
            btnCreateProduction.Size = new Size(25, 25);
            btnCreateProduction.TabIndex = 1;
            btnCreateProduction.Text = "+";
            btnCreateProduction.UseVisualStyleBackColor = false;
            btnCreateProduction.Click += btnCreateProduction_Click;
            // 
            // btCreateAsset
            // 
            btCreateAsset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btCreateAsset.BackColor = Color.FromArgb(80, 80, 80);
            btCreateAsset.FlatAppearance.BorderSize = 0;
            btCreateAsset.FlatStyle = FlatStyle.Flat;
            btCreateAsset.ForeColor = Color.White;
            btCreateAsset.Location = new Point(12, 536);
            btCreateAsset.Name = "btCreateAsset";
            btCreateAsset.Size = new Size(192, 25);
            btCreateAsset.TabIndex = 0;
            btCreateAsset.Text = "Add New";
            btCreateAsset.UseVisualStyleBackColor = false;
            btCreateAsset.Click += btCreateAsset_Click;
            // 
            // cbProdList
            // 
            cbProdList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbProdList.BackColor = Color.FromArgb(60, 60, 60);
            cbProdList.CausesValidation = false;
            cbProdList.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProdList.FlatStyle = FlatStyle.Flat;
            cbProdList.ForeColor = Color.White;
            cbProdList.FormattingEnabled = true;
            cbProdList.Items.AddRange(new object[] { "OBSERVER ", "SPARK", "SILENCE" });
            cbProdList.Location = new Point(12, 34);
            cbProdList.Name = "cbProdList";
            cbProdList.Size = new Size(161, 23);
            cbProdList.TabIndex = 3;
            cbProdList.SelectedIndexChanged += cbProdList_SelectedIndexChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(80, 80, 80);
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, toolsToolStripMenuItem, toolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(215, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { userSettingsToolStripMenuItem, assetSettingsToolStripMenuItem, prodSettingsToolStripMenuItem });
            settingsToolStripMenuItem.ForeColor = Color.White;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // userSettingsToolStripMenuItem
            // 
            userSettingsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            userSettingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            userSettingsToolStripMenuItem.ForeColor = Color.White;
            userSettingsToolStripMenuItem.Name = "userSettingsToolStripMenuItem";
            userSettingsToolStripMenuItem.Size = new Size(147, 22);
            userSettingsToolStripMenuItem.Text = "User Settings";
            userSettingsToolStripMenuItem.Click += userSettingsToolStripMenuItem_Click;
            // 
            // assetSettingsToolStripMenuItem
            // 
            assetSettingsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            assetSettingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            assetSettingsToolStripMenuItem.ForeColor = Color.White;
            assetSettingsToolStripMenuItem.Name = "assetSettingsToolStripMenuItem";
            assetSettingsToolStripMenuItem.Size = new Size(147, 22);
            assetSettingsToolStripMenuItem.Text = "Asset Settings";
            assetSettingsToolStripMenuItem.Click += assetSettingsToolStripMenuItem_Click;
            // 
            // prodSettingsToolStripMenuItem
            // 
            prodSettingsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            prodSettingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            prodSettingsToolStripMenuItem.ForeColor = Color.White;
            prodSettingsToolStripMenuItem.Name = "prodSettingsToolStripMenuItem";
            prodSettingsToolStripMenuItem.Size = new Size(147, 22);
            prodSettingsToolStripMenuItem.Text = "Prod Settings";
            prodSettingsToolStripMenuItem.Click += prodSettingsToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { checkFoldersStructureToolStripMenuItem });
            toolsToolStripMenuItem.ForeColor = Color.White;
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // checkFoldersStructureToolStripMenuItem
            // 
            checkFoldersStructureToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            checkFoldersStructureToolStripMenuItem.ForeColor = Color.White;
            checkFoldersStructureToolStripMenuItem.Name = "checkFoldersStructureToolStripMenuItem";
            checkFoldersStructureToolStripMenuItem.Size = new Size(196, 22);
            checkFoldersStructureToolStripMenuItem.Text = "CheckFolders Structure";
            checkFoldersStructureToolStripMenuItem.Click += checkFoldersStructureToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.ForeColor = Color.White;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(24, 20);
            toolStripMenuItem1.Text = "?";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // plAssetTaskInfo
            // 
            plAssetTaskInfo.BackColor = Color.FromArgb(55, 55, 55);
            plAssetTaskInfo.Controls.Add(flpAssetInspect);
            plAssetTaskInfo.Dock = DockStyle.Fill;
            plAssetTaskInfo.Location = new Point(0, 0);
            plAssetTaskInfo.Name = "plAssetTaskInfo";
            plAssetTaskInfo.Size = new Size(196, 402);
            plAssetTaskInfo.TabIndex = 3;
            // 
            // flpAssetInspect
            // 
            flpAssetInspect.AutoScroll = true;
            flpAssetInspect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpAssetInspect.BackColor = Color.Transparent;
            flpAssetInspect.Dock = DockStyle.Fill;
            flpAssetInspect.FlowDirection = FlowDirection.TopDown;
            flpAssetInspect.Location = new Point(0, 0);
            flpAssetInspect.Name = "flpAssetInspect";
            flpAssetInspect.Size = new Size(196, 402);
            flpAssetInspect.TabIndex = 3;
            flpAssetInspect.WrapContents = false;
            // 
            // lblAssetType
            // 
            lblAssetType.AutoSize = true;
            lblAssetType.BackColor = Color.Transparent;
            lblAssetType.Font = new Font("Nirmala UI", 12F, FontStyle.Bold | FontStyle.Italic);
            lblAssetType.ForeColor = Color.Silver;
            lblAssetType.Location = new Point(16, 44);
            lblAssetType.Margin = new Padding(0);
            lblAssetType.Name = "lblAssetType";
            lblAssetType.Size = new Size(101, 21);
            lblAssetType.TabIndex = 2;
            lblAssetType.Text = "placeholder";
            // 
            // lblAssetName
            // 
            lblAssetName.AutoSize = true;
            lblAssetName.BackColor = Color.Transparent;
            lblAssetName.Font = new Font("Nirmala UI", 20F, FontStyle.Bold);
            lblAssetName.ForeColor = Color.White;
            lblAssetName.Location = new Point(0, 0);
            lblAssetName.Margin = new Padding(0);
            lblAssetName.Name = "lblAssetName";
            lblAssetName.Size = new Size(222, 37);
            lblAssetName.TabIndex = 1;
            lblAssetName.Text = "PLACEHOLDER |";
            lblAssetName.TextAlign = ContentAlignment.BottomLeft;
            // 
            // pnlPipelineStatus
            // 
            pnlPipelineStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlPipelineStatus.BackColor = Color.FromArgb(40, 40, 40);
            pnlPipelineStatus.Controls.Add(flpPipelineStatus);
            pnlPipelineStatus.Location = new Point(0, 0);
            pnlPipelineStatus.Name = "pnlPipelineStatus";
            pnlPipelineStatus.Size = new Size(555, 349);
            pnlPipelineStatus.TabIndex = 4;
            // 
            // flpPipelineStatus
            // 
            flpPipelineStatus.AutoScroll = true;
            flpPipelineStatus.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPipelineStatus.Dock = DockStyle.Fill;
            flpPipelineStatus.FlowDirection = FlowDirection.TopDown;
            flpPipelineStatus.Location = new Point(0, 0);
            flpPipelineStatus.Name = "flpPipelineStatus";
            flpPipelineStatus.Size = new Size(555, 349);
            flpPipelineStatus.TabIndex = 3;
            flpPipelineStatus.WrapContents = false;
            // 
            // pnlDeptBtn
            // 
            pnlDeptBtn.Controls.Add(tableLayoutPanel1);
            pnlDeptBtn.Dock = DockStyle.Bottom;
            pnlDeptBtn.Location = new Point(0, 352);
            pnlDeptBtn.Name = "pnlDeptBtn";
            pnlDeptBtn.Size = new Size(555, 50);
            pnlDeptBtn.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Center;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(flpDeptButton, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(555, 43);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // flpDeptButton
            // 
            flpDeptButton.Location = new Point(57, 10);
            flpDeptButton.Margin = new Padding(3, 10, 3, 3);
            flpDeptButton.Name = "flpDeptButton";
            flpDeptButton.Size = new Size(441, 30);
            flpDeptButton.TabIndex = 0;
            // 
            // pnlTopRight
            // 
            pnlTopRight.BackColor = Color.FromArgb(65, 65, 65);
            pnlTopRight.Controls.Add(lblAssetType);
            pnlTopRight.Controls.Add(flpAssetDescription);
            pnlTopRight.Dock = DockStyle.Top;
            pnlTopRight.Location = new Point(3, 3);
            pnlTopRight.Name = "pnlTopRight";
            pnlTopRight.Size = new Size(755, 74);
            pnlTopRight.TabIndex = 5;
            // 
            // flpAssetDescription
            // 
            flpAssetDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpAssetDescription.Controls.Add(lblAssetName);
            flpAssetDescription.Controls.Add(lbDescription);
            flpAssetDescription.Location = new Point(7, 3);
            flpAssetDescription.Name = "flpAssetDescription";
            flpAssetDescription.Size = new Size(745, 68);
            flpAssetDescription.TabIndex = 5;
            // 
            // lbDescription
            // 
            lbDescription.AutoSize = true;
            lbDescription.BackColor = Color.Transparent;
            lbDescription.Font = new Font("Nirmala UI", 10F);
            lbDescription.ForeColor = Color.Silver;
            lbDescription.Location = new Point(222, 10);
            lbDescription.Margin = new Padding(0, 10, 0, 0);
            lbDescription.Name = "lbDescription";
            lbDescription.Size = new Size(156, 19);
            lbDescription.TabIndex = 4;
            lbDescription.Text = "description Place Holder";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(panel1);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(tablpanTabBtn);
            splitMain.Panel2.Controls.Add(tabCtrlMain);
            splitMain.Size = new Size(991, 569);
            splitMain.SplitterDistance = 215;
            splitMain.TabIndex = 6;
            // 
            // tablpanTabBtn
            // 
            tablpanTabBtn.BackColor = Color.FromArgb(30, 30, 30);
            tablpanTabBtn.BackgroundImageLayout = ImageLayout.Center;
            tablpanTabBtn.ColumnCount = 3;
            tablpanTabBtn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tablpanTabBtn.ColumnStyles.Add(new ColumnStyle());
            tablpanTabBtn.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tablpanTabBtn.Controls.Add(flowLayoutPanel1, 1, 0);
            tablpanTabBtn.Dock = DockStyle.Top;
            tablpanTabBtn.Location = new Point(0, 0);
            tablpanTabBtn.Margin = new Padding(0);
            tablpanTabBtn.Name = "tablpanTabBtn";
            tablpanTabBtn.RowCount = 1;
            tablpanTabBtn.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablpanTabBtn.Size = new Size(772, 43);
            tablpanTabBtn.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.None;
            flowLayoutPanel1.Controls.Add(btnTab2);
            flowLayoutPanel1.Controls.Add(btnTab1);
            flowLayoutPanel1.Location = new Point(305, 8);
            flowLayoutPanel1.Margin = new Padding(8);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(162, 27);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btnTab2
            // 
            btnTab2.BackColor = Color.Transparent;
            btnTab2.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnTab2.FlatStyle = FlatStyle.Flat;
            btnTab2.ForeColor = Color.White;
            btnTab2.Location = new Point(3, 3);
            btnTab2.Name = "btnTab2";
            btnTab2.Size = new Size(75, 23);
            btnTab2.TabIndex = 1;
            btnTab2.Text = "Asset";
            btnTab2.UseVisualStyleBackColor = false;
            btnTab2.Click += btnTab2_Click;
            // 
            // btnTab1
            // 
            btnTab1.BackColor = Color.Transparent;
            btnTab1.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnTab1.FlatStyle = FlatStyle.Flat;
            btnTab1.ForeColor = Color.White;
            btnTab1.Location = new Point(84, 3);
            btnTab1.Name = "btnTab1";
            btnTab1.Size = new Size(75, 23);
            btnTab1.TabIndex = 0;
            btnTab1.Text = "Workfiles";
            btnTab1.UseVisualStyleBackColor = false;
            btnTab1.Click += btnTab1_Click;
            // 
            // tabCtrlMain
            // 
            tabCtrlMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabCtrlMain.BackgroundColor = Color.FromArgb(64, 64, 64);
            tabCtrlMain.Controls.Add(tabPWork);
            tabCtrlMain.Controls.Add(tabPAsset);
            tabCtrlMain.Controls.Add(tabPShelude);
            tabCtrlMain.ItemSize = new Size(100, 30);
            tabCtrlMain.Location = new Point(0, 49);
            tabCtrlMain.Multiline = true;
            tabCtrlMain.Name = "tabCtrlMain";
            tabCtrlMain.Padding = new Point(10, 5);
            tabCtrlMain.SelectedIndex = 0;
            tabCtrlMain.SelectedTabColor = Color.FromArgb(64, 64, 64);
            tabCtrlMain.SelectedTextColor = Color.White;
            tabCtrlMain.Size = new Size(769, 520);
            tabCtrlMain.SizeMode = TabSizeMode.Fixed;
            tabCtrlMain.TabIndex = 0;
            tabCtrlMain.UnselectedTabColor = Color.FromArgb(30, 30, 30);
            tabCtrlMain.UnselectedTextColor = Color.White;
            // 
            // tabPWork
            // 
            tabPWork.BackColor = Color.FromArgb(40, 40, 40);
            tabPWork.Controls.Add(splitContainer3);
            tabPWork.Controls.Add(pnlTopRight);
            tabPWork.Location = new Point(4, 34);
            tabPWork.Name = "tabPWork";
            tabPWork.Padding = new Padding(3);
            tabPWork.Size = new Size(761, 482);
            tabPWork.TabIndex = 0;
            tabPWork.Text = "Works";
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(3, 77);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(pnlDeptBtn);
            splitContainer3.Panel1.Controls.Add(pnlPipelineStatus);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(plAssetTaskInfo);
            splitContainer3.Size = new Size(755, 402);
            splitContainer3.SplitterDistance = 555;
            splitContainer3.TabIndex = 6;
            // 
            // tabPAsset
            // 
            tabPAsset.BackColor = Color.FromArgb(40, 40, 40);
            tabPAsset.Controls.Add(flpAssetTask);
            tabPAsset.Controls.Add(panel2);
            tabPAsset.Location = new Point(4, 34);
            tabPAsset.Name = "tabPAsset";
            tabPAsset.Padding = new Padding(3);
            tabPAsset.Size = new Size(761, 482);
            tabPAsset.TabIndex = 1;
            tabPAsset.Text = "Asset";
            // 
            // flpAssetTask
            // 
            flpAssetTask.AutoScroll = true;
            flpAssetTask.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpAssetTask.Dock = DockStyle.Fill;
            flpAssetTask.FlowDirection = FlowDirection.TopDown;
            flpAssetTask.Location = new Point(3, 77);
            flpAssetTask.Name = "flpAssetTask";
            flpAssetTask.Size = new Size(755, 402);
            flpAssetTask.TabIndex = 7;
            flpAssetTask.WrapContents = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(65, 65, 65);
            panel2.Controls.Add(btnEditAsset);
            panel2.Controls.Add(lblAssetType2);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(755, 74);
            panel2.TabIndex = 6;
            // 
            // btnEditAsset
            // 
            btnEditAsset.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditAsset.BackColor = Color.Transparent;
            btnEditAsset.Cursor = Cursors.Hand;
            btnEditAsset.FlatAppearance.BorderSize = 0;
            btnEditAsset.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnEditAsset.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnEditAsset.FlatStyle = FlatStyle.Flat;
            btnEditAsset.Font = new Font("Segoe UI", 15F);
            btnEditAsset.ForeColor = Color.White;
            btnEditAsset.Image = Properties.Resources.Save;
            btnEditAsset.Location = new Point(697, 4);
            btnEditAsset.Name = "btnEditAsset";
            btnEditAsset.Size = new Size(55, 61);
            btnEditAsset.TabIndex = 6;
            btnEditAsset.UseVisualStyleBackColor = false;
            btnEditAsset.Click += btnEditAsset_Click;
            // 
            // lblAssetType2
            // 
            lblAssetType2.AutoSize = true;
            lblAssetType2.BackColor = Color.Transparent;
            lblAssetType2.Font = new Font("Nirmala UI", 12F, FontStyle.Bold | FontStyle.Italic);
            lblAssetType2.ForeColor = Color.Silver;
            lblAssetType2.Location = new Point(16, 44);
            lblAssetType2.Margin = new Padding(0);
            lblAssetType2.Name = "lblAssetType2";
            lblAssetType2.Size = new Size(101, 21);
            lblAssetType2.TabIndex = 2;
            lblAssetType2.Text = "placeholder";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel2.Controls.Add(lblAssetName2);
            flowLayoutPanel2.Controls.Add(lbDescription2);
            flowLayoutPanel2.Location = new Point(7, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(649, 42);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // lblAssetName2
            // 
            lblAssetName2.AutoSize = true;
            lblAssetName2.BackColor = Color.Transparent;
            lblAssetName2.Font = new Font("Nirmala UI", 20F, FontStyle.Bold);
            lblAssetName2.ForeColor = Color.White;
            lblAssetName2.Location = new Point(0, 0);
            lblAssetName2.Margin = new Padding(0);
            lblAssetName2.Name = "lblAssetName2";
            lblAssetName2.Size = new Size(222, 37);
            lblAssetName2.TabIndex = 1;
            lblAssetName2.Text = "PLACEHOLDER |";
            lblAssetName2.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lbDescription2
            // 
            lbDescription2.AutoSize = true;
            lbDescription2.BackColor = Color.Transparent;
            lbDescription2.Font = new Font("Nirmala UI", 10F);
            lbDescription2.ForeColor = Color.Silver;
            lbDescription2.Location = new Point(222, 10);
            lbDescription2.Margin = new Padding(0, 10, 0, 0);
            lbDescription2.Name = "lbDescription2";
            lbDescription2.Size = new Size(156, 19);
            lbDescription2.TabIndex = 4;
            lbDescription2.Text = "description Place Holder";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            button1.BackColor = Color.Transparent;
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 15F);
            button1.ForeColor = Color.White;
            button1.Image = Properties.Resources.EditBtnIcon;
            button1.Location = new Point(1243, 3);
            button1.Name = "button1";
            button1.Size = new Size(55, 35);
            button1.TabIndex = 3;
            button1.UseVisualStyleBackColor = false;
            // 
            // tabPShelude
            // 
            tabPShelude.BackColor = Color.FromArgb(40, 40, 40);
            tabPShelude.Location = new Point(4, 34);
            tabPShelude.Name = "tabPShelude";
            tabPShelude.Size = new Size(761, 482);
            tabPShelude.TabIndex = 2;
            tabPShelude.Text = "Shelude";
            // 
            // AssetManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(991, 569);
            Controls.Add(splitMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "AssetManagerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DuckPipe";
            contextMenuTree.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            plAssetTaskInfo.ResumeLayout(false);
            pnlPipelineStatus.ResumeLayout(false);
            pnlDeptBtn.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            pnlTopRight.ResumeLayout(false);
            pnlTopRight.PerformLayout();
            flpAssetDescription.ResumeLayout(false);
            flpAssetDescription.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            tablpanTabBtn.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            tabCtrlMain.ResumeLayout(false);
            tabPWork.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            tabPAsset.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }
        #endregion

        private TreeView tvAssetList;
        private Panel panel1;
        private Button btCreateAsset;
        private Button btnCreateProduction;
        private ContextMenuStrip contextMenuTree;
        private ToolStripMenuItem tsmiRename;
        private ToolStripMenuItem tsmiDelete;
        private Panel plAssetTaskInfo;
        private Label lblAssetType;
        private Label lblAssetName;
        private Panel pnlPipelineStatus;
        private Panel pnlTopRight;
        private SplitContainer splitMain;
        private SplitContainer splitContainer3;
        private FlowLayoutPanel flpAssetInspect;
        private Panel pnlDeptBtn;
        private FlowLayoutPanel flpDeptButton;
        private FlowLayoutPanel flpPipelineStatus;
        public ComboBox cbProdList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem userSettingsToolStripMenuItem;
        private ToolStripMenuItem assetSettingsToolStripMenuItem;
        private ToolStripMenuItem prodSettingsToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem viewInExplorerToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem checkFoldersStructureToolStripMenuItem;
        private Label lbDescription;
        private FlowLayoutPanel flpAssetDescription;
        private CustomTabControl tabCtrlMain;
        private TabPage tabPWork;
        private TabPage tabPAsset;
        private Button btnTab1;
        private Button btnTab2;
        private TableLayoutPanel tablpanTabBtn;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel1;
        private TabPage tabPShelude;
        private Panel panel2;
        private Button btnEditAsset;
        private Label lblAssetType2;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label lblAssetName2;
        private Label lbDescription2;
        private Button button1;
        private FlowLayoutPanel flpAssetTask;
    }
}