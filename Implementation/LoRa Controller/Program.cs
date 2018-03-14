using LoRa_Controller.DirectConnection;
using LoRa_Controller.Device;
using LoRa_Controller.Interface;
using LoRa_Controller.Log;
using LoRa_Controller.Networking;
using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;
using static LoRa_Controller.Device.DirectDevice;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;
using LoRa_Controller.Interface.Controls;

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
			logger.Start();

			BackgroundWorker BackgroundWorker = new BackgroundWorker();
			BackgroundWorker.WorkerReportsProgress = true;
			BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
			BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
			BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

			BackgroundWorker.RunWorkerAsync();
		}
		#endregion

		#region Private methods
		private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			byte[] receivedData;
			BackgroundWorker worker = (BackgroundWorker)sender;

			connectionHandler.SendGeneralCommand(Command.GetAddress);

			while (connectionHandler.Connected)
			{
				receivedData = connectionHandler.ReceiveData();
				if (serverHandler != null)
				{
					byte[] command = serverHandler.Receive();
					if (command[0] != (byte)Command.Invalid)
						connectionHandler.SendCommand(command);
					serverHandler.Send(receivedData.ToString());
				}

				if (receivedData.Length != 0)
					worker.ReportProgress(0, receivedData);
			}
		}

		private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			int radioDeviceAddress = 0;
			bool newRadioDevice;
			byte[] receivedData = (byte[])e.UserState;
			string receivedDataString = null;
			Command command = (Command) receivedData[Idx_command];
			int source = receivedData[Idx_sourceAddress];
			int target = receivedData[Idx_targetAddress];
			Response response = (Response)receivedData[Idx_commandParameter];

			switch (source)
			{
				case Address_master:
					radioDeviceAddress = 1;
					switch (command)
					{
						case Command.IsPresent:
							receivedDataString = "Master asked if present";
							break;
						case Command.Bandwidth:
							receivedDataString = "Bandwidth set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.OutputPower:
							receivedDataString = "Output power set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.CodingRate:
							receivedDataString = "Coding rate set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.SpreadingFactor:
							receivedDataString = "Spreading factor set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.RxSymTimeout:
							receivedDataString = "Rx timeout (sym) set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.RxMsTimeout:
							int rxTimeout = receivedData[Idx_commandParameter + 0] << 24 |
											receivedData[Idx_commandParameter + 1] << 16 |
											receivedData[Idx_commandParameter + 2] << 8 |
											receivedData[Idx_commandParameter + 3];
							receivedDataString = "Rx timeout (ms) set by master to " + rxTimeout.ToString();
							break;
						case Command.TxTimeout:
							int txTimeout = receivedData[Idx_commandParameter + 0] << 24 |
											receivedData[Idx_commandParameter + 1] << 16 |
											receivedData[Idx_commandParameter + 2] << 8 |
											receivedData[Idx_commandParameter + 3];
							receivedDataString = "Tx timeout (ms) set by master to " + txTimeout.ToString();
							break;
						case Command.PreambleSize:
							receivedDataString = "Preamble size set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.PayloadMaxSize:
							receivedDataString = "Payload max size set by master to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.VariablePayload:
							receivedDataString = "Variable payload set by master to " + ((receivedData[Idx_commandParameter + 3] == 1)? "true" : "false");
							break;
						case Command.PerformCRC:
							receivedDataString = "Perform CRC set by master to " + ((receivedData[Idx_commandParameter + 3] == 1) ? "true" : "false");
							break;
						default:
							receivedDataString = "Unknown command " + command + " from " + source + " to " + target;
							break;
					}
					break;
				case Address_general:
					if (directDevice.Address >= Address_beacon)
						radioDeviceAddress = 1;

					switch (command)
					{
						default:
							receivedDataString = "Unknown command " + command + " from " + source + " to " + target;
							break;
					}
					break;
				case Address_PC:
					switch (command)
					{
						case Command.GetAddress:
							if (!directDeviceInitialized)
							{
								directDevice.Address = target;
								((Button)mainWindow.directNodeGroupBox.CheckBeacons.field).Click += new EventHandler(SendDevicesPresent);
								((TextBox)mainWindow.directNodeGroupBox.AddressControl.field).TextChanged += new EventHandler(AddressFieldChanged);
								((Button)mainWindow.directNodeGroupBox.SetAddress.field).Click += new EventHandler(SetAddress);
								directDeviceInitialized = true;
								mainWindow.SetDirectlyConnectedNodeType();

								if (directDevice.Address == Address_master)
									receivedDataString = "Connected to master";
								else if (directDevice.Address >= Address_beacon)
									receivedDataString = "Connected to beacon " + directDevice.Address;
								else if (directDevice.Address >= Address_general)
									receivedDataString = "Connected to new/unknown device";
							}
							break;
						case Command.SetAddress:
							if (response == Response.ACK)
							{
								if (directDevice.Address != target)
								{
									directDevice.Address = target;
									mainWindow.SetDirectlyConnectedNodeType();

									receivedDataString = "Changed to master";
								}
							}
							else
							{
								receivedDataString = "Could not set address";
							}
							break;
						case Command.IsPresent:
							receivedDataString = "Checking for present devices";
							break;
						case Command.Bandwidth:
							receivedDataString = "Bandwidth set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.OutputPower:
							receivedDataString = "Output power set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.CodingRate:
							receivedDataString = "Coding rate set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.SpreadingFactor:
							receivedDataString = "Spreading factor set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.RxSymTimeout:
							receivedDataString = "Rx timeout (sym) set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.RxMsTimeout:
							int rxTimeout = receivedData[Idx_commandParameter + 0] << 24 |
											receivedData[Idx_commandParameter + 1] << 16 |
											receivedData[Idx_commandParameter + 2] << 8 |
											receivedData[Idx_commandParameter + 3];
							receivedDataString = "Rx timeout (ms) set to " + rxTimeout.ToString();
							break;
						case Command.TxTimeout:
							int txTimeout = receivedData[Idx_commandParameter + 0] << 24 |
											receivedData[Idx_commandParameter + 1] << 16 |
											receivedData[Idx_commandParameter + 2] << 8 |
											receivedData[Idx_commandParameter + 3];
							receivedDataString = "Tx timeout (ms) set to " + txTimeout.ToString();
							break;
						case Command.PreambleSize:
							receivedDataString = "Preamble size set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.PayloadMaxSize:
							receivedDataString = "Payload max size set to " + receivedData[Idx_commandParameter + 3];
							break;
						case Command.VariablePayload:
							receivedDataString = "Variable payload set to " + ((receivedData[Idx_commandParameter + 3] == 1) ? "true" : "false");
							break;
						case Command.PerformCRC:
							receivedDataString = "Perform CRC set to " + ((receivedData[Idx_commandParameter + 3] == 1) ? "true" : "false");
							break;
						default:
							receivedDataString = "Unknown command " + command + " from " + source + " to " + target;
							break;
					}
					break;
				default: //beacon
					radioDeviceAddress = source;

					switch (command)
					{
						case Command.IsPresent:
							if (response == Response.ACK)
								receivedDataString = "Beacon " + source + " present";
							else
								receivedDataString = "Beacon " + source + " sent uknown command " + command;
							break;
						default:
							receivedDataString = "Unknown command " + command + " from " + source + " to " + target;
							break;
					}
					break;
			}
			if (response == Response.NACK)
			{
				switch ((Error)receivedData[Idx_commandParameter + 1])
				{
					case Error.RADIO_RX_TIMEOUT:
						if (target == Address_general)
						{
							radioDeviceAddress = 0;
							receivedDataString = "No beacon responded";
						}
						else
							receivedDataString = "Beacon " + target + " did not respond";
						foreach (RadioDevice device in radioDevices)
							if (device.Address == radioDeviceAddress)
							{
								device.Connected = false;
								mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateConnectedStatus(false);
							}
						break;
					case Error.RADIO_TX_TIMEOUT:
						receivedDataString = "Tx timeout too small to send messages";
						break;
				}
			}
			
			if (radioDeviceAddress != 0)
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
					if (radioDeviceAddress == Address_master)
					{
						logger.Write("Radio device master");
					}
					else
						logger.Write("Radio device beacon " + directDevice.Address);
				}

				foreach (RadioDevice device in radioDevices)
					if (device.Address == radioDeviceAddress)
					{/*
					if (line.Contains("Rssi") && line.Contains(","))
					{
						string logString;
						device.Connected = true;
						device.UpdateSignalQuality(line);
						mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateConnectedStatus(true);
						mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateRSSI(device.RSSI);
						mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateSNR(device.SNR);
						if (radioDeviceAddress == MasterDeviceAddress)
							logString = "Master, ";
						else
							logString = "Beacon " + directDevice.Address;
						logString += device.RSSI + ", " + device.SNR;
						logger.Write(logString);
					}
					break;*/
					}
			}
			if (receivedDataString != null)
			{
				logger.Write(receivedDataString);
				mainWindow.UpdateLog(receivedDataString);
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
			connectionHandler.SendCommand(directDevice.Address, Command.SetAddress, mainWindow.directNodeGroupBox.Address);
		}

		private static void SendDevicesPresent(object sender, EventArgs e)
		{
			connectionHandler.SendGeneralCommand(Command.IsPresent);
		}
		#endregion

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}
