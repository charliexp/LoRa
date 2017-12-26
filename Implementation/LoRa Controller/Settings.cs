using LoRa_Controller.Connection;
using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller
{
	static class Settings
	{
		#region Public constants
		public const string FilePath = "settings.ini";

		public const string LogFolder = "LogFolder";
		public const string COMPort = "COMPort";
		public const string IPAddress = "IPAddress";
		public const string TCPPort = "TCPPort";
		#endregion

		#region Public methods
		public static void Load()
		{
			string[] settingLines;
			string settingName;
			string settingValue;

			settingLines = File.ReadAllLines(FilePath);

			foreach (string settingLine in settingLines)
			{
				settingName = settingLine.Remove(settingLine.IndexOf('=') - 1);
				settingValue = settingLine.Substring(settingLine.LastIndexOf('=') + 2);

				if (settingName.Equals(LogFolder))
				{
					Program.mainWindow.logger.Folder = settingValue;
				}

				if (settingName.Equals(COMPort))
				{
					if (Program.mainWindow.ConnectionType == DeviceHandler.ConnectionType.Serial)
						((SerialHandler)DeviceHandler._connectionHandler).PortName = settingValue;
				}

				if (settingName.Equals(IPAddress))
				{
					if (Program.mainWindow.ConnectionType == DeviceHandler.ConnectionType.Internet)
						((InternetHandler)DeviceHandler._connectionHandler).IPAddress = settingValue;
				}

				if (settingName.Equals(TCPPort))
				{
					if (Program.mainWindow.ConnectionType == DeviceHandler.ConnectionType.Internet)
						((InternetHandler)DeviceHandler._connectionHandler).Port = Int32.Parse(settingValue);
				}
			}
		}

		public static void Save(string name, string value)
		{
			StreamWriter settingsFileStreamWriter;

			if (File.Exists(FilePath))
			{
				bool written = false;
				string[] settingLines = File.ReadAllLines(FilePath);
				settingsFileStreamWriter = new StreamWriter(File.Open(FilePath, FileMode.Create));

				foreach (string settingLine in settingLines)
				{
					if (settingLine.Contains(name))
					{
						settingsFileStreamWriter.WriteLine(name + " = " + value);
						written = true;
					}
					else
						settingsFileStreamWriter.WriteLine(settingLine);
				}

				if (!written)
					settingsFileStreamWriter.WriteLine(name + " = " + value);
			}
			else
			{
				settingsFileStreamWriter = new StreamWriter(File.Open(FilePath, FileMode.Create));
				settingsFileStreamWriter.WriteLine(name + " = " + value);
			}

			settingsFileStreamWriter.Close();
		}
		#endregion
	}
}
