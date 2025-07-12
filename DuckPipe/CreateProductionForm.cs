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
    public partial class CreateProductionForm : Form
    {
        public CreateProductionForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            ProductionName = txtProductionName.Text.Trim();
            if (string.IsNullOrEmpty(ProductionName))
            {
                MessageBox.Show("Veuillez entrer un nom de production.");
                return;
            }

            DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
