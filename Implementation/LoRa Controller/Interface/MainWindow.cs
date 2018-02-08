using LoRa_Controller.Device;
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
		const int logMaxEntries = 12;
		private const string remoteConnection = "Remote";
		public DirectNodeGroupBox directNodeGroupBox;
		public List<RadioNodeGroupBox> radioNodeGroupBoxes;

        public MainWindow()
		{
			directNodeGroupBox = new DirectNodeGroupBox("Directly Connected Node");
			radioNodeGroupBoxes = new List<RadioNodeGroupBox>();

			InitializeComponent();
        }
		
        private void Form1_Load(object sender, EventArgs e)
		{
			Controls.Add(directNodeGroupBox);

			Application.ApplicationExit += new EventHandler(OnFormExit);
			logFolderTextBox.Text = Program.logger.Folder;
		}
		
		private void ChangeLogFolderButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog;

			folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = "Select the folder to store the logs.";
			folderBrowserDialog.ShowNewFolderButton = true;
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				Program.logger.Folder = folderBrowserDialog.SelectedPath;
				logFolderTextBox.Text = folderBrowserDialog.SelectedPath;
			}
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
		
		public void UpdateLog(List<string> data)
		{
			if (logGroupBox.Enabled)
			{
				while (logListBox.Items.Count + data.Count > logMaxEntries)
					logListBox.Items.RemoveAt(0);
				logListBox.Items.AddRange(data.ToArray());
				logListBox.TopIndex = logListBox.Items.Count - 1;
			}
		}

		public void SetDirectlyConnectedNodeType()
		{
			directNodeGroupBox.address = Program.directDevice.Address;
			if (Program.directDevice.nodeType == NodeType.Master)
			{
				directNodeGroupBox.NodeType.field.Text = "Master";
				directNodeGroupBox.Draw(0);
			}
			else
				directNodeGroupBox.NodeType.field.Text = "Beacon " + Program.directDevice.Address;
		}

		public void UpdateRadioConnectedNodes()
		{
			for (int i = radioNodeGroupBoxes.Count; i < Program.radioDevices.Count; i++)
			{
				radioNodeGroupBoxes.Add(new RadioNodeGroupBox("Radio Node"));
				radioNodeGroupBoxes[i].address = Program.radioDevices[i].Address;
				if (Program.radioDevices[i].nodeType == NodeType.Master)
					radioNodeGroupBoxes[i].Text = "Master";
				else
					radioNodeGroupBoxes[i].Text = "Beacon " + Program.radioDevices[i].Address;
				Controls.Add(radioNodeGroupBoxes[i]);
				radioNodeGroupBoxes[i].Draw(radioNodeGroupBoxes.Count);
			}
		}
	}
}
