using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using DuckPipe.Core.Services;
using DuckPipe.Core.Manipulator;

namespace DuckPipe.Forms.Builder.Tabs
{
    internal class ScheduleTabBuilder
    {
        private static TimelineContext _ctx;

        public static void InitTimelineTab(Panel pnlShelude, AssetManagerForm form)
        {
            _ctx = new TimelineContext
            {
                PnlShelude = pnlShelude,
                Form = form
            };
        }

        public static TimelineContext GetContext(string selectedProd)
        {
            if (_ctx == null)
                throw new InvalidOperationException("Timeline not initialized. Call InitTimeline first.");

            return new TimelineContext
            {
                SelectedProd = selectedProd,
                ProdPath = Path.Combine(ProductionService.GetProductionRootPath(), selectedProd),
                PnlShelude = _ctx.PnlShelude,
                Form = _ctx.Form
            };
        }

        public class TimelineContext
        {
            public string SelectedProd { get; set; }
            public string ProdPath { get; set; }
            public AssetManagerForm Form { get; set; }

            public Panel PnlShelude { get; set; }
        }

        public static void DisplaySchedule(TimelineContext ctx)
        {
            ctx.PnlShelude.Controls.Clear();

            if (string.IsNullOrEmpty(ctx.SelectedProd)) return;

            var allNodes = NodeManip.GetAllNodesInProduction(ctx.ProdPath);

            string configJsonPath = ProductionService.getConfigJsonPath(ctx.ProdPath);
            using var doc = JsonDocument.Parse(File.ReadAllText(configJsonPath));

            DateTime startingDate = DateTime.Parse(doc.RootElement.GetProperty("created").GetString());
            DateTime endDate = DateTime.Parse(doc.RootElement.GetProperty("deliveryDay").GetString());

            int totalDays = (int)(endDate - startingDate).TotalDays;
            int todayOffset = (int)(DateTime.Today - startingDate).TotalDays;

            var taskColors = ProductionService.GetTaskColorsFromConfig(doc);

            var mainTable = CreateMainTable(totalDays, ctx);
            var timelineHeader = CreateTimelineHeader(startingDate, totalDays);

            mainTable.Controls.Add(timelineHeader, 1, 0);

            AddNodeRows(mainTable, allNodes, taskColors, startingDate, todayOffset, timelineHeader.Width);

            var scrollWrapper = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            scrollWrapper.Controls.Add(mainTable);

            ctx.PnlShelude.Controls.Add(scrollWrapper);
        }

        private static TableLayoutPanel CreateMainTable(int totalDays, TimelineContext ctx)
        {
            var mainTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(40, 40, 40),
                Padding = new Padding(0)
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, totalDays * 15));

            var cbFilter = new IconComboBox
            {
                Anchor = AnchorStyles.Left,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Name = "sheludeFILTER",
                Tag = mainTable
            };

            List<string> users = ProductionService.LoadProdUsers(ctx.ProdPath);
            cbFilter.Items.AddRange(new string[] { "All", "Characters", "Props", "Environments", "Sequences", "Shots" });
            cbFilter.Items.AddRange(users.ToArray());
            cbFilter.SelectedIndex = 0;

            mainTable.Controls.Add(cbFilter, 0, 0);
            cbFilter.SelectedIndexChanged += (s, e) => OnFilterChanged(mainTable, ctx);

            return mainTable;
        }

        private static void OnFilterChanged(TableLayoutPanel mainTable, TimelineContext ctx)
        {
            ClearTableLayout(mainTable);

            string configJsonPath = ProductionService.getConfigJsonPath(ctx.ProdPath);
            using var doc = JsonDocument.Parse(File.ReadAllText(configJsonPath));
            var taskColors = ProductionService.GetTaskColorsFromConfig(doc);

            var allNodes = NodeManip.GetAllNodesInProduction(ctx.ProdPath);
            DateTime startingDate = DateTime.Parse(doc.RootElement.GetProperty("created").GetString());
            int todayOffset = (int)(DateTime.Today - startingDate).TotalDays;

            int timelineWidth = mainTable.GetControlFromPosition(1, 0).Width;

            AddNodeRows(mainTable, allNodes, taskColors, startingDate, todayOffset, timelineWidth);
        }

        private static void ClearTableLayout(TableLayoutPanel tableLayout)
        {
            for (int i = tableLayout.Controls.Count - 1; i >= 0; i--)
            {
                var control = tableLayout.Controls[i];
                var position = tableLayout.GetRow(control);
                if (position > 0)
                {
                    tableLayout.Controls.RemoveAt(i);
                    control.Dispose();
                }
            }

            for (int i = tableLayout.RowStyles.Count - 1; i >= 1; i--)
            {
                tableLayout.RowStyles.RemoveAt(i);
            }

            tableLayout.RowCount = 1;
        }

        private static FlowLayoutPanel CreateTimelineHeader(DateTime startDate, int totalDays)
        {
            var timelineHeader = new FlowLayoutPanel
            {
                Height = 45,
                Width = totalDays * 15,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.FromArgb(40, 40, 40),
            };

            DateTime currentDate = startDate;
            int remainingDays = totalDays;
            int monthIndex = 0;

            while (remainingDays > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                int startDay = (currentDate.Day == 1) ? 1 : currentDate.Day;
                int remainingDaysInMonth = daysInMonth - startDay + 1;
                int daysToDisplay = Math.Min(remainingDaysInMonth, remainingDays);

                Color bgColor = (monthIndex % 2 == 0) ? Color.FromArgb(50, 50, 50) : Color.FromArgb(30, 30, 30);

                var monthPanel = new FlowLayoutPanel
                {
                    Height = 45,
                    Width = daysToDisplay * 15,
                    FlowDirection = FlowDirection.TopDown,
                    Margin = new Padding(0),
                    BackColor = bgColor,
                    WrapContents = false
                };

                monthPanel.Controls.Add(new Label
                {
                    Text = currentDate.ToString("MMMM"),
                    Height = 15,
                    Width = daysToDisplay * 15,
                    Margin = new Padding(0),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 7, FontStyle.Bold),
                    ForeColor = Color.White
                });

                var dayPanel = new FlowLayoutPanel
                {
                    Height = 15,
                    Width = daysToDisplay * 15,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(0),
                    WrapContents = false
                };

                for (int d = 0; d < daysToDisplay; d++)
                {
                    int dayNumber = currentDate.Day + d;
                    dayPanel.Controls.Add(new Label
                    {
                        Text = dayNumber.ToString(),
                        Width = 15,
                        Height = 15,
                        Font = new Font("Segoe UI", 6),
                        BackColor = bgColor,
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(0)
                    });
                }

                monthPanel.Controls.Add(dayPanel);
                timelineHeader.Controls.Add(monthPanel);

                remainingDays -= daysToDisplay;
                currentDate = currentDate.AddDays(daysToDisplay);
                monthIndex++;
            }

            return timelineHeader;
        }

        private static void AddNodeRows(TableLayoutPanel mainTable, Dictionary<string, Dictionary<string, Dictionary<string, TaskData>>> allNodes, Dictionary<string, Color> taskColors, DateTime startingDate, int todayOffset, int timelineWidth)
        {
            ComboBox cbFilter = mainTable.GetControlFromPosition(0, 0) as ComboBox;
            string selectedValue = cbFilter.SelectedItem?.ToString();

            foreach (var typeEntry in allNodes)
            {
                if (selectedValue == "All" || typeEntry.Key == selectedValue)
                {
                    foreach (var nodeEntry in typeEntry.Value)
                    {
                        string nodeName = nodeEntry.Key;
                        var tasks = nodeEntry.Value;

                        mainTable.RowCount += 1;
                        int currentRow = mainTable.RowCount - 1;

                        var lblNode = new Label
                        {
                            Text = nodeName,
                            Width = 150,
                            Height = 15,
                            Font = new Font("Segoe UI", 8),
                            ForeColor = Color.White,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Dock = DockStyle.Top,
                            BackColor = Color.FromArgb(60, 60, 60),
                            Padding = new Padding(0)
                        };

                        mainTable.Controls.Add(lblNode, 0, currentRow);

                        var taskLine = CreateTaskLine(tasks, taskColors, startingDate, todayOffset, timelineWidth);
                        mainTable.Controls.Add(taskLine, 1, currentRow);
                    }
                }

            }
        }

        private static Panel CreateTaskLine(Dictionary<string, TaskData> tasks, Dictionary<string, Color> taskColors, DateTime startingDate, int todayOffset, int width)
        {
            var taskLine = new Panel
            {
                Width = width,
                Height = tasks.Count * 14,
                BackColor = Color.FromArgb(60, 60, 60),
                AutoScroll = true,
                Padding = new Padding(0)
            };

            // Ligne "Today"
            var taskTodayLine = new Panel
            {
                Width = 2,
                Height = taskLine.Height,
                BackColor = Color.Red,
                Location = new Point(todayOffset * 15, 0),
                Margin = new Padding(0)
            };
            taskLine.Controls.Add(taskTodayLine);
            taskTodayLine.BringToFront();

            int taskIndex = 0;
            foreach (var taskEntry in tasks)
            {
                DateTime start = DateTime.Parse(taskEntry.Value.StartDate);
                DateTime end = DateTime.Parse(taskEntry.Value.DueDate);

                int offsetDays = (int)(start - startingDate).TotalDays + 1;
                int durationDays = Math.Max(1, (int)(end - start).TotalDays + 1);

                int leftMargin = offsetDays * 15;
                int blockWidth = durationDays * 15;

                Color blockColor = taskColors.TryGetValue(taskEntry.Key.ToUpper(), out var color)
                    ? color
                    : Color.FromArgb(70, 70, 70);

                var block = new Panel
                {
                    Width = blockWidth,
                    Height = 10,
                    BackColor = blockColor,
                    Margin = new Padding(0),
                    Location = new Point(leftMargin, taskIndex * 14)
                };

                var lblTask = new Label
                {
                    Text = $"{taskEntry.Key}: {taskEntry.Value.User} ({taskEntry.Value.StartDate} → {taskEntry.Value.DueDate})",
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 6),
                };

                block.Controls.Add(lblTask);

                var tooltip = new ToolTip();
                tooltip.SetToolTip(lblTask, lblTask.Text);

                taskLine.Controls.Add(block);

                taskIndex++;
            }

            return taskLine;
        }
    }
}
