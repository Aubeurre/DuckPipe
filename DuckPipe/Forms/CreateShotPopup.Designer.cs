namespace DuckPipe.Forms
{
    partial class CreateShotPopup
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
            flowLayoutPanel5 = new FlowLayoutPanel();
            flowLayoutPanel3 = new FlowLayoutPanel();
            lbSequence = new Label();
            cbSeqChoice = new ComboBox();
            groupBox2 = new GroupBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            label2 = new Label();
            txtShotDesc = new TextBox();
            flowLayoutPanel4 = new FlowLayoutPanel();
            label3 = new Label();
            txtShotName = new TextBox();
            lbRange = new Label();
            txtRangeIn = new TextBox();
            lbRangeSeparator = new Label();
            txtRangeOut = new TextBox();
            btnAddShot = new Button();
            flowLayoutPanel5.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            groupBox2.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel5.AutoScroll = true;
            flowLayoutPanel5.BackColor = Color.FromArgb(35, 35, 35);
            flowLayoutPanel5.Controls.Add(flowLayoutPanel3);
            flowLayoutPanel5.Controls.Add(groupBox2);
            flowLayoutPanel5.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel5.Location = new Point(12, 12);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Padding = new Padding(5);
            flowLayoutPanel5.Size = new Size(404, 136);
            flowLayoutPanel5.TabIndex = 20;
            flowLayoutPanel5.WrapContents = false;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel3.Controls.Add(lbSequence);
            flowLayoutPanel3.Controls.Add(cbSeqChoice);
            flowLayoutPanel3.Location = new Point(8, 8);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(386, 27);
            flowLayoutPanel3.TabIndex = 21;
            // 
            // lbSequence
            // 
            lbSequence.AutoSize = true;
            lbSequence.ForeColor = Color.White;
            lbSequence.Location = new Point(3, 7);
            lbSequence.Margin = new Padding(3, 7, 3, 7);
            lbSequence.Name = "lbSequence";
            lbSequence.Size = new Size(51, 15);
            lbSequence.TabIndex = 11;
            lbSequence.Text = "Seqence";
            // 
            // cbSeqChoice
            // 
            cbSeqChoice.BackColor = Color.FromArgb(30, 30, 30);
            cbSeqChoice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSeqChoice.FlatStyle = FlatStyle.Flat;
            cbSeqChoice.ForeColor = Color.White;
            cbSeqChoice.FormattingEnabled = true;
            cbSeqChoice.Location = new Point(60, 3);
            cbSeqChoice.Name = "cbSeqChoice";
            cbSeqChoice.Size = new Size(154, 23);
            cbSeqChoice.TabIndex = 10;
            cbSeqChoice.Tag = "";
            cbSeqChoice.SelectedIndexChanged += cbSeqChoice_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(flowLayoutPanel2);
            groupBox2.Controls.Add(flowLayoutPanel4);
            groupBox2.ForeColor = Color.WhiteSmoke;
            groupBox2.Location = new Point(8, 41);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(386, 82);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "Shots";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(label2);
            flowLayoutPanel2.Controls.Add(txtShotDesc);
            flowLayoutPanel2.Dock = DockStyle.Top;
            flowLayoutPanel2.Location = new Point(3, 46);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(380, 33);
            flowLayoutPanel2.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(3, 7);
            label2.Margin = new Padding(3, 7, 3, 7);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 12;
            label2.Text = "Description";
            // 
            // txtShotDesc
            // 
            txtShotDesc.BackColor = Color.FromArgb(30, 30, 30);
            txtShotDesc.BorderStyle = BorderStyle.FixedSingle;
            txtShotDesc.Dock = DockStyle.Fill;
            flowLayoutPanel2.SetFlowBreak(txtShotDesc, true);
            txtShotDesc.ForeColor = Color.White;
            txtShotDesc.Location = new Point(76, 3);
            txtShotDesc.Multiline = true;
            txtShotDesc.Name = "txtShotDesc";
            txtShotDesc.Size = new Size(278, 23);
            txtShotDesc.TabIndex = 13;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(label3);
            flowLayoutPanel4.Controls.Add(txtShotName);
            flowLayoutPanel4.Controls.Add(lbRange);
            flowLayoutPanel4.Controls.Add(txtRangeIn);
            flowLayoutPanel4.Controls.Add(lbRangeSeparator);
            flowLayoutPanel4.Controls.Add(txtRangeOut);
            flowLayoutPanel4.Dock = DockStyle.Top;
            flowLayoutPanel4.Location = new Point(3, 19);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(380, 27);
            flowLayoutPanel4.TabIndex = 17;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Left;
            label3.ForeColor = Color.White;
            label3.Location = new Point(3, 7);
            label3.Margin = new Padding(3, 7, 3, 7);
            label3.Name = "label3";
            label3.Size = new Size(14, 15);
            label3.TabIndex = 5;
            label3.Text = "P";
            // 
            // txtShotName
            // 
            txtShotName.BackColor = Color.FromArgb(30, 30, 30);
            txtShotName.BorderStyle = BorderStyle.FixedSingle;
            txtShotName.ForeColor = Color.White;
            txtShotName.Location = new Point(23, 3);
            txtShotName.Name = "txtShotName";
            txtShotName.Size = new Size(62, 23);
            txtShotName.TabIndex = 6;
            // 
            // lbRange
            // 
            lbRange.AutoSize = true;
            lbRange.ForeColor = Color.White;
            lbRange.Location = new Point(91, 7);
            lbRange.Margin = new Padding(3, 7, 3, 7);
            lbRange.Name = "lbRange";
            lbRange.Size = new Size(115, 15);
            lbRange.TabIndex = 13;
            lbRange.Text = "                         Range";
            // 
            // txtRangeIn
            // 
            txtRangeIn.BackColor = Color.FromArgb(30, 30, 30);
            txtRangeIn.BorderStyle = BorderStyle.FixedSingle;
            txtRangeIn.ForeColor = Color.White;
            txtRangeIn.Location = new Point(212, 3);
            txtRangeIn.Name = "txtRangeIn";
            txtRangeIn.Size = new Size(62, 23);
            txtRangeIn.TabIndex = 16;
            // 
            // lbRangeSeparator
            // 
            lbRangeSeparator.AutoSize = true;
            lbRangeSeparator.ForeColor = Color.White;
            lbRangeSeparator.Location = new Point(280, 7);
            lbRangeSeparator.Margin = new Padding(3, 7, 3, 7);
            lbRangeSeparator.Name = "lbRangeSeparator";
            lbRangeSeparator.Size = new Size(12, 15);
            lbRangeSeparator.TabIndex = 18;
            lbRangeSeparator.Text = "-";
            // 
            // txtRangeOut
            // 
            txtRangeOut.BackColor = Color.FromArgb(30, 30, 30);
            txtRangeOut.BorderStyle = BorderStyle.FixedSingle;
            txtRangeOut.ForeColor = Color.White;
            txtRangeOut.Location = new Point(298, 3);
            txtRangeOut.Name = "txtRangeOut";
            txtRangeOut.Size = new Size(62, 23);
            txtRangeOut.TabIndex = 17;
            // 
            // btnAddShot
            // 
            btnAddShot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAddShot.BackColor = Color.FromArgb(80, 80, 80);
            btnAddShot.FlatAppearance.BorderSize = 0;
            btnAddShot.FlatStyle = FlatStyle.Flat;
            btnAddShot.ForeColor = Color.White;
            btnAddShot.Location = new Point(12, 154);
            btnAddShot.Name = "btnAddShot";
            btnAddShot.Size = new Size(404, 23);
            btnAddShot.TabIndex = 20;
            btnAddShot.Text = "OK";
            btnAddShot.UseVisualStyleBackColor = false;
            btnAddShot.Click += btnAddShot_Click;
            // 
            // CreateShotPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(428, 189);
            Controls.Add(flowLayoutPanel5);
            Controls.Add(btnAddShot);
            Name = "CreateShotPopup";
            Text = "CreateShotPopup";
            Load += CreateShotPopup_Load;
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            groupBox2.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel5;
        private GroupBox groupBox2;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label2;
        private TextBox txtShotDesc;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label label3;
        private TextBox txtShotName;
        private Label lbRange;
        private TextBox txtRangeIn;
        private Label lbRangeSeparator;
        private TextBox txtRangeOut;
        private Button btnAddShot;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label lbSequence;
        private ComboBox cbSeqChoice;
    }
}