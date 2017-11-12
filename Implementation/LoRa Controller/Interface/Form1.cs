using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller
{
    public partial class Form1 : Form
    {
        MasterHandler masterHandler;
        Logger logger;
        const string settingsFilePath = "settings.ini";
        StreamWriter settingsFileStreamWriter;
        bool masterReported;
		const int logMaxEntries = 12;
		private FolderBrowserDialog folderBrowserDialog;

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
            masterBandwidthComboBox.SelectedIndexChanged += new System.EventHandler(BandwidthComboBox_SelectedIndexChanged);
            masterCodingRateComboBox.SelectedIndexChanged += new System.EventHandler(codingRateComboBox_SelectedIndexChanged);

            beaconBandwidthComboBox.SelectedIndex = 0;
            beaconCodingRateComboBox.SelectedIndex = 3;
            beaconBandwidthComboBox.SelectedIndexChanged += new System.EventHandler(BandwidthComboBox_SelectedIndexChanged);
            beaconCodingRateComboBox.SelectedIndexChanged += new System.EventHandler(codingRateComboBox_SelectedIndexChanged);

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
		}
		private async void ComPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			string comPort = (string)((ComboBox)sender).SelectedItem;
			List<string> receivedData;

			if (comPort != null)
			{
				if (masterHandler != null)
				{
					logger.finish();
				}

                masterHandler = new MasterHandler(comPort);
				masterHandler.ConnectedToBoard = new EventHandler<ConnectionEventArgs>(COMPortConnected);
				masterHandler.UnableToConnectToBoard = new EventHandler<ConnectionEventArgs>(COMPortConnected);
				masterHandler.DisconnectedFromBoard = new EventHandler<ConnectionEventArgs>(COMPortDisconnected);
				masterHandler.ConnectToBoard();
				masterReported = false;
			
				while (masterHandler.IsConnectedToBoard)
				{
					receivedData = await masterHandler.ReceiveDataAsync();
					UpdateLog(receivedData);
					UpdateRSSI(masterHandler.RSSI);
					UpdateSNR(masterHandler.SNR);
					UpdateCurrentErrors(masterHandler.Errors);
					UpdateTotalErrors(masterHandler.TotalErrors);
					UpdateRadioConnectionStatus(masterHandler.IsRadioConnected);

					if (!masterReported)
					{
						masterReported = true;
						if (masterHandler.IsMaster)
							await logger.write("Connected as master");
						else
							await logger.write("Connected as slave");
					}

					if (masterHandler.IsRadioConnected)
                        await logger.write(masterHandler.RSSI + ", " + masterHandler.SNR);
                    else
                        await logger.write("error");
                }
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

		public void COMPortConnected(object sender, ConnectionEventArgs e)
        {
			if (e.Connected)
			{
				serialStatusTextBox.Text = "Connected to board";
				EnableLogControls();
				EnableRadioControls();
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
				if (masterHandler.IsRadioConnected)
					rssiTextBox.Text = value.ToString();
				else
					rssiTextBox.Clear();
			}
		}

		public void UpdateSNR(int value)
		{
			if (radioConnectionGroupBox.Enabled && snrTextBox.Enabled)
			{
				if (masterHandler.IsRadioConnected)
					snrTextBox.Text = value.ToString();
				else
					snrTextBox.Clear();
			}
		}

		public void UpdateCurrentErrors(uint value)
		{
			if (radioConnectionGroupBox.Enabled && currentErrorsTextBox.Enabled)
			{
				if (masterHandler.IsRadioConnected)
					currentErrorsTextBox.Text = value.ToString();
				else
					currentErrorsTextBox.Clear();
			}
		}

		public void UpdateTotalErrors(uint value)
		{
			if (radioConnectionGroupBox.Enabled && totalErrorsTextBox.Enabled)
			{
				if (masterHandler.IsRadioConnected)
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
                {
                    beaconSettingsGroupBox.Enabled = true;
                    radioStatusTextBox.Text = "Connected";
                }
                else
                {
                    beaconSettingsGroupBox.Enabled = false;
                    radioStatusTextBox.Text = "Not connected";
                }
            }
		}

		private async void BandwidthComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			await masterHandler.SendCommandAsync(MasterHandler.Commands.Bandwidth, (byte) ((ComboBox)sender).SelectedIndex);
		}

        private async void codingRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.CodingRate, (byte)(((ComboBox)sender).SelectedIndex + 1));
        }

        private async void outputPowerNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.OutputPower, (byte)(((NumericUpDown)sender).Value));
        }

        private async void spreadingFactorNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.SpreadingFactor, (byte)(((NumericUpDown)sender).Value));
        }

        private async void rxSymTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.RxSymTimeout, (byte)(((NumericUpDown)sender).Value));
        }


        private async void rxMsTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.RxMsTimeout, (int)(((NumericUpDown)sender).Value));
        }
        private async void txTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.TxTimeout, (int)(((NumericUpDown)sender).Value));
        }

        private async void preambleNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.PreambleSize, (byte)(((NumericUpDown)sender).Value));
        }

        private async void payloadNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.PayloadMaxSize, (byte)(((NumericUpDown)sender).Value));
        }

        private async void variablePayloadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.VariablePayload, (byte)(((CheckBox)sender).Checked ? 1 : 0));
        }

        private async void crcCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await masterHandler.SendCommandAsync(MasterHandler.Commands.PerformCRC, (byte)(((CheckBox)sender).Checked? 1 : 0));
        }
    }
}
