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
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
			List<string> receivedData;
			BackgroundWorker worker = (BackgroundWorker)sender;

			while (connectionHandler.Connected)
			{
				receivedData = connectionHandler.ReceiveData();
				if (serverHandler != null)
				{
					byte[] command = serverHandler.Receive();
					if (command[0] != (byte)Commands.Invalid)
						connectionHandler.SendCommand(command);
					foreach (string s in receivedData)
						serverHandler.Send(s);
				}
				worker.ReportProgress(0, receivedData);
			}
		}

		private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			int radioDeviceAddress = 0;
			bool newRadioDevice;

			foreach (string line in (List<string>)e.UserState)
			{
				if (directDevice.Address == 0)
				{
					if (line.Contains("I am a master"))
					{
						directDevice.Address = MasterDeviceAddress;
						mainWindow.SetDirectlyConnectedNodeType();
						logger.Write("Direct device master");
					}
					else if (line.Contains("I am a beacon"))
					{
						directDevice.Address = Byte.Parse(line.Substring(line.LastIndexOf(' ') + 1));
						mainWindow.SetDirectlyConnectedNodeType();
						logger.Write("Direct device beacon " + directDevice.Address);
					}
				}
				else
				{
					if (directDevice.Address == MasterDeviceAddress && line.Contains("ACK"))
						radioDeviceAddress = Int32.Parse(line.Remove(line.LastIndexOf(' ')).Substring(line.IndexOf(' ') + 1));
					else if (line.Contains("Asked if present"))
						radioDeviceAddress = 1;

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
						if (radioDeviceAddress == MasterDeviceAddress)
							logger.Write("Radio device master");
						else
							logger.Write("Radio device beacon " + directDevice.Address);
					}
				}

				if (line.Contains("Rssi") && line.Contains(","))
				{
					foreach (RadioDevice device in radioDevices)
						if (device.Address == radioDeviceAddress)
						{
							string logString;
							device.updateSignalQuality(line);
							mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateRSSI(device.RSSI);
							mainWindow.radioNodeGroupBoxes[radioDevices.IndexOf(device)].UpdateSNR(device.SNR);
							if (radioDeviceAddress == MasterDeviceAddress)
								logString = "Master, ";
							else
								logString = "Beacon " + directDevice.Address;
							logString += device.RSSI + ", " + device.SNR;
							logger.Write(logString);
							break;
						}
				}
			}
			mainWindow.UpdateLog((List<string>)e.UserState);
		}

		private static void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			mainWindow.BoardDisconnected();
		}
		#endregion
	}
}
