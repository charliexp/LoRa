using LoRa_Controller.Connection;
using LoRa_Controller.Device;
using LoRa_Controller.Interface;
using LoRa_Controller.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LoRa_Controller
{
	static class Program
	{
		public static MainWindow mainWindow;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			ConnectionDialog connectionDialog = new ConnectionDialog();
			DialogResult result = connectionDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				mainWindow = new MainWindow(connectionDialog.ConnectionType, connectionDialog.Parameters);
				Settings.Load();

				Application.Run(mainWindow);
			}
		}
	}
}
