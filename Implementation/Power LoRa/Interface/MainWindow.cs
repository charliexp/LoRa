using Power_LoRa.Device;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Power_LoRa.Device.BaseDevice;

namespace Power_LoRa.Interface
{
    public partial class MainWindow : Form
    {
        #region Properties
        public DirectNodeGroupBox DirectNodeInterface { get; private set; }
		public List<RadioNodeGroupBox> RadioNodeInterfaces { get; private set; }
        public FlowLayoutPanel FlowLayout { get; private set; }
        public TableLayoutPanel TableLayout { get; private set; }
        #endregion

        #region Constructors
        public MainWindow()
		{
			DirectNodeInterface = new DirectNodeGroupBox("Directly Connected Node");
			RadioNodeInterfaces = new List<RadioNodeGroupBox>();

            FlowLayout = new FlowLayoutPanel
            {
                AutoSize = true,
                Name = "FlowLayout"
            };

            TableLayout = new TableLayoutPanel
            {
                AutoSize = true,
                RowCount = 2,
                Location = new Point(0, 0),
                Name = "TableLayout",
            };

            Controls.Add(TableLayout);
            TableLayout.Controls.Add(FlowLayout);

            FlowLayout.FlowDirection = FlowDirection.LeftToRight;
            FlowLayout.Controls.Add(DirectNodeInterface);

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
        public void SetDirectlyConnectedNodeType()
        {
            DirectNodeInterface.Address = Program.directDevice.Address;
            DirectNodeInterface.SetAddress.Field.Enabled = false;
            switch (Program.directDevice.Type)
            {
                case NodeType.Gateway:
                    DirectNodeInterface.Text = "Gateway";
                    break;
                case NodeType.EndDevice:
                    DirectNodeInterface.Text = "End device " + Program.directDevice.Address;
                    break;
                case NodeType.Unknown:
                    DirectNodeInterface.Text = "Unknown/new";
                    break;
            }
        }
        public void UpdateRadioConnectedNodes()
        {
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
            }
        }
        #endregion
    }
}
