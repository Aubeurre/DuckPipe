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
using DuckPipe.Core.Services;
using DuckPipe.Core.Manipulator;

namespace DuckPipe.Forms
{
    public partial class AddreferencesPopup : Form
    {
        public string NodeName { get; private set; }
        public string Department { get; private set; }
        public string hostName { get; private set; }
        public AddreferencesPopup(List<string> nodes, string nodePath)
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            cbNode.Items.AddRange(nodes.ToArray());
            cbNode.SelectedIndex = 0;
            FillRefList(nodePath);
            hostName = nodePath;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NodeName = cbNode.SelectedItem?.ToString();
            Department = cbDepartment.SelectedItem?.ToString();

            DialogResult = DialogResult.OK;
        }
        private void FillRefList(string nodePath)
        {
            foreach (var refPath in NodeManip.GetAllRefs(nodePath))
            {
                listRefAdded.Items.Add(Path.GetFileName(refPath) + " (" + refPath + ")");
            }
        }

        private void cmsRemove_Opening(object sender, CancelEventArgs e)
        {
            if (listRefAdded.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeManip.RemoveRef(listRefAdded.SelectedItem.ToString(), hostName);
        }
    }
}
