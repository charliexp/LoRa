using LoRa_Controller.Connection;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public class DeviceHandler
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
			IsMaster = 'y',
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
		public static IConnectionHandler _connectionHandler;
		public NodeType _nodeType;
		#endregion

		#region Protected variables
		protected byte _address;
		protected int _rssi;
		protected int _snr;
		protected uint _oldErrors;
		protected uint _errors;
		protected uint _totalErrors;
		#endregion

		#region Public properties
		public int RSSI
		{
			get { return _rssi; }
		}

		public int SNR
		{
			get { return _snr; }
		}
		
		public byte Address
		{
			get { return _address; }
			set { _address = value; }
		}

		public uint Errors
		{
			get { return _errors; }
		}

		public uint TotalErrors
		{
			get { return _totalErrors; }
		}

		public bool Connected
		{
			get { return _connectionHandler.Connected; }
		}
		#endregion

		#region Constructors
		public DeviceHandler()
		{
			_address = 1;
			_nodeType = NodeType.Unknown;

			_errors = 0;
			_oldErrors = 0;
			_totalErrors = 0;
		}

		public DeviceHandler(ConnectionType connectionType, string connectionName) : this()
		{
			switch (connectionType)
			{
				case ConnectionType.Serial:
					_connectionHandler = new SerialHandler(connectionName);
					break;
			}
		}

		public DeviceHandler(ConnectionType connectionType, List<string> parameters) : this()
		{
			switch (connectionType)
			{
				case ConnectionType.Serial:
					_connectionHandler = new SerialHandler(parameters[0]);
					break;
				case ConnectionType.Internet:
					_connectionHandler = new InternetHandler(parameters[0], Int32.Parse(parameters[1]));
					break;
			}
		}
		#endregion

		#region Public methods
		public void ConnectToBoard()
		{
			_connectionHandler.Open();
		}

		public async Task SendCommandAsync(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				await (_connectionHandler.SendCharAsync(new byte[] { command[i] }));
		}

		public async Task SendCommandAsync(Commands command)
		{
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { _address }));
			await (_connectionHandler.SendCharAsync(new byte[] { Convert.ToByte(command) }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
		}

		public async Task SendCommandAsync(Commands command, byte value)
		{
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { _address }));
			await (_connectionHandler.SendCharAsync(new byte[] { Convert.ToByte(command) }));
			await (_connectionHandler.SendCharAsync(new byte[] { value }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
		}

		public async Task SendCommandAsync(Commands command, int value)
		{
			await (_connectionHandler.SendCharAsync(new byte[] { 0 }));
			await (_connectionHandler.SendCharAsync(new byte[] { _address }));
			await (_connectionHandler.SendCharAsync(new byte[] { Convert.ToByte(command) }));
			await (_connectionHandler.SendCharAsync(new byte[] { (byte)(value >> 24) }));
			await (_connectionHandler.SendCharAsync(new byte[] { (byte)(value >> 16) }));
			await (_connectionHandler.SendCharAsync(new byte[] { (byte)(value >> 8) }));
			await (_connectionHandler.SendCharAsync(new byte[] { (byte)value }));
		}

		public async Task<List<string>> ReceiveDataAsync()
		{
			List<string> receivedData = new List<string>();
			string receivedLine = "";

			while (_connectionHandler.Connected && !receivedLine.Contains("Done") &&
										   !receivedLine.Contains("Timeout") &&
										   !receivedLine.Contains(":"))
			{
				receivedLine = "";
				while (_connectionHandler.Connected && !receivedLine.Contains("\r"))
				{
					byte[] receivedByte = new byte[1];

					if (await _connectionHandler.ReadCharAsync(receivedByte))
					{
						if (receivedByte[0] > 0)
							receivedLine += Convert.ToChar(receivedByte[0]);
					}
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
				_nodeType = NodeType.Master;
			}
			else if (receivedData.Contains("I am a slave"))
			{
				_nodeType = NodeType.Beacon;
				String tempString = receivedData.Substring(receivedData.LastIndexOf(' ') + 1);
				_address = Byte.Parse(tempString);
			}
			else if (receivedData.Contains("Rssi") && receivedData.Contains(","))
			{
				String tempString = receivedData.Remove(receivedData.IndexOf(' '));

				if (tempString.Length != 0)
				{
					tempString = tempString.Substring(receivedData.IndexOf('=') + 1);
					_rssi = Int16.Parse(tempString);
				}
				tempString = receivedData.Substring(receivedData.LastIndexOf('=') + 1);
				_snr = Int16.Parse(tempString);
			}
		}
		#endregion
	}
}
