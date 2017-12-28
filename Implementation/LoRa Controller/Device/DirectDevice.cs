using LoRa_Controller.Connection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public class DirectDevice
	{
		#region Public enums
		public enum Commands
		{
			Bandwidth = 'a',
			OutputPower = 'b',
			SpreadingFactor = 'c',
			CodingRate = 'd',
			RxSymTimeout = 'e',
			RxMsTimeout = 'f',
			TxTimeout = 'g',
			PreambleSize = 'h',
			PayloadMaxSize = 'i',
			VariablePayload = 'j',
			PerformCRC = 'k',
			
			NodeType = 'y',
			Invalid = 'z'
		}

		public enum ConnectionType
		{
			Serial,
			Internet
		}

		public enum NodeType
		{
			Unknown,
			Master,
			Beacon,
		}
		#endregion

		#region Public constants
		public const int CommandMaxLength = 7;
		#endregion

		#region Public variables
		public static IConnectionHandler connectionHandler;
		public NodeType nodeType;
		#endregion

		#region Protected variables
		protected byte address;
		protected int rssi;
		protected int snr;
		protected uint oldErrors;
		protected uint errors;
		protected uint totalErrors;
		#endregion

		#region Public properties
		public int RSSI
		{
			get { return rssi; }
		}

		public int SNR
		{
			get { return snr; }
		}
		
		public byte Address
		{
			get { return address; }
			set { address = value; }
		}

		public uint Errors
		{
			get { return errors; }
		}

		public uint TotalErrors
		{
			get { return totalErrors; }
		}

		public bool Connected
		{
			get { return connectionHandler.Connected; }
		}
		#endregion

		#region Constructors
		public DirectDevice()
		{
			address = 1;
			nodeType = NodeType.Unknown;

			errors = 0;
			oldErrors = 0;
			totalErrors = 0;
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
			SendCommand(Commands.NodeType);
		}

		public void SendCommand(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				connectionHandler.WriteByte(command[i]);
		}

		public void SendCommand(Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = address;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}
		
		public void SendCommand(Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = address;
			commandBytes[2] = Convert.ToByte(command);
			commandBytes[3] = (byte)(value >> 24);
			commandBytes[4] = (byte)(value >> 16);
			commandBytes[5] = (byte)(value >> 8);
			commandBytes[6] = (byte)(value);
			SendCommand(commandBytes);
		}

		public async Task SendCommandAsync(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				await (connectionHandler.SendByteAsync(command[i]));
		}

		public async Task SendCommandAsync(Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = address;
			commandBytes[2] = Convert.ToByte(command);
			await SendCommandAsync(commandBytes);
		}
		
		public async Task SendCommandAsync(Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = address;
			commandBytes[2] = Convert.ToByte(command);
			commandBytes[3] = (byte)(value >> 24);
			commandBytes[4] = (byte)(value >> 16);
			commandBytes[5] = (byte)(value >> 8);
			commandBytes[6] = (byte)(value);
			await SendCommandAsync(commandBytes);
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

					}
				}
				receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

				ParseData(receivedLine);
				receivedData.Add(receivedLine);
			}

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

				ParseData(receivedLine);
				receivedData.Add(receivedLine);
			}

			return receivedData;
		}
		#endregion

		#region Protected methods
		protected virtual void ParseData(string receivedData)
		{
			if (receivedData.Contains("I am a master"))
			{
				nodeType = NodeType.Master;
			}
			else if (receivedData.Contains("I am a slave"))
			{
				nodeType = NodeType.Beacon;
				String tempString = receivedData.Substring(receivedData.LastIndexOf(' ') + 1);
				address = Byte.Parse(tempString);
			}
			else if (receivedData.Contains("Rssi") && receivedData.Contains(","))
			{
				String tempString = receivedData.Remove(receivedData.IndexOf(' '));

				if (tempString.Length != 0)
				{
					tempString = tempString.Substring(receivedData.IndexOf('=') + 1);
					rssi = Int16.Parse(tempString);
				}
				tempString = receivedData.Substring(receivedData.LastIndexOf('=') + 1);
				snr = Int16.Parse(tempString);
			}
		}
		#endregion
	}
}
