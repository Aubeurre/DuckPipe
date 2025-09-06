using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DuckPipe.Core.Services;
using DuckPipe.Core.Utils;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using DuckPipe.Core.Manager;
using DuckPipe.Core.Manipulator;

namespace DuckPipe.Forms.Builder.Tabs
{
    internal class WorkTabBuilder
    {
        private static WorkTabContext _ctx;

        public static void InitWorkTab(
            FlowLayoutPanel flpPipelineStatus,
            FlowLayoutPanel flpNodeInspect,
            FlowLayoutPanel flpDeptButton,
            Label lblNodeName,
            Label lblNodeType,
            Label lblDescription,
            AssetManagerForm form)
        {
            _ctx = new WorkTabContext
            {
                FlpPipelineStatus = flpPipelineStatus,
                FlpNodeInspect = flpNodeInspect,
                FlpDeptButton = flpDeptButton,
                LblNodeName = lblNodeName,
                LblNodeType = lblNodeType,
                LblDescription = lblDescription,
                Form = form
            };
        }

        public class WorkTabContext
        {
            public string NodePath { get; set; }
            public string SelectedProd { get; set; }
            public AssetManagerForm Form { get; set; }

            public Label LblNodeName { get; set; }
            public Label LblNodeType { get; set; }
            public Label LblDescription { get; set; }
            public FlowLayoutPanel FlpPipelineStatus { get; set; }
            public FlowLayoutPanel FlpNodeInspect { get; set; }
            public FlowLayoutPanel FlpDeptButton { get; set; }
        }

        public static WorkTabContext GetContext(string nodePath, string selectedProd)
        {
            if (_ctx == null)
                throw new InvalidOperationException("WorkTab not initialized. Call InitWorkTab first.");

            // On clone avec les infos dynamiques
            return new WorkTabContext
            {
                NodePath = nodePath,
                SelectedProd = selectedProd,
                Form = _ctx.Form,
                FlpPipelineStatus = _ctx.FlpPipelineStatus,
                FlpNodeInspect = _ctx.FlpNodeInspect,
                FlpDeptButton = _ctx.FlpDeptButton,
                LblNodeName = _ctx.LblNodeName,
                LblNodeType = _ctx.LblNodeType,
                LblDescription = _ctx.LblDescription
            };
        }

        /// <summary> 
        /// 

        private static float GetScaleFactor()
        {
            // Get the system DPI scale factor
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                return g.DpiX / 96f; // 96 DPI = 100%
            }
        }

        public static Panel BuildWorkDepartmentsPanel(
            string jsonPath,
            string deptName,
            int panelWidth,
            Action<string> onNodeItemSelected,
            ImageList statusImageList,
            Dictionary<string, string> statusIcons,
            string selectedProd,
            string rootPath)
        {
            float scale = GetScaleFactor();

            var departmentPanel = new RoundedPanel
            {
                AutoSize = false,
                Size = new Size((int)((panelWidth - 30 * scale) ), (int)(80 * scale)),
                BackColor = Color.FromArgb(80, 80, 80),
                Padding = new Padding((int)(8 * scale)),
                Margin = new Padding((int)(5 * scale)),
                Location = new Point((int)(20 * scale), (int)(20 * scale)),
                BorderColor = Color.FromArgb(90, 90, 90),
                BorderRadius = 5,
                BorderThickness = 0,
            };

            departmentPanel.Controls.Add(new Label
            {
                Text = $" - {deptName}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // fonts fixes
                Location = new Point((int)(5 * scale), (int)(5 * scale)),
                ForeColor = Color.White,
            });

            var listView = new ListView()
            {
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.None,
            };
            listView.HeaderStyle = ColumnHeaderStyle.None;

            listView.Columns.Add("Fichier", (int)((panelWidth - 180 * scale)));
            listView.Columns.Add("User", (int)(70 * scale));
            listView.Columns.Add("Version", (int)(50 * scale));

            listView.MouseClick += (s, e) =>
            {
                if (listView.FocusedItem != null)
                {
                    string nodePath = listView.FocusedItem.Tag?.ToString();
                    if (!string.IsNullOrEmpty(nodePath))
                        onNodeItemSelected(nodePath);
                }
            };

            listView.Tag = Tuple.Create(jsonPath, deptName);

            Panel listContainer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = (int)(50 * scale),
                Padding = new Padding((int)(3 * scale)),
            };
            listContainer.Controls.Add(listView);
            departmentPanel.Controls.Add(listContainer);

            FillDepartementPanel(listView, jsonPath, deptName, statusImageList, statusIcons, selectedProd, rootPath);

            return departmentPanel;
        }

        public static void FillDepartementPanel(
            ListView listView,
            string nodeJsonPath,
            string department,
            ImageList statusImageList,
            Dictionary<string, string> statusIcons,
            string selectedProd,
            string rootPath)
        {
            listView.Items.Clear();

            // node.json
            JsonDocument doc = NodeManip.LoadNodeJson(nodeJsonPath);
            if (!doc.RootElement.TryGetProperty("workfile", out JsonElement workfiles)) return;

            listView.SmallImageList = statusImageList;

            // workfiles
            foreach (JsonProperty fileEntry in workfiles.EnumerateObject())
            {
                JsonElement fileData = fileEntry.Value;

                if (!fileData.TryGetProperty("department", out JsonElement deptProp)) continue;
                string dept = deptProp.GetString() ?? "";
                if (dept != department) continue;

                string workPath = NodeManip.ReplaceEnvVariables(Path.Combine(Path.GetDirectoryName(nodeJsonPath), "Work", dept));
                string status = fileData.GetProperty("status").GetString() ?? "not_started";
                string version = fileData.GetProperty("version").GetString() ?? "v000";
                string fileName = fileData.GetProperty("workFile").GetString() ?? "";
                string file = Path.Combine(workPath, fileName);

                if (!file.EndsWith(".json") && !file.EndsWith(".lock"))
                {
                    string userLocked = LockNodeFileManager.GetuserLocked(file);
                    if (userLocked != "")
                    {
                        userLocked = $"🔒 {userLocked}";
                    }

                    ListViewItem item = new ListViewItem($" |  {fileName}");
                    item.SubItems.Add(userLocked);
                    item.SubItems.Add(version);

                    if (statusImageList.Images.ContainsKey(status))
                    {
                        item.ImageKey = status;
                    }

                    item.Tag = file;
                    listView.Items.Add(item);
                }
            }
        }

        public static void DisplayHeader(
            JsonNode jsonNode,
            NodeContext nodeServicectx,
            WorkTabContext worktabctx)
        {

            // Met à jour les labels
            worktabctx.LblNodeName.Text = $"{nodeServicectx.NodeName} |";
            worktabctx.LblNodeType.Text = nodeServicectx.NodeType;

            var nodeInfos = jsonNode?["nodeInfos"];
            var description = nodeInfos?["description"]?.GetValue<string>();

            if (description != null)
            {
                if (worktabctx.NodePath.Contains("\\Shots\\"))
                {
                    string rangeIn = nodeInfos?["inFrame"]?.GetValue<string>() ?? "";
                    string rangeOut = nodeInfos?["outFrame"]?.GetValue<string>() ?? "";
                    worktabctx.LblDescription.Text = $"[ {rangeIn} - {rangeOut} ] {description}";
                }
                else
                {
                    worktabctx.LblDescription.Text = description;
                }
            }
        }

        public static void DisplayDepartments(WorkTabContext ctx)
        {
            ctx.FlpPipelineStatus.SuspendLayout();
            ctx.FlpPipelineStatus.Controls.Clear();
            ctx.FlpNodeInspect.Controls.Clear();
            ctx.FlpDeptButton.Controls.Clear();

            string jsonPath = Path.Combine(ctx.NodePath, "node.json");
            var jsonNode = JsonHelper.ParseJson(jsonPath);
            if (jsonNode?["workfile"] is null)
                return;

            // On construit le dictionnaire département -> (status, version)
            var departmentMap = new Dictionary<string, (string status, string version)>();
            foreach (var fileEntry in jsonNode["workfile"].AsObject())
            {
                var fileData = fileEntry.Value;
                string dept = fileData?["department"]?.GetValue<string>() ?? "Unknown";
                string status = fileData?["status"]?.GetValue<string>() ?? "not_started";
                string version = fileData?["version"]?.GetValue<string>() ?? "v001";

                departmentMap[dept] = (status, version);
            }

            // On crée les panels pour chaque département
            string prodPath = Path.Combine(ProductionService.GetProductionRootPath(), ctx.SelectedProd);
            ImageList statusImageList = ProductionService.LoadStatusIconsIntoImageList(
                out Dictionary<string, string> statusIcons, prodPath);

            foreach (var deptName in departmentMap.Keys)
            {
                var departmentPanel = BuildWorkDepartmentsPanel(
                    jsonPath,
                    deptName,
                    ctx.FlpPipelineStatus.ClientSize.Width,
                    OnNodeItemSelected(ctx),
                    statusImageList,
                    statusIcons,
                    ctx.SelectedProd,
                    prodPath
                );
                ctx.FlpPipelineStatus.Controls.Add(departmentPanel);
            }


            ctx.FlpPipelineStatus.ResumeLayout();
        }


        private static Button CreateActionButton(
            string label,
            Action<string, AssetManagerForm> onValidSelection,
            AssetManagerForm form,
            string nodePath)
        {
            float scale = GetScaleFactor();

            var btn = new Button
            {
                Text = label,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(105, 105, 105),
                ForeColor = Color.White,
                Padding = new Padding((int)(0)),
                Margin = new Padding((int)(0)),
        AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size((int)(60 * scale), (int)(25 * scale))
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.LightGray;

            btn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(nodePath))
                    onValidSelection(nodePath, form);
                else
                    MessageBox.Show("Aucun node sélectionné dans l’arborescence.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };

            return btn;
        }

        public static void AddActionButtons(
            string nodePath, 
            FlowLayoutPanel flpDeptButton, 
            AssetManagerForm form)
        {
            flpDeptButton.Controls.Clear();


            // on check le lock pour voir quelles actions l'user peut faire.
            // on ajoutera un SuperUser plus tard
            string workFolderPath = Path.GetDirectoryName(nodePath);
            string fileName = Path.GetFileNameWithoutExtension(nodePath);
            string FileExt = Path.GetExtension(nodePath);
            string lockFile = Path.Combine(workFolderPath, $"{fileName}{FileExt}.lock");
            string lockedByUser = "";
            if (!File.Exists(lockFile))
            {
                Button grabBtn = CreateActionButton("Grab", LockNodeFileManager.TryLockFile, form, nodePath); //if no lock file
                flpDeptButton.Controls.Add(grabBtn);
            }
            else
            {
                lockedByUser = File.ReadAllText(lockFile);
                if (lockedByUser == ProductionService.GetUserName())
                {
                    Button ungrabBtn = CreateActionButton("Ungrab", LockNodeFileManager.UnlockFile, form, nodePath); //if user is grabbed
                    flpDeptButton.Controls.Add(ungrabBtn);
                }
            }
            Button runBtn = CreateActionButton("Run", NodeManip.LaunchNode, form, nodePath);
            flpDeptButton.Controls.Add(runBtn);
            if (File.Exists(lockFile))
            {
                lockedByUser = File.ReadAllText(lockFile);
                if (lockedByUser == ProductionService.GetUserName())
                {
                    Button ExecBtn = CreateActionButton("Exec", NodeManip.ExecNode, form, nodePath); //if user is grabbed
                    flpDeptButton.Controls.Add(ExecBtn);
                    Button incrementBtn = CreateActionButton("Increment", NodeManip.VersionNode, form, nodePath); //if user is grabbed
                    flpDeptButton.Controls.Add(incrementBtn);
                    Button publishBtn = CreateActionButton("Publish", NodeManip.PublishNode, form, nodePath); //if user is grabbed
                    flpDeptButton.Controls.Add(publishBtn);
                }
                    
            }
            Button addBtn = CreateActionButton("Add Note", NodeManip.AddNote, form, nodePath);
            flpDeptButton.Controls.Add(addBtn);

            Button editRefNode = CreateActionButton("Edit Ref Node", NodeManip.AddRef, form, nodePath);
            flpDeptButton.Controls.Add(editRefNode);

            float ratio = 0.85f;
            form.splitContWorkPanel.SplitterDistance = (int)(form.splitContWorkPanel.Height * ratio);
            form.splitContWorkPanel.Panel2Collapsed = false;
        }

        public static void DisplayCommitsPanel(
     string nodePath,
     FlowLayoutPanel flpNodeInspect)
        {
            float scale = GetScaleFactor();
            flpNodeInspect.SuspendLayout();
            flpNodeInspect.Controls.Clear();

            var commits = NodeManip.GetNodeCommits(nodePath);
            commits.Reverse();

            foreach (var commit in commits)
            {
                var panel = new Panel
                {
                    Width = (int)((flpNodeInspect.ClientSize.Width - 10 * scale)),
                    AutoSize = false,
                    BackColor = Color.LightGray,
                    Padding = new Padding((int)(5 * scale)),
                    Margin = new Padding((int)(3 * scale))
                };

                var lblHeader = new Label
                {
                    Text = $"v{commit.Version} - {commit.Timestamp:dd/MM/yyyy HH:mm} \n - {commit.User}",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                var lblMessage = new Label
                {
                    Text = commit.Message,
                    AutoSize = true,
                    MaximumSize = new Size(panel.Width - (int)(10 * scale), 0),
                    Font = new Font("Segoe UI", 9)
                };

                panel.Controls.Add(lblHeader);
                panel.Controls.Add(lblMessage);
                lblMessage.Location = new Point(0, lblHeader.Bottom + (int)(4 * scale));

                flpNodeInspect.Controls.Add(panel);
            }

            flpNodeInspect.ResumeLayout();
        }

        public static Action<string> OnNodeItemSelected(WorkTabContext ctx)
        {
            return (selectedNodePath) =>
            {
                DisplayCommitsPanel(selectedNodePath, ctx.FlpNodeInspect);
                AddActionButtons(selectedNodePath, ctx.FlpDeptButton, ctx.Form);
            };
        }

        public static void ClearPanel(WorkTabContext ctx)
        {
            ctx.LblNodeName.Text = "";
            ctx.LblNodeType.Text = "";
            ctx.LblDescription.Text = "";
            ctx.FlpPipelineStatus.Controls.Clear();
            ctx.FlpNodeInspect.Controls.Clear();
            ctx.FlpDeptButton.Controls.Clear();
        }

        public static void RefreshTab(WorkTabContext ctx)
        {
            string jsonPath = Path.Combine(ctx.NodePath, "node.json");
            var jsonNode = JsonHelper.ParseJson(jsonPath);
            var nodeServicectx = NodeService.ExtractNodeContext(ctx.NodePath);

            ctx.FlpPipelineStatus.Controls.Clear();
            ctx.FlpNodeInspect.Controls.Clear();
            ctx.FlpDeptButton.Controls.Clear();

            DisplayHeader(jsonNode, nodeServicectx, ctx);
            DisplayDepartments(ctx);
        }
    }
}