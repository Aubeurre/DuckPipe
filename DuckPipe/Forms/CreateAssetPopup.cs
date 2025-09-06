using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuckPipe.Core.Services;

namespace DuckPipe
{
    public partial class CreateAssetPopup : Form
    {
        private AssetManagerForm mainForm;
        public CreateAssetPopup(AssetManagerForm form)
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            mainForm = form;

            cbNodeType.SelectedIndex = 0;
        }
        public string NodeName => txtNodeName.Text.Trim();
        public string NodeType => cbNodeType.SelectedItem?.ToString();
        public string Description => tbDescription.Text.Trim();
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNodeName.Text))
            {
                MessageBox.Show("Veuillez entrer un nom.");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CreateAssetPopup_Load(object sender, EventArgs e)
        {
            if (mainForm.cbProdList.SelectedItem == null)
                return;
        }

        private void cbNodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lbSequence_Click(object sender, EventArgs e)
        {

        }
    }
}
