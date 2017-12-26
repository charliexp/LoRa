using System;
using System.IO;

namespace LoRa_Controller.Settings
{
	static class SettingHandler
	{
		#region Public constants
		private const string FilePath = "settings.ini";
		private const string DefaultIPAddress = "127.0.0.1";
		private const int DefaultTCPPort = 13000;
		#endregion
		
		#region Public variables
		public static Setting LogFolder = new Setting("LogFolder");
		public static Setting COMPort = new Setting("COMPort");
		public static Setting IPAddress = new Setting("IPAddress");
		public static Setting TCPPort = new Setting("TCPPort");
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

				if (settingName.Equals(LogFolder.Name))
				{
					LogFolder.Value = settingValue;
				}

				if (settingName.Equals(COMPort.Name))
				{
					COMPort.Value = settingValue;
				}

				if (settingName.Equals(IPAddress.Name))
				{
					IPAddress.Value = settingValue;
				}

				if (settingName.Equals(TCPPort.Name))
				{
					TCPPort.Value = Int32.Parse(settingValue);
				}
			}

			GetDefaultSettings();
		}

		private static void GetDefaultSettings()
		{
			if (LogFolder.Value == null)
			{
				LogFolder.Value = Directory.GetCurrentDirectory();
				Save(LogFolder);
			}
			if (COMPort.Value == null)
			{
				COMPort.Value = "None";
				Save(COMPort);
			}
			if (IPAddress.Value == null)
			{
				IPAddress.Value = DefaultIPAddress;
				Save(IPAddress);
			}
			if (TCPPort.Value == null)
			{
				TCPPort.Value = DefaultTCPPort;
				Save(TCPPort);
			}
		}
		
		public static void Save(Setting setting)
		{
			StreamWriter settingsFileStreamWriter;

			if (File.Exists(FilePath))
			{
				bool written = false;
				string[] settingLines = File.ReadAllLines(FilePath);
				settingsFileStreamWriter = new StreamWriter(File.Open(FilePath, FileMode.Create));

				foreach (string settingLine in settingLines)
				{
					if (settingLine.Contains(setting.Name))
					{
						settingsFileStreamWriter.WriteLine(setting.Name + " = " + setting.Value);
						written = true;
					}
					else
						settingsFileStreamWriter.WriteLine(settingLine);
				}

				if (!written)
					settingsFileStreamWriter.WriteLine(setting.Name + " = " + setting.Value);
			}
			else
			{
				settingsFileStreamWriter = new StreamWriter(File.Open(FilePath, FileMode.Create));
				settingsFileStreamWriter.WriteLine(setting.Name + " = " + setting.Value);
			}

			settingsFileStreamWriter.Close();
		}
		#endregion
	}
}
