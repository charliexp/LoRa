using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	class DeviceHandler
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
			IsMaster = 'y'
		}
		#endregion

		#region Public variables
		public SerialHandler _serialDevice;
		#endregion

		#region Protected variables
		protected byte _address;
		protected bool _isMaster;
		protected bool _isMasterReported;
		protected int _rssi;
		protected int _snr;
		protected uint _oldErrors;
		protected uint _errors;
		protected uint _totalErrors;
		#endregion

		#region Public properties
		public bool IsMaster
		{
			get { return _isMaster; }
		}

		public bool IsMasterReported
		{
			get { return _isMasterReported; }
		}

		public int RSSI
		{
			get { return _rssi; }
		}

		public int SNR
		{
			get { return _snr; }
		}

		public bool IsConnected
		{
			get { return _serialDevice.isConnected; }
		}

		public EventHandler<ConnectionEventArgs> ConnectedToBoard
		{
			set { _serialDevice.ConnectSucceeded += value; }
		}

		public EventHandler<ConnectionEventArgs> UnableToConnectToBoard
		{
			set { _serialDevice.ConnectFailed += value; }
		}

		public EventHandler<ConnectionEventArgs> DisconnectedFromBoard
		{
			set { _serialDevice.Disconnected += value; }
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
		#endregion

		#region Constructors
		public DeviceHandler()
		{
			_address = 0;
			_serialDevice = null;
			_isMasterReported = false;

			_errors = 0;
			_oldErrors = 0;
			_totalErrors = 0;
		}
		public DeviceHandler(string comPortName)
		{
			_address = 1;
			_serialDevice = new SerialHandler(comPortName);
			_serialDevice.ConnectSucceeded += UpdateConnectionStatus;
			_serialDevice.ConnectFailed += UpdateConnectionStatus;
			_serialDevice.Disconnected += UpdateConnectionStatus;
			
			_isMasterReported = false;

			_errors = 0;
			_oldErrors = 0;
			_totalErrors = 0;
		}
		#endregion

		#region Public methods
		public void ConnectToBoard()
		{
			_serialDevice.Open();
		}

		public static string[] getAvailablePorts()
		{
			return SerialPort.GetPortNames();
		}

		public async Task SendCommandAsync(Commands command)
		{
			await (_serialDevice.SendCharAsync(new byte[] { _address }));
			await (_serialDevice.SendCharAsync(new byte[] { Convert.ToByte(command) }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
		}

		public async Task SendCommandAsync(Commands command, byte value)
		{
			await (_serialDevice.SendCharAsync(new byte[] { _address }));
			await (_serialDevice.SendCharAsync(new byte[] { Convert.ToByte(command) }));
			await (_serialDevice.SendCharAsync(new byte[] { value }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
			await (_serialDevice.SendCharAsync(new byte[] { 0 }));
		}

		public async Task SendCommandAsync(Commands command, int value)
		{
			await (_serialDevice.SendCharAsync(new byte[] { _address }));
			await (_serialDevice.SendCharAsync(new byte[] { Convert.ToByte(command) }));
			await (_serialDevice.SendCharAsync(new byte[] { (byte)(value >> 24) }));
			await (_serialDevice.SendCharAsync(new byte[] { (byte)(value >> 16) }));
			await (_serialDevice.SendCharAsync(new byte[] { (byte)(value >> 8) }));
			await (_serialDevice.SendCharAsync(new byte[] { (byte)value }));
		}

		public async Task<List<string>> ReceiveDataAsync()
		{
			List<string> receivedData = new List<string>();
			string receivedLine = "";

			while (_serialDevice.IsOpen && !receivedLine.Contains("Done") &&
										   !receivedLine.Contains("Timeout") &&
										   !receivedLine.Contains(":"))
			{
				receivedLine = "";
				while (_serialDevice.IsOpen && !receivedLine.Contains("\r"))
				{
					byte[] receivedByte = new byte[1];

					if (await _serialDevice.ReadCharAsync(receivedByte))
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
		protected void UpdateConnectionStatus(object sender, ConnectionEventArgs e)
		{
			_serialDevice.isConnected = e.Connected;
		}

		protected virtual void ParseData(string receivedData)
		{
			if (receivedData.Contains("I am a master"))
			{
				_isMasterReported = true;
				_isMaster = true;
			}
			else if (receivedData.Contains("I am a slave"))
			{
				_isMasterReported = true;
				_isMaster = false;
				String tempString = receivedData.Substring(receivedData.IndexOf(' ') + 1);
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
