namespace DuckPipe
{
    partial class CreateNodePopup
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
            cbSeqChoice = new ComboBox();
            tbDescription = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel4 = new FlowLayoutPanel();
            flowLayoutPanel5 = new FlowLayoutPanel();
            tbRangeIn = new TextBox();
            lbRangeSeparator = new Label();
            tbRangeOut = new TextBox();
            lbRange = new Label();
            lbSequence = new Label();
            label3 = new Label();
            label4 = new Label();
            label1 = new Label();
            flowLayoutPanel3 = new FlowLayoutPanel();
            flowLayoutPanel4.SuspendLayout();
            flowLayoutPanel5.SuspendLayout();
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
            btnOK.Location = new Point(223, 179);
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
            cbNodeType.Items.AddRange(new object[] { "Characters", "Props", "Environments", "Sequences", "Shots" });
            cbNodeType.Location = new Point(3, 32);
            cbNodeType.Name = "cbNodeType";
            cbNodeType.Size = new Size(154, 23);
            cbNodeType.TabIndex = 6;
            cbNodeType.Tag = "";
            cbNodeType.SelectedIndexChanged += cbNodeType_SelectedIndexChanged;
            // 
            // cbSeqChoice
            // 
            cbSeqChoice.BackColor = Color.FromArgb(30, 30, 30);
            cbSeqChoice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSeqChoice.FlatStyle = FlatStyle.Flat;
            cbSeqChoice.ForeColor = Color.White;
            cbSeqChoice.FormattingEnabled = true;
            cbSeqChoice.Location = new Point(3, 90);
            cbSeqChoice.Name = "cbSeqChoice";
            cbSeqChoice.Size = new Size(154, 23);
            cbSeqChoice.TabIndex = 8;
            cbSeqChoice.Tag = "";
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
            flowLayoutPanel4.Controls.Add(cbSeqChoice);
            flowLayoutPanel4.Controls.Add(flowLayoutPanel5);
            flowLayoutPanel4.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel4.Location = new Point(101, 12);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(164, 158);
            flowLayoutPanel4.TabIndex = 15;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.Controls.Add(tbRangeIn);
            flowLayoutPanel5.Controls.Add(lbRangeSeparator);
            flowLayoutPanel5.Controls.Add(tbRangeOut);
            flowLayoutPanel5.Location = new Point(3, 119);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(154, 30);
            flowLayoutPanel5.TabIndex = 12;
            // 
            // tbRangeIn
            // 
            tbRangeIn.BackColor = Color.FromArgb(30, 30, 30);
            tbRangeIn.BorderStyle = BorderStyle.FixedSingle;
            tbRangeIn.ForeColor = Color.White;
            tbRangeIn.Location = new Point(3, 3);
            tbRangeIn.Name = "tbRangeIn";
            tbRangeIn.Size = new Size(62, 23);
            tbRangeIn.TabIndex = 13;
            // 
            // lbRangeSeparator
            // 
            lbRangeSeparator.AutoSize = true;
            lbRangeSeparator.ForeColor = Color.White;
            lbRangeSeparator.Location = new Point(71, 7);
            lbRangeSeparator.Margin = new Padding(3, 7, 3, 7);
            lbRangeSeparator.Name = "lbRangeSeparator";
            lbRangeSeparator.Size = new Size(12, 15);
            lbRangeSeparator.TabIndex = 15;
            lbRangeSeparator.Text = "-";
            // 
            // tbRangeOut
            // 
            tbRangeOut.BackColor = Color.FromArgb(30, 30, 30);
            tbRangeOut.BorderStyle = BorderStyle.FixedSingle;
            tbRangeOut.ForeColor = Color.White;
            tbRangeOut.Location = new Point(89, 3);
            tbRangeOut.Name = "tbRangeOut";
            tbRangeOut.Size = new Size(62, 23);
            tbRangeOut.TabIndex = 14;
            // 
            // lbRange
            // 
            lbRange.AutoSize = true;
            lbRange.ForeColor = Color.White;
            lbRange.Location = new Point(3, 123);
            lbRange.Margin = new Padding(3, 7, 3, 7);
            lbRange.Name = "lbRange";
            lbRange.Size = new Size(40, 15);
            lbRange.TabIndex = 11;
            lbRange.Text = "Range";
            // 
            // lbSequence
            // 
            lbSequence.AutoSize = true;
            lbSequence.ForeColor = Color.White;
            lbSequence.Location = new Point(3, 94);
            lbSequence.Margin = new Padding(3, 7, 3, 7);
            lbSequence.Name = "lbSequence";
            lbSequence.Size = new Size(51, 15);
            lbSequence.TabIndex = 9;
            lbSequence.Text = "Seqence";
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
            label4.Text = "NodeType";
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
            label1.Text = "Node Name";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(label1);
            flowLayoutPanel3.Controls.Add(label4);
            flowLayoutPanel3.Controls.Add(label3);
            flowLayoutPanel3.Controls.Add(lbSequence);
            flowLayoutPanel3.Controls.Add(lbRange);
            flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel3.Location = new Point(12, 12);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(83, 158);
            flowLayoutPanel3.TabIndex = 14;
            // 
            // CreateNodePopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(273, 214);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "CreateNodePopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Create Node";
            Load += CreateNodePopup_Load;
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel5.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private TextBox txtNodeName;
        private ComboBox cbNodeType;
        private ComboBox cbSeqChoice;
        private TextBox tbDescription;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel5;
        private TextBox tbRangeIn;
        private Label lbRangeSeparator;
        private TextBox tbRangeOut;
        private Label lbRange;
        private Label lbSequence;
        private Label label3;
        private Label label4;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel3;
    }
}