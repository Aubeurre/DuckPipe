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
            mainForm = form;

            cbAssetType.SelectedIndex = 0;
            cbSeqChoice.Visible = false;
            lbSequence.Visible = false;
            lbRange.Visible = false;
            tbRangeIn.Visible = false;
            tbRangeOut.Visible = false;
            lbRangeSeparator.Visible = false;
        }
        public string AssetName => txtAssetName.Text.Trim();
        public string AssetType => cbAssetType.SelectedItem?.ToString();
        public string SeqName => cbSeqChoice.SelectedItem?.ToString() ?? "";
        public string Description => tbDescription.Text.Trim();
        public string rangeIn => tbRangeIn.Text.Trim();
        public string rangeOut => tbRangeOut.Text.Trim();
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

        private void CreateAssetPopup_Load(object sender, EventArgs e)
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

        private void cbAssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbAssetType.SelectedItem?.ToString() == "Shots"))
            {
                cbSeqChoice.Visible = true;
                lbSequence.Visible = true;
                lbRange.Visible = true;
                tbRangeIn.Visible = true;
                tbRangeOut.Visible = true;
                lbRangeSeparator.Visible = true;
            }
            else
            {
                cbSeqChoice.Visible = false;
                lbSequence.Visible = false;
                lbRange.Visible = false;
                tbRangeIn.Visible = false;
                tbRangeOut.Visible = false;
                lbRangeSeparator.Visible = false;
            }
        }
    }
}
