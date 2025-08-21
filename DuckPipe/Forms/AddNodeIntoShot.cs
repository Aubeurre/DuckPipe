using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckPipe.Forms
{
    public partial class AddNodeIntoShot : Form
    {
        public AddNodeIntoShot(string proodRootPath)
        {
            InitializeComponent();
            LoadTreeView(proodRootPath);
        }

        private void tvSource_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void LoadTreeView(string proodRootPath)
        {
            tvSource.Nodes.Clear();

            TreeNode charaRootNode = new TreeNode("Characters");
            tvSource.Nodes.Add(charaRootNode);
            TreeNode propsRootNode = new TreeNode("Props");
            tvSource.Nodes.Add(propsRootNode);
            TreeNode envRootNode = new TreeNode("Environments");
            tvSource.Nodes.Add(envRootNode);
        }

    }
}
