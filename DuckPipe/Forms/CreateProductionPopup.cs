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
    public partial class CreateProductionPopup : Form
    {
        public CreateProductionPopup()
        {
            InitializeComponent();
        }

        public Dictionary<string, Dictionary<string, DeptInfo>> ProductionStructure { get; private set; }


        #region DRAG AND DROP
        private void DropPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ValueTuple<string, Color>)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }


        private void DropPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ValueTuple<string, Color>)))
            {
                var (workName, color) = ((string, Color))e.Data.GetData(typeof(ValueTuple<string, Color>));
                FlowLayoutPanel pnl = sender as FlowLayoutPanel;


                Panel dropPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.None,
                    AllowDrop = true,
                    AutoSize = true,
                    Width = pnl.ClientSize.Width - 4,
                    Height = 50,
                    BackColor = Color.FromArgb(50, 50, 50),
                    Margin = new Padding(2),
                    WrapContents = false,
                    FlowDirection = FlowDirection.TopDown,
                    Tag = workName
                };
                dropPanel.DragEnter += DropPanel_DragEnter;
                dropPanel.DragDrop += DropFile_DragDrop;
                pnl.Controls.Add(dropPanel);


                Label lbl = new Label
                {
                    Text = workName,
                    ForeColor = color,
                    BackColor = Color.FromArgb(90, 90, 90),
                    AutoSize = false,
                    Width = pnl.ClientSize.Width - 4,
                    Height = 25,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(2),
                };

                dropPanel.Controls.Add(lbl);
            }
        }
        private void DropFile_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ValueTuple<string, Color>)))
            {
                var (workName, color) = ((string, Color))e.Data.GetData(typeof(ValueTuple<string, Color>));
                FlowLayoutPanel pnl = sender as FlowLayoutPanel;

                Label extlbl = new Label
                {
                    Text = workName,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(70, 70, 70),
                    AutoSize = false,
                    Width = pnl.ClientSize.Width - 12,
                    Height = 20,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(4, 1, 4, 1),
                    Name = "Extlbl"
                };

                pnl.Controls.Add(extlbl);
            }
        }

        private void LviewDeptNames_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item != null)
            {
                ListViewItem item = (ListViewItem)e.Item;
                var dragData = (item.Text, item.ForeColor);
                DoDragDrop(dragData, DragDropEffects.Copy);
            }
        }

        #endregion

        public class DeptInfo
        {
            public string ColorHex { get; set; }
            public List<string> Works { get; set; }
        }

        private Dictionary<string, DeptInfo> GetWorksFromPanel(FlowLayoutPanel panel)
        {
            Dictionary<string, DeptInfo> DeptPanelList = new Dictionary<string, DeptInfo>();

            foreach (FlowLayoutPanel flowLayoutPanel in panel.Controls.OfType<FlowLayoutPanel>())
            {
                List<string> works = new List<string>();

                foreach (Label label in flowLayoutPanel.Controls.OfType<Label>().Where(l => l.Name == "Extlbl"))
                {
                    works.Add(label.Text);
                }

                Label label1 = flowLayoutPanel.Controls.OfType<Label>().FirstOrDefault();
                Color color = label1.ForeColor;
                string hexColor = ColorTranslator.ToHtml(color);

                string deptName = flowLayoutPanel.Tag?.ToString() ?? "Unknown";

                DeptPanelList[deptName] = new DeptInfo
                {
                    ColorHex = hexColor,
                    Works = works
                };
            }

            return DeptPanelList;
        }


        private void btnOK_Click_1(object sender, EventArgs e)
        {

            ProductionName = txtProductionName.Text.Trim();
            if (string.IsNullOrEmpty(ProductionName))
            {
                MessageBox.Show("Veuillez entrer un nom de production.");
                return;
            }

            // Récupérer tous les Works déposés
            var propsWorks = GetWorksFromPanel(flpPropsPanel);
            var charaWorks = GetWorksFromPanel(flpCharactersPanel);
            var envWorks = GetWorksFromPanel(flpEnvironmentsPanel);
            var seqWorks = GetWorksFromPanel(flpSequencesPanel);
            var shotsWorks = GetWorksFromPanel(flpShotsPanel);


            // on cree la structure [props: propsWorks, chara: charaWorks, env: envWorks, seq: seqWorks, shots: shotsWorks]

            ProductionStructure = new Dictionary<string, Dictionary<string, DeptInfo>>
    {
        { "Props", propsWorks },
        { "Characters", charaWorks },
        { "Environments", envWorks },
        { "Sequences", seqWorks },
        { "Shots", shotsWorks }
    };

            DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
