using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuckPipe.Core;

namespace DuckPipe.Forms
{
    public partial class TimeLogAddPopup : Form
    {
        public string AssetName { get; private set; }
        public string Department { get; private set; }
        public string Hours { get; private set; }
        public TimeLogAddPopup(List<string> assets)
        {
            InitializeComponent();
            cbAsset.Items.AddRange(assets.ToArray());
            cbAsset.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            {
                AssetName = cbAsset.SelectedItem?.ToString();
                Department = cbDepartment.SelectedItem?.ToString();
                Hours = tbTimeLogged.Text.Trim();

                if (string.IsNullOrEmpty(Hours))
                {
                    MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
