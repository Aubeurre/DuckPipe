namespace DuckPipe.Forms
{
    partial class CreateSequencePopup
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
            flowLayoutPanel5 = new FlowLayoutPanel();
            btnAddShot = new Button();
            flowLayoutPanel6 = new FlowLayoutPanel();
            gbSequence = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label4 = new Label();
            txtNodeDesc = new TextBox();
            flowLayoutPanel3 = new FlowLayoutPanel();
            label1 = new Label();
            txtNodeName = new TextBox();
            txtShotDesc = new TextBox();
            flowLayoutPanel6.SuspendLayout();
            gbSequence.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
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
            btnOK.Location = new Point(375, 271);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(38, 23);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel5.AutoScroll = true;
            flowLayoutPanel5.BackColor = Color.FromArgb(35, 35, 35);
            flowLayoutPanel5.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel5.Location = new Point(12, 132);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Padding = new Padding(5);
            flowLayoutPanel5.Size = new Size(401, 133);
            flowLayoutPanel5.TabIndex = 19;
            flowLayoutPanel5.WrapContents = false;
            // 
            // btnAddShot
            // 
            btnAddShot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAddShot.AutoSize = true;
            btnAddShot.BackColor = Color.FromArgb(80, 80, 80);
            btnAddShot.FlatAppearance.BorderSize = 0;
            btnAddShot.FlatStyle = FlatStyle.Flat;
            btnAddShot.ForeColor = Color.White;
            btnAddShot.Location = new Point(12, 271);
            btnAddShot.Name = "btnAddShot";
            btnAddShot.Size = new Size(66, 25);
            btnAddShot.TabIndex = 20;
            btnAddShot.Text = "Add Shot";
            btnAddShot.UseVisualStyleBackColor = false;
            btnAddShot.Click += btnAddShot_Click;
            // 
            // flowLayoutPanel6
            // 
            flowLayoutPanel6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel6.AutoScroll = true;
            flowLayoutPanel6.AutoSize = true;
            flowLayoutPanel6.BackColor = Color.FromArgb(35, 35, 35);
            flowLayoutPanel6.Controls.Add(gbSequence);
            flowLayoutPanel6.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel6.Location = new Point(12, 12);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Padding = new Padding(5);
            flowLayoutPanel6.Size = new Size(402, 114);
            flowLayoutPanel6.TabIndex = 20;
            // 
            // gbSequence
            // 
            gbSequence.Controls.Add(flowLayoutPanel1);
            gbSequence.Controls.Add(flowLayoutPanel3);
            gbSequence.ForeColor = Color.WhiteSmoke;
            gbSequence.Location = new Point(8, 8);
            gbSequence.Name = "gbSequence";
            gbSequence.Size = new Size(368, 92);
            gbSequence.TabIndex = 18;
            gbSequence.TabStop = false;
            gbSequence.Text = "Sequence";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(txtNodeDesc);
            flowLayoutPanel1.Controls.Add(txtShotDesc);
            flowLayoutPanel1.Location = new Point(6, 54);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(356, 33);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(3, 7);
            label4.Margin = new Padding(3, 7, 3, 7);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 12;
            label4.Text = "Description";
            // 
            // txtNodeDesc
            // 
            txtNodeDesc.BackColor = Color.FromArgb(30, 30, 30);
            txtNodeDesc.BorderStyle = BorderStyle.FixedSingle;
            txtNodeDesc.Dock = DockStyle.Fill;
            flowLayoutPanel1.SetFlowBreak(txtNodeDesc, true);
            txtNodeDesc.ForeColor = Color.White;
            txtNodeDesc.Location = new Point(76, 3);
            txtNodeDesc.Multiline = true;
            txtNodeDesc.Name = "txtNodeDesc";
            txtNodeDesc.Size = new Size(274, 23);
            txtNodeDesc.TabIndex = 13;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel3.Controls.Add(label1);
            flowLayoutPanel3.Controls.Add(txtNodeName);
            flowLayoutPanel3.Location = new Point(6, 22);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(356, 27);
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
            label1.Size = new Size(13, 15);
            label1.TabIndex = 5;
            label1.Text = "S";
            // 
            // txtNodeName
            // 
            txtNodeName.BackColor = Color.FromArgb(30, 30, 30);
            txtNodeName.BorderStyle = BorderStyle.FixedSingle;
            txtNodeName.ForeColor = Color.White;
            txtNodeName.Location = new Point(22, 3);
            txtNodeName.Name = "txtNodeName";
            txtNodeName.Size = new Size(62, 23);
            txtNodeName.TabIndex = 6;
            // 
            // txtShotDesc
            // 
            txtShotDesc.BackColor = Color.FromArgb(30, 30, 30);
            txtShotDesc.BorderStyle = BorderStyle.FixedSingle;
            txtShotDesc.Dock = DockStyle.Fill;
            txtShotDesc.ForeColor = Color.White;
            txtShotDesc.Location = new Point(3, 32);
            txtShotDesc.Multiline = true;
            txtShotDesc.Name = "txtShotDesc";
            txtShotDesc.Size = new Size(132, 0);
            txtShotDesc.TabIndex = 14;
            // 
            // CreateSequencePopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(425, 306);
            Controls.Add(btnAddShot);
            Controls.Add(flowLayoutPanel6);
            Controls.Add(flowLayoutPanel5);
            Controls.Add(btnOK);
            Name = "CreateSequencePopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CreateSequence";
            Load += CreateSequencePopup_Load;
            flowLayoutPanel6.ResumeLayout(false);
            gbSequence.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private FlowLayoutPanel flowLayoutPanel5;
        private FlowLayoutPanel flowLayoutPanel6;
        private GroupBox gbSequence;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label4;
        private TextBox txtNodeDesc;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label label1;
        private TextBox txtNodeName;
        private Button btnAddShot;
        private TextBox txtShotDesc;
    }
}