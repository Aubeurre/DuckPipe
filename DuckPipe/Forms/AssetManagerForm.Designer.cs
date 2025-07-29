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
            toolStripMenuItem1 = new ToolStripMenuItem();
            plAssetTaskInfo = new Panel();
            flpAssetInspect = new FlowLayoutPanel();
            lblAssetType = new Label();
            lblAssetName = new Label();
            pnlPipelineStatus = new Panel();
            flpPipelineStatus = new FlowLayoutPanel();
            pnlDeptBtn = new Panel();
            flpDeptButton = new FlowLayoutPanel();
            pnlTopRight = new Panel();
            btnEditAsset = new Button();
            splitContainer2 = new SplitContainer();
            splitContainer3 = new SplitContainer();
            contextMenuTree.SuspendLayout();
            panel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            plAssetTaskInfo.SuspendLayout();
            pnlPipelineStatus.SuspendLayout();
            pnlDeptBtn.SuspendLayout();
            pnlTopRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
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
            tvAssetList.Size = new Size(230, 441);
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
            panel1.Size = new Size(253, 543);
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
            btnCreateProduction.Location = new Point(217, 34);
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
            btCreateAsset.Location = new Point(12, 510);
            btCreateAsset.Name = "btCreateAsset";
            btCreateAsset.Size = new Size(230, 25);
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
            cbProdList.Size = new Size(199, 23);
            cbProdList.TabIndex = 3;
            cbProdList.SelectedIndexChanged += cbProdList_SelectedIndexChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(80, 80, 80);
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, toolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(253, 24);
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
            plAssetTaskInfo.Size = new Size(205, 469);
            plAssetTaskInfo.TabIndex = 3;
            // 
            // flpAssetInspect
            // 
            flpAssetInspect.AutoScroll = true;
            flpAssetInspect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpAssetInspect.Dock = DockStyle.Fill;
            flpAssetInspect.FlowDirection = FlowDirection.TopDown;
            flpAssetInspect.Location = new Point(0, 0);
            flpAssetInspect.Name = "flpAssetInspect";
            flpAssetInspect.Size = new Size(205, 469);
            flpAssetInspect.TabIndex = 3;
            flpAssetInspect.WrapContents = false;
            // 
            // lblAssetType
            // 
            lblAssetType.AutoSize = true;
            lblAssetType.BackColor = Color.Transparent;
            lblAssetType.Font = new Font("Nirmala UI", 12F, FontStyle.Bold | FontStyle.Italic);
            lblAssetType.ForeColor = Color.Silver;
            lblAssetType.Location = new Point(10, 43);
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
            lblAssetName.Location = new Point(6, 6);
            lblAssetName.Margin = new Padding(0);
            lblAssetName.Name = "lblAssetName";
            lblAssetName.Size = new Size(206, 37);
            lblAssetName.TabIndex = 1;
            lblAssetName.Text = "PLACEHOLDER";
            lblAssetName.TextAlign = ContentAlignment.BottomLeft;
            // 
            // pnlPipelineStatus
            // 
            pnlPipelineStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlPipelineStatus.BackColor = Color.FromArgb(40, 40, 40);
            pnlPipelineStatus.Controls.Add(flpPipelineStatus);
            pnlPipelineStatus.Location = new Point(0, 0);
            pnlPipelineStatus.Name = "pnlPipelineStatus";
            pnlPipelineStatus.Size = new Size(634, 421);
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
            flpPipelineStatus.Size = new Size(634, 421);
            flpPipelineStatus.TabIndex = 3;
            flpPipelineStatus.WrapContents = false;
            // 
            // pnlDeptBtn
            // 
            pnlDeptBtn.Controls.Add(flpDeptButton);
            pnlDeptBtn.Dock = DockStyle.Bottom;
            pnlDeptBtn.Location = new Point(0, 427);
            pnlDeptBtn.Name = "pnlDeptBtn";
            pnlDeptBtn.Size = new Size(634, 42);
            pnlDeptBtn.TabIndex = 4;
            // 
            // flpDeptButton
            // 
            flpDeptButton.Dock = DockStyle.Fill;
            flpDeptButton.Location = new Point(0, 0);
            flpDeptButton.Name = "flpDeptButton";
            flpDeptButton.Size = new Size(634, 42);
            flpDeptButton.TabIndex = 0;
            // 
            // pnlTopRight
            // 
            pnlTopRight.BackColor = Color.FromArgb(65, 65, 65);
            pnlTopRight.Controls.Add(btnEditAsset);
            pnlTopRight.Controls.Add(lblAssetType);
            pnlTopRight.Controls.Add(lblAssetName);
            pnlTopRight.Dock = DockStyle.Top;
            pnlTopRight.Location = new Point(0, 0);
            pnlTopRight.Name = "pnlTopRight";
            pnlTopRight.Size = new Size(843, 74);
            pnlTopRight.TabIndex = 5;
            // 
            // btnEditAsset
            // 
            btnEditAsset.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditAsset.BackColor = Color.FromArgb(80, 80, 80);
            btnEditAsset.FlatAppearance.BorderSize = 0;
            btnEditAsset.FlatStyle = FlatStyle.Flat;
            btnEditAsset.Font = new Font("Segoe UI", 15F);
            btnEditAsset.ForeColor = Color.White;
            btnEditAsset.Location = new Point(776, 12);
            btnEditAsset.Name = "btnEditAsset";
            btnEditAsset.Size = new Size(55, 52);
            btnEditAsset.TabIndex = 3;
            btnEditAsset.Text = "Edit";
            btnEditAsset.UseVisualStyleBackColor = false;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(panel1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer3);
            splitContainer2.Panel2.Controls.Add(pnlTopRight);
            splitContainer2.Size = new Size(1100, 543);
            splitContainer2.SplitterDistance = 253;
            splitContainer2.TabIndex = 6;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 74);
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
            splitContainer3.Size = new Size(843, 469);
            splitContainer3.SplitterDistance = 634;
            splitContainer3.TabIndex = 6;
            // 
            // AssetManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(1100, 543);
            Controls.Add(splitContainer2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "AssetManagerForm";
            Text = "DuckPipe";
            contextMenuTree.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            plAssetTaskInfo.ResumeLayout(false);
            pnlPipelineStatus.ResumeLayout(false);
            pnlDeptBtn.ResumeLayout(false);
            pnlTopRight.ResumeLayout(false);
            pnlTopRight.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
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
        private SplitContainer splitContainer2;
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
        private Button btnEditAsset;
    }
}