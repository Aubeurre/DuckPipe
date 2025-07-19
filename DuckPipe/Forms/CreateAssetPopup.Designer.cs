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
            txtAssetName = new TextBox();
            label1 = new Label();
            cbAssetType = new ComboBox();
            label2 = new Label();
            lbSequence = new Label();
            cbSeqChoice = new ComboBox();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.FromArgb(80, 80, 80);
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(176, 64);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(38, 23);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // txtAssetName
            // 
            txtAssetName.BackColor = Color.FromArgb(30, 30, 30);
            txtAssetName.BorderStyle = BorderStyle.FixedSingle;
            txtAssetName.ForeColor = Color.White;
            txtAssetName.Location = new Point(93, 6);
            txtAssetName.Name = "txtAssetName";
            txtAssetName.Size = new Size(121, 23);
            txtAssetName.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 3;
            label1.Text = "Asset Name";
            // 
            // cbAssetType
            // 
            cbAssetType.BackColor = Color.FromArgb(30, 30, 30);
            cbAssetType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAssetType.FlatStyle = FlatStyle.Flat;
            cbAssetType.ForeColor = Color.White;
            cbAssetType.FormattingEnabled = true;
            cbAssetType.Items.AddRange(new object[] { "Characters", "Props", "Environments", "Sequences", "Shots" });
            cbAssetType.Location = new Point(93, 35);
            cbAssetType.Name = "cbAssetType";
            cbAssetType.Size = new Size(121, 23);
            cbAssetType.TabIndex = 6;
            cbAssetType.Tag = "";
            cbAssetType.SelectedIndexChanged += cbAssetType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 7;
            label2.Text = "Asset Type";
            // 
            // lbSequence
            // 
            lbSequence.AutoSize = true;
            lbSequence.ForeColor = Color.White;
            lbSequence.Location = new Point(12, 68);
            lbSequence.Name = "lbSequence";
            lbSequence.Size = new Size(51, 15);
            lbSequence.TabIndex = 9;
            lbSequence.Text = "Seqence";
            // 
            // cbSeqChoice
            // 
            cbSeqChoice.BackColor = Color.FromArgb(30, 30, 30);
            cbSeqChoice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSeqChoice.FlatStyle = FlatStyle.Flat;
            cbSeqChoice.ForeColor = Color.White;
            cbSeqChoice.FormattingEnabled = true;
            cbSeqChoice.Location = new Point(93, 65);
            cbSeqChoice.Name = "cbSeqChoice";
            cbSeqChoice.Size = new Size(77, 23);
            cbSeqChoice.TabIndex = 8;
            cbSeqChoice.Tag = "";
            // 
            // CreateAssetPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(225, 94);
            Controls.Add(lbSequence);
            Controls.Add(cbSeqChoice);
            Controls.Add(label2);
            Controls.Add(cbAssetType);
            Controls.Add(btnOK);
            Controls.Add(txtAssetName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CreateAssetPopup";
            Text = "Create Asset";
            Load += CreateAssetPopup_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private TextBox txtAssetName;
        private Label label1;
        private ComboBox cbAssetType;
        private Label label2;
        private Label lbSequence;
        private ComboBox cbSeqChoice;
    }
}