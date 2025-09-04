using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DuckPipe.Core.Services;
using DuckPipe.Core.Utils;
using DuckPipe.Forms.Builder.Shared;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Diagnostics;
using DuckPipe.Core.Manipulator;

namespace DuckPipe.Forms.Builder.NodeTab
{
    internal static class NodeTabBuilder
    {

        private static bool _isUserChange = true;
        private static EventHandler saveClickHandler;
        private static NodeTabContext _ctx;

        public static void InitNodeTab(
    FlowLayoutPanel flpNodeTask,
    Label lblNodeName,
    Label lblNodeType,
    Label lblDescription,
    IconComboBox cbbNodeStatus,
    Button btnEditNode,
    AssetManagerForm form)
        {
            _ctx = new NodeTabContext
            {
                FlpNodeTask = flpNodeTask,
                LblNodeName = lblNodeName,
                LblNodeType = lblNodeType,
                LblDescription = lblDescription,
                CbbNodeStatus = cbbNodeStatus,
                BtnEditNode = btnEditNode,
                Form = form
            };
        }

        public class NodeTabContext
        {
            public string NodePath { get; set; }
            public string SelectedProd { get; set; }
            public AssetManagerForm Form { get; set; }

            public Label LblNodeName { get; set; }
            public Label LblNodeType { get; set; }
            public Label LblDescription { get; set; }
            public IconComboBox CbbNodeStatus { get; set; }
            public Button BtnEditNode { get; set; }

            public FlowLayoutPanel FlpNodeTask { get; set; }
        }
        public static NodeTabContext GetContext(string nodePath, string selectedProd)
        {
            if (_ctx == null)
                throw new InvalidOperationException("NodeTab not initialized. Call InitNodeTab first.");

            return new NodeTabContext
            {
                NodePath = nodePath,
                SelectedProd = selectedProd,
                Form = _ctx.Form,
                FlpNodeTask = _ctx.FlpNodeTask,
                LblNodeName = _ctx.LblNodeName,
                LblNodeType = _ctx.LblNodeType,
                LblDescription = _ctx.LblDescription,
                CbbNodeStatus = _ctx.CbbNodeStatus,
                BtnEditNode = _ctx.BtnEditNode
            };
        }

        public static void ClearPanel(NodeTabContext ctx)
        {
            ctx.LblNodeName.Text = "";
            ctx.LblNodeType.Text = "";
            ctx.LblDescription.Text = "";
            ctx.CbbNodeStatus.Visible = false;
            ctx.BtnEditNode.Visible = false;
            ctx.FlpNodeTask.Controls.Clear();
        }

        public static void DisplayHeader(NodeTabContext ctx)
        {
            var nodeCtx = NodeService.ExtractNodeContext(ctx.NodePath);

            ctx.LblNodeName.Text = $"{nodeCtx.NodeName} |";
            ctx.LblNodeType.Text = nodeCtx.NodeType;
            ctx.BtnEditNode.Visible = true;
            ctx.CbbNodeStatus.Visible = true;

            string jsonPath = Path.Combine(ctx.NodePath, "node.json");
            if (!File.Exists(jsonPath)) return;

            var jsonNode = JsonHelper.ParseJson(jsonPath);
            var nodeInfos = jsonNode?["nodeInfos"];
            if (nodeInfos is null) return;

            // Status
            if (nodeInfos["status"]?.GetValue<string>() is string status)
            {
                PopulateCbbNodeStatus(ctx);
                _isUserChange = false;
                ctx.CbbNodeStatus.SelectedItem = status;
                ctx.CbbNodeStatus.Tag = ctx.NodePath;
                _isUserChange = true;
            }

            // Description
            if (nodeInfos["description"]?.GetValue<string>() is string description)
            {
                if (ctx.NodePath.Contains("\\Shots\\"))
                {
                    string rangeIn = nodeInfos?["inFrame"]?.GetValue<string>() ?? "";
                    string rangeOut = nodeInfos?["outFrame"]?.GetValue<string>() ?? "";
                    ctx.LblDescription.Text = $"[ {rangeIn} - {rangeOut} ] {description}";
                }
                else
                {
                    ctx.LblDescription.Text = description;
                }
            }
        }

        private static void PopulateCbbNodeStatus(NodeTabContext ctx)
        {
            if (ctx.CbbNodeStatus.Items.Count == 0)
            {
                string prodPath = Path.Combine(ProductionService.GetProductionRootPath(), ctx.SelectedProd);
                ImageList _ = ProductionService.LoadStatusIconsIntoImageList(out Dictionary<string, string> statusIcons, prodPath);
                ComboBoxBuilder.PopulateStatusIconCombo(ctx.CbbNodeStatus, statusIcons);
            }
        }

        public static void cbbNodeStatus_SelectedIndexChanged(NodeTabContext ctx)
        {
            // Protection contre les triggers pendant le setup
            if (!_isUserChange)
                return;

            if (ctx.CbbNodeStatus.SelectedItem == null)
                return;


            string selectedStatus = ctx.CbbNodeStatus.SelectedItem?.ToString();

            string nodePath = ctx.CbbNodeStatus.Tag.ToString();
            string NodeName = new DirectoryInfo(nodePath).Name;
            string rootPath = ProductionService.GetProductionRootPath();


            // allNodes.json
            string configPath = Path.Combine(rootPath, ctx.SelectedProd, "Dev", "DangerZone", "allNodes.json");

            JsonNode jsonNodeconfigPath = JsonHelper.ParseJson(configPath);

            jsonNodeconfigPath[NodeName]["status"] = selectedStatus;

            File.WriteAllText(configPath, jsonNodeconfigPath.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true
            }));


            // Node.json
            string nodeJsonPath = Path.Combine(nodePath, "node.json");

            JsonNode jsonNodeNodePath = JsonHelper.ParseJson(nodeJsonPath);

            jsonNodeNodePath["nodeInfos"]["status"] = selectedStatus;

            File.WriteAllText(nodeJsonPath, jsonNodeNodePath.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        public static void DisplayImagePanel(NodeTabContext ctx)
        {
            ctx.FlpNodeTask.SuspendLayout();

            Panel pbpPnel = new()
            {
                AutoSize = false,
                Size = new Size(ctx.FlpNodeTask.ClientSize.Width - 30, 100),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(5),
                Margin = new Padding(5)
            };

            // Label
            pbpPnel.Controls.Add(new Label
            {
                Text = " - Arts",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(5, 5),
                ForeColor = Color.White,
            });

            var images = NodeManip.GetAllImages(ctx.NodePath);

            int offsetX = 10;
            foreach (var image in images)
            {
                var thumb = new PictureBox
                {
                    ImageLocation = image.FullPath,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 60),
                    Cursor = Cursors.Hand,
                    Tag = image.FullPath,
                    Location = new Point(offsetX, 30)
                };

                thumb.DoubleClick += (s, e) =>
                {
                    MessageBox.Show($"Ouverture de: {Path.GetFileName(image.FullPath)}, soyez patient");
                    Process.Start("explorer", $"\"{image.FullPath}\"");
                };

                pbpPnel.Controls.Add(thumb);
                offsetX += 170;
            }

            ctx.FlpNodeTask.Controls.Add(pbpPnel);
            ctx.FlpNodeTask.ResumeLayout();
        }

        public static void DisplayDepartments(NodeTabContext ctx)
        {
            ctx.FlpNodeTask.SuspendLayout();
            ctx.FlpNodeTask.Controls.Clear();

            string jsonPath = Path.Combine(ctx.NodePath, "node.json");
            if (!File.Exists(jsonPath))
                return;

            var jsonNode = JsonHelper.ParseJson(jsonPath);

            // Playblast panel si c'est un shot
            if (ctx.NodePath.Contains("\\Shots\\"))
                DisplayPlayBlastPanel(ctx);

            var tasksNode = jsonNode?["Tasks"];
            if (tasksNode is null)
            {
                ctx.FlpNodeTask.ResumeLayout();
                return;
            }

            // icônes + users
            string prodPath = Path.Combine(ProductionService.GetProductionRootPath(), ctx.SelectedProd);
            ImageList statusImageList = ProductionService.LoadStatusIconsIntoImageList(out Dictionary<string, string> statusIcons, prodPath);
            List<string> users = ProductionService.LoadProdUsers(prodPath);

            foreach (var task in tasksNode.AsObject())
            {
                string dept = task.Key;
                var data = task.Value;
                string status = data?["status"]?.GetValue<string>() ?? "not_started";
                string user = data?["user"]?.GetValue<string>() ?? "";
                string startDate = data?["startDate"]?.GetValue<string>() ?? "";
                string dueDate = data?["dueDate"]?.GetValue<string>() ?? "";

                var taskPanel = NodeTabBuilder.BuildTaskPanel(
                    jsonPath, dept, status, user, startDate, dueDate, statusIcons, users
                );

                taskPanel.Width = ctx.FlpNodeTask.ClientSize.Width - 30;
                ctx.FlpNodeTask.Controls.Add(taskPanel);
            }

            ctx.FlpNodeTask.ResumeLayout();

            // bouton Save
            if (saveClickHandler != null)
                ctx.BtnEditNode.Click -= saveClickHandler;

            saveClickHandler = (s, e) => NodeManip.SaveTaskDataFromUI(jsonPath, ctx.FlpNodeTask);
            ctx.BtnEditNode.Click += saveClickHandler;
        }

        private static void DisplayPlayBlastPanel(NodeTabContext ctx)
        {
            ctx.FlpNodeTask.SuspendLayout();

            Panel pbpPnel = new()
            {
                AutoSize = false,
                Size = new Size(ctx.FlpNodeTask.ClientSize.Width - 30, 100),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(5),
                Margin = new Padding(5)
            };

            // Label
            pbpPnel.Controls.Add(new Label
            {
                Text = " - Playblasts",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(5, 5),
                ForeColor = Color.White,
            });

            var playblasts = ShotManip.GetAllPlayblats(ctx.NodePath);

            int offsetX = 10;
            foreach (var playblast in playblasts)
            {
                var image = new PictureBox
                {
                    ImageLocation = ShotManip.GenerateThumbnail(playblast.FullPath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 60),
                    Cursor = Cursors.Hand,
                    Tag = playblast.FullPath,
                    Location = new Point(offsetX, 30)
                };

                image.DoubleClick += (s, e) =>
                {
                    MessageBox.Show($"Ouverture de: {Path.GetFileName(playblast.FullPath)}, soyez patient");
                    Process.Start("explorer", $"\"{playblast.FullPath}\"");
                };

                pbpPnel.Controls.Add(image);
                offsetX += 170;
            }

            ctx.FlpNodeTask.Controls.Add(pbpPnel);
            ctx.FlpNodeTask.ResumeLayout();
        }


        public static Panel BuildTaskPanel(
            string nodeJsonPath,
            string deptName,
            string status,
            string user,
            string startDate,
            string dueDate,
            Dictionary<string, string> statusIcons,
            List<string> users)
        {
            // ---- Panel racine
            var departmentPanel = new RoundedPanel
            {
                AutoSize = false,
                Size = new Size( /* largeur sera gérée par le parent */ 600, 80),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding(8),
                Margin = new Padding(5),
                BorderColor = Color.FromArgb(90, 90, 90),
                BorderRadius = 5,
                BorderThickness = 0,
            };

            // ---- Main table
            var mainTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            departmentPanel.Controls.Add(mainTable);

            // Label département
            var lblDept = new Label
            {
                Text = $" - {deptName}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
            };

            // ---- Table “big”
            var bigTable = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(60, 60, 60),
                Padding = new Padding(3),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
            };
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            bigTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            bigTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            bigTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Labels
            var lblUser = new Label
            {
                Text = "Assigned to:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            var lblIn = new Label
            {
                Text = "Start Date:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            var lblOut = new Label
            {
                Text = "Due Date:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            var lblStatus = new Label
            {
                Text = "Status:",
                ForeColor = Color.FromArgb(120, 120, 120),
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                Tag = deptName,
            };

            // User combo
            var cbUser = new IconComboBox
            {
                Anchor = AnchorStyles.Left,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Tag = deptName,
                Name = "cbUser",
            };
            cbUser.Items.AddRange(users.ToArray());
            cbUser.SelectedItem = user;

            // Dates
            var dtpStart = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy",
                Value = DateTime.TryParse(startDate, out var parsedStart) ? parsedStart : DateTime.Today,
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(60, 60, 60),
                CalendarTitleBackColor = Color.FromArgb(90, 90, 90),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray,
                Tag = deptName,
                Name = "dtpStartDate"
            };
            dtpStart.BackColor = Color.FromArgb(60, 60, 60);
            dtpStart.ForeColor = Color.White;
            dtpStart.Font = new Font("Segoe UI", 9);

            var dtpDue = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy",
                Value = DateTime.TryParse(dueDate, out var parsedDue) ? parsedDue : DateTime.Today,
                Anchor = AnchorStyles.Left,
                Dock = DockStyle.Fill,
                CalendarForeColor = Color.White,
                CalendarMonthBackground = Color.FromArgb(60, 60, 60),
                CalendarTitleBackColor = Color.FromArgb(90, 90, 90),
                CalendarTitleForeColor = Color.White,
                CalendarTrailingForeColor = Color.Gray,
                Tag = deptName,
                Name = "dtpDueDate"
            };
            dtpDue.BackColor = Color.FromArgb(60, 60, 60);
            dtpDue.ForeColor = Color.White;
            dtpDue.Font = new Font("Segoe UI", 9);

            // Status combo (avec IconMap)
            var cbStatus = new IconComboBox
            {
                Anchor = AnchorStyles.Left,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Tag = deptName,
                Name = "cbStatus",
            };

            cbStatus.Items.AddRange(statusIcons.Keys.ToArray());
            cbStatus.IconMap = statusIcons.ToDictionary(
                kv => kv.Key,
                kv => Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kv.Value))
            );
            _isUserChange = false;
            cbStatus.SelectedItem = status;
            _isUserChange = true;

            // Placement
            mainTable.Controls.Add(lblDept, 0, 0);
            mainTable.Controls.Add(bigTable, 1, 0);

            bigTable.Controls.Add(lblUser, 0, 0);
            bigTable.Controls.Add(lblIn, 1, 0);
            bigTable.Controls.Add(lblOut, 2, 0);
            bigTable.Controls.Add(lblStatus, 3, 0);

            bigTable.Controls.Add(cbUser, 0, 1);
            bigTable.Controls.Add(dtpStart, 1, 1);
            bigTable.Controls.Add(dtpDue, 2, 1);
            bigTable.Controls.Add(cbStatus, 3, 1);

            return departmentPanel;
        }
    }
}
