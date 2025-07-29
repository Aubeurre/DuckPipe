namespace DuckPipe.Forms
{
    partial class AddAssetIntoShot
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
            splitContainer1 = new SplitContainer();
            btnRemove = new Button();
            btnAdd = new Button();
            tvDest = new TreeView();
            tvSource = new TreeView();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tvSource);
            splitContainer1.Panel1.Controls.Add(btnAdd);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tvDest);
            splitContainer1.Panel2.Controls.Add(btnRemove);
            splitContainer1.Size = new Size(739, 485);
            splitContainer1.SplitterDistance = 365;
            splitContainer1.TabIndex = 0;
            // 
            // btnRemove
            // 
            btnRemove.BackColor = Color.FromArgb(80, 80, 80);
            btnRemove.Dock = DockStyle.Bottom;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.ForeColor = Color.White;
            btnRemove.Location = new Point(0, 462);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(370, 23);
            btnRemove.TabIndex = 0;
            btnRemove.Text = "Remove Selected";
            btnRemove.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(80, 80, 80);
            btnAdd.Dock = DockStyle.Bottom;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(0, 462);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(365, 23);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add Selected";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // tvDest
            // 
            tvDest.BackColor = Color.FromArgb(30, 30, 30);
            tvDest.BorderStyle = BorderStyle.None;
            tvDest.Dock = DockStyle.Fill;
            tvDest.Location = new Point(0, 0);
            tvDest.Name = "tvDest";
            tvDest.Size = new Size(370, 462);
            tvDest.TabIndex = 1;
            // 
            // tvSource
            // 
            tvSource.BackColor = Color.FromArgb(30, 30, 30);
            tvSource.BorderStyle = BorderStyle.None;
            tvSource.Dock = DockStyle.Fill;
            tvSource.Location = new Point(0, 0);
            tvSource.Name = "tvSource";
            tvSource.Size = new Size(365, 462);
            tvSource.TabIndex = 2;
            tvSource.AfterSelect += tvSource_AfterSelect;
            // 
            // AddAssetIntoShot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(739, 485);
            Controls.Add(splitContainer1);
            Name = "AddAssetIntoShot";
            Text = "AddAssetIntoShot";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button btnRemove;
        private Button btnAdd;
        private TreeView tvDest;
        private TreeView tvSource;
    }
}