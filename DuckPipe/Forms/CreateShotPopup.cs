using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuckPipe.Core.Services;

namespace DuckPipe.Forms
{
    public partial class CreateShotPopup : Form
    {
        private AssetManagerForm mainForm;
        public CreateShotPopup(AssetManagerForm form)
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            mainForm = form;

        }
        public string NodeName => txtShotName.Text.Trim();
        public string HostName => cbSeqChoice.SelectedItem?.ToString();
        public string Description => txtShotDesc.Text.Trim();
        public string RangeIn => txtRangeIn.Text.Trim();
        public string RangeOut => txtRangeOut.Text.Trim();

        private void CreateShotPopup_Load(object sender, EventArgs e)
        {
            if (mainForm.cbProdList.SelectedItem == null)
                return;

            string prodRoot = ProductionService.GetProductionRootPath();
            string selectedProd = mainForm.cbProdList.SelectedItem.ToString();
            string sequencesPath = Path.Combine(prodRoot, selectedProd, "Shots", "Sequences");

            if (!Directory.Exists(sequencesPath))
                return;

            var sequenceFolders = Directory.GetDirectories(sequencesPath)
                                           .Select(Path.GetFileName)
                                           .ToArray();

            cbSeqChoice.Items.Clear();
            cbSeqChoice.Items.AddRange(sequenceFolders);
            if (cbSeqChoice.Items.Count > 0)
            {
                cbSeqChoice.SelectedIndex = 0;
            }
        }

        private void cbSeqChoice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddShot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtShotName.Text))
            {
                MessageBox.Show("Veuillez entrer un nom.");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
    }
    }
}
