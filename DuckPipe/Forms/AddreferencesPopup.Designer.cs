namespace DuckPipe.Forms
{
    partial class AddreferencesPopup
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
            components = new System.ComponentModel.Container();
            btnAdd = new Button();
            cbNode = new ComboBox();
            cbDepartment = new ComboBox();
            cmsRemove = new ContextMenuStrip(components);
            removeToolStripMenuItem = new ToolStripMenuItem();
            listRefAdded = new ListBox();
            cmsRemove.SuspendLayout();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom;
            btnAdd.BackColor = Color.FromArgb(80, 80, 80);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(331, 159);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(51, 25);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // cbNode
            // 
            cbNode.Anchor = AnchorStyles.Bottom;
            cbNode.BackColor = Color.FromArgb(30, 30, 30);
            cbNode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbNode.FlatStyle = FlatStyle.Flat;
            cbNode.ForeColor = Color.White;
            cbNode.FormattingEnabled = true;
            cbNode.Location = new Point(11, 159);
            cbNode.Name = "cbNode";
            cbNode.Size = new Size(154, 23);
            cbNode.TabIndex = 13;
            cbNode.Tag = "";
            // 
            // cbDepartment
            // 
            cbDepartment.Anchor = AnchorStyles.Bottom;
            cbDepartment.BackColor = Color.FromArgb(30, 30, 30);
            cbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDepartment.FlatStyle = FlatStyle.Flat;
            cbDepartment.ForeColor = Color.White;
            cbDepartment.FormattingEnabled = true;
            cbDepartment.Items.AddRange(new object[] { "Modeling", "Rig", "Cfx", "Surfacing", "Facial", "Assemble", "-----", "Layout", "Anim", "CfxShot", "Lighting", "-----", "Art", "Board", "-----", "Misc" });
            cbDepartment.Location = new Point(171, 159);
            cbDepartment.Name = "cbDepartment";
            cbDepartment.Size = new Size(154, 23);
            cbDepartment.TabIndex = 14;
            cbDepartment.Tag = "";
            // 
            // cmsRemove
            // 
            cmsRemove.Items.AddRange(new ToolStripItem[] { removeToolStripMenuItem });
            cmsRemove.Name = "cmsRemove";
            cmsRemove.Size = new Size(181, 48);
            cmsRemove.Opening += cmsRemove_Opening;
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(180, 22);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += removeToolStripMenuItem_Click;
            // 
            // listRefAdded
            // 
            listRefAdded.BackColor = Color.FromArgb(30, 30, 30);
            listRefAdded.ContextMenuStrip = cmsRemove;
            listRefAdded.ForeColor = Color.White;
            listRefAdded.FormattingEnabled = true;
            listRefAdded.ItemHeight = 15;
            listRefAdded.Location = new Point(11, 12);
            listRefAdded.Name = "listRefAdded";
            listRefAdded.Size = new Size(371, 139);
            listRefAdded.TabIndex = 17;
            // 
            // AddreferencesPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(391, 192);
            Controls.Add(listRefAdded);
            Controls.Add(cbNode);
            Controls.Add(cbDepartment);
            Controls.Add(btnAdd);
            Name = "AddreferencesPopup";
            Text = "AddreferencesPopup";
            cmsRemove.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnAdd;
        private ComboBox cbNode;
        private ComboBox cbDepartment;
        private ContextMenuStrip cmsRemove;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ListBox listRefAdded;
    }
}