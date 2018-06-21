using LoRa_Controller.Connection;
using LoRa_Controller.Device;
using LoRa_Controller.Interface;
using LoRa_Controller.Log;
using LoRa_Controller.Networking;
using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static LoRa_Controller.Connection.BaseConnectionHandler;
using System.IO;
using LoRa_Controller.Connection.Messages;
using static LoRa_Controller.Connection.Messages.Message;

namespace LoRa_Controller
{
	public static class Program
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

        #region Public methods
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
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

            mainWindow.AddControl(logger.Interface);

			connectionDialog.Show();
            Application.Run(mainWindow);
        }
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

            if (!Directory.Exists((string)SettingHandler.LogFolder.Value))
            {
                logger.Folder = Directory.GetCurrentDirectory();
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
            Frame directConnectionFrame;

			BackgroundWorker worker = (BackgroundWorker)sender;
            
            connectionHandler.Write(new Connection.Messages.Message(CommandType.IsPresent));

            while (connectionHandler.Connected)
			{
                directConnectionFrame = connectionHandler.Read();
				if (serverHandler != null)
				{/*
                    Frame remoteFrame = serverHandler.Read();
					if (receivedClientMessage.Command != CommandType.Invalid)
						connectionHandler.Write(receivedClientMessage);
					serverHandler.Write(receivedDirectlyMessage.ToString());*/
				}
                
                if (directConnectionFrame != null)
				    worker.ReportProgress(0, directConnectionFrame);
			}
		}
		private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			int radioDeviceAddress = Int32.MaxValue;
			bool newRadioDevice;
			Frame frame = (Frame)e.UserState;

            foreach(Connection.Messages.Message message in frame.Messages)
            {
                switch(message.Command)
                {
                    case CommandType.IsPresent:
                        if (!directDeviceInitialized)
                        {
                            directDevice.Address = frame.EndDevice;
                            ((TextBox)mainWindow.DirectNodeInterface.addressControl.Field).TextChanged += new EventHandler(AddressFieldChanged);
                            ((Button)mainWindow.DirectNodeInterface.SetAddress.Field).Click += new EventHandler(SetAddress);
                            directDeviceInitialized = true;
                            mainWindow.SetDirectlyConnectedNodeType();
                        }
                        break;
                    case CommandType.Timestamp:
                        break;
                }
            }

            logger.Write(frame);
            /*switch (message.Source)
			{
				case (int)AddressType.Master:
					if (directDevice.Address == (int)AddressType.Master)
						radioDeviceAddress = message.Target;
					else
						radioDeviceAddress = 1;
					switch (message.Command)
					{
						case CommandType.IsPresent:
							receivedDataString = "Master asked if present";
							break;
						case CommandType.Bandwidth:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.DirectNodeInterface.Bandwidth.SetValue(message.Parameters[0]);
							receivedDataString = "Bandwidth set by master to " + message.Parameters[0];
							break;
						case CommandType.OutputPower:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.DirectNodeInterface.OutputPower.SetValue(message.Parameters[0]);
							receivedDataString = "Output power set by master to " + message.Parameters[0];
							break;
						case CommandType.CodingRate:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.DirectNodeInterface.CodingRate.SetValue(message.Parameters[0]);
							receivedDataString = "Coding rate set by master to " + message.Parameters[0];
							break;
						case CommandType.SpreadingFactor:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.DirectNodeInterface.SpreadingFactor.SetValue(message.Parameters[0]);
							receivedDataString = "Spreading factor set by master to " + message.Parameters[0];
							break;
						case CommandType.RxSymTimeout:
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.DirectNodeInterface.RxSymTimeout.SetValue(message.Parameters[0]);
							receivedDataString = "Rx timeout (sym) set by master to " + message.Parameters[0];
							break;
						case CommandType.RxMsTimeout:
							int rxTimeout = message.Parameters[0];
							if (directDevice.Address != (int)AddressType.Master)
								mainWindow.DirectNodeInterface.RxMsTimeout.SetValue(rxTimeout);
							receivedDataString = "Rx timeout (ms) set by master to " + rxTimeout.ToString();
							break;
						case CommandType.TxTimeout:
							int txTimeout = message.Parameters[0];
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.DirectNodeInterface.TxTimeout.SetValue(txTimeout);
							receivedDataString = "Tx timeout (ms) set by master to " + txTimeout.ToString();
							break;
						case CommandType.PreambleSize:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.DirectNodeInterface.PreambleSize.SetValue(message.Parameters[0]);
							receivedDataString = "Preamble size set by master to " + message.Parameters[0];
							break;
						case CommandType.PayloadMaxSize:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.DirectNodeInterface.PayloadMaxSize.SetValue(message.Parameters[0]);
							receivedDataString = "Payload max size set by master to " + message.Parameters[0];
							break;
						case CommandType.VariablePayload:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.DirectNodeInterface.VariablePayload.SetValue(message.Parameters[0] == 1);
							receivedDataString = "Variable payload set by master to " + ((message.Parameters[0] == 1) ? "true" : "false");
							break;
						case CommandType.PerformCRC:
							if (directDevice.Address != (int) AddressType.Master)
								mainWindow.DirectNodeInterface.PerformCRC.SetValue(message.Parameters[0] == 1);
							receivedDataString = "Perform CRC set by master to " + ((message.Parameters[0] == 1) ? "true" : "false");
							break;
						default:
							receivedDataString = "Unknown command " + message.Command + " from " + message.Source + " to " + message.Target;
							break;
					}
					break;
				case (int)AddressType.General:
					if (directDevice.Address >= (int)AddressType.Beacon)
					{
						radioDeviceAddress = 1;
					}

					switch (message.Command)
					{
						case CommandType.IsPresent:
							break;
						default:
							receivedDataString = "Unknown command " + message.Command + " from " + message.Source + " to " + message.Target;
							break;
					}
					break;
				case (int)AddressType.PC:
					switch (message.Command)
					{
						case CommandType.GetAddress:
							if (!directDeviceInitialized)
							{
								directDevice.Address = message.Target;
								((TextBox)mainWindow.DirectNodeInterface.addressControl.Field).TextChanged += new EventHandler(AddressFieldChanged);
								((Button)mainWindow.DirectNodeInterface.SetAddress.Field).Click += new EventHandler(SetAddress);
								directDeviceInitialized = true;
								mainWindow.SetDirectlyConnectedNodeType();

								if (directDevice.Address == (int) AddressType.Master)
								{
									receivedDataString = "Connected to master";
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
								if (directDevice.Address != message.Target)
								{
									directDevice.Address = message.Target;
									mainWindow.SetDirectlyConnectedNodeType();

									if (directDevice.Type == BaseDevice.NodeType.Gateway)
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
							receivedDataString = "Bandwidth set to " + message.Parameters[0];
							break;
						case CommandType.OutputPower:
							receivedDataString = "Output power set to " + message.Parameters[0];
							break;
						case CommandType.CodingRate:
							receivedDataString = "Coding rate set to " + message.Parameters[0];
							break;
						case CommandType.SpreadingFactor:
							receivedDataString = "Spreading factor set to " + message.Parameters[0];
							break;
						case CommandType.RxSymTimeout:
							receivedDataString = "Rx timeout (sym) set to " + message.Parameters[0];
							break;
						case CommandType.RxMsTimeout:
							int rxTimeout = message.Parameters[0];
							receivedDataString = "Rx timeout (ms) set to " + rxTimeout.ToString();
							break;
						case CommandType.TxTimeout:
							int txTimeout = message.Parameters[0];
							receivedDataString = "Tx timeout (ms) set to " + txTimeout.ToString();
							break;
						case CommandType.PreambleSize:
							receivedDataString = "Preamble size set to " + message.Parameters[0];
							break;
						case CommandType.PayloadMaxSize:
							receivedDataString = "Payload max size set to " + message.Parameters[0];
							break;
						case CommandType.VariablePayload:
							receivedDataString = "Variable payload set to " + ((message.Parameters[0] == 1) ? "true" : "false");
							break;
						case CommandType.PerformCRC:
							receivedDataString = "Perform CRC set to " + ((message.Parameters[0] == 1) ? "true" : "false");
							break;
						default:
							receivedDataString = "Unknown command " + message.Command + " from " + message.Source + " to " + message.Target;
							break;
					}
					break;
				default: //beacon
					radioDeviceAddress = message.Source;

					switch (message.Command)
					{
						case CommandType.IsPresent:
							if (message.Response == ResponseType.ACK)
								receivedDataString = "Beacon " + message.Source + " present";
							break;
						default:
							if (message.Response == ResponseType.ACK)
								receivedDataString = "Beacon " + message.Source + " ACK";
							if (message.Response == ResponseType.NAK)
								receivedDataString = "Beacon " + message.Source + " NACK";
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
						if (message.RSSI != 0 && message.SNR != 0)
						{
							string logString;
							device.Connected = true;
							device.UpdateSignalQuality(message.RSSI, message.SNR);
							mainWindow.RadioNodeInterfaces[radioDevices.IndexOf(device)].UpdateRSSI(-device.RSSI);
							mainWindow.RadioNodeInterfaces[radioDevices.IndexOf(device)].UpdateSNR(device.SNR);
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
				switch ((Error)message.Parameters[0])
				{
					case Error.RADIO_RX_TIMEOUT:
						if (message.Source == (int)AddressType.General)
						{
							receivedDataString = "No beacon responded";
							foreach (RadioDevice device in radioDevices)
							{
								device.Connected = false;
							}
						}
						else
						{
							receivedDataString = "Beacon " + message.Source + " did not respond";
							foreach (RadioDevice device in radioDevices)
								if (device.Address == radioDeviceAddress)
								{
									device.Connected = false;
								}
						}
						break;
					case Error.RADIO_TX_TIMEOUT:
						receivedDataString = "Tx timeout too small to send messages";
						break;
				}
			}
			logger.Write(message);*/
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
				newAddress = Int32.Parse(((TextBox)mainWindow.DirectNodeInterface.addressControl.Field).Text);
				if (newAddress != directDevice.Address && newAddress > 0)
					mainWindow.DirectNodeInterface.SetAddress.Field.Enabled = true;
				else
					mainWindow.DirectNodeInterface.SetAddress.Field.Enabled = false;
			}
			catch
			{
				mainWindow.DirectNodeInterface.SetAddress.Field.Enabled = false;
			}
		}
		private static void SetAddress(object sender, EventArgs e)
        {
            connectionHandler.Write(new Connection.Messages.Message(CommandType.SetAddress, mainWindow.DirectNodeInterface.Address));
		}
		#endregion

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}
