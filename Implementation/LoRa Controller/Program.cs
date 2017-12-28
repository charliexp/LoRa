using LoRa_Controller.Connection;
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
using static LoRa_Controller.Device.DirectDevice;

namespace LoRa_Controller
{
	static class Program
	{
		public static Logger logger;
		public static DirectDevice DirectDevice;
		public static bool DeviceNodeTypeProcessed;
		public static MainWindow MainWindow;
		public static Server serverHandler;
		public static List<string> ReceivedData;
		public static ConnectionDialog ConnectionDialog;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			SettingHandler.Load();

			logger = new Logger("log_", "txt");

			ConnectionDialog = new ConnectionDialog();
			MainWindow = new MainWindow();

			ConnectionDialog.Show();
			Application.Run(MainWindow);
		}

		public static void StartConnection(ConnectionType connectionType, List<string> parameters)
		{
			MainWindow.Enabled = true;
			if (connectionType == ConnectionType.Serial)
			{
				DirectDevice = new DirectDevice(ConnectionType.Serial, parameters[0]);

				serverHandler = new Server();
				serverHandler.Start();
			}
			else if (connectionType == ConnectionType.Internet)
			{
				DirectDevice = new DirectDevice(ConnectionType.Internet, parameters);
			}

			DirectDevice.Connect();
			if (DirectDevice.Connected)
				MainWindow.BoardConnected();
			else
				MainWindow.BoardUnableToConnect();
			logger.Start();

			BackgroundWorker BackgroundWorker = new BackgroundWorker();
			BackgroundWorker.WorkerReportsProgress = true;
			BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
			BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
			BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

			BackgroundWorker.RunWorkerAsync();
		}

		private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender;
			DeviceNodeTypeProcessed = false;

			while (DirectDevice.Connected)
			{
				ReceivedData = DirectDevice.ReceiveData();
				if (serverHandler != null)
				{
					byte[] command = serverHandler.Receive();
					if (command[0] != (byte)Commands.Invalid)
						DirectDevice.SendCommand(command);
					foreach (string s in ReceivedData)
						serverHandler.Send(s);
				}
				worker.ReportProgress(0);
			}
		}

		private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			MainWindow.UpdateLog(ReceivedData);
			MainWindow.UpdateRSSI(DirectDevice.RSSI);
			MainWindow.UpdateSNR(DirectDevice.SNR);
			MainWindow.UpdateCurrentErrors(DirectDevice.Errors);
			MainWindow.UpdateTotalErrors(DirectDevice.TotalErrors);
			MainWindow.UpdateRadioConnectedNodeType();

			if (!DeviceNodeTypeProcessed && DirectDevice.nodeType != NodeType.Unknown)
			{
				if (DirectDevice.nodeType == NodeType.Master)
				{
					DirectDevice = new MasterDevice(DirectDevice);
					logger.Write("Connected to master");
				}
				else
				{
					DirectDevice = new BeaconDevice(DirectDevice);
					logger.Write("Connected to beacon " + DirectDevice.Address);
				}
				MainWindow.UpdateDirectlyConnectedNodeType();
			}
			
			if (DirectDevice.RSSI != 0 && DirectDevice.SNR != 0)
				logger.Write(DirectDevice.RSSI + ", " + DirectDevice.SNR);
			else
				logger.Write("error");
		}

		private static void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MainWindow.BoardDisconnected();
		}
	}
}
