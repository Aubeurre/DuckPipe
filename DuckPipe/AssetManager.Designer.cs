using System.Text.Json;

namespace DuckPipe
{
    partial class AssetManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetManager));
            tvAssetList = new TreeView();
            contextMenuTree = new ContextMenuStrip(components);
            tsmiRename = new ToolStripMenuItem();
            tsmiDelete = new ToolStripMenuItem();
            panel1 = new Panel();
            btnCreateProduction = new Button();
            btCreateAsset = new Button();
            cbProdList = new ComboBox();
            plAssetTaskInfo = new Panel();
            lblAssetType = new Label();
            lblAssetName = new Label();
            pnlPipelineStatus = new Panel();
            flpPipelineStatus = new FlowLayoutPanel();
            panel2 = new Panel();
            splitContainer2 = new SplitContainer();
            splitContainer3 = new SplitContainer();
            contextMenuTree.SuspendLayout();
            panel1.SuspendLayout();
            pnlPipelineStatus.SuspendLayout();
            panel2.SuspendLayout();
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
            tvAssetList.Location = new Point(12, 43);
            tvAssetList.Name = "tvAssetList";
            tvAssetList.Size = new Size(289, 476);
            tvAssetList.TabIndex = 0;
            tvAssetList.AfterSelect += tvAssetList_AfterSelect;
            // 
            // contextMenuTree
            // 
            contextMenuTree.Items.AddRange(new ToolStripItem[] { tsmiRename, tsmiDelete });
            contextMenuTree.Name = "contextMenuTree";
            contextMenuTree.Size = new Size(134, 48);
            contextMenuTree.Opening += contextMenuTree_Opening;
            // 
            // tsmiRename
            // 
            tsmiRename.Name = "tsmiRename";
            tsmiRename.Size = new Size(133, 22);
            tsmiRename.Text = "Renommer";
            tsmiRename.Click += tsmiRename_Click;
            // 
            // tsmiDelete
            // 
            tsmiDelete.Name = "tsmiDelete";
            tsmiDelete.Size = new Size(133, 22);
            tsmiDelete.Text = "Supprimer";
            tsmiDelete.Click += tsmiDelete_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 45, 45);
            panel1.Controls.Add(btnCreateProduction);
            panel1.Controls.Add(btCreateAsset);
            panel1.Controls.Add(cbProdList);
            panel1.Controls.Add(tvAssetList);
            panel1.Dock = DockStyle.Fill;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(312, 556);
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
            btnCreateProduction.Location = new Point(276, 12);
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
            btCreateAsset.Location = new Point(12, 523);
            btCreateAsset.Name = "btCreateAsset";
            btCreateAsset.Size = new Size(289, 25);
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
            cbProdList.Location = new Point(12, 12);
            cbProdList.Name = "cbProdList";
            cbProdList.Size = new Size(258, 23);
            cbProdList.TabIndex = 3;
            cbProdList.SelectedIndexChanged += cbProdList_SelectedIndexChanged;
            // 
            // plAssetTaskInfo
            // 
            plAssetTaskInfo.BackColor = Color.FromArgb(55, 55, 55);
            plAssetTaskInfo.Dock = DockStyle.Fill;
            plAssetTaskInfo.Location = new Point(0, 0);
            plAssetTaskInfo.Name = "plAssetTaskInfo";
            plAssetTaskInfo.Size = new Size(796, 131);
            plAssetTaskInfo.TabIndex = 3;
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
            pnlPipelineStatus.BackColor = Color.FromArgb(40, 40, 40);
            pnlPipelineStatus.Controls.Add(flpPipelineStatus);
            pnlPipelineStatus.Dock = DockStyle.Fill;
            pnlPipelineStatus.Location = new Point(0, 0);
            pnlPipelineStatus.Name = "pnlPipelineStatus";
            pnlPipelineStatus.Size = new Size(796, 347);
            pnlPipelineStatus.TabIndex = 4;
            // 
            // flpPipelineStatus
            // 
            flpPipelineStatus.AutoScroll = true;
            flpPipelineStatus.Dock = DockStyle.Top;
            flpPipelineStatus.FlowDirection = FlowDirection.TopDown;
            flpPipelineStatus.Location = new Point(0, 0);
            flpPipelineStatus.Name = "flpPipelineStatus";
            flpPipelineStatus.Size = new Size(796, 347);
            flpPipelineStatus.TabIndex = 2;
            flpPipelineStatus.WrapContents = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(65, 65, 65);
            panel2.Controls.Add(lblAssetType);
            panel2.Controls.Add(lblAssetName);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(796, 74);
            panel2.TabIndex = 5;
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
            splitContainer2.Panel2.Controls.Add(panel2);
            splitContainer2.Size = new Size(1112, 556);
            splitContainer2.SplitterDistance = 312;
            splitContainer2.TabIndex = 6;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 74);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(pnlPipelineStatus);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(plAssetTaskInfo);
            splitContainer3.Size = new Size(796, 482);
            splitContainer3.SplitterDistance = 347;
            splitContainer3.TabIndex = 6;
            // 
            // AssetManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(1112, 556);
            Controls.Add(splitContainer2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AssetManager";
            Text = "DuckPipe";
            contextMenuTree.ResumeLayout(false);
            panel1.ResumeLayout(false);
            pnlPipelineStatus.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private ComboBox cbProdList;
        private Button btCreateAsset;
        private Button btnCreateProduction;
        private ContextMenuStrip contextMenuTree;
        private ToolStripMenuItem tsmiRename;
        private ToolStripMenuItem tsmiDelete;
        private Panel plAssetTaskInfo;
        private Label lblAssetType;
        private Label lblAssetName;
        private Panel pnlPipelineStatus;
        private Panel panel2;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer3;
        private FlowLayoutPanel flpPipelineStatus;
    }
}