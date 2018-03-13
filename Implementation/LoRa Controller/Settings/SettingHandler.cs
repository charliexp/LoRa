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
			StreamWriter settingsFileStreamWriter;

			string[] settingLines;
			string name;
			string value;

			try
			{
				settingLines = File.ReadAllLines(FilePath);

				foreach (string setting in settingLines)
				{
					name = setting.Remove(setting.IndexOf('=') - 1);
					value = setting.Substring(setting.LastIndexOf('=') + 2);

					if (name.Equals(LogFolder.Name))
					{
						LogFolder.Value = value;
					}

					if (name.Equals(COMPort.Name))
					{
						COMPort.Value = value;
					}

					if (name.Equals(IPAddress.Name))
					{
						IPAddress.Value = value;
					}

					if (name.Equals(TCPPort.Name))
					{
						TCPPort.Value = Int32.Parse(value);
					}
				}
			}
			catch (FileNotFoundException)
			{
				settingsFileStreamWriter = new StreamWriter(File.Open(FilePath, FileMode.Create));
				settingsFileStreamWriter.Close();
			}
			finally
			{
				GetDefaultSettings();

				Save(LogFolder);
				Save(COMPort);
				Save(IPAddress);
				Save(TCPPort);
			}
		}

		private static void GetDefaultSettings()
		{
			if (LogFolder.Value == null)
			{
				LogFolder.Value = Directory.GetCurrentDirectory();
			}
			if (COMPort.Value == null)
			{
				COMPort.Value = "None";
			}
			if (IPAddress.Value == null)
			{
				IPAddress.Value = DefaultIPAddress;
			}
			if (TCPPort.Value == null)
			{
				TCPPort.Value = DefaultTCPPort;
			}
		}
		
		public static void Save(Setting setting)
		{
			StreamWriter settingsFileStreamWriter;
			
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

			settingsFileStreamWriter.Close();
		}
		#endregion
	}
}
