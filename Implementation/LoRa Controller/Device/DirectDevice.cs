using LoRa_Controller.Connection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public class DirectDevice : BaseDevice
	{
		#region Public enums
		public enum ConnectionType
		{
			Serial,
			Internet
		}
		#endregion

		#region Public variables
		public List<RadioDevice> radioDevices;
		#endregion
		
		#region Public properties
		public override bool Connected
		{
			get { return connectionHandler.Connected; }
		}
		#endregion

		#region Constructors
		public DirectDevice() : base()
		{
			radioDevices = new List<RadioDevice>();
		}

		public DirectDevice(ConnectionType connectionType, string connectionName) : this()
		{
			switch (connectionType)
			{
				case ConnectionType.Serial:
					connectionHandler = new SerialHandler(connectionName);
					break;
			}
		}

		public DirectDevice(ConnectionType connectionType, List<string> parameters) : this()
		{
			switch (connectionType)
			{
				case ConnectionType.Serial:
					connectionHandler = new SerialHandler(parameters[0]);
					break;
				case ConnectionType.Internet:
					connectionHandler = new InternetHandler(parameters[0], Int32.Parse(parameters[1]));
					break;
			}
		}
		#endregion

		#region Public methods
		public void Connect()
		{
			connectionHandler.Open();
			SendGeneralCommand(Commands.NodeType);
		}

		public List<string> ReceiveData()
		{
			List<string> receivedData = new List<string>();
			string receivedLine = "";

			while (connectionHandler.Connected && !receivedLine.Contains("Done") &&
										   !receivedLine.Contains("Timeout") &&
										   !receivedLine.Contains(":"))
			{
				receivedLine = "";
				while (connectionHandler.Connected && !receivedLine.Contains("\r"))
				{
					try
					{
						receivedLine += Convert.ToChar(connectionHandler.ReadByte());
					}
					catch
					{
						connectionHandler.Close();
					}
				}
				receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

				receivedData.Add(receivedLine);
			}
			ParseData(receivedData);

			return receivedData;
		}

		public async Task<List<string>> ReceiveDataAsync()
		{
			List<string> receivedData = new List<string>();
			string receivedLine = "";

			while (connectionHandler.Connected && !receivedLine.Contains("Done") &&
										   !receivedLine.Contains("Timeout") &&
										   !receivedLine.Contains(":"))
			{
				receivedLine = "";
				while (connectionHandler.Connected && !receivedLine.Contains("\r"))
				{
					receivedLine += Convert.ToChar(await connectionHandler.ReadByteAsync());
				}
				receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

				receivedData.Add(receivedLine);
			}
			ParseData(receivedData);

			return receivedData;
		}
		#endregion

		#region Protected methods
		protected virtual void ParseData(List<string> receivedData)
		{
			foreach (string line in receivedData)
			{
				if (line.Contains("I am a master"))
				{
					nodeType = NodeType.Master;
					Address = 1;
				}
				else if (line.Contains("I am a beacon"))
				{
					nodeType = NodeType.Beacon;
					Address = Byte.Parse(line.Substring(line.LastIndexOf(' ') + 1));
				}
				else if (line.Contains("ACK"))
				{
					int radioDeviceAddress = Int32.Parse(line.Remove(line.LastIndexOf(' ')).Substring(line.IndexOf(' ') + 1));
					bool newDevice = true;

					foreach (RadioDevice device in radioDevices)
					{
						if (device.Address == radioDeviceAddress)
						{
							newDevice = false;
							break;
						}
					}
					if (newDevice)
						radioDevices.Add(new RadioDevice(radioDeviceAddress));
				}
				else if (line.Contains("Asked if present"))
				{
					int radioDeviceAddress = 1;
					bool newDevice = true;

					foreach (RadioDevice device in radioDevices)
					{
						if (device.Address == radioDeviceAddress)
						{
							newDevice = false;
							break;
						}
					}
					if (newDevice)
						radioDevices.Add(new RadioDevice(radioDeviceAddress));
				}
				else if (line.Contains("Rssi") && line.Contains(","))
				{
					String tempString = line.Remove(line.IndexOf(' '));

					if (tempString.Length != 0)
					{
						tempString = tempString.Substring(line.IndexOf('=') + 1);
						//rssi = Int16.Parse(tempString);
					}
					tempString = line.Substring(line.LastIndexOf('=') + 1);
					//snr = Int16.Parse(tempString);
				}
			}
		}
		#endregion
	}
}
