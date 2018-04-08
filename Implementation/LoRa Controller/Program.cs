using LoRa_Controller.DirectConnection;
using LoRa_Controller.Device;
using LoRa_Controller.Interface;
using LoRa_Controller.Log;
using LoRa_Controller.Networking;
using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;
using System.IO;
using static LoRa_Controller.Device.Message;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller
{
	static class Program
	{
		#region Public variables
		public static List<RadioDevice> radioDevices;
		public static BaseConnectionHandler connectionHandler;
		public static Logger logger;
		public static DirectDevice directDevice;
		public static MainWindow mainWindow;
		public static Server serverHandler;
		public static ConnectionDialog connectionDialog;

		public static bool directDeviceInitialized;
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main()
        {
			if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware(); Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			directDeviceInitialized = false;

			directDevice = new DirectDevice();
			radioDevices = new List<RadioDevice>();
			SettingHandler.Load();

			logger = new Logger("log_", "txt");

			connectionDialog = new ConnectionDialog();
			mainWindow = new MainWindow();

			connectionDialog.Show();
			Application.Run(mainWindow);
		}

		#region Public methods
		public static void StartConnection(ConnectionType connectionType, List<string> parameters)
		{
			mainWindow.Enabled = true;
			if (connectionType == ConnectionType.Serial)
			{
				connectionHandler = new SerialHandler(parameters[0]);

				serverHandler = new Server();
				serverHandler.Start();
			}
			else if (connectionType == ConnectionType.Internet)
			{
				connectionHandler = new InternetHandler(parameters[0], Int32.Parse(parameters[1]));
			}

			connectionHandler.Open();
			if (connectionHandler.Connected)
				mainWindow.BoardConnected();
			else
				mainWindow.BoardUnableToConnect();

			if (!Directory.Exists((string) SettingHandler.LogFolder.Value))
			{
				logger.Folder = Directory.GetCurrentDirectory();
				mainWindow.logGroupBox.FolderTextBox.Text = logger.Folder;
				SettingHandler.Save(SettingHandler.LogFolder);
			}
			logger.Start();

			BackgroundWorker BackgroundWorker = new BackgroundWorker
			{
				WorkerReportsProgress = true
			};
			BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
			BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
			BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

			BackgroundWorker.RunWorkerAsync();
		}
		#endregion

		#region Private methods
		private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
            Device.Message receivedSerialMessage;
			BackgroundWorker worker = (BackgroundWorker)sender;

            Device.Message getAddressMessage = new Device.Message(CommandType.GetAddress);
            
            connectionHandler.Write(getAddressMessage);

            while (connectionHandler.Connected)
			{
				receivedSerialMessage = connectionHandler.Read();
				if (serverHandler != null)
				{
					Device.Message receivedClientMessage = serverHandler.Read();
					if (receivedClientMessage.command != CommandType.Invalid)
						connectionHandler.Write(receivedClientMessage);
					serverHandler.Write(receivedSerialMessage.ToString());
				}
                
				worker.ReportProgress(0, receivedSerialMessage);
			}
		}

		private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			int radioDeviceAddress = Int32.MaxValue;
			bool newRadioDevice;
			Device.Message message = (Device.Message)e.UserState;
			string receivedDataString = null;

			switch (message.source)
			{
				case (int)AddressType.Master:
					if (directDevice.Address == (int)AddressType.Master)
						radioDeviceAddress = message.target;
					else
						radioDeviceAddress = 1;
					switch (message.command)
					{
						case CommandType.IsPresent:
							receivedDataString = "Master asked if present";
							break;
						case CommandType.Bandwidth:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.directNodeGroupBox.Bandwidth.SetValue(message.parameters[0]);
							receivedDataString = "Bandwidth set by master to " + message.parameters[0];
							break;
						case CommandType.OutputPower:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.directNodeGroupBox.OutputPower.SetValue(message.parameters[0]);
							receivedDataString = "Output power set by master to " + message.parameters[0];
							break;
						case CommandType.CodingRate:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.directNodeGroupBox.CodingRate.SetValue(message.parameters[0]);
							receivedDataString = "Coding rate set by master to " + message.parameters[0];
							break;
						case CommandType.SpreadingFactor:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.directNodeGroupBox.SpreadingFactor.SetValue(message.parameters[0]);
							receivedDataString = "Spreading factor set by master to " + message.parameters[0];
							break;
						case CommandType.RxSymTimeout:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.directNodeGroupBox.RxSymTimeout.SetValue(message.parameters[0]);
							receivedDataString = "Rx timeout (sym) set by master to " + message.parameters[0];
							break;
						case CommandType.RxMsTimeout:
							int rxTimeout = message.parameters[0];
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.directNodeGroupBox.RxMsTimeout.SetValue(rxTimeout);
							receivedDataString = "Rx timeout (ms) set by master to " + rxTimeout.ToString();
							break;
						case CommandType.TxTimeout:
							int txTimeout = message.parameters[0];
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.directNodeGroupBox.TxTimeout.SetValue(txTimeout);
							receivedDataString = "Tx timeout (ms) set by master to " + txTimeout.ToString();
							break;
						case CommandType.PreambleSize:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.directNodeGroupBox.PreambleSize.SetValue(message.parameters[0]);
							receivedDataString = "Preamble size set by master to " + message.parameters[0];
							break;
						case CommandType.PayloadMaxSize:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.directNodeGroupBox.PayloadMaxSize.SetValue(message.parameters[0]);
							receivedDataString = "Payload max size set by master to " + message.parameters[0];
							break;
						case CommandType.VariablePayload:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.directNodeGroupBox.VariablePayload.SetValue(message.parameters[0] == 1);
							receivedDataString = "Variable payload set by master to " + ((message.parameters[0] == 1) ? "true" : "false");
							break;
						case CommandType.PerformCRC:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.directNodeGroupBox.PerformCRC.SetValue(message.parameters[0] == 1);
							receivedDataString = "Perform CRC set by master to " + ((message.parameters[0] == 1) ? "true" : "false");
							break;
						default:
							receivedDataString = "Unknown command " + message.command + " from " + message.source + " to " + message.target;
							break;
					}
					break;
				case (int)AddressType.General:
					if (directDevice.Address >= (int)AddressType.Beacon)
					{
						radioDeviceAddress = 1;
					}

					switch (message.command)
					{
						case CommandType.IsPresent:
							break;
						default:
							receivedDataString = "Unknown command " + message.command + " from " + message.source + " to " + message.target;
							break;
					}
					break;
				case (int)AddressType.PC:
					switch (message.command)
					{
						case CommandType.GetAddress:
							if (!directDeviceInitialized)
							{
								directDevice.Address = message.target;
								((TextBox)mainWindow.directNodeGroupBox.AddressControl.field).TextChanged += new EventHandler(AddressFieldChanged);
								((Button)mainWindow.directNodeGroupBox.SetAddress.field).Click += new EventHandler(SetAddress);
								directDeviceInitialized = true;
								mainWindow.SetDirectlyConnectedNodeType();

								if (directDevice.Address == (int) AddressType.Master)
								{
									receivedDataString = "Connected to master";
									((Button)mainWindow.directNodeGroupBox.CheckBeacons.field).Click += new EventHandler(SendDevicesPresent);
								}
								else if (directDevice.Address >= (int)AddressType.Beacon)
									receivedDataString = "Connected to beacon " + directDevice.Address;
								else if (directDevice.Address >= (int)AddressType.General)
									receivedDataString = "Connected to new/unknown device";
							}
							break;
						case CommandType.SetAddress:
							if (message.Response == ResponseType.ACK)
							{
								if (directDevice.Address != message.target)
								{
									directDevice.Address = message.target;
									mainWindow.SetDirectlyConnectedNodeType();

									if (directDevice.nodeType == BaseDevice.NodeType.Master)
										receivedDataString = "Changed to master";
									else
										receivedDataString = "Changed to beacon " + directDevice.Address;
								}
							}
							else
							{
								receivedDataString = "Could not set address";
							}
							break;
						case CommandType.IsPresent:
							receivedDataString = "Checking for present devices";
							break;
						case CommandType.Bandwidth:
							receivedDataString = "Bandwidth set to " + message.parameters[0];
							break;
						case CommandType.OutputPower:
							receivedDataString = "Output power set to " + message.parameters[0];
							break;
						case CommandType.CodingRate:
							receivedDataString = "Coding rate set to " + message.parameters[0];
							break;
						case CommandType.SpreadingFactor:
							receivedDataString = "Spreading factor set to " + message.parameters[0];
							break;
						case CommandType.RxSymTimeout:
							receivedDataString = "Rx timeout (sym) set to " + message.parameters[0];
							break;
						case CommandType.RxMsTimeout:
							int rxTimeout = message.parameters[0];
							receivedDataString = "Rx timeout (ms) set to " + rxTimeout.ToString();
							break;
						case CommandType.TxTimeout:
							int txTimeout = message.parameters[0];
							receivedDataString = "Tx timeout (ms) set to " + txTimeout.ToString();
							break;
						case CommandType.PreambleSize:
							receivedDataString = "Preamble size set to " + message.parameters[0];
							break;
						case CommandType.PayloadMaxSize:
							receivedDataString = "Payload max size set to " + message.parameters[0];
							break;
						case CommandType.VariablePayload:
							receivedDataString = "Variable payload set to " + ((message.parameters[0] == 1) ? "true" : "false");
							break;
						case CommandType.PerformCRC:
							receivedDataString = "Perform CRC set to " + ((message.parameters[0] == 1) ? "true" : "false");
							break;
						default:
							receivedDataString = "Unknown command " + message.command + " from " + message.source + " to " + message.target;
							break;
					}
					break;
				default: //beacon
					radioDeviceAddress = message.source;

					switch (message.command)
					{
						case CommandType.IsPresent:
							if (message.Response == ResponseType.ACK)
								receivedDataString = "Beacon " + message.source + " present";
							break;
						default:
							if (message.Response == ResponseType.ACK)
								receivedDataString = "Beacon " + message.source + " ACK";
							if (message.Response == ResponseType.NACK)
								receivedDataString = "Beacon " + message.source + " NACK";
							break;
					}
					break;
			}
			
			if (message.Response == ResponseType.ACK)
			{
				if (radioDeviceAddress != Int32.MaxValue)
				{
					newRadioDevice = true;
					foreach (RadioDevice device in radioDevices)
						if (device.Address == radioDeviceAddress)
						{
							newRadioDevice = false;
							break;
						}
					if (radioDeviceAddress != 0 && newRadioDevice)
					{
						radioDevices.Add(new RadioDevice(radioDeviceAddress));
						mainWindow.UpdateRadioConnectedNodes();
						if (radioDeviceAddress == (int) AddressType.Master)
						{
							logger.Write("Radio device master");
						}
						else
							logger.Write("Radio device beacon " + directDevice.Address);
					}
				}
					
				foreach (RadioDevice device in radioDevices)
					if (device.Address == radioDeviceAddress)
					{
						if (message.rssi != 0 && message.snr != 0)
						{
							string logString;
							device.Connected = true;
							device.UpdateSignalQuality(message.rssi, message.snr);
							mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateConnectedStatus(true);
							mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateRSSI(device.RSSI);
							mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateSNR(device.SNR);
							if (radioDeviceAddress == (int) AddressType.Master)
								logString = "Master, ";
							else
								logString = "Beacon " + directDevice.Address + ", ";
							logString += device.RSSI + ", " + device.SNR;
							logger.Write(logString);
						}
					}
			}
			else
			{
				switch ((Error)message.parameters[0])
				{
					case Error.RADIO_RX_TIMEOUT:
						if (message.source == (int)AddressType.General)
						{
							receivedDataString = "No beacon responded";
							foreach (RadioDevice device in radioDevices)
							{
								device.Connected = false;
								mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateConnectedStatus(false);
							}
						}
						else
						{
							receivedDataString = "Beacon " + message.source + " did not respond";
							foreach (RadioDevice device in radioDevices)
								if (device.Address == radioDeviceAddress)
								{
									device.Connected = false;
									mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateConnectedStatus(false);
								}
						}
						break;
					case Error.RADIO_TX_TIMEOUT:
						receivedDataString = "Tx timeout too small to send messages";
						break;
				}
			}
			if (receivedDataString != null)
			{
				logger.Write(receivedDataString);
				mainWindow.logGroupBox.UpdateLog(receivedDataString);
			}
		}

		private static void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			mainWindow.BoardDisconnected();
		}

		private static void AddressFieldChanged(object sender, EventArgs e)
		{
			int newAddress;
			try
			{
				newAddress = Int32.Parse(((TextBox)mainWindow.directNodeGroupBox.AddressControl.field).Text);
				if (newAddress != directDevice.Address && newAddress > 0)
					mainWindow.directNodeGroupBox.SetAddress.field.Enabled = true;
				else
					mainWindow.directNodeGroupBox.SetAddress.field.Enabled = false;
			}
			catch
			{
				mainWindow.directNodeGroupBox.SetAddress.field.Enabled = false;
			}
		}

		private static void SetAddress(object sender, EventArgs e)
        {
            Device.Message message = new Device.Message(directDevice.Address, CommandType.SetAddress, mainWindow.directNodeGroupBox.Address);

            connectionHandler.Write(message);
		}

		private static void SendDevicesPresent(object sender, EventArgs e)
        {
            Device.Message message = new Device.Message(CommandType.IsPresent);

            connectionHandler.Write(message);
		}
		#endregion

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}
