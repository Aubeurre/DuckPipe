using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckPipe
{
    public partial class MessageBoxPopup : Form
    {
        public MessageBoxPopup()
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
        }
        public string CommitMessage => rtbMessage.Text;

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }
    }
}
