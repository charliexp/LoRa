using LoRa_Controller.Device;
using LoRa_Controller.Interface.Node;
using LoRa_Controller.Log;
using LoRa_Controller.Networking;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface
{
    public partial class MainWindow : Form
	{
		const int logMaxEntries = 12;
		private const string remoteConnection = "Remote";
		public NodeGroupBox directNodeGroupBox;
		public List<NodeGroupBox> radioNodeGroupBox;

        public MainWindow()
		{
			directNodeGroupBox = new NodeGroupBox("Directly Connected Node");
			radioNodeGroupBox = new List<NodeGroupBox>();

			InitializeComponent();
        }
		
        private void Form1_Load(object sender, EventArgs e)
		{
			Controls.Add(directNodeGroupBox);
			directNodeGroupBox.Draw(0);

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
        
		public void EnableLogControls()
		{
			logGroupBox.Enabled = true;
			logListBox.Enabled = true;
		}

		public void DisableLogControls()
		{
			logGroupBox.Enabled = false;
			logFolderTextBox.Clear();
			logListBox.Items.Clear();
		}
		
		public void EnableRadioControls()
		{
			//radioConnectionGroupBox.Enabled = true;
		}

		public void DisableRadioControls()
		{/*
			radioConnectionGroupBox.Enabled = false;
			radioStatusTextBox.Clear();
			rssiTextBox.Clear();
			snrTextBox.Clear();
			currentErrorsTextBox.Clear();*/
		}

		public void ClearTotalErrors()
		{
			//totalErrorsTextBox.Clear();
		}

		public void BoardConnected()
		{
			directNodeGroupBox.Status.field.Text = "Connected";
		}

		public void BoardUnableToConnect()
		{
			directNodeGroupBox.Status.field.Text = "Could not connect";
		}

		public void BoardDisconnected()
		{
			directNodeGroupBox.Status.field.Text = "Disonnected";
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

		public void UpdateRSSI(int value)
		{/*
			if (radioConnectionGroupBox.Enabled && rssiTextBox.Enabled)
			{
				if (value != 0)
					rssiTextBox.Text = value.ToString();
				else
					rssiTextBox.Clear();
			}*/
		}

		public void UpdateSNR(int value)
		{/*
			if (radioConnectionGroupBox.Enabled && snrTextBox.Enabled)
			{
				if (value != 0)
					snrTextBox.Text = value.ToString();
				else
					snrTextBox.Clear();
			}*/
		}

		public void UpdateCurrentErrors(uint value)
		{/*
			if (radioConnectionGroupBox.Enabled && currentErrorsTextBox.Enabled)
			{
				if (value != 0)
					currentErrorsTextBox.Text = value.ToString();
				else
					currentErrorsTextBox.Clear();
			}*/
		}

		public void UpdateTotalErrors(uint value)
		{/*
			if (radioConnectionGroupBox.Enabled && totalErrorsTextBox.Enabled)
			{
				if (value != 0)
					totalErrorsTextBox.Text = value.ToString();
				else
					totalErrorsTextBox.Clear();
			}*/
		}

		public void UpdateDirectlyConnectedNodeType()
		{
			if (Program.DeviceHandler is MasterDevice)
				directNodeGroupBox.NodeType.field.Text = "Master";
			else
				directNodeGroupBox.NodeType.field.Text = "Beacon " + Program.DeviceHandler.Address;
		}

		public void UpdateRadioConnectedNodeType()
		{
			if (radioNodeGroupBox.Count < 2)
			{
				radioNodeGroupBox.Add(new NodeGroupBox("Radio node"));
				Controls.Add(radioNodeGroupBox[radioNodeGroupBox.Count - 1]);
				radioNodeGroupBox[radioNodeGroupBox.Count - 1].Draw(radioNodeGroupBox.Count);

			}
		}
	}
}
