using LoRa_Controller.Device;
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
        public DirectNodeGroupBox DirectNodeGroupBox { get; private set; }
		public List<RadioNodeGroupBox> RadioNodeGroupBoxes { get; private set; }
        public LogGroupBox LogGroupBox { get; private set; }
        #endregion

        #region Constructors
        public MainWindow()
		{
			DirectNodeGroupBox = new DirectNodeGroupBox("Directly Connected Node");
			RadioNodeGroupBoxes = new List<RadioNodeGroupBox>();
			LogGroupBox = new LogGroupBox();

			InitializeComponent();
        }
        #endregion

        #region Private methods
        private void Form1_Load(object sender, EventArgs e)
		{
			Controls.Add(DirectNodeGroupBox);
			Controls.Add(LogGroupBox);

			Application.ApplicationExit += new EventHandler(OnFormExit);
			LogGroupBox.FolderTextBox.Text = Program.logger.Folder;
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
            DirectNodeGroupBox.UpdateConnectedStatus(true);
        }
        public void BoardUnableToConnect()
        {
            //directNodeGroupBox.UpdateStatus(false);
        }
        public void BoardDisconnected()
        {
            DirectNodeGroupBox.UpdateConnectedStatus(false);
        }
        public void SetDirectlyConnectedNodeType()
        {
            DirectNodeGroupBox.Address = Program.directDevice.Address;
            DirectNodeGroupBox.SetAddress.Field.Enabled = false;
            switch (Program.directDevice.Type)
            {
                case NodeType.Master:
                    DirectNodeGroupBox.NodeType.Field.Text = "Master";
                    break;
                case NodeType.Beacon:
                    DirectNodeGroupBox.NodeType.Field.Text = "Beacon " + Program.directDevice.Address;
                    break;
                case NodeType.Unknown:
                    DirectNodeGroupBox.NodeType.Field.Text = "Unknown/new";
                    break;
            }
            DirectNodeGroupBox.Draw(0);
            LogGroupBox.Draw(1);
            LogGroupBox.Location = new Point(InterfaceConstants.GroupBoxLocationX +
                (RadioNodeGroupBoxes.Count + 1) * (DirectNodeGroupBox.Width + InterfaceConstants.GroupBoxLocationX),
                InterfaceConstants.GroupBoxLocationY);
        }
        public void UpdateRadioConnectedNodes()
        {
            for (int i = RadioNodeGroupBoxes.Count; i < Program.radioDevices.Count; i++)
            {
                RadioNodeGroupBoxes.Add(new RadioNodeGroupBox("Radio Node"));
                RadioNodeGroupBoxes[i].Address = Program.radioDevices[i].Address;
                if (Program.radioDevices[i].Type == NodeType.Master)
                    RadioNodeGroupBoxes[i].Text = "Master";
                else
                    RadioNodeGroupBoxes[i].Text = "Beacon " + Program.radioDevices[i].Address;
                Controls.Add(RadioNodeGroupBoxes[i]);
                RadioNodeGroupBoxes[i].Draw(RadioNodeGroupBoxes.Count);
            }
            LogGroupBox.Draw(Program.radioDevices.Count);

            LogGroupBox.Location = new Point(InterfaceConstants.GroupBoxLocationX +
                (RadioNodeGroupBoxes.Count + 1) * (DirectNodeGroupBox.Width + InterfaceConstants.GroupBoxLocationX),
                InterfaceConstants.GroupBoxLocationY);
        }
        #endregion
    }
}
