using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.DirectConnection
{
	public abstract class BaseConnectionHandler
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
			IsPresent = 'z',
			Invalid = 0
		}

		public enum ConnectionType
		{
			Serial,
			Internet
		}

		internal Task SendCommandAsync(object address, Commands parameter, int value)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Public abstract properties
		public abstract bool Connected
		{
			get;
		}
		#endregion

		#region Public constants
		public const int CommandMaxLength = 7;
		#endregion

		#region Public abstract methods
		public virtual void Open()
		{
		}
		public abstract void Close();
		public abstract void WriteByte(byte byteToSend);
		public abstract byte ReadByte();
		public abstract Task SendByteAsync(byte byteToSend);
		public abstract Task<byte> ReadByteAsync();
		#endregion

		#region Public methods
		public List<string> ReceiveData()
		{
			List<string> receivedData = new List<string>();
			string receivedLine = "";

			while (Connected && !receivedLine.Contains("rxDone") &&
										   !receivedLine.Contains("not responding") &&
										   !receivedLine.Contains(":") &&
										   !receivedLine.Contains("I am a"))
			{
				receivedLine = "";
				while (Connected && !receivedLine.Contains("\r"))
				{
					try
					{
						receivedLine += Convert.ToChar(ReadByte());
					}
					catch
					{
						Close();
					}
				}
				receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

				receivedData.Add(receivedLine);
			}
			return receivedData;
		}

		public async Task<List<string>> ReceiveDataAsync()
		{
			List<string> receivedData = new List<string>();
			string receivedLine = "";

			while (Connected && !receivedLine.Contains("rxDone") &&
										   !receivedLine.Contains("not responding") &&
										   !receivedLine.Contains(":"))
			{
				receivedLine = "";
				while (Connected && !receivedLine.Contains("\r"))
				{
					receivedLine += Convert.ToChar(await ReadByteAsync());
				}
				receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

				receivedData.Add(receivedLine);
			}
			return receivedData;
		}

		public void SendGeneralCommand(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				WriteByte(command[i]);
		}

		public void SendGeneralCommand(Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = BaseDevice.GeneralCallAddress;
			commandBytes[2] = Convert.ToByte(command);
			SendGeneralCommand(commandBytes);
		}

		public void SendGeneralCommand(Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = BaseDevice.GeneralCallAddress;
			commandBytes[2] = Convert.ToByte(command);
			commandBytes[3] = (byte)(value >> 24);
			commandBytes[4] = (byte)(value >> 16);
			commandBytes[5] = (byte)(value >> 8);
			commandBytes[6] = (byte)(value);
			SendGeneralCommand(commandBytes);
		}

		public void SendCommand(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				WriteByte(command[i]);
		}

		public void SendCommand(int address, Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendCommand(int address, Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = (byte)address;
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
				await (SendByteAsync(command[i]));
		}

		public async Task SendCommandAsync(int address, Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			await SendCommandAsync(commandBytes);
		}

		public async Task SendCommandAsync(int address, Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			commandBytes[3] = (byte)(value >> 24);
			commandBytes[4] = (byte)(value >> 16);
			commandBytes[5] = (byte)(value >> 8);
			commandBytes[6] = (byte)(value);
			await SendCommandAsync(commandBytes);
		}
		#endregion
	}
}
