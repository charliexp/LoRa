using LoRa_Controller.Interface.Node.GroupBoxes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Interface
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
            DirectNodeInterface.UpdateConnectedStatus(true);
        }
        public void BoardUnableToConnect()
        {
            //directNodeGroupBox.UpdateStatus(false);
        }
        public void BoardDisconnected()
        {
            DirectNodeInterface.UpdateConnectedStatus(false);
        }
        public void SetDirectlyConnectedNodeType()
        {
            DirectNodeInterface.Address = Program.directDevice.Address;
            DirectNodeInterface.SetAddress.Field.Enabled = false;
            switch (Program.directDevice.Type)
            {
                case NodeType.Master:
                    DirectNodeInterface.NodeType.Field.Text = "Master";
                    break;
                case NodeType.Beacon:
                    DirectNodeInterface.NodeType.Field.Text = "Beacon " + Program.directDevice.Address;
                    break;
                case NodeType.Unknown:
                    DirectNodeInterface.NodeType.Field.Text = "Unknown/new";
                    break;
            }
            DirectNodeInterface.Draw(0);
        }
        public void UpdateRadioConnectedNodes()
        {
            for (int i = RadioNodeInterfaces.Count; i < Program.radioDevices.Count; i++)
            {
                RadioNodeInterfaces.Add(new RadioNodeGroupBox("Radio Node"));
                RadioNodeInterfaces[i].Address = Program.radioDevices[i].Address;
                if (Program.radioDevices[i].Type == NodeType.Master)
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
