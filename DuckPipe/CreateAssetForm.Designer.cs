namespace DuckPipe
{
    partial class CreateAssetForm
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
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new Point(176, 64);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(38, 23);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // txtAssetName
            // 
            txtAssetName.Location = new Point(93, 6);
            txtAssetName.Name = "txtAssetName";
            txtAssetName.Size = new Size(121, 23);
            txtAssetName.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 3;
            label1.Text = "Asset Name";
            label1.Click += label1_Click;
            // 
            // cbAssetType
            // 
            cbAssetType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAssetType.FlatStyle = FlatStyle.Flat;
            cbAssetType.FormattingEnabled = true;
            cbAssetType.Items.AddRange(new object[] { "Characters", "Props", "Environments", "Sequences", "Shots" });
            cbAssetType.Location = new Point(93, 35);
            cbAssetType.Name = "cbAssetType";
            cbAssetType.Size = new Size(121, 23);
            cbAssetType.TabIndex = 6;
            cbAssetType.Tag = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 7;
            label2.Text = "Asset Type";
            // 
            // CreateAssetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(225, 94);
            Controls.Add(label2);
            Controls.Add(cbAssetType);
            Controls.Add(btnOK);
            Controls.Add(txtAssetName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CreateAssetForm";
            Text = "Create Asset";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private TextBox txtAssetName;
        private Label label1;
        private ComboBox cbAssetType;
        private Label label2;
    }
}