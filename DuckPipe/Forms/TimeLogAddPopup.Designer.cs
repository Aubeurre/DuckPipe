namespace DuckPipe.Forms
{
    partial class TimeLogAddPopup
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
            flowLayoutPanel3 = new FlowLayoutPanel();
            label1 = new Label();
            label4 = new Label();
            label3 = new Label();
            flowLayoutPanel4 = new FlowLayoutPanel();
            cbNode = new ComboBox();
            cbDepartment = new ComboBox();
            tbTimeLogged = new TextBox();
            btnOK = new Button();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(label1);
            flowLayoutPanel3.Controls.Add(label4);
            flowLayoutPanel3.Controls.Add(label3);
            flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel3.Location = new Point(12, 12);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(83, 94);
            flowLayoutPanel3.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 7);
            label1.Margin = new Padding(3, 7, 3, 7);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 5;
            label1.Text = "Node";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Left;
            label4.ForeColor = Color.White;
            label4.Location = new Point(3, 36);
            label4.Margin = new Padding(3, 7, 3, 7);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 6;
            label4.Text = "Department";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(3, 65);
            label3.Margin = new Padding(3, 7, 3, 7);
            label3.Name = "label3";
            label3.Size = new Size(77, 15);
            label3.TabIndex = 10;
            label3.Text = "Time Logged";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(cbNode);
            flowLayoutPanel4.Controls.Add(cbDepartment);
            flowLayoutPanel4.Controls.Add(tbTimeLogged);
            flowLayoutPanel4.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel4.Location = new Point(101, 12);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(164, 94);
            flowLayoutPanel4.TabIndex = 18;
            // 
            // cbNode
            // 
            cbNode.BackColor = Color.FromArgb(30, 30, 30);
            cbNode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbNode.FlatStyle = FlatStyle.Flat;
            cbNode.ForeColor = Color.White;
            cbNode.FormattingEnabled = true;
            cbNode.Location = new Point(3, 3);
            cbNode.Name = "cbNode";
            cbNode.Size = new Size(154, 23);
            cbNode.TabIndex = 6;
            cbNode.Tag = "";
            // 
            // cbDepartment
            // 
            cbDepartment.BackColor = Color.FromArgb(30, 30, 30);
            cbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDepartment.FlatStyle = FlatStyle.Flat;
            cbDepartment.ForeColor = Color.White;
            cbDepartment.FormattingEnabled = true;
            cbDepartment.Items.AddRange(new object[] { "Modeling", "Rig", "Cfx", "Surfacing", "Facial", "Assemble", "-----", "Layout", "Anim", "Lighting", "-----", "Art", "Board", "-----", "Misc" });
            cbDepartment.Location = new Point(3, 32);
            cbDepartment.Name = "cbDepartment";
            cbDepartment.Size = new Size(154, 23);
            cbDepartment.TabIndex = 12;
            cbDepartment.Tag = "";
            cbDepartment.SelectedIndexChanged += cbDepartment_SelectedIndexChanged;
            // 
            // tbTimeLogged
            // 
            tbTimeLogged.BackColor = Color.FromArgb(30, 30, 30);
            tbTimeLogged.BorderStyle = BorderStyle.FixedSingle;
            tbTimeLogged.ForeColor = Color.White;
            tbTimeLogged.Location = new Point(3, 61);
            tbTimeLogged.Name = "tbTimeLogged";
            tbTimeLogged.Size = new Size(154, 23);
            tbTimeLogged.TabIndex = 11;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.BackColor = Color.FromArgb(80, 80, 80);
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(227, 112);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(38, 23);
            btnOK.TabIndex = 16;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // TimeLogAddPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(271, 146);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(btnOK);
            Name = "TimeLogAddPopup";
            Text = "TimeLogAddPopup";
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel3;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label lbSequence;
        private Label lbRange;
        private FlowLayoutPanel flowLayoutPanel4;
        private TextBox txtNodeName;
        private ComboBox cbNode;
        private TextBox tbTimeLogged;
        private ComboBox cbSeqChoice;
        private FlowLayoutPanel flowLayoutPanel5;
        private TextBox tbRangeIn;
        private Label lbRangeSeparator;
        private TextBox tbRangeOut;
        private Button btnOK;
        private ComboBox cbDepartment;
    }
}