﻿using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Power_LoRa.Interface
{
    public partial class MainWindow : Form
    {
        #region Private variables
        private ChartControl bigChart;
        #endregion

        #region Properties
        public List<RadioNodeGroupBox> RadioNodeInterfaces { get; private set; }
        public FlowLayoutPanel FlowLayout { get; private set; }
        public TableLayoutPanel TableLayout { get; private set; }
        public ChartControl BigChart
        {
            //TODO: remove get
            get
            {
                return bigChart;
            }
            set
            {
                bigChart.Titles.Clear();
                bigChart.Series.Clear();

                foreach (Title title in value.Titles)
                    bigChart.Titles.Add(title);
                foreach (Series series in value.Series)
                    bigChart.Series.Add(series);
            }
        }
        #endregion

        #region Constructors
        public MainWindow()
		{
			RadioNodeInterfaces = new List<RadioNodeGroupBox>();

            bigChart = new ChartControl(null, "Big Chart")
            {
                Dock = DockStyle.Fill,
            };
            bigChart.Legends.Clear();
            bigChart.Legends.Add(new Legend()
            {
                Enabled = true,
                IsDockedInsideChartArea = true,
                DockedToChartArea = "ChartArea",
                Docking = Docking.Top | Docking.Left,
                Name = "LeftLegend",
            });
            bigChart.Legends.Add(new Legend()
            {
                Enabled = true,
                IsDockedInsideChartArea = true,
                DockedToChartArea = "ChartArea",
                Docking = Docking.Top | Docking.Right,
                Name = "RightLegend",
            });
            bigChart.ChartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            bigChart.ChartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            bigChart.ChartArea.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            bigChart.ChartArea.AxisX.Enabled = AxisEnabled.True;
            bigChart.ChartArea.AxisY.Enabled = AxisEnabled.True;
            bigChart.ChartArea.AxisY2.Enabled = AxisEnabled.True;
            bigChart.ChartArea.AxisX.LabelStyle.Enabled = true;
            bigChart.ChartArea.AxisY.LabelStyle.Enabled = true;
            bigChart.ChartArea.AxisY2.LabelStyle.Enabled = true;
            bigChart.ChartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
            bigChart.ChartArea.Position.Auto = true;
            bigChart.MaxPoints = 100;

            FlowLayout = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Margin = new Padding(0),
                Name = "FlowLayout",
            };

            TableLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 2,
                Padding = new Padding(3),
                Name = "TableLayout",
            };

            Controls.Add(TableLayout);
            TableLayout.Controls.Add(FlowLayout);
            TableLayout.Controls.Add(bigChart);
            TableLayout.SetRowSpan(bigChart, 2);

            InitializeComponent();

            Application.ApplicationExit += new EventHandler(OnFormExit);
        }
        #endregion

        #region Private methods
        private void OnFormExit(object sender, EventArgs e)
        {
            if (Program.logger != null)
                Program.logger.Finish();
        }
        #endregion

        #region Public methods
        public void AddControl(Control control)
        {
            TableLayout.Controls.Add(control);
        }
        public void BoardConnected()
        {
        }
        public void BoardUnableToConnect()
        {
        }
        public void BoardDisconnected()
        {
        }
        public void UpdateRadioConnectedNodes()
        {/*
            for (int i = RadioNodeInterfaces.Count; i < Program.radioDevices.Count; i++)
            {
                RadioNodeInterfaces.Add(new RadioNodeGroupBox("Radio Node"));
                RadioNodeInterfaces[i].Address = Program.radioDevices[i].Address;
                if (Program.radioDevices[i].Type == NodeType.Gateway)
                    RadioNodeInterfaces[i].Text = "Master";
                else
                    RadioNodeInterfaces[i].Text = "Beacon " + Program.radioDevices[i].Address;
                FlowLayout.Controls.Add(RadioNodeInterfaces[i]);
                RadioNodeInterfaces[i].Draw(RadioNodeInterfaces.Count);
            }*/
        }
        #endregion
    }
}
