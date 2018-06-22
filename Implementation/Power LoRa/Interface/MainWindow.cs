using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Power_LoRa.Interface
{
    public partial class MainWindow : Form
    {
        #region Properties
		public List<RadioNodeGroupBox> RadioNodeInterfaces { get; private set; }
        public FlowLayoutPanel FlowLayout { get; private set; }
        public TableLayoutPanel TableLayout { get; private set; }
        public ChartControl BigChart { get; private set; }
        #endregion

        #region Constructors
        public MainWindow()
		{
			RadioNodeInterfaces = new List<RadioNodeGroupBox>();

            BigChart = new ChartControl("Big Chart");
            FlowLayout = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Name = "FlowLayout",
            };

            TableLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2,
                Location = new Point(0, 0),
                Name = "TableLayout",
            };

            Controls.Add(TableLayout);
            TableLayout.Controls.Add(FlowLayout);
            TableLayout.Controls.Add(BigChart);


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
