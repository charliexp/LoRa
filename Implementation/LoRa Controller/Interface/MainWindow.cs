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
		private const string remoteConnection = "Remote";
		public DirectNodeGroupBox directNodeGroupBox;
		public List<RadioNodeGroupBox> radioNodeGroupBoxes;
		public LogGroupBox logGroupBox;

        public MainWindow()
		{
			directNodeGroupBox = new DirectNodeGroupBox("Directly Connected Node");
			radioNodeGroupBoxes = new List<RadioNodeGroupBox>();
			logGroupBox = new LogGroupBox();

			InitializeComponent();
        }
		
        private void Form1_Load(object sender, EventArgs e)
		{
			Controls.Add(directNodeGroupBox);
			Controls.Add(logGroupBox);

			Application.ApplicationExit += new EventHandler(OnFormExit);
			logGroupBox.FolderTextBox.Text = Program.logger.Folder;
		}
		
        public void OnFormExit(object sender, EventArgs e)
        {
            if (Program.logger != null)
				Program.logger.Finish();
        }
        
		public void BoardConnected()
		{
			directNodeGroupBox.UpdateConnectedStatus(true);
		}

		public void BoardUnableToConnect()
		{
			//directNodeGroupBox.UpdateStatus(false);
		}

		public void BoardDisconnected()
		{
			directNodeGroupBox.UpdateConnectedStatus(false);
		}
		
		public void SetDirectlyConnectedNodeType()
		{
			directNodeGroupBox.Address = Program.directDevice.Address;
			directNodeGroupBox.SetAddress.field.Enabled = false;
			switch(Program.directDevice.nodeType)
			{
				case NodeType.Master:
					directNodeGroupBox.NodeType.field.Text = "Master";
					break;
				case NodeType.Beacon:
					directNodeGroupBox.NodeType.field.Text = "Beacon " + Program.directDevice.Address;
					break;
				case NodeType.Unknown:
					directNodeGroupBox.NodeType.field.Text = "Unknown/new";
					break;
			}
			directNodeGroupBox.Draw(0);
			logGroupBox.Draw(1);
			logGroupBox.Location = new Point(InterfaceConstants.GroupBoxLocationX +
				(radioNodeGroupBoxes.Count + 1) * (directNodeGroupBox.Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);
		}

		public void UpdateRadioConnectedNodes()
		{
			for (int i = radioNodeGroupBoxes.Count; i < Program.radioDevices.Count; i++)
			{
				radioNodeGroupBoxes.Add(new RadioNodeGroupBox("Radio Node"));
				radioNodeGroupBoxes[i].Address = Program.radioDevices[i].Address;
				if (Program.radioDevices[i].nodeType == NodeType.Master)
					radioNodeGroupBoxes[i].Text = "Master";
				else
					radioNodeGroupBoxes[i].Text = "Beacon " + Program.radioDevices[i].Address;
				Controls.Add(radioNodeGroupBoxes[i]);
				radioNodeGroupBoxes[i].Draw(radioNodeGroupBoxes.Count);
			}
			logGroupBox.Draw(Program.radioDevices.Count);

			logGroupBox.Location = new Point(InterfaceConstants.GroupBoxLocationX +
				(radioNodeGroupBoxes.Count + 1) * (directNodeGroupBox.Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);
		}
	}
}
