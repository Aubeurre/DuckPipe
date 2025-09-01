using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DuckPipe.Core.Manipulator;
using DuckPipe.Core.Services;

namespace DuckPipe.Forms.Builder.Tabs
{
    internal class StatsChartTabBuilder
    {
        private static StatsTabContext _ctx;

        public static void InitStatsTab(
            TableLayoutPanel tblpnlTimeLogs,
            FlowLayoutPanel flpAllDeptTimeLogsGraphs,
            ComboBox cbbGraphList,
            Label lblTotalProjectHours,
            Label lblTotalProjectNodes,
            Label lblTotalProjectShots,
            AssetManagerForm form)
        {
            _ctx = new StatsTabContext
            {
                TblpnlTimeLogs = tblpnlTimeLogs,
                FlpAllDeptTimeLogsGraphs = flpAllDeptTimeLogsGraphs,
                CbbGraphList = cbbGraphList,
                LblTotalProjectHours = lblTotalProjectHours,
                LblTotalProjectNodes = lblTotalProjectNodes,
                LblTotalProjectShots = lblTotalProjectShots,
                Form = form
            };
        }

        public static StatsTabContext GetContext(string selectedProd)
        {
            if (_ctx == null)
                throw new InvalidOperationException("StatsTab not initialized. Call InitStatsTab first.");

            return new StatsTabContext
            {
                SelectedProd = selectedProd,
                RootPath = ProductionService.GetProductionRootPath(),
                TblpnlTimeLogs = _ctx.TblpnlTimeLogs,
                FlpAllDeptTimeLogsGraphs = _ctx.FlpAllDeptTimeLogsGraphs,
                CbbGraphList = _ctx.CbbGraphList,
                LblTotalProjectHours = _ctx.LblTotalProjectHours,
                LblTotalProjectNodes = _ctx.LblTotalProjectNodes,
                LblTotalProjectShots = _ctx.LblTotalProjectShots,
                Form = _ctx.Form
            };
        }

        public class StatsTabContext
        {
            public string SelectedProd { get; set; }
            public string RootPath { get; set; }
            public AssetManagerForm Form { get; set; }

            public TableLayoutPanel TblpnlTimeLogs { get; set; }
            public FlowLayoutPanel FlpAllDeptTimeLogsGraphs { get; set; }
            public ComboBox CbbGraphList { get; set; }
            public Label LblTotalProjectHours { get; set; }
            public Label LblTotalProjectNodes { get; set; }
            public Label LblTotalProjectShots { get; set; }
        }

        private static void EmptyTimeLogs(StatsTabContext ctx)
        {
            for (int i = ctx.TblpnlTimeLogs.RowCount - 1; i > 0; i--)
            {
                for (int j = 0; j < ctx.TblpnlTimeLogs.ColumnCount; j++)
                {
                    var control = ctx.TblpnlTimeLogs.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        ctx.TblpnlTimeLogs.Controls.Remove(control);
                        control.Dispose();
                    }
                }
                ctx.TblpnlTimeLogs.RowStyles.RemoveAt(i);
                ctx.TblpnlTimeLogs.RowCount--;
            }
        }

        public static void DisplayTimeLogs(StatsTabContext ctx)
        {
            EmptyTimeLogs(ctx);

            string rootPath = ProductionService.GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, ctx.SelectedProd);
            var logs = TimeLogManager.GetAll(prodPath);
            logs.Reverse();

            int row = 1;
            foreach (var log in logs)
            {
                ctx.TblpnlTimeLogs.RowCount++;
                ctx.TblpnlTimeLogs.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                // NodeName
                var lblNode = new Label
                {
                    Text = log.NodeName,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true
                };
                ctx.TblpnlTimeLogs.Controls.Add(lblNode, 0, row);

                // Department
                var lblDept = new Label
                {
                    Text = log.Department,
                    ForeColor = Color.White,
                    AutoSize = true
                };
                ctx.TblpnlTimeLogs.Controls.Add(lblDept, 1, row);

                // Artist
                var lblArtist = new Label
                {
                    Text = log.Artist,
                    ForeColor = Color.White,
                    AutoSize = true
                };
                ctx.TblpnlTimeLogs.Controls.Add(lblArtist, 2, row);

                // Hours
                var lblHours = new Label
                {
                    Text = log.Hours.ToString(),
                    ForeColor = Color.White,
                    AutoSize = true
                };
                ctx.TblpnlTimeLogs.Controls.Add(lblHours, 3, row);

                // Date
                var lblDate = new Label
                {
                    Text = log.Date.ToString("yyyy-MM-dd"),
                    ForeColor = Color.White,
                    AutoSize = true
                };
                ctx.TblpnlTimeLogs.Controls.Add(lblDate, 4, row);

                row++;
            }
        }

        public static void DisplayStats(StatsTabContext ctx)
        {
            string rootPath = ProductionService.GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, ctx.SelectedProd);
            ctx.LblTotalProjectHours.Text = $"Total Logged Hours: {TimeLogStats.GetTotalHours(prodPath).ToString()}";
            ctx.LblTotalProjectNodes.Text = $"Total Assets: {TimeLogStats.GetTotalAssets(prodPath).ToString()}";
            ctx.LblTotalProjectShots.Text = $"Total Shots: {TimeLogStats.GetTotalShots(prodPath).ToString()}";

            // un peu de magie pour les scrollbar vertical
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            ctx.TblpnlTimeLogs.Padding = new Padding(0, 0, vertScrollWidth, 0);
            ctx.FlpAllDeptTimeLogsGraphs.Padding = new Padding(0, 0, vertScrollWidth, 0);

            ctx.CbbGraphList.SelectedIndex = 0;
        }

        public static void SetEmptyChart(Chart chart, ChartArea area)
        {
            // On supprime les lignes de grille et on garde juste l'axe X
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
            area.AxisY.LabelStyle.Enabled = false; // Supprime les labels Y
            area.AxisY.MajorTickMark.Enabled = false; // Supprime les ticks Y
            area.AxisY.LineWidth = 0;
            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisX.MajorTickMark.Enabled = false; // Pas de ticks verticaux
            chart.ChartAreas[0].BackColor = Color.FromArgb(50, 50, 50);
            chart.ChartAreas[0].BorderColor = Color.FromArgb(50, 50, 50);
            area.BackColor = Color.FromArgb(50, 50, 50);
            chart.BackColor = Color.FromArgb(50, 50, 50);
            area.AxisY.IsMarginVisible = false;
        }

        public static Chart CreateEmptyChart(StatsTabContext ctx, Series series)
        {
            var chart = new Chart();
            var area = new ChartArea();
            chart.ChartAreas.Add(area);
            SetEmptyChart(chart, area);

            chart.Series.Add(series);
            chart.Size = new Size(ctx.FlpAllDeptTimeLogsGraphs.Width - 10, ctx.FlpAllDeptTimeLogsGraphs.Height - 10);
            return chart;
        }

        public static Series CreateEmptySerie(string title)
        {
            var series = new Series(title)
            {
                ChartType = SeriesChartType.StackedColumn,
                BorderWidth = 0,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White
            };
            series["PointWidth"] = "1";
            return series;
        }

        public static void DisplayDeptHourChart(StatsTabContext ctx)
        {
            string rootPath = ProductionService.GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, ctx.SelectedProd);

            string configJsonPath = ProductionService.getConfigJsonPath(prodPath);

            using var configDoc = JsonDocument.Parse(File.ReadAllText(configJsonPath));
            var colors = ProductionService.GetTaskColorsFromConfig(configDoc);

            Series series = CreateEmptySerie("Heures par département");
            Chart chart = CreateEmptyChart(ctx, series);

            foreach (var dept in TimeLogStats.GetAllDepartments(prodPath))
            {
                double hours = TimeLogStats.GetTotalHoursByDepartment(prodPath, dept);
                var point = series.Points.Add(hours);
                point.AxisLabel = dept;
                point.Label = $"{hours:0.#} h";
                if (colors.TryGetValue(dept.ToUpper(), out var color))
                    point.Color = color;
            }
            ctx.FlpAllDeptTimeLogsGraphs.Controls.Add(chart);
        }

        public static void DisplaNodeHourChart(StatsTabContext ctx, string NodeType)
        {
            string rootPath = ProductionService.GetProductionRootPath();
            string prodPath = Path.Combine(rootPath, ctx.SelectedProd);

            string configJsonPath = ProductionService.getConfigJsonPath(prodPath);
            using var configDoc = JsonDocument.Parse(File.ReadAllText(configJsonPath));
            var colors = ProductionService.GetTaskColorsFromConfig(configDoc);

            var allNodes = NodeManip.GetAllNodesInProduction(prodPath);

            // Création du chart vide avec style
            var chart = new Chart();
            var area = new ChartArea();
            chart.ChartAreas.Add(area);
            SetEmptyChart(chart, area);

            chart.Legends.Add(new Legend());
            chart.Size = new Size(ctx.FlpAllDeptTimeLogsGraphs.Width - 10, ctx.FlpAllDeptTimeLogsGraphs.Height - 10);

            // Récupération de tous les départements
            var departments = TimeLogStats.GetAllDepartments(prodPath);
            var seriesDict = new Dictionary<string, Series>();

            // Création d'une série par département (StackedColumn)
            foreach (var dept in departments)
            {
                Series series = new Series(dept)
                {
                    ChartType = SeriesChartType.StackedColumn,
                    LabelForeColor = Color.White
                };
                if (colors.TryGetValue(dept.ToUpper(), out var color))
                    series.Color = color;

                chart.Series.Add(series);
                seriesDict[dept] = series;
            }

            // Parcours des nodes du type NodeType
            if (allNodes.TryGetValue(NodeType, out var charaNodes))
            {
                foreach (var nodeKvp in charaNodes)
                {
                    string nodeName = nodeKvp.Key;

                    foreach (var dept in departments)
                    {
                        double hours = TimeLogStats.GetTotalHoursByNodeAndDept(prodPath, $"{NodeType}/{nodeName}", dept);

                        // Ajoute le point à la série correspondant au département
                        var point = seriesDict[dept].Points.Add(hours);
                        point.AxisLabel = nodeName;
                        point.Label = hours > 0 ? $"{hours:0.#} h" : "";
                    }
                }
            }

            ctx.FlpAllDeptTimeLogsGraphs.Controls.Add(chart);
        }
    }
}
