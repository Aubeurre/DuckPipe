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
            label1 = new Label();
            txtProductionName = new TextBox();
            btnOK = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 9);
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
            txtProductionName.Location = new Point(12, 28);
            txtProductionName.Name = "txtProductionName";
            txtProductionName.Size = new Size(100, 23);
            txtProductionName.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.FromArgb(80, 80, 80);
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(118, 27);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(38, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click_1;
            // 
            // CreateProductionPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(176, 64);
            Controls.Add(btnOK);
            Controls.Add(txtProductionName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CreateProductionPopup";
            Text = "CreateProductionForm";
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
    }
}