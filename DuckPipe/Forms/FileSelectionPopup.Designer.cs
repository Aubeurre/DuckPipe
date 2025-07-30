namespace DuckPipe
{
    partial class FileSelectionPopup
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
            listBoxFiles = new ListBox();
            btnOpen = new Button();
            lbDepartementName = new Label();
            SuspendLayout();
            // 
            // listBoxFiles
            // 
            listBoxFiles.BackColor = Color.FromArgb(30, 30, 30);
            listBoxFiles.BorderStyle = BorderStyle.None;
            listBoxFiles.ForeColor = Color.White;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.ItemHeight = 15;
            listBoxFiles.Location = new Point(12, 12);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(316, 90);
            listBoxFiles.TabIndex = 0;
            // 
            // btnOpen
            // 
            btnOpen.BackColor = Color.FromArgb(80, 80, 80);
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.ForeColor = Color.White;
            btnOpen.Location = new Point(253, 144);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(75, 23);
            btnOpen.TabIndex = 1;
            btnOpen.Text = "Select";
            btnOpen.UseVisualStyleBackColor = false;
            btnOpen.Click += btnOpen_Click;
            // 
            // lbDepartementName
            // 
            lbDepartementName.AutoSize = true;
            lbDepartementName.ForeColor = Color.White;
            lbDepartementName.Location = new Point(12, 109);
            lbDepartementName.Name = "lbDepartementName";
            lbDepartementName.Size = new Size(38, 15);
            lbDepartementName.TabIndex = 2;
            lbDepartementName.Text = "label1";
            // 
            // FileSelectionPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(340, 176);
            Controls.Add(lbDepartementName);
            Controls.Add(btnOpen);
            Controls.Add(listBoxFiles);
            Name = "FileSelectionPopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FileSelectionPopup";
            Load += FileSelectionPopup_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxFiles;
        private Button btnOpen;
        private Label lbDepartementName;
    }
}