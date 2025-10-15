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
            tvNodeList = new TreeView();
            contextMenuTree = new ContextMenuStrip(components);
            tsmiRename = new ToolStripMenuItem();
            tsmiDelete = new ToolStripMenuItem();
            viewInExplorerToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            button3 = new Button();
            button2 = new Button();
            btCreateNode = new Button();
            btnCreateProduction = new Button();
            cbProdList = new ComboBox();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            userSettingsToolStripMenuItem = new ToolStripMenuItem();
            nodeSettingsToolStripMenuItem = new ToolStripMenuItem();
            prodSettingsToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            checkFoldersStructureToolStripMenuItem = new ToolStripMenuItem();
            ensureLocalStructureToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            plNodeTaskInfo = new Panel();
            flpNodeInspect = new FlowLayoutPanel();
            lblNodeType = new Label();
            lblNodeName = new Label();
            pnlPipelineStatus = new Panel();
            flpPipelineStatus = new FlowLayoutPanel();
            pnlDeptBtn = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            flpDeptButton = new FlowLayoutPanel();
            pnlTopRight = new Panel();
            flpNodeDescription = new FlowLayoutPanel();
            lbDescription = new Label();
            splitMain = new SplitContainer();
            tablpanTabBtn = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnTab2 = new Button();
            btnTab1 = new Button();
            btnTab3 = new Button();
            btnTab4 = new Button();
            tabCtrlMain = new CustomTabControl();
            tabPNode = new TabPage();
            flpNodeTask = new FlowLayoutPanel();
            panel2 = new Panel();
            flowLayoutPanel4 = new FlowLayoutPanel();
            tableLayoutPanel9 = new TableLayoutPanel();
            flowLayoutPanel3 = new FlowLayoutPanel();
            cbbNodeStatus = new IconComboBox();
            btnEditNode = new Button();
            lblNodeType2 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            lblNodeName2 = new Label();
            lbDescription2 = new Label();
            button1 = new Button();
            tabPWork = new TabPage();
            splitContainer3 = new SplitContainer();
            splitContWorkPanel = new SplitContainer();
            tabPShelude = new TabPage();
            pnlShelude = new Panel();
            tabPStats = new TabPage();
            splitStatMain = new SplitContainer();
            tableLayoutPanel7 = new TableLayoutPanel();
            tblpnlTimeLogs = new TableLayoutPanel();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblStatsNode = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnAddTimelog = new Button();
            tableLayoutPanel6 = new TableLayoutPanel();
            flpAllDeptTimeLogsGraphs = new FlowLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            cbbGraphList = new ComboBox();
            panel3 = new Panel();
            tableLayoutPanel5 = new TableLayoutPanel();
            lblTotalProjectNodes = new Label();
            lblTotalProjectHouresLogged = new Label();
            lblTotalProjectShots = new Label();
            contextMenuTree.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            menuStrip1.SuspendLayout();
            plNodeTaskInfo.SuspendLayout();
            pnlPipelineStatus.SuspendLayout();
            pnlDeptBtn.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlTopRight.SuspendLayout();
            flpNodeDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            tablpanTabBtn.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tabCtrlMain.SuspendLayout();
            tabPNode.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            tabPWork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContWorkPanel).BeginInit();
            splitContWorkPanel.Panel1.SuspendLayout();
            splitContWorkPanel.Panel2.SuspendLayout();
            splitContWorkPanel.SuspendLayout();
            tabPShelude.SuspendLayout();
            tabPStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitStatMain).BeginInit();
            splitStatMain.Panel1.SuspendLayout();
            splitStatMain.Panel2.SuspendLayout();
            splitStatMain.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tblpnlTimeLogs.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // tvNodeList
            // 
            tvNodeList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvNodeList.BackColor = Color.FromArgb(30, 30, 30);
            tvNodeList.BorderStyle = BorderStyle.None;
            tvNodeList.ContextMenuStrip = contextMenuTree;
            tvNodeList.ForeColor = Color.White;
            tvNodeList.LineColor = Color.White;
            tvNodeList.Location = new Point(12, 65);
            tvNodeList.Name = "tvNodeList";
            tvNodeList.Size = new Size(186, 407);
            tvNodeList.TabIndex = 0;
            tvNodeList.AfterSelect += tvNodeList_AfterSelect;
            // 
            // contextMenuTree
            // 
            contextMenuTree.ImageScalingSize = new Size(28, 28);
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
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(btnCreateProduction);
            panel1.Controls.Add(cbProdList);
            panel1.Controls.Add(tvNodeList);
            panel1.Controls.Add(menuStrip1);
            panel1.Dock = DockStyle.Fill;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(220, 546);
            panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(tableLayoutPanel8);
            groupBox1.ForeColor = Color.WhiteSmoke;
            groupBox1.Location = new Point(12, 478);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(185, 56);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Create";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.AutoSize = true;
            tableLayoutPanel8.ColumnCount = 3;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.Controls.Add(button3, 2, 0);
            tableLayoutPanel8.Controls.Add(button2, 1, 0);
            tableLayoutPanel8.Controls.Add(btCreateNode, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 19);
            tableLayoutPanel8.Margin = new Padding(0);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Size = new Size(179, 34);
            tableLayoutPanel8.TabIndex = 10;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(80, 80, 80);
            button3.Dock = DockStyle.Fill;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.ForeColor = Color.White;
            button3.Location = new Point(121, 3);
            button3.Name = "button3";
            button3.Size = new Size(55, 28);
            button3.TabIndex = 3;
            button3.Text = "Shot";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btCreateShot_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(80, 80, 80);
            button2.Dock = DockStyle.Fill;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.White;
            button2.Location = new Point(62, 3);
            button2.Name = "button2";
            button2.Size = new Size(53, 28);
            button2.TabIndex = 2;
            button2.Text = "Seq";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btCreateSeq_Click;
            // 
            // btCreateNode
            // 
            btCreateNode.BackColor = Color.FromArgb(80, 80, 80);
            btCreateNode.Dock = DockStyle.Fill;
            btCreateNode.FlatAppearance.BorderSize = 0;
            btCreateNode.FlatStyle = FlatStyle.Flat;
            btCreateNode.ForeColor = Color.White;
            btCreateNode.Location = new Point(3, 3);
            btCreateNode.Name = "btCreateNode";
            btCreateNode.Size = new Size(53, 28);
            btCreateNode.TabIndex = 1;
            btCreateNode.Text = "Asset";
            btCreateNode.UseVisualStyleBackColor = false;
            btCreateNode.Click += btCreateAsset_Click;
            // 
            // btnCreateProduction
            // 
            btnCreateProduction.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCreateProduction.BackColor = Color.FromArgb(80, 80, 80);
            btnCreateProduction.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnCreateProduction.FlatAppearance.BorderSize = 0;
            btnCreateProduction.FlatStyle = FlatStyle.Flat;
            btnCreateProduction.ForeColor = Color.White;
            btnCreateProduction.Location = new Point(173, 34);
            btnCreateProduction.Name = "btnCreateProduction";
            btnCreateProduction.Size = new Size(25, 25);
            btnCreateProduction.TabIndex = 1;
            btnCreateProduction.Text = "+";
            btnCreateProduction.UseVisualStyleBackColor = false;
            btnCreateProduction.Click += btnCreateProduction_Click;
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
            cbProdList.Size = new Size(155, 23);
            cbProdList.TabIndex = 3;
            cbProdList.SelectedIndexChanged += cbProdList_SelectedIndexChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(80, 80, 80);
            menuStrip1.ImageScalingSize = new Size(28, 28);
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, toolsToolStripMenuItem, toolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(220, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { userSettingsToolStripMenuItem, nodeSettingsToolStripMenuItem, prodSettingsToolStripMenuItem });
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
            userSettingsToolStripMenuItem.Size = new Size(148, 22);
            userSettingsToolStripMenuItem.Text = "User Settings";
            userSettingsToolStripMenuItem.Click += userSettingsToolStripMenuItem_Click;
            // 
            // nodeSettingsToolStripMenuItem
            // 
            nodeSettingsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            nodeSettingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            nodeSettingsToolStripMenuItem.ForeColor = Color.White;
            nodeSettingsToolStripMenuItem.Name = "nodeSettingsToolStripMenuItem";
            nodeSettingsToolStripMenuItem.Size = new Size(148, 22);
            nodeSettingsToolStripMenuItem.Text = "Node Settings";
            nodeSettingsToolStripMenuItem.Click += nodeSettingsToolStripMenuItem_Click;
            // 
            // prodSettingsToolStripMenuItem
            // 
            prodSettingsToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            prodSettingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            prodSettingsToolStripMenuItem.ForeColor = Color.White;
            prodSettingsToolStripMenuItem.Name = "prodSettingsToolStripMenuItem";
            prodSettingsToolStripMenuItem.Size = new Size(148, 22);
            prodSettingsToolStripMenuItem.Text = "Prod Settings";
            prodSettingsToolStripMenuItem.Click += prodSettingsToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { checkFoldersStructureToolStripMenuItem, ensureLocalStructureToolStripMenuItem });
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
            checkFoldersStructureToolStripMenuItem.Size = new Size(245, 22);
            checkFoldersStructureToolStripMenuItem.Text = "Ensure Prod Structure (Dev only)";
            checkFoldersStructureToolStripMenuItem.Click += checkFoldersStructureToolStripMenuItem_Click;
            // 
            // ensureLocalStructureToolStripMenuItem
            // 
            ensureLocalStructureToolStripMenuItem.BackColor = Color.FromArgb(64, 64, 64);
            ensureLocalStructureToolStripMenuItem.ForeColor = Color.White;
            ensureLocalStructureToolStripMenuItem.Name = "ensureLocalStructureToolStripMenuItem";
            ensureLocalStructureToolStripMenuItem.Size = new Size(245, 22);
            ensureLocalStructureToolStripMenuItem.Text = "Ensure Local Structure";
            ensureLocalStructureToolStripMenuItem.Click += ensureLocalStructureToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.ForeColor = Color.White;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(24, 20);
            toolStripMenuItem1.Text = "?";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // plNodeTaskInfo
            // 
            plNodeTaskInfo.BackColor = Color.FromArgb(55, 55, 55);
            plNodeTaskInfo.Controls.Add(flpNodeInspect);
            plNodeTaskInfo.Dock = DockStyle.Fill;
            plNodeTaskInfo.Location = new Point(0, 0);
            plNodeTaskInfo.Name = "plNodeTaskInfo";
            plNodeTaskInfo.Size = new Size(194, 367);
            plNodeTaskInfo.TabIndex = 3;
            // 
            // flpNodeInspect
            // 
            flpNodeInspect.AutoScroll = true;
            flpNodeInspect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpNodeInspect.BackColor = Color.Transparent;
            flpNodeInspect.Dock = DockStyle.Fill;
            flpNodeInspect.FlowDirection = FlowDirection.TopDown;
            flpNodeInspect.Location = new Point(0, 0);
            flpNodeInspect.Name = "flpNodeInspect";
            flpNodeInspect.Size = new Size(194, 367);
            flpNodeInspect.TabIndex = 3;
            flpNodeInspect.WrapContents = false;
            // 
            // lblNodeType
            // 
            lblNodeType.AutoSize = true;
            lblNodeType.BackColor = Color.Transparent;
            lblNodeType.Font = new Font("Nirmala UI", 12F, FontStyle.Bold | FontStyle.Italic);
            lblNodeType.ForeColor = Color.Silver;
            lblNodeType.Location = new Point(16, 44);
            lblNodeType.Margin = new Padding(0);
            lblNodeType.Name = "lblNodeType";
            lblNodeType.Size = new Size(101, 21);
            lblNodeType.TabIndex = 2;
            lblNodeType.Text = "placeholder";
            // 
            // lblNodeName
            // 
            lblNodeName.AutoSize = true;
            lblNodeName.BackColor = Color.Transparent;
            lblNodeName.Font = new Font("Nirmala UI", 20F, FontStyle.Bold);
            lblNodeName.ForeColor = Color.White;
            lblNodeName.Location = new Point(0, 0);
            lblNodeName.Margin = new Padding(0);
            lblNodeName.Name = "lblNodeName";
            lblNodeName.Size = new Size(222, 37);
            lblNodeName.TabIndex = 1;
            lblNodeName.Text = "PLACEHOLDER |";
            lblNodeName.TextAlign = ContentAlignment.BottomLeft;
            // 
            // pnlPipelineStatus
            // 
            pnlPipelineStatus.BackColor = Color.FromArgb(40, 40, 40);
            pnlPipelineStatus.Controls.Add(flpPipelineStatus);
            pnlPipelineStatus.Dock = DockStyle.Fill;
            pnlPipelineStatus.Location = new Point(0, 0);
            pnlPipelineStatus.Name = "pnlPipelineStatus";
            pnlPipelineStatus.Size = new Size(534, 367);
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
            flpPipelineStatus.Size = new Size(534, 367);
            flpPipelineStatus.TabIndex = 3;
            flpPipelineStatus.WrapContents = false;
            // 
            // pnlDeptBtn
            // 
            pnlDeptBtn.Controls.Add(tableLayoutPanel1);
            pnlDeptBtn.Dock = DockStyle.Fill;
            pnlDeptBtn.Location = new Point(0, 0);
            pnlDeptBtn.Name = "pnlDeptBtn";
            pnlDeptBtn.Size = new Size(150, 46);
            pnlDeptBtn.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Center;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(flpDeptButton, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(150, 46);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // flpDeptButton
            // 
            flpDeptButton.BackColor = Color.FromArgb(30, 30, 30);
            flpDeptButton.Location = new Point(-206, 10);
            flpDeptButton.Margin = new Padding(3, 10, 3, 3);
            flpDeptButton.Name = "flpDeptButton";
            flpDeptButton.Padding = new Padding(10, 0, 0, 0);
            flpDeptButton.Size = new Size(563, 33);
            flpDeptButton.TabIndex = 1;
            // 
            // pnlTopRight
            // 
            pnlTopRight.BackColor = Color.FromArgb(65, 65, 65);
            pnlTopRight.Controls.Add(lblNodeType);
            pnlTopRight.Controls.Add(flpNodeDescription);
            pnlTopRight.Dock = DockStyle.Top;
            pnlTopRight.Location = new Point(3, 3);
            pnlTopRight.Name = "pnlTopRight";
            pnlTopRight.Size = new Size(732, 74);
            pnlTopRight.TabIndex = 5;
            // 
            // flpNodeDescription
            // 
            flpNodeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flpNodeDescription.Controls.Add(lblNodeName);
            flpNodeDescription.Controls.Add(lbDescription);
            flpNodeDescription.Location = new Point(7, 3);
            flpNodeDescription.Name = "flpNodeDescription";
            flpNodeDescription.Size = new Size(722, 68);
            flpNodeDescription.TabIndex = 5;
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
            splitMain.Size = new Size(977, 546);
            splitMain.SplitterDistance = 220;
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
            tablpanTabBtn.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tablpanTabBtn.Size = new Size(753, 43);
            tablpanTabBtn.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.None;
            flowLayoutPanel1.Controls.Add(btnTab2);
            flowLayoutPanel1.Controls.Add(btnTab1);
            flowLayoutPanel1.Controls.Add(btnTab3);
            flowLayoutPanel1.Controls.Add(btnTab4);
            flowLayoutPanel1.Location = new Point(37, 8);
            flowLayoutPanel1.Margin = new Padding(8);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(678, 27);
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
            btnTab2.TabIndex = 5;
            btnTab2.Text = "Node";
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
            btnTab1.TabIndex = 4;
            btnTab1.Text = "Workfiles";
            btnTab1.UseVisualStyleBackColor = false;
            btnTab1.Click += btnTab1_Click;
            // 
            // btnTab3
            // 
            btnTab3.BackColor = Color.Transparent;
            btnTab3.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnTab3.FlatStyle = FlatStyle.Flat;
            btnTab3.ForeColor = Color.White;
            btnTab3.Location = new Point(165, 3);
            btnTab3.Name = "btnTab3";
            btnTab3.Size = new Size(75, 23);
            btnTab3.TabIndex = 6;
            btnTab3.Text = "Schedule";
            btnTab3.UseVisualStyleBackColor = false;
            btnTab3.Click += btnTab3_Click;
            // 
            // btnTab4
            // 
            btnTab4.BackColor = Color.Transparent;
            btnTab4.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnTab4.FlatStyle = FlatStyle.Flat;
            btnTab4.ForeColor = Color.White;
            btnTab4.Location = new Point(246, 3);
            btnTab4.Name = "btnTab4";
            btnTab4.Size = new Size(75, 23);
            btnTab4.TabIndex = 7;
            btnTab4.Text = "Stats";
            btnTab4.UseVisualStyleBackColor = false;
            btnTab4.Click += btnTab4_Click;
            // 
            // tabCtrlMain
            // 
            tabCtrlMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabCtrlMain.BackgroundColor = Color.FromArgb(64, 64, 64);
            tabCtrlMain.Controls.Add(tabPNode);
            tabCtrlMain.Controls.Add(tabPWork);
            tabCtrlMain.Controls.Add(tabPShelude);
            tabCtrlMain.Controls.Add(tabPStats);
            tabCtrlMain.ItemSize = new Size(100, 30);
            tabCtrlMain.Location = new Point(0, 49);
            tabCtrlMain.Multiline = true;
            tabCtrlMain.Name = "tabCtrlMain";
            tabCtrlMain.Padding = new Point(10, 5);
            tabCtrlMain.SelectedIndex = 0;
            tabCtrlMain.SelectedTabColor = Color.FromArgb(64, 64, 64);
            tabCtrlMain.SelectedTextColor = Color.White;
            tabCtrlMain.Size = new Size(746, 485);
            tabCtrlMain.SizeMode = TabSizeMode.Fixed;
            tabCtrlMain.TabIndex = 0;
            tabCtrlMain.UnselectedTabColor = Color.FromArgb(30, 30, 30);
            tabCtrlMain.UnselectedTextColor = Color.White;
            // 
            // tabPNode
            // 
            tabPNode.BackColor = Color.FromArgb(40, 40, 40);
            tabPNode.Controls.Add(flpNodeTask);
            tabPNode.Controls.Add(panel2);
            tabPNode.Location = new Point(4, 34);
            tabPNode.Name = "tabPNode";
            tabPNode.Padding = new Padding(3);
            tabPNode.Size = new Size(738, 447);
            tabPNode.TabIndex = 1;
            tabPNode.Text = "Node";
            // 
            // flpNodeTask
            // 
            flpNodeTask.AutoScroll = true;
            flpNodeTask.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpNodeTask.Dock = DockStyle.Fill;
            flpNodeTask.FlowDirection = FlowDirection.TopDown;
            flpNodeTask.Location = new Point(3, 77);
            flpNodeTask.Name = "flpNodeTask";
            flpNodeTask.Size = new Size(732, 367);
            flpNodeTask.TabIndex = 7;
            flpNodeTask.WrapContents = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(65, 65, 65);
            panel2.Controls.Add(flowLayoutPanel4);
            panel2.Controls.Add(lblNodeType2);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(732, 74);
            panel2.TabIndex = 6;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(tableLayoutPanel9);
            flowLayoutPanel4.Controls.Add(btnEditNode);
            flowLayoutPanel4.Dock = DockStyle.Right;
            flowLayoutPanel4.Location = new Point(518, 0);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(214, 74);
            flowLayoutPanel4.TabIndex = 10;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.BackColor = Color.FromArgb(65, 65, 65);
            tableLayoutPanel9.BackgroundImageLayout = ImageLayout.Center;
            tableLayoutPanel9.ColumnCount = 1;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Controls.Add(flowLayoutPanel3, 0, 1);
            tableLayoutPanel9.Dock = DockStyle.Left;
            tableLayoutPanel9.Location = new Point(0, 0);
            tableLayoutPanel9.Margin = new Padding(0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 3;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle());
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50.0000076F));
            tableLayoutPanel9.Size = new Size(141, 67);
            tableLayoutPanel9.TabIndex = 8;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(cbbNodeStatus);
            flowLayoutPanel3.Location = new Point(8, 19);
            flowLayoutPanel3.Margin = new Padding(8);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(125, 27);
            flowLayoutPanel3.TabIndex = 1;
            // 
            // cbbNodeStatus
            // 
            cbbNodeStatus.BackColor = Color.FromArgb(60, 60, 60);
            cbbNodeStatus.CausesValidation = false;
            cbbNodeStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cbbNodeStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNodeStatus.FlatStyle = FlatStyle.Flat;
            cbbNodeStatus.ForeColor = Color.White;
            cbbNodeStatus.FormattingEnabled = true;
            cbbNodeStatus.IconMap = (Dictionary<string, Image>)resources.GetObject("cbbNodeStatus.IconMap");
            cbbNodeStatus.Location = new Point(3, 3);
            cbbNodeStatus.Name = "cbbNodeStatus";
            cbbNodeStatus.Size = new Size(120, 24);
            cbbNodeStatus.TabIndex = 7;
            cbbNodeStatus.SelectedIndexChanged += cbbNodeStatus_SelectedIndexChanged;
            // 
            // btnEditNode
            // 
            btnEditNode.BackColor = Color.Transparent;
            btnEditNode.Cursor = Cursors.Hand;
            btnEditNode.FlatAppearance.BorderSize = 0;
            btnEditNode.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnEditNode.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnEditNode.FlatStyle = FlatStyle.Flat;
            btnEditNode.Font = new Font("Segoe UI", 15F);
            btnEditNode.ForeColor = Color.White;
            btnEditNode.Image = Properties.Resources.Save;
            btnEditNode.Location = new Point(144, 3);
            btnEditNode.Name = "btnEditNode";
            btnEditNode.Size = new Size(55, 61);
            btnEditNode.TabIndex = 9;
            btnEditNode.UseVisualStyleBackColor = false;
            // 
            // lblNodeType2
            // 
            lblNodeType2.AutoSize = true;
            lblNodeType2.BackColor = Color.Transparent;
            lblNodeType2.Font = new Font("Nirmala UI", 12F, FontStyle.Bold | FontStyle.Italic);
            lblNodeType2.ForeColor = Color.Silver;
            lblNodeType2.Location = new Point(16, 44);
            lblNodeType2.Margin = new Padding(0);
            lblNodeType2.Name = "lblNodeType2";
            lblNodeType2.Size = new Size(101, 21);
            lblNodeType2.TabIndex = 2;
            lblNodeType2.Text = "placeholder";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel2.Controls.Add(lblNodeName2);
            flowLayoutPanel2.Controls.Add(lbDescription2);
            flowLayoutPanel2.Location = new Point(7, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(505, 42);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // lblNodeName2
            // 
            lblNodeName2.AutoSize = true;
            lblNodeName2.BackColor = Color.Transparent;
            lblNodeName2.Font = new Font("Nirmala UI", 20F, FontStyle.Bold);
            lblNodeName2.ForeColor = Color.White;
            lblNodeName2.Location = new Point(0, 0);
            lblNodeName2.Margin = new Padding(0);
            lblNodeName2.Name = "lblNodeName2";
            lblNodeName2.Size = new Size(222, 37);
            lblNodeName2.TabIndex = 1;
            lblNodeName2.Text = "PLACEHOLDER |";
            lblNodeName2.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lbDescription2
            // 
            lbDescription2.AutoEllipsis = true;
            lbDescription2.AutoSize = true;
            lbDescription2.BackColor = Color.Transparent;
            flowLayoutPanel2.SetFlowBreak(lbDescription2, true);
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
            button1.Location = new Point(1220, 3);
            button1.Name = "button1";
            button1.Size = new Size(55, 35);
            button1.TabIndex = 3;
            button1.UseVisualStyleBackColor = false;
            // 
            // tabPWork
            // 
            tabPWork.BackColor = Color.FromArgb(40, 40, 40);
            tabPWork.Controls.Add(splitContainer3);
            tabPWork.Controls.Add(pnlTopRight);
            tabPWork.Location = new Point(4, 34);
            tabPWork.Name = "tabPWork";
            tabPWork.Padding = new Padding(3);
            tabPWork.Size = new Size(738, 447);
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
            splitContainer3.Panel1.Controls.Add(splitContWorkPanel);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(plNodeTaskInfo);
            splitContainer3.Size = new Size(732, 367);
            splitContainer3.SplitterDistance = 534;
            splitContainer3.TabIndex = 6;
            // 
            // splitContWorkPanel
            // 
            splitContWorkPanel.Dock = DockStyle.Fill;
            splitContWorkPanel.IsSplitterFixed = true;
            splitContWorkPanel.Location = new Point(0, 0);
            splitContWorkPanel.Name = "splitContWorkPanel";
            splitContWorkPanel.Orientation = Orientation.Horizontal;
            // 
            // splitContWorkPanel.Panel1
            // 
            splitContWorkPanel.Panel1.Controls.Add(pnlPipelineStatus);
            // 
            // splitContWorkPanel.Panel2
            // 
            splitContWorkPanel.Panel2.Controls.Add(pnlDeptBtn);
            splitContWorkPanel.Panel2Collapsed = true;
            splitContWorkPanel.Size = new Size(534, 367);
            splitContWorkPanel.SplitterDistance = 313;
            splitContWorkPanel.TabIndex = 5;
            // 
            // tabPShelude
            // 
            tabPShelude.AutoScroll = true;
            tabPShelude.BackColor = Color.FromArgb(40, 40, 40);
            tabPShelude.Controls.Add(pnlShelude);
            tabPShelude.Location = new Point(4, 34);
            tabPShelude.Name = "tabPShelude";
            tabPShelude.Size = new Size(738, 447);
            tabPShelude.TabIndex = 2;
            tabPShelude.Text = "Shelude";
            // 
            // pnlShelude
            // 
            pnlShelude.AutoScroll = true;
            pnlShelude.Dock = DockStyle.Fill;
            pnlShelude.Location = new Point(0, 0);
            pnlShelude.Name = "pnlShelude";
            pnlShelude.Size = new Size(738, 447);
            pnlShelude.TabIndex = 0;
            // 
            // tabPStats
            // 
            tabPStats.BackColor = Color.FromArgb(40, 40, 40);
            tabPStats.Controls.Add(splitStatMain);
            tabPStats.Location = new Point(4, 34);
            tabPStats.Name = "tabPStats";
            tabPStats.Size = new Size(738, 447);
            tabPStats.TabIndex = 3;
            tabPStats.Text = "Stats";
            // 
            // splitStatMain
            // 
            splitStatMain.Dock = DockStyle.Fill;
            splitStatMain.Location = new Point(0, 0);
            splitStatMain.Name = "splitStatMain";
            splitStatMain.Orientation = Orientation.Horizontal;
            // 
            // splitStatMain.Panel1
            // 
            splitStatMain.Panel1.Controls.Add(tableLayoutPanel7);
            // 
            // splitStatMain.Panel2
            // 
            splitStatMain.Panel2.Controls.Add(tableLayoutPanel6);
            splitStatMain.Size = new Size(738, 447);
            splitStatMain.SplitterDistance = 100;
            splitStatMain.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(tblpnlTimeLogs, 0, 1);
            tableLayoutPanel7.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(0, 0);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle());
            tableLayoutPanel7.Size = new Size(738, 100);
            tableLayoutPanel7.TabIndex = 1;
            // 
            // tblpnlTimeLogs
            // 
            tblpnlTimeLogs.AutoScroll = true;
            tblpnlTimeLogs.ColumnCount = 5;
            tblpnlTimeLogs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblpnlTimeLogs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblpnlTimeLogs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblpnlTimeLogs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblpnlTimeLogs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblpnlTimeLogs.Controls.Add(label4, 4, 0);
            tblpnlTimeLogs.Controls.Add(label3, 3, 0);
            tblpnlTimeLogs.Controls.Add(label2, 2, 0);
            tblpnlTimeLogs.Controls.Add(label1, 1, 0);
            tblpnlTimeLogs.Controls.Add(lblStatsNode, 0, 0);
            tblpnlTimeLogs.Dock = DockStyle.Fill;
            tblpnlTimeLogs.Location = new Point(0, 40);
            tblpnlTimeLogs.Margin = new Padding(0);
            tblpnlTimeLogs.Name = "tblpnlTimeLogs";
            tblpnlTimeLogs.RowCount = 2;
            tblpnlTimeLogs.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblpnlTimeLogs.RowStyles.Add(new RowStyle());
            tblpnlTimeLogs.Size = new Size(738, 95);
            tblpnlTimeLogs.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(80, 80, 80);
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(588, 0);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(150, 20);
            label4.TabIndex = 4;
            label4.Text = "Last Update";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(80, 80, 80);
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(441, 0);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(147, 20);
            label3.TabIndex = 3;
            label3.Text = "Hours Logged";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(80, 80, 80);
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(294, 0);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(147, 20);
            label2.TabIndex = 2;
            label2.Text = "Artist";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(80, 80, 80);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(147, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(147, 20);
            label1.TabIndex = 1;
            label1.Text = "Department";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStatsNode
            // 
            lblStatsNode.AutoSize = true;
            lblStatsNode.BackColor = Color.FromArgb(80, 80, 80);
            lblStatsNode.Dock = DockStyle.Fill;
            lblStatsNode.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatsNode.ForeColor = Color.White;
            lblStatsNode.Location = new Point(0, 0);
            lblStatsNode.Margin = new Padding(0);
            lblStatsNode.Name = "lblStatsNode";
            lblStatsNode.Size = new Size(147, 20);
            lblStatsNode.TabIndex = 0;
            lblStatsNode.Text = "Node";
            lblStatsNode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 237F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(btnAddTimelog, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(732, 34);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // btnAddTimelog
            // 
            btnAddTimelog.BackColor = Color.FromArgb(80, 80, 80);
            btnAddTimelog.Dock = DockStyle.Fill;
            btnAddTimelog.FlatStyle = FlatStyle.Flat;
            btnAddTimelog.Font = new Font("Segoe UI", 10F);
            btnAddTimelog.ForeColor = Color.White;
            btnAddTimelog.Location = new Point(250, 3);
            btnAddTimelog.Name = "btnAddTimelog";
            btnAddTimelog.Size = new Size(231, 28);
            btnAddTimelog.TabIndex = 0;
            btnAddTimelog.Text = "Add Time Log";
            btnAddTimelog.UseVisualStyleBackColor = false;
            btnAddTimelog.Click += btnAddTimelog_Click;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(flpAllDeptTimeLogsGraphs, 0, 2);
            tableLayoutPanel6.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel6.Controls.Add(panel3, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 3;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle());
            tableLayoutPanel6.Size = new Size(738, 343);
            tableLayoutPanel6.TabIndex = 1;
            // 
            // flpAllDeptTimeLogsGraphs
            // 
            flpAllDeptTimeLogsGraphs.AutoScroll = true;
            flpAllDeptTimeLogsGraphs.AutoSize = true;
            flpAllDeptTimeLogsGraphs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpAllDeptTimeLogsGraphs.Dock = DockStyle.Fill;
            flpAllDeptTimeLogsGraphs.FlowDirection = FlowDirection.TopDown;
            flpAllDeptTimeLogsGraphs.Location = new Point(3, 73);
            flpAllDeptTimeLogsGraphs.Name = "flpAllDeptTimeLogsGraphs";
            flpAllDeptTimeLogsGraphs.Size = new Size(732, 267);
            flpAllDeptTimeLogsGraphs.TabIndex = 11;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 345F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(cbbGraphList, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 33);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(732, 34);
            tableLayoutPanel4.TabIndex = 9;
            // 
            // cbbGraphList
            // 
            cbbGraphList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbbGraphList.BackColor = Color.FromArgb(60, 60, 60);
            cbbGraphList.CausesValidation = false;
            cbbGraphList.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbGraphList.FlatStyle = FlatStyle.Flat;
            cbbGraphList.ForeColor = Color.White;
            cbbGraphList.FormattingEnabled = true;
            cbbGraphList.Items.AddRange(new object[] { "----------------", "Hours by Departments", "Hours by Artists", "----------------", "Hours by Characters", "Hours by Props", "Hours by Environments", "Hours by Sequences", "Hours by Shot", "----------------", "Production Percent" });
            cbbGraphList.Location = new Point(196, 3);
            cbbGraphList.Name = "cbbGraphList";
            cbbGraphList.Size = new Size(339, 23);
            cbbGraphList.TabIndex = 0;
            cbbGraphList.SelectedIndexChanged += cbbGraphList_SelectedIndexChanged;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(30, 30, 30);
            panel3.Controls.Add(tableLayoutPanel5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(732, 24);
            panel3.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel5.Controls.Add(lblTotalProjectNodes, 1, 0);
            tableLayoutPanel5.Controls.Add(lblTotalProjectHouresLogged, 0, 0);
            tableLayoutPanel5.Controls.Add(lblTotalProjectShots, 2, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel5.Size = new Size(732, 24);
            tableLayoutPanel5.TabIndex = 1;
            // 
            // lblTotalProjectNodes
            // 
            lblTotalProjectNodes.AutoSize = true;
            lblTotalProjectNodes.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalProjectNodes.ForeColor = Color.White;
            lblTotalProjectNodes.ImageAlign = ContentAlignment.BottomRight;
            lblTotalProjectNodes.Location = new Point(246, 0);
            lblTotalProjectNodes.Name = "lblTotalProjectNodes";
            lblTotalProjectNodes.Size = new Size(171, 21);
            lblTotalProjectNodes.TabIndex = 1;
            lblTotalProjectNodes.Text = "lblTotalProjectNodes";
            lblTotalProjectNodes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalProjectHouresLogged
            // 
            lblTotalProjectHouresLogged.AutoSize = true;
            lblTotalProjectHouresLogged.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalProjectHouresLogged.ForeColor = Color.White;
            lblTotalProjectHouresLogged.ImageAlign = ContentAlignment.BottomRight;
            lblTotalProjectHouresLogged.Location = new Point(3, 0);
            lblTotalProjectHouresLogged.Name = "lblTotalProjectHouresLogged";
            lblTotalProjectHouresLogged.Size = new Size(233, 21);
            lblTotalProjectHouresLogged.TabIndex = 0;
            lblTotalProjectHouresLogged.Text = "lblTotalProjectHouresLogged";
            lblTotalProjectHouresLogged.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalProjectShots
            // 
            lblTotalProjectShots.AutoSize = true;
            lblTotalProjectShots.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalProjectShots.ForeColor = Color.White;
            lblTotalProjectShots.ImageAlign = ContentAlignment.BottomRight;
            lblTotalProjectShots.Location = new Point(490, 0);
            lblTotalProjectShots.Name = "lblTotalProjectShots";
            lblTotalProjectShots.Size = new Size(164, 21);
            lblTotalProjectShots.TabIndex = 2;
            lblTotalProjectShots.Text = "lblTotalProjectShots";
            lblTotalProjectShots.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AssetManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(977, 546);
            Controls.Add(splitMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "AssetManagerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DuckPipe";
            Load += AssetManagerForm_Load;
            contextMenuTree.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            plNodeTaskInfo.ResumeLayout(false);
            pnlPipelineStatus.ResumeLayout(false);
            pnlDeptBtn.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            pnlTopRight.ResumeLayout(false);
            pnlTopRight.PerformLayout();
            flpNodeDescription.ResumeLayout(false);
            flpNodeDescription.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            tablpanTabBtn.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            tabCtrlMain.ResumeLayout(false);
            tabPNode.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            tabPWork.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            splitContWorkPanel.Panel1.ResumeLayout(false);
            splitContWorkPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContWorkPanel).EndInit();
            splitContWorkPanel.ResumeLayout(false);
            tabPShelude.ResumeLayout(false);
            tabPStats.ResumeLayout(false);
            splitStatMain.Panel1.ResumeLayout(false);
            splitStatMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitStatMain).EndInit();
            splitStatMain.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tblpnlTimeLogs.ResumeLayout(false);
            tblpnlTimeLogs.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
        }
        #endregion

        private TreeView tvNodeList;
        private Panel panel1;
        private Button btnCreateProduction;
        private ContextMenuStrip contextMenuTree;
        private ToolStripMenuItem tsmiRename;
        private ToolStripMenuItem tsmiDelete;
        private Panel plNodeTaskInfo;
        private Label lblNodeType;
        private Label lblNodeName;
        private Panel pnlPipelineStatus;
        private Panel pnlTopRight;
        private SplitContainer splitMain;
        private SplitContainer splitContainer3;
        private FlowLayoutPanel flpNodeInspect;
        private Panel pnlDeptBtn;
        public ComboBox cbProdList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem userSettingsToolStripMenuItem;
        private ToolStripMenuItem nodeSettingsToolStripMenuItem;
        private ToolStripMenuItem prodSettingsToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem viewInExplorerToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem checkFoldersStructureToolStripMenuItem;
        private Label lbDescription;
        private FlowLayoutPanel flpNodeDescription;
        private CustomTabControl tabCtrlMain;
        private TabPage tabPWork;
        private TabPage tabPNode;
        private TableLayoutPanel tablpanTabBtn;
        private FlowLayoutPanel flowLayoutPanel1;
        private TabPage tabPShelude;
        private Panel panel2;
        private Label lblNodeType2;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label lblNodeName2;
        private Label lbDescription2;
        private Button button1;
        private FlowLayoutPanel flpNodeTask;
        private Panel pnlShelude;
        private TabPage tabPStats;
        private SplitContainer splitStatMain;
        private TableLayoutPanel tableLayoutPanel3;
        private Button btnAddTimelog;
        private Panel panel3;
        private Label lblTotalProjectHouresLogged;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private Label lblTotalProjectShots;
        private Label lblTotalProjectNodes;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tblpnlTimeLogs;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label lblStatsNode;
        private TableLayoutPanel tableLayoutPanel4;
        public ComboBox cbbGraphList;
        private FlowLayoutPanel flpAllDeptTimeLogsGraphs;
        public IconComboBox cbbNodeStatus;
        private GroupBox groupBox1;
        private Button btCreateNode;
        private Button button2;
        private Button button3;
        private TableLayoutPanel tableLayoutPanel8;
        private FlowLayoutPanel flpPipelineStatus;
        internal SplitContainer splitContWorkPanel;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flpDeptButton;
        private TableLayoutPanel tableLayoutPanel9;
        private FlowLayoutPanel flowLayoutPanel3;
        private Button btnEditNode;
        private FlowLayoutPanel flowLayoutPanel4;
        private ToolStripMenuItem ensureLocalStructureToolStripMenuItem;
        private Button btnTab2;
        private Button btnTab1;
        private Button btnTab3;
        private Button btnTab4;
    }
}