namespace DuckPipe
{
    partial class MessageBoxPopup
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
            rtbMessage = new RichTextBox();
            btnSend = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // rtbMessage
            // 
            rtbMessage.BackColor = Color.FromArgb(30, 30, 30);
            rtbMessage.BorderStyle = BorderStyle.FixedSingle;
            rtbMessage.ForeColor = Color.White;
            rtbMessage.Location = new Point(12, 27);
            rtbMessage.Name = "rtbMessage";
            rtbMessage.Size = new Size(305, 97);
            rtbMessage.TabIndex = 0;
            rtbMessage.Text = "";
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(80, 80, 80);
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(242, 130);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 1;
            btnSend.Text = "Envoyer";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(112, 15);
            label1.TabIndex = 2;
            label1.Text = "Ajouter un message";
            // 
            // MessageBoxPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(329, 165);
            Controls.Add(label1);
            Controls.Add(btnSend);
            Controls.Add(rtbMessage);
            Name = "MessageBoxPopup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "MessageBoxPopup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbMessage;
        private Button btnSend;
        private Label label1;
    }
}