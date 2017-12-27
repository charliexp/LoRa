using LoRa_Controller.Device;
using LoRa_Controller.Interface.DirectlyConnected;
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
		public DirectlyConnectedUI DirectlyConnectedUI;

        public MainWindow()
		{
			DirectlyConnectedUI = new DirectlyConnectedUI();
			Controls.Add(DirectlyConnectedUI.GroupBox);
			InitializeComponent();
        }
		
        private void Form1_Load(object sender, EventArgs e)
		{
            beaconBandwidthComboBox.SelectedIndex = 0;
            beaconCodingRateComboBox.SelectedIndex = 3;
            beaconBandwidthComboBox.SelectedIndexChanged += new EventHandler(BandwidthComboBox_SelectedIndexChanged);
            beaconCodingRateComboBox.SelectedIndexChanged += new EventHandler(CodingRateComboBox_SelectedIndexChanged);

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
			radioConnectionGroupBox.Enabled = true;
		}

		public void DisableRadioControls()
		{
			radioConnectionGroupBox.Enabled = false;
			radioStatusTextBox.Clear();
			rssiTextBox.Clear();
			snrTextBox.Clear();
			currentErrorsTextBox.Clear();
		}

		public void ClearTotalErrors()
		{
			totalErrorsTextBox.Clear();
		}

		public void BoardConnected()
		{
			EnableLogControls();
			EnableRadioControls();
		}

		public void BoardUnableToConnect()
		{
			DisableLogControls();
			DisableRadioControls();
			ClearTotalErrors();
		}

		public void BoardDisconnected()
		{
			DisableLogControls();
			DisableRadioControls();
			ClearTotalErrors();
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
		{
			if (radioConnectionGroupBox.Enabled && rssiTextBox.Enabled)
			{
				if (value != 0)
					rssiTextBox.Text = value.ToString();
				else
					rssiTextBox.Clear();
			}
		}

		public void UpdateSNR(int value)
		{
			if (radioConnectionGroupBox.Enabled && snrTextBox.Enabled)
			{
				if (value != 0)
					snrTextBox.Text = value.ToString();
				else
					snrTextBox.Clear();
			}
		}

		public void UpdateCurrentErrors(uint value)
		{
			if (radioConnectionGroupBox.Enabled && currentErrorsTextBox.Enabled)
			{
				if (value != 0)
					currentErrorsTextBox.Text = value.ToString();
				else
					currentErrorsTextBox.Clear();
			}
		}

		public void UpdateTotalErrors(uint value)
		{
			if (radioConnectionGroupBox.Enabled && totalErrorsTextBox.Enabled)
			{
				if (value != 0)
					totalErrorsTextBox.Text = value.ToString();
				else
					totalErrorsTextBox.Clear();
			}
		}

		public void UpdateDirectlyConnectedNodeType()
		{
			if (Program.DeviceHandler is MasterDevice)
				DirectlyConnectedUI.NodeTypeTextBox.Text = "Master";
			else
				DirectlyConnectedUI.NodeTypeTextBox.Text = "Beacon " + Program.DeviceHandler.Address;
		}

		private async void BandwidthComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
            await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.Bandwidth, (byte)((ComboBox)sender).SelectedIndex);
        }

        private async void CodingRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.CodingRate, (byte)((ComboBox)sender).SelectedIndex + 1);
		}

        private async void OutputPowerNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.OutputPower, (byte)(((NumericUpDown)sender).Value));
		}

        private async void SpreadingFactorNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.SpreadingFactor, (byte)(((NumericUpDown)sender).Value));
		}

        private async void RxSymTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.RxSymTimeout, (byte)(((NumericUpDown)sender).Value));
		}
		
        private async void RxMsTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.RxMsTimeout, (int)(((NumericUpDown)sender).Value));
		}
        private async void TxTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.TxTimeout, (int)(((NumericUpDown)sender).Value));
		}

        private async void PreambleNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.PreambleSize, (byte)(((NumericUpDown)sender).Value));
		}

        private async void PayloadNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.PayloadMaxSize, (byte)(((NumericUpDown)sender).Value));
		}

        private async void VariablePayloadCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.VariablePayload, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}

        private async void CrcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)Program.DeviceHandler).BeaconHandler.SendCommandAsync(Commands.PerformCRC, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}
	}
}
