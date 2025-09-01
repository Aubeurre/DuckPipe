namespace DuckPipe
{
    partial class CreateAssetPopup
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
            btnOK = new Button();
            txtNodeName = new TextBox();
            cbNodeType = new ComboBox();
            tbDescription = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel4 = new FlowLayoutPanel();
            label3 = new Label();
            label4 = new Label();
            label1 = new Label();
            flowLayoutPanel3 = new FlowLayoutPanel();
            flowLayoutPanel4.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.BackColor = Color.FromArgb(80, 80, 80);
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(223, 111);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(38, 23);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // txtNodeName
            // 
            txtNodeName.BackColor = Color.FromArgb(30, 30, 30);
            txtNodeName.BorderStyle = BorderStyle.FixedSingle;
            txtNodeName.ForeColor = Color.White;
            txtNodeName.Location = new Point(3, 3);
            txtNodeName.Name = "txtNodeName";
            txtNodeName.Size = new Size(154, 23);
            txtNodeName.TabIndex = 4;
            // 
            // cbNodeType
            // 
            cbNodeType.BackColor = Color.FromArgb(30, 30, 30);
            cbNodeType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbNodeType.FlatStyle = FlatStyle.Flat;
            cbNodeType.ForeColor = Color.White;
            cbNodeType.FormattingEnabled = true;
            cbNodeType.Items.AddRange(new object[] { "Characters", "Props", "Environments" });
            cbNodeType.Location = new Point(3, 32);
            cbNodeType.Name = "cbNodeType";
            cbNodeType.Size = new Size(154, 23);
            cbNodeType.TabIndex = 6;
            cbNodeType.Tag = "";
            cbNodeType.SelectedIndexChanged += cbNodeType_SelectedIndexChanged;
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(30, 30, 30);
            tbDescription.BorderStyle = BorderStyle.FixedSingle;
            tbDescription.ForeColor = Color.White;
            tbDescription.Location = new Point(3, 61);
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new Size(154, 23);
            tbDescription.TabIndex = 11;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(273, 0);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(txtNodeName);
            flowLayoutPanel4.Controls.Add(cbNodeType);
            flowLayoutPanel4.Controls.Add(tbDescription);
            flowLayoutPanel4.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel4.Location = new Point(101, 12);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(164, 93);
            flowLayoutPanel4.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(3, 65);
            label3.Margin = new Padding(3, 7, 3, 7);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 10;
            label3.Text = "Description";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Left;
            label4.ForeColor = Color.White;
            label4.Location = new Point(3, 36);
            label4.Margin = new Padding(3, 7, 3, 7);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 6;
            label4.Text = "AssetType";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 7);
            label1.Margin = new Padding(3, 7, 3, 7);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 5;
            label1.Text = "Asset Name";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(label1);
            flowLayoutPanel3.Controls.Add(label4);
            flowLayoutPanel3.Controls.Add(label3);
            flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel3.Location = new Point(12, 12);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(83, 93);
            flowLayoutPanel3.TabIndex = 14;
            // 
            // CreateAssetPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(273, 146);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CreateAssetPopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Create Node";
            Load += CreateAssetPopup_Load;
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private TextBox txtNodeName;
        private ComboBox cbNodeType;
        private TextBox tbDescription;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label label3;
        private Label label4;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel3;
    }
}