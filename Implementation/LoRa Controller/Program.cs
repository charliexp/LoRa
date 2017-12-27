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
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller
{
	static class Program
	{
		public static Logger logger;
		public static DeviceHandler DeviceHandler;
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
				DeviceHandler = new DeviceHandler(ConnectionType.Serial, parameters[0]);

				serverHandler = new Server();
				serverHandler.Start();
			}
			else if (connectionType == ConnectionType.Internet)
			{
				DeviceHandler = new DeviceHandler(ConnectionType.Internet, parameters);
			}

			DeviceHandler.ConnectToBoard();
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

			while (DeviceHandler.Connected)
			{
				ReceivedData = DeviceHandler.ReceiveData();
				if (serverHandler != null)
				{
					byte[] command = serverHandler.Receive();
					if (command[0] != (byte)Commands.Invalid)
						DeviceHandler.SendCommand(command);
					foreach (string s in ReceivedData)
						serverHandler.Send(s);
				}
				worker.ReportProgress(0);
			}
		}

		private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (DeviceHandler.Connected)
				MainWindow.BoardConnected();
			else
				MainWindow.BoardUnableToConnect();

			MainWindow.UpdateLog(ReceivedData);
			MainWindow.UpdateRSSI(DeviceHandler.RSSI);
			MainWindow.UpdateSNR(DeviceHandler.SNR);
			MainWindow.UpdateCurrentErrors(DeviceHandler.Errors);
			MainWindow.UpdateTotalErrors(DeviceHandler.TotalErrors);

			if (!DeviceNodeTypeProcessed && DeviceHandler._nodeType != NodeType.Unknown)
			{
				if (DeviceHandler._nodeType == NodeType.Master)
				{
					DeviceHandler = new MasterDevice(DeviceHandler);
					logger.Write("Connected to master");
				}
				else
				{
					DeviceHandler = new BeaconDevice(DeviceHandler);
					logger.Write("Connected to beacon " + DeviceHandler.Address);
				}
			}

			if (DeviceHandler is MasterDevice)
			{
				MainWindow.UpdateRadioConnectionStatus(((MasterDevice)DeviceHandler).HasBeaconConnected);
			}
			
			if (DeviceHandler.RSSI != 0 && DeviceHandler.SNR != 0)
				logger.Write(DeviceHandler.RSSI + ", " + DeviceHandler.SNR);
			else
				logger.Write("error");
		}

		private static void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MainWindow.BoardDisconnected();
		}
	}
}
