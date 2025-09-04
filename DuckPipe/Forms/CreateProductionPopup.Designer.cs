namespace DuckPipe
{
    partial class CreateProductionPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ListViewGroup listViewGroup1 = new ListViewGroup("Assets", HorizontalAlignment.Center);
            ListViewGroup listViewGroup2 = new ListViewGroup("Shots", HorizontalAlignment.Center);
            ListViewGroup listViewGroup3 = new ListViewGroup("Art", HorizontalAlignment.Center);
            ListViewGroup listViewGroup4 = new ListViewGroup("WorkFile", HorizontalAlignment.Center);
            ListViewItem listViewItem1 = new ListViewItem(new string[] { "Anim" }, -1, Color.Tomato, Color.Empty, null);
            ListViewItem listViewItem2 = new ListViewItem(new string[] { "Assemble" }, -1, Color.LightCoral, Color.Empty, null);
            ListViewItem listViewItem3 = new ListViewItem("blend");
            ListViewItem listViewItem4 = new ListViewItem(new string[] { "Board" }, -1, Color.SandyBrown, Color.Empty, null);
            ListViewItem listViewItem5 = new ListViewItem(new string[] { "Cfx" }, -1, Color.IndianRed, Color.Empty, null);
            ListViewItem listViewItem6 = new ListViewItem(new string[] { "CfxShot" }, -1, Color.OliveDrab, Color.Empty, null);
            ListViewItem listViewItem7 = new ListViewItem(new string[] { "Comp" }, -1, Color.SkyBlue, Color.Empty, null);
            ListViewItem listViewItem8 = new ListViewItem(new string[] { "Concept" }, -1, Color.Violet, Color.Empty, null);
            ListViewItem listViewItem9 = new ListViewItem(new string[] { "Facial" }, -1, Color.GreenYellow, Color.Empty, null);
            ListViewItem listViewItem10 = new ListViewItem(new string[] { "Fx" }, -1, Color.Pink, Color.Empty, null);
            ListViewItem listViewItem11 = new ListViewItem(new string[] { "Groom" }, -1, Color.MediumSlateBlue, Color.Empty, null);
            ListViewItem listViewItem12 = new ListViewItem(new string[] { "Layout" }, -1, Color.LimeGreen, Color.Empty, null);
            ListViewItem listViewItem13 = new ListViewItem(new string[] { "Light" }, -1, Color.Gold, Color.Empty, null);
            ListViewItem listViewItem14 = new ListViewItem("ma");
            ListViewItem listViewItem15 = new ListViewItem(new string[] { "Model" }, -1, Color.LightSkyBlue, Color.Empty, null);
            ListViewItem listViewItem16 = new ListViewItem(new string[] { "Rig" }, -1, Color.Orange, Color.Empty, null);
            ListViewItem listViewItem17 = new ListViewItem("spp");
            ListViewItem listViewItem18 = new ListViewItem(new string[] { "Surf" }, -1, Color.RoyalBlue, Color.Empty, null);
            label1 = new Label();
            txtProductionName = new TextBox();
            btnOK = new Button();
            pnlGrid = new Panel();
            tlpnlType = new TableLayoutPanel();
            flpSequencesPanel = new FlowLayoutPanel();
            flpEnvironmentsPanel = new FlowLayoutPanel();
            flpCharactersPanel = new FlowLayoutPanel();
            flpPropsPanel = new FlowLayoutPanel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            lblProps = new Label();
            flpShotsPanel = new FlowLayoutPanel();
            lviewDeptNames = new ListView();
            pnlGrid.SuspendLayout();
            tlpnlType.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(220, 20);
            label1.Name = "label1";
            label1.Size = new Size(124, 15);
            label1.TabIndex = 0;
            label1.Text = "Nom de la production";
            // 
            // txtProductionName
            // 
            txtProductionName.BackColor = Color.FromArgb(30, 30, 30);
            txtProductionName.BorderStyle = BorderStyle.FixedSingle;
            txtProductionName.ForeColor = Color.White;
            txtProductionName.Location = new Point(350, 15);
            txtProductionName.Name = "txtProductionName";
            txtProductionName.Size = new Size(100, 23);
            txtProductionName.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOK.BackColor = Color.FromArgb(80, 80, 80);
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(729, 13);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(108, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click_1;
            // 
            // pnlGrid
            // 
            pnlGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlGrid.Controls.Add(tlpnlType);
            pnlGrid.Location = new Point(12, 57);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new Size(666, 472);
            pnlGrid.TabIndex = 3;
            // 
            // tlpnlType
            // 
            tlpnlType.ColumnCount = 5;
            tlpnlType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpnlType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpnlType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpnlType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpnlType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpnlType.Controls.Add(flpSequencesPanel, 3, 1);
            tlpnlType.Controls.Add(flpEnvironmentsPanel, 2, 1);
            tlpnlType.Controls.Add(flpCharactersPanel, 1, 1);
            tlpnlType.Controls.Add(flpPropsPanel, 0, 1);
            tlpnlType.Controls.Add(label5, 4, 0);
            tlpnlType.Controls.Add(label4, 3, 0);
            tlpnlType.Controls.Add(label3, 2, 0);
            tlpnlType.Controls.Add(label2, 1, 0);
            tlpnlType.Controls.Add(lblProps, 0, 0);
            tlpnlType.Controls.Add(flpShotsPanel, 4, 1);
            tlpnlType.Dock = DockStyle.Fill;
            tlpnlType.Location = new Point(0, 0);
            tlpnlType.Name = "tlpnlType";
            tlpnlType.RowCount = 2;
            tlpnlType.RowStyles.Add(new RowStyle(SizeType.Percent, 11.1702127F));
            tlpnlType.RowStyles.Add(new RowStyle(SizeType.Percent, 88.82979F));
            tlpnlType.Size = new Size(666, 472);
            tlpnlType.TabIndex = 0;
            // 
            // flpSequencesPanel
            // 
            flpSequencesPanel.AllowDrop = true;
            flpSequencesPanel.AutoScroll = true;
            flpSequencesPanel.BackColor = Color.FromArgb(35, 35, 35);
            flpSequencesPanel.Dock = DockStyle.Fill;
            flpSequencesPanel.FlowDirection = FlowDirection.TopDown;
            flpSequencesPanel.Location = new Point(402, 55);
            flpSequencesPanel.Name = "flpSequencesPanel";
            flpSequencesPanel.Size = new Size(127, 414);
            flpSequencesPanel.TabIndex = 9;
            flpSequencesPanel.WrapContents = false;
            flpSequencesPanel.DragDrop += DropPanel_DragDrop;
            flpSequencesPanel.DragEnter += DropPanel_DragEnter;
            // 
            // flpEnvironmentsPanel
            // 
            flpEnvironmentsPanel.AllowDrop = true;
            flpEnvironmentsPanel.AutoScroll = true;
            flpEnvironmentsPanel.BackColor = Color.FromArgb(35, 35, 35);
            flpEnvironmentsPanel.Dock = DockStyle.Fill;
            flpEnvironmentsPanel.FlowDirection = FlowDirection.TopDown;
            flpEnvironmentsPanel.Location = new Point(269, 55);
            flpEnvironmentsPanel.Name = "flpEnvironmentsPanel";
            flpEnvironmentsPanel.Size = new Size(127, 414);
            flpEnvironmentsPanel.TabIndex = 8;
            flpEnvironmentsPanel.WrapContents = false;
            flpEnvironmentsPanel.DragDrop += DropPanel_DragDrop;
            flpEnvironmentsPanel.DragEnter += DropPanel_DragEnter;
            // 
            // flpCharactersPanel
            // 
            flpCharactersPanel.AllowDrop = true;
            flpCharactersPanel.AutoScroll = true;
            flpCharactersPanel.BackColor = Color.FromArgb(35, 35, 35);
            flpCharactersPanel.Dock = DockStyle.Fill;
            flpCharactersPanel.FlowDirection = FlowDirection.TopDown;
            flpCharactersPanel.Location = new Point(136, 55);
            flpCharactersPanel.Name = "flpCharactersPanel";
            flpCharactersPanel.Size = new Size(127, 414);
            flpCharactersPanel.TabIndex = 7;
            flpCharactersPanel.WrapContents = false;
            flpCharactersPanel.DragDrop += DropPanel_DragDrop;
            flpCharactersPanel.DragEnter += DropPanel_DragEnter;
            // 
            // flpPropsPanel
            // 
            flpPropsPanel.AllowDrop = true;
            flpPropsPanel.AutoScroll = true;
            flpPropsPanel.BackColor = Color.FromArgb(35, 35, 35);
            flpPropsPanel.Dock = DockStyle.Fill;
            flpPropsPanel.FlowDirection = FlowDirection.TopDown;
            flpPropsPanel.Location = new Point(3, 55);
            flpPropsPanel.Name = "flpPropsPanel";
            flpPropsPanel.Size = new Size(127, 414);
            flpPropsPanel.TabIndex = 6;
            flpPropsPanel.WrapContents = false;
            flpPropsPanel.DragDrop += DropPanel_DragDrop;
            flpPropsPanel.DragEnter += DropPanel_DragEnter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(40, 40, 40);
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline);
            label5.ForeColor = Color.White;
            label5.Location = new Point(535, 0);
            label5.Name = "label5";
            label5.Size = new Size(128, 52);
            label5.TabIndex = 4;
            label5.Text = "SHOTS";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(40, 40, 40);
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline);
            label4.ForeColor = Color.White;
            label4.Location = new Point(402, 0);
            label4.Name = "label4";
            label4.Size = new Size(127, 52);
            label4.TabIndex = 3;
            label4.Text = "SEQUENCES";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(40, 40, 40);
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline);
            label3.ForeColor = Color.White;
            label3.Location = new Point(269, 0);
            label3.Name = "label3";
            label3.Size = new Size(127, 52);
            label3.TabIndex = 2;
            label3.Text = "ENVIRONMENTS";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(40, 40, 40);
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline);
            label2.ForeColor = Color.White;
            label2.Location = new Point(136, 0);
            label2.Name = "label2";
            label2.Size = new Size(127, 52);
            label2.TabIndex = 1;
            label2.Text = "CHARACTERS";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblProps
            // 
            lblProps.AutoSize = true;
            lblProps.BackColor = Color.FromArgb(40, 40, 40);
            lblProps.Dock = DockStyle.Fill;
            lblProps.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline);
            lblProps.ForeColor = Color.White;
            lblProps.Location = new Point(3, 0);
            lblProps.Name = "lblProps";
            lblProps.Size = new Size(127, 52);
            lblProps.TabIndex = 0;
            lblProps.Text = "PROPS";
            lblProps.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpShotsPanel
            // 
            flpShotsPanel.AllowDrop = true;
            flpShotsPanel.AutoScroll = true;
            flpShotsPanel.BackColor = Color.FromArgb(35, 35, 35);
            flpShotsPanel.Dock = DockStyle.Fill;
            flpShotsPanel.FlowDirection = FlowDirection.TopDown;
            flpShotsPanel.Location = new Point(535, 55);
            flpShotsPanel.Name = "flpShotsPanel";
            flpShotsPanel.Size = new Size(128, 414);
            flpShotsPanel.TabIndex = 5;
            flpShotsPanel.WrapContents = false;
            flpShotsPanel.DragDrop += DropPanel_DragDrop;
            flpShotsPanel.DragEnter += DropPanel_DragEnter;
            // 
            // lviewDeptNames
            // 
            lviewDeptNames.Alignment = ListViewAlignment.SnapToGrid;
            lviewDeptNames.AllowDrop = true;
            lviewDeptNames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lviewDeptNames.AutoArrange = false;
            lviewDeptNames.BackColor = Color.FromArgb(30, 30, 30);
            lviewDeptNames.BorderStyle = BorderStyle.FixedSingle;
            lviewDeptNames.ForeColor = Color.White;
            lviewDeptNames.FullRowSelect = true;
            listViewGroup1.CollapsedState = ListViewGroupCollapsedState.Expanded;
            listViewGroup1.Header = "Assets";
            listViewGroup1.HeaderAlignment = HorizontalAlignment.Center;
            listViewGroup1.Name = "Assets";
            listViewGroup1.Subtitle = "Assets";
            listViewGroup1.Tag = "Assets";
            listViewGroup2.CollapsedState = ListViewGroupCollapsedState.Expanded;
            listViewGroup2.FooterAlignment = HorizontalAlignment.Center;
            listViewGroup2.Header = "Shots";
            listViewGroup2.HeaderAlignment = HorizontalAlignment.Center;
            listViewGroup2.Name = "Shots";
            listViewGroup2.Subtitle = "Shots";
            listViewGroup2.Tag = "Shots";
            listViewGroup3.FooterAlignment = HorizontalAlignment.Center;
            listViewGroup3.Header = "Art";
            listViewGroup3.HeaderAlignment = HorizontalAlignment.Center;
            listViewGroup3.Name = "Art";
            listViewGroup4.FooterAlignment = HorizontalAlignment.Center;
            listViewGroup4.Header = "WorkFile";
            listViewGroup4.HeaderAlignment = HorizontalAlignment.Center;
            listViewGroup4.Name = "WorkFile";
            lviewDeptNames.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2, listViewGroup3, listViewGroup4 });
            lviewDeptNames.HeaderStyle = ColumnHeaderStyle.None;
            lviewDeptNames.ImeMode = ImeMode.NoControl;
            listViewItem1.Group = listViewGroup2;
            listViewItem2.Group = listViewGroup1;
            listViewItem3.Group = listViewGroup4;
            listViewItem4.Group = listViewGroup3;
            listViewItem5.Group = listViewGroup1;
            listViewItem6.Group = listViewGroup2;
            listViewItem7.Group = listViewGroup2;
            listViewItem8.Group = listViewGroup3;
            listViewItem9.Group = listViewGroup1;
            listViewItem10.Group = listViewGroup2;
            listViewItem11.Group = listViewGroup1;
            listViewItem12.Group = listViewGroup2;
            listViewItem13.Group = listViewGroup2;
            listViewItem14.Group = listViewGroup4;
            listViewItem15.Group = listViewGroup1;
            listViewItem16.Group = listViewGroup1;
            listViewItem17.Group = listViewGroup4;
            listViewItem18.Group = listViewGroup1;
            lviewDeptNames.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9, listViewItem10, listViewItem11, listViewItem12, listViewItem13, listViewItem14, listViewItem15, listViewItem16, listViewItem17, listViewItem18 });
            lviewDeptNames.Location = new Point(684, 57);
            lviewDeptNames.Margin = new Padding(1);
            lviewDeptNames.Name = "lviewDeptNames";
            lviewDeptNames.RightToLeft = RightToLeft.No;
            lviewDeptNames.Size = new Size(153, 472);
            lviewDeptNames.Sorting = SortOrder.Ascending;
            lviewDeptNames.TabIndex = 4;
            lviewDeptNames.UseCompatibleStateImageBehavior = false;
            lviewDeptNames.View = View.SmallIcon;
            lviewDeptNames.ItemDrag += LviewDeptNames_ItemDrag;
            // 
            // CreateProductionPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(846, 537);
            Controls.Add(lviewDeptNames);
            Controls.Add(pnlGrid);
            Controls.Add(btnOK);
            Controls.Add(txtProductionName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CreateProductionPopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CreateProductionForm";
            pnlGrid.ResumeLayout(false);
            tlpnlType.ResumeLayout(false);
            tlpnlType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        public string ProductionName { get; private set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ProductionName = txtProductionName.Text.Trim();
            if (string.IsNullOrEmpty(ProductionName))
            {
                MessageBox.Show("Veuillez entrer un nom de production.");
                return;
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        private Label label1;
        private TextBox txtProductionName;
        private Button btnOK;
        private Panel pnlGrid;
        private TableLayoutPanel tlpnlType;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label lblProps;
        private ListView lviewDeptNames;
        private FlowLayoutPanel flpShotsPanel;
        private FlowLayoutPanel flpSequencesPanel;
        private FlowLayoutPanel flpEnvironmentsPanel;
        private FlowLayoutPanel flpCharactersPanel;
        private FlowLayoutPanel flpPropsPanel;
    }
}