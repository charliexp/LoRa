using LoRa_Controller.Interface;
using LoRa_Controller.Log;
using System;
using System.IO;
using System.Windows.Forms;

namespace LoRa_Controller
{
	static class Program
	{
		public const string settingsFilePath = "settings.ini";
		static MainWindow mainWindow;

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
				LoadSettings();

				Application.Run(mainWindow);
			}
		}

		static void LoadSettings()
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
					if (mainWindow.logger.isOpen())
						mainWindow.logger.finish();
					mainWindow.logger.Folder = settingValue;
				}
			}
		}
	}
}
