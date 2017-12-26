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
		public static DeviceHandler DeviceHandler;
        public Logger logger;
		const int logMaxEntries = 12;
		private FolderBrowserDialog folderBrowserDialog;
		private const string remoteConnection = "Remote";
		private bool deviceNodeTypeProcessed;
		public ConnectionType ConnectionType;
		Server serverHandler;
		public DirectlyConnectedUI DirectlyConnectedUI;

        public MainWindow(ConnectionType connectionType, List<string> parameters)
		{
			logger = new Logger("log_", "dd.MM.yyyy", "txt");
			ConnectionType = connectionType;
			DirectlyConnectedUI = new DirectlyConnectedUI();

			if (connectionType == ConnectionType.Serial)
			{
				DeviceHandler = new DeviceHandler(ConnectionType.Serial, parameters[0]);

				serverHandler = new Server();
				serverHandler.Start();
			}
			else if (connectionType == ConnectionType.Internet)
			{
				DeviceHandler = new DeviceHandler(ConnectionType.Internet, parameters);
			}

			Controls.Add(DirectlyConnectedUI.GroupBox);

			InitializeComponent();
        }
		
        private async void Form1_Load(object sender, EventArgs e)
		{
            beaconBandwidthComboBox.SelectedIndex = 0;
            beaconCodingRateComboBox.SelectedIndex = 3;
            beaconBandwidthComboBox.SelectedIndexChanged += new EventHandler(BandwidthComboBox_SelectedIndexChanged);
            beaconCodingRateComboBox.SelectedIndexChanged += new EventHandler(CodingRateComboBox_SelectedIndexChanged);

            folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = "Select the folder to store the logs.";
			folderBrowserDialog.ShowNewFolderButton = true;
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			Application.ApplicationExit += new EventHandler(OnApplicationExit);

			logFolderTextBox.Text = logger.Folder;
			logger.start();
			
			List<string> receivedData;

			DeviceHandler.ConnectToBoard();
			if (DeviceHandler.Connected)
			{
				DirectlyConnectedUI.StatusTextBox.Text = "Connected";
				BoardConnected();
			}
			else
			{
				BoardUnableToConnect();
			}
			deviceNodeTypeProcessed = false;

			while (DeviceHandler.Connected)
			{
				receivedData = await DeviceHandler.ReceiveDataAsync();
				if (serverHandler != null)
				{
					byte[] command = await serverHandler.Receive();
					if (command[0] != (byte)Commands.Invalid)
						await DeviceHandler.SendCommandAsync(command);
					foreach (string s in receivedData)
						await serverHandler.Send(s);
				}
				UpdateLog(receivedData);
				UpdateRSSI(DeviceHandler.RSSI);
				UpdateSNR(DeviceHandler.SNR);
				UpdateCurrentErrors(DeviceHandler.Errors);
				UpdateTotalErrors(DeviceHandler.TotalErrors);

				if (!deviceNodeTypeProcessed && DeviceHandler._nodeType != NodeType.Unknown)
				{
					if (DeviceHandler._nodeType == NodeType.Master)
					{
						DeviceHandler = new MasterDevice(DeviceHandler);
						radioStatusTextBox.Text = "Master";
						await logger.write("Connected to master");
					}
					else
					{
						DeviceHandler = new BeaconDevice(DeviceHandler);
						radioStatusTextBox.Text = "Beacon " + DeviceHandler.Address;
						await logger.write("Connected to beacon " + DeviceHandler.Address);
					}
				}

				if (DeviceHandler is MasterDevice)
				{
					UpdateRadioConnectionStatus(((MasterDevice)DeviceHandler).HasBeaconConnected);
				}

				if (DeviceHandler.RSSI != 0 && DeviceHandler.SNR != 0)
					await logger.write(DeviceHandler.RSSI + ", " + DeviceHandler.SNR);
				else
					await logger.write("error");
			}
			BoardDisconnected();
		}
		
		private void ChangeLogFolderButton_Click(object sender, EventArgs e)
		{
			DialogResult result = folderBrowserDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				logger.Folder = folderBrowserDialog.SelectedPath;
				logFolderTextBox.Text = folderBrowserDialog.SelectedPath;
			}
		}

        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (logger != null)
                logger.finish();
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

		public async void BoardConnected()
        {
			EnableLogControls();
			EnableRadioControls();
			await DeviceHandler.SendCommandAsync(Commands.IsMaster);
		}

		public void BoardUnableToConnect()
		{
			DisableLogControls();
			DisableRadioControls();
			ClearTotalErrors();
		}

		public void BoardDisconnected()
		{
			DirectlyConnectedUI.StatusTextBox.Text = "Disconnected";
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

		public void UpdateRadioConnectionStatus(bool connected)
		{
			if (radioConnectionGroupBox.Enabled && radioStatusTextBox.Enabled)
			{
				if (connected)
                    beaconSettingsGroupBox.Enabled = true;
                else
                    beaconSettingsGroupBox.Enabled = false;
            }
		}

		private async void BandwidthComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
            await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.Bandwidth, (byte)((ComboBox)sender).SelectedIndex);
        }

        private async void CodingRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.CodingRate, (byte)((ComboBox)sender).SelectedIndex + 1);
		}

        private async void OutputPowerNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.OutputPower, (byte)(((NumericUpDown)sender).Value));
		}

        private async void SpreadingFactorNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.SpreadingFactor, (byte)(((NumericUpDown)sender).Value));
		}

        private async void RxSymTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.RxSymTimeout, (byte)(((NumericUpDown)sender).Value));
		}
		
        private async void RxMsTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.RxMsTimeout, (int)(((NumericUpDown)sender).Value));
		}
        private async void TxTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.TxTimeout, (int)(((NumericUpDown)sender).Value));
		}

        private async void PreambleNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.PreambleSize, (byte)(((NumericUpDown)sender).Value));
		}

        private async void PayloadNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.PayloadMaxSize, (byte)(((NumericUpDown)sender).Value));
		}

        private async void VariablePayloadCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.VariablePayload, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}

        private async void CrcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			await ((MasterDevice)DeviceHandler).BeaconHandler.SendCommandAsync(Commands.PerformCRC, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}
	}
}
