﻿using LoRa_Controller.Device;
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
			directNodeGroupBox.Status.field.BackColor = Color.LightGreen;
		}

		public void BoardUnableToConnect()
		{
			directNodeGroupBox.Status.field.Text = "Could not connect";
			directNodeGroupBox.Status.field.BackColor = Color.PaleVioletRed;
		}

		public void BoardDisconnected()
		{
			directNodeGroupBox.Status.field.Text = "Disonnected";
			directNodeGroupBox.Status.field.BackColor = Color.PaleVioletRed;
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
		
		public void UpdateDirectlyConnectedNodeType()
		{
			if (Program.DirectDevice.nodeType == NodeType.Master)
				directNodeGroupBox.NodeType.field.Text = "Master";
			else
				directNodeGroupBox.NodeType.field.Text = "Beacon " + Program.DirectDevice.Address;
		}

		public void UpdateRadioConnectedNodes()
		{
			for (int i = radioNodeGroupBoxes.Count; i < Program.DirectDevice.radioDevices.Count; i++)
			{
				radioNodeGroupBoxes.Add(new RadioNodeGroupBox("Radio Node"));
				if (Program.DirectDevice.radioDevices[i].nodeType == NodeType.Master)
					radioNodeGroupBoxes[i].Text = "Master";
				else
					radioNodeGroupBoxes[i].Text = "Beacon " + Program.DirectDevice.radioDevices[i].Address;
				Controls.Add(radioNodeGroupBoxes[i]);
				radioNodeGroupBoxes[i].Draw(radioNodeGroupBoxes.Count);
			}

			foreach (BaseNodeGroupBox radioNodeGroupBox in radioNodeGroupBoxes)
			{
				((TextBox)radioNodeGroupBox.Status.field).Text = "Connected";
				((TextBox)radioNodeGroupBox.Status.field).BackColor = Color.LightGreen;
			}
		}
	}
}
