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

namespace DuckPipe
{
    public partial class CreateAssetPopup : Form
    {
        public CreateAssetPopup()
        {
            InitializeComponent();
            cbAssetType.SelectedIndex = 0;
        }
        public string AssetName => txtAssetName.Text.Trim();
        public string AssetType => cbAssetType.SelectedItem?.ToString();
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAssetName.Text))
            {
                MessageBox.Show("Veuillez entrer un nom.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
