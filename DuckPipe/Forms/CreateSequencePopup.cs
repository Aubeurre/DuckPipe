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
    public partial class CreateSequencePopup : Form
    {
        private AssetManagerForm mainForm;
        public CreateSequencePopup(AssetManagerForm form)
        {
            InitializeComponent();
            mainForm = form;

        }
        public string NodeName => txtNodeName.Text.Trim();
        public string Description => txtNodeDesc.Text.Trim();

        private void CreateSequencePopup_Load(object sender, EventArgs e)
        {
            if (mainForm.cbProdList.SelectedItem == null)
                return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNodeName.Text))
            {
                MessageBox.Show("Veuillez entrer un nom.");
                return;
            }

            var shots = GetShots();

            DialogResult = DialogResult.OK;
            Close();
        }

        private GroupBox CreateShotGroup()
        {
            GroupBox newShotGroup = new GroupBox
            {
                ForeColor = Color.WhiteSmoke,
                Text = "Shot",
                AutoSize = false,
                Size = new Size(gbSequence.ClientSize.Width, 100),
                Padding = new Padding(5)
            };

            // === Layout principal vertical ===
            FlowLayoutPanel mainPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Dock = DockStyle.Fill,
                AutoScroll = false,
                AutoSize = false
            };

            // === Ligne Nom + Range === (TableLayoutPanel au lieu de FlowLayout)
            TableLayoutPanel shotInfoPanel = new TableLayoutPanel
            {
                ColumnCount = 7,
                RowCount = 1,
                Dock = DockStyle.Top,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Colonnes fixes
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20));  // Label P
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));  // txtName
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize ));  // Label empty
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // Label Range
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // txtRangeIn
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15));  // "-"
            shotInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // txtRangeOut

            Label lblName = new Label
            {
                Text = "P",
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            TextBox txtName = new TextBox
            {
                Name = "txtName",
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            Label lblEmpty = new Label
            {
                Text = "",
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 2, 0)
            };

            Label lblRange = new Label
            {
                Text = "Range",
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(2, 0, 2, 0)
            };

            TextBox txtRangeIn = new TextBox
            {
                Name = "txtRangeIn",
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            Label lblRangeSep = new Label
            {
                Text = "-",
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            TextBox txtRangeOut = new TextBox
            {
                Name = "txtRangeOut",
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            shotInfoPanel.Controls.Add(lblName, 0, 0);
            shotInfoPanel.Controls.Add(txtName, 1, 0);
            shotInfoPanel.Controls.Add(lblEmpty, 2, 0);
            shotInfoPanel.Controls.Add(lblRange, 3, 0);
            shotInfoPanel.Controls.Add(txtRangeIn, 4, 0);
            shotInfoPanel.Controls.Add(lblRangeSep, 5, 0);
            shotInfoPanel.Controls.Add(txtRangeOut, 6, 0);

            // === Ligne Description ===
            FlowLayoutPanel shotDescPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Top
            };

            Label lblDesc = new Label
            {
                Text = "Description",
                ForeColor = Color.White,
                Margin = new Padding(1, 1, 1, 1)
            };

            TextBox txtDesc = new TextBox
            {
                Name = "txtDesc",
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = false,
                Size = new Size(gbSequence.ClientSize.Width -125, 30),
                Height = 25
            };

            shotDescPanel.Controls.AddRange(new Control[] { lblDesc, txtDesc });

            // Ajout des panels au mainPanel
            mainPanel.Controls.Add(shotInfoPanel);
            mainPanel.Controls.Add(shotDescPanel);

            // Ajout au groupbox
            newShotGroup.Controls.Add(mainPanel);

            return newShotGroup;
        }

        private void btnAddShot_Click(object sender, EventArgs e)
        {
            var newShotGroup = CreateShotGroup();
            flowLayoutPanel5.Controls.Add(newShotGroup);
            flowLayoutPanel5.Controls.SetChildIndex(newShotGroup, flowLayoutPanel5.Controls.Count - 2);

            // un peu de magie pour les scrollbar vertical
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            flowLayoutPanel5.Padding = new Padding(0, 0, vertScrollWidth, 0);

        }

        public List<ShotContext> GetShots()
        {
            List<ShotContext> shots = new List<ShotContext>();

            foreach (Control ctrl in flowLayoutPanel5.Controls)
            {
                if (ctrl is GroupBox group && group.Text == "Shot")
                {
                    TextBox txtName = group.Controls.Find("txtName", true).FirstOrDefault() as TextBox;
                    TextBox txtRangeIn = group.Controls.Find("txtRangeIn", true).FirstOrDefault() as TextBox;
                    TextBox txtRangeOut = group.Controls.Find("txtRangeOut", true).FirstOrDefault() as TextBox;
                    TextBox txtDesc = group.Controls.Find("txtDesc", true).FirstOrDefault() as TextBox;

                    if (txtName != null)
                    {
                        shots.Add(new ShotContext
                        {
                            Name = $"P{txtName.Text.Trim()}",
                            RangeIn = txtRangeIn?.Text.Trim(),
                            RangeOut = txtRangeOut?.Text.Trim(),
                            Description = txtDesc?.Text.Trim()
                        });
                    }
                }
            }

            return shots;
        }


    }
}
