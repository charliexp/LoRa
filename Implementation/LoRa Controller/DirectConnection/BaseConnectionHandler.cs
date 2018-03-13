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
			IsPresent = 0x10,

			NodeType = 0x20,
			SetAddress = 0x21,

			Bandwidth = 0x40,
			OutputPower = 0x41,
			SpreadingFactor = 0x42,
			CodingRate = 0x43,
			RxSymTimeout = 0x44,
			RxMsTimeout = 0x45,
			TxTimeout = 0x46,
			PreambleSize = 0x47,
			PayloadMaxSize = 0x48,
			VariablePayload = 0x49,
			PerformCRC = 0x4a,

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
		public const int PCAddress = 0xff;
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
		public string ReceiveData()
		{
			string receivedData = "";
			
			while (Connected && !receivedData.Contains("\r"))
			{
				try
				{
					receivedData += Convert.ToChar(ReadByte());
				}
				catch
				{
					Close();
				}
			}
			receivedData = receivedData.TrimEnd(new char[] { '\n', '\r' });
			
			return receivedData;
		}

		public async Task<string> ReceiveDataAsync()
		{
			string receivedData = "";
			while (Connected && !receivedData.Contains("\r"))
			{
				try
				{
					receivedData += Convert.ToChar(await ReadByteAsync());
				}
				catch
				{
					Close();
				}
			}
			receivedData = receivedData.TrimEnd(new char[] { '\n', '\r' });
			
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
			commandBytes[0] = PCAddress;
			commandBytes[1] = BaseDevice.GeneralCallAddress;
			commandBytes[2] = Convert.ToByte(command);
			SendGeneralCommand(commandBytes);
		}

		public void SendGeneralCommand(Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = PCAddress;
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
			commandBytes[0] = PCAddress;
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendCommand(int address, Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = PCAddress;
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
			commandBytes[0] = PCAddress;
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			await SendCommandAsync(commandBytes);
		}

		public async Task SendCommandAsync(int address, Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = PCAddress;
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
