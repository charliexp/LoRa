using LoRa_Controller.Interface.Log;
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
        public LogGroupBox LogInterface { get; private set; }
        public FlowLayoutPanel FlowLayout { get; private set; }
        public TableLayoutPanel TableLayout { get; private set; }
        #endregion

        #region Constructors
        public MainWindow()
		{
			DirectNodeInterface = new DirectNodeGroupBox("Directly Connected Node");
			RadioNodeInterfaces = new List<RadioNodeGroupBox>();
			LogInterface = new LogGroupBox();

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
            
            InitializeComponent();
        }
        #endregion

        #region Private methods
        private void Form1_Load(object sender, EventArgs e)
		{
            Controls.Add(TableLayout);
            TableLayout.Controls.Add(FlowLayout);
            TableLayout.Controls.Add(LogInterface);

            TableLayout.SetRow(FlowLayout, 0);
            TableLayout.SetRow(LogInterface, 1);

            FlowLayout.FlowDirection = FlowDirection.LeftToRight;
            FlowLayout.Controls.Add(DirectNodeInterface);

			Application.ApplicationExit += new EventHandler(OnFormExit);
			LogInterface.FolderTextBox.Text = Program.logger.Folder;
        }
        private void OnFormExit(object sender, EventArgs e)
        {
            if (Program.logger != null)
                Program.logger.Finish();
        }
        #endregion

        #region Public methods
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

            LogInterface.Location = new Point(InterfaceConstants.GroupBoxLocationX +
                (RadioNodeInterfaces.Count + 1) * (DirectNodeInterface.Width + InterfaceConstants.GroupBoxLocationX),
                InterfaceConstants.GroupBoxLocationY);
        }
        #endregion
    }
}
