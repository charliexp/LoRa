using LoRa_Controller.Device;
using LoRa_Controller.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller
{
    public partial class Form1 : Form
    {
		DeviceHandler deviceHandler;
        Logger logger;
        const string settingsFilePath = "settings.ini";
        StreamWriter settingsFileStreamWriter;
		const int logMaxEntries = 12;
		private FolderBrowserDialog folderBrowserDialog;
		private const string remoteConnection = "Remote";
		Server serverHandler;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void LoadSettings()
        {
            string[] settingLines;
            string settingName;
            string settingValue;
            
            settingLines = File.ReadAllLines(settingsFilePath);
            
            if (settingLines.Length > 0)
            {
                settingName = settingLines[0].Remove(settingLines[0].IndexOf('=') - 1);
                settingValue = settingLines[0].Substring(settingLines[0].LastIndexOf('=') + 2);

                if (settingName.Equals("LogFolder"))
                {
                    if (logger.isOpen())
                        logger.finish();
                    logger.Folder = settingValue;
                    logFolderTextBox.Text = settingValue;
                }
            }
        }

        private void SaveSettings()
        {
            string settingName = "LogFolder";
            string settingValue = logger.Folder;
            settingsFileStreamWriter = new StreamWriter(File.Open(settingsFilePath, FileMode.Create));

            settingsFileStreamWriter.WriteLine(settingName + " = " + settingValue);
            settingsFileStreamWriter.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
		{
            masterBandwidthComboBox.SelectedIndex = 0;
            masterCodingRateComboBox.SelectedIndex = 3;
            masterBandwidthComboBox.SelectedIndexChanged += new EventHandler(BandwidthComboBox_SelectedIndexChanged);
            masterCodingRateComboBox.SelectedIndexChanged += new EventHandler(CodingRateComboBox_SelectedIndexChanged);

            beaconBandwidthComboBox.SelectedIndex = 0;
            beaconCodingRateComboBox.SelectedIndex = 3;
            beaconBandwidthComboBox.SelectedIndexChanged += new EventHandler(BandwidthComboBox_SelectedIndexChanged);
            beaconCodingRateComboBox.SelectedIndexChanged += new EventHandler(CodingRateComboBox_SelectedIndexChanged);

            folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = "Select the folder to store the logs.";
			folderBrowserDialog.ShowNewFolderButton = true;
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
            logger = new Logger("log_", "dd.MM.yyyy", "txt");
            LoadSettings();
            logger.start();
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

		private void ComPortComboBox_DropDown(object sender, EventArgs e)
		{
			ComboBox comboBox = ((ComboBox)sender);
			string[] comPortsList = MasterHandler.getAvailablePorts();

			comboBox.Items.Clear();
			foreach (string port in comPortsList)
				if (!comboBox.Items.Contains(port))
					comboBox.Items.Add(port);
			comboBox.Items.Add(remoteConnection);
		}
		private async void ComPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			string comPort = (string)((ComboBox)sender).SelectedItem;
			List<string> receivedData;

			if (comPort != remoteConnection)
			{
				if (deviceHandler != null)
				{
					logger.finish();
				}

				deviceHandler = new DeviceHandler(comPort);
				deviceHandler.ConnectedToBoard = new EventHandler<ConnectionEventArgs>(COMPortConnected);
				deviceHandler.UnableToConnectToBoard = new EventHandler<ConnectionEventArgs>(COMPortConnected);
				deviceHandler.DisconnectedFromBoard = new EventHandler<ConnectionEventArgs>(COMPortDisconnected);
				deviceHandler.ConnectToBoard();

				serverHandler = new Server();
				serverHandler.StartListening();

				while (deviceHandler.IsConnected)
				{
					receivedData = await deviceHandler.ReceiveDataAsync();
					UpdateLog(receivedData);
					UpdateRSSI(deviceHandler.RSSI);
					UpdateSNR(deviceHandler.SNR);
					UpdateCurrentErrors(deviceHandler.Errors);
					UpdateTotalErrors(deviceHandler.TotalErrors);

					if (deviceHandler.IsMasterReported)
					{
						if (deviceHandler.IsMaster)
						{
							deviceHandler = new MasterHandler(comPort);
							radioStatusTextBox.Text = "Master";
							await logger.write("Connected to master");
						}
						else
						{
							deviceHandler = new BeaconHandler(comPort);
							radioStatusTextBox.Text = "Slave";
							await logger.write("Connected to slave");
						}
					}
					
					if (deviceHandler is MasterHandler)
					{
						UpdateRadioConnectionStatus(((MasterHandler) deviceHandler).HasBeaconConnected);
						if (((MasterHandler)deviceHandler).HasBeaconConnected)
							await logger.write(deviceHandler.RSSI + ", " + deviceHandler.SNR);
						else
							await logger.write("error");
					}
                }
			}
			else
			{

			}
		}
		
        public void OnApplicationExit(object sender, EventArgs e)
        {
            SaveSettings();
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
			masterSettingsGroupBox.Enabled = true;
		}

		public void DisableRadioControls()
		{
			radioConnectionGroupBox.Enabled = false;
			masterSettingsGroupBox.Enabled = false;
			radioStatusTextBox.Clear();
			rssiTextBox.Clear();
			snrTextBox.Clear();
			currentErrorsTextBox.Clear();
		}

		public void ClearTotalErrors()
		{
			totalErrorsTextBox.Clear();
		}

		public async void COMPortConnected(object sender, ConnectionEventArgs e)
        {
			if (e.Connected)
			{
				serialStatusTextBox.Text = "Connected to board";
				EnableLogControls();
				EnableRadioControls();
				await deviceHandler.SendCommandAsync(Commands.IsMaster);
			}
			else
			{
				serialStatusTextBox.Text = "Couldn't connect to board";
				DisableLogControls();
				DisableRadioControls();
				ClearTotalErrors();
			}
		}

		public void COMPortDisconnected(object sender, ConnectionEventArgs e)
		{
			if (e.DisconnectedOnPurpose)
			{
				serialStatusTextBox.Text = "Board not connected";
				DisableLogControls();
				DisableRadioControls();
				ClearTotalErrors();
			}
			else
			{
				serialStatusTextBox.Text = "Board disconnected";
				DisableLogControls();
				DisableRadioControls();
			}
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
            if (sender.Equals(masterBandwidthComboBox))
			    await deviceHandler.SendCommandAsync(Commands.Bandwidth, (byte) ((ComboBox)sender).SelectedIndex);
            else if (sender.Equals(beaconBandwidthComboBox))
                await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.Bandwidth, (byte)((ComboBox)sender).SelectedIndex);
        }

        private async void CodingRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterCodingRateComboBox))
				await deviceHandler.SendCommandAsync(Commands.CodingRate, (byte)(((ComboBox)sender).SelectedIndex + 1));
			else if (sender.Equals(beaconCodingRateComboBox))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.CodingRate, (byte)((ComboBox)sender).SelectedIndex + 1);
		}

        private async void OutputPowerNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterOutputPowerNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.OutputPower, (byte)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconOutputPowerNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.OutputPower, (byte)(((NumericUpDown)sender).Value));
		}

        private async void SpreadingFactorNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterSpreadingFactorNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.SpreadingFactor, (byte)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconSpreadingFactorNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.SpreadingFactor, (byte)(((NumericUpDown)sender).Value));
		}

        private async void RxSymTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterRxSymTimeoutNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.RxSymTimeout, (byte)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconRxSymTimeoutNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.RxSymTimeout, (byte)(((NumericUpDown)sender).Value));
		}
		
        private async void RxMsTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterRxMsTimeoutNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.RxMsTimeout, (int)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconRxMsTimeoutNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.RxMsTimeout, (int)(((NumericUpDown)sender).Value));
		}
        private async void TxTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterTxTimeoutNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.TxTimeout, (int)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconTxTimeoutNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.TxTimeout, (int)(((NumericUpDown)sender).Value));
		}

        private async void PreambleNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterPreambleNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.PreambleSize, (byte)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconPreambleNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.PreambleSize, (byte)(((NumericUpDown)sender).Value));
		}

        private async void PayloadNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterPayloadNumericUpDown))
				await deviceHandler.SendCommandAsync(Commands.PayloadMaxSize, (byte)(((NumericUpDown)sender).Value));
			else if (sender.Equals(beaconPayloadNumericUpDown))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.PayloadMaxSize, (byte)(((NumericUpDown)sender).Value));
		}

        private async void VariablePayloadCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterVariablePayloadCheckBox))
				await deviceHandler.SendCommandAsync(Commands.VariablePayload, (byte)(((CheckBox)sender).Checked ? 1 : 0));
			else if (sender.Equals(beaconVariablePayloadCheckBox))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.VariablePayload, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}

        private async void CrcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (sender.Equals(masterCrcCheckBox))
				await deviceHandler.SendCommandAsync(Commands.PerformCRC, (byte)(((CheckBox)sender).Checked? 1 : 0));
			else if (sender.Equals(beaconCrcCheckBox))
				await ((MasterHandler)deviceHandler).BeaconHandler.SendCommandAsync(Commands.PerformCRC, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}
    }
}
