namespace DuckPipe.Forms
{
    partial class RenamerForm
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
            btnRename = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // btnRename
            // 
            btnRename.BackColor = Color.FromArgb(80, 80, 80);
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.FlatStyle = FlatStyle.Flat;
            btnRename.ForeColor = Color.White;
            btnRename.Location = new Point(127, 65);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(75, 23);
            btnRename.TabIndex = 0;
            btnRename.Text = "Rename";
            btnRename.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 13);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 1;
            label1.Text = "New Name";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(30, 30, 30);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Location = new Point(12, 36);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(190, 23);
            textBox1.TabIndex = 2;
            // 
            // RenamerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(214, 100);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(btnRename);
            Name = "RenamerForm";
            Text = "RenamerForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRename;
        private Label label1;
        private TextBox textBox1;
    }
}