using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.DirectConnection
{
	public abstract class BaseConnectionHandler
	{
		#region Public enums
		public enum Command
		{
			IsPresent = 0x10,

			GetAddress = 0x20,
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

		public enum Error
		{
			RADIO_RX_TIMEOUT = 0xf2,
			RADIO_RX_ERROR = 0xf3,
			RADIO_TX_TIMEOUT = 0xf5,
		};


		public enum ConnectionType
		{
			Serial,
			Internet
		}

		internal Task SendCommandAsync(object address, Command parameter, int value)
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
		public const int Address_general = 0;
		public const int Address_master = 1;
		public const int Address_beacon = 2;
		public const int Address_PC = 0xff;
		public const int CommandMaxLength = 7;

		public const int Idx_sourceAddress = 0;
		public const int Idx_targetAddress = 1;
		public const int Idx_command = 2;
		public const int Idx_commandParameter = 3;

		public const int ParametersMaxSize = 4;

		public enum Response
		{
			ACK = 1,
			NACK = 0xff
		};

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
		public byte[] ReceiveData()
		{
			List<byte> receivedData = new List<byte>();
			
			while (Connected && receivedData.Count != ParametersMaxSize + 3)
			{
				try
				{
					receivedData.Add(ReadByte());
				}
				catch
				{
					Close();
				}
			}
			
			return receivedData.ToArray();
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

		public void SendCommand(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				WriteByte(command[i]);
		}

		public void SendGeneralCommand(Command command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = Address_general;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendGeneralCommand(Command command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = Address_general;
			commandBytes[2] = Convert.ToByte(command);
			commandBytes[3] = (byte)(value >> 24);
			commandBytes[4] = (byte)(value >> 16);
			commandBytes[5] = (byte)(value >> 8);
			commandBytes[6] = (byte)(value);
			SendCommand(commandBytes);
		}

		public void SendCommand(int address, Command command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendCommand(int address, Command command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
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

		public async Task SendCommandAsync(int address, Command command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			await SendCommandAsync(commandBytes);
		}

		public async Task SendCommandAsync(int address, Command command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
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
