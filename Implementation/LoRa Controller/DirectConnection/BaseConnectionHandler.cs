﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.DirectConnection
{
	public abstract class BaseConnectionHandler
	{
		#region Public enums
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

		internal Task SendCommandAsync(object address, CommandType parameter, int value)
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
		public const int Idx_RSSI = ParametersMaxSize + 3;
		public const int Idx_SNR = ParametersMaxSize + 4;

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
			
			while (Connected && receivedData.Count != ParametersMaxSize + 5)
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

		public async Task<byte[]> ReceiveDataAsync()
		{
			List<byte> receivedData = new List<byte>();

			while (Connected && receivedData.Count != ParametersMaxSize + 5)
			{
				try
				{
					receivedData.Add(await ReadByteAsync());
				}
				catch
				{
					Close();
				}
			}

			return receivedData.ToArray();
		}

		public void SendCommand(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				WriteByte(command[i]);
		}

		public void SendGeneralCommand(CommandType command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = Address_general;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendGeneralCommand(CommandType command, int value)
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

		public void SendCommand(int address, CommandType command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendCommand(int address, CommandType command, int value)
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

		public async Task SendCommandAsync(int address, CommandType command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[0] = Address_PC;
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			await SendCommandAsync(commandBytes);
		}

		public async Task SendCommandAsync(int address, CommandType command, int value)
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
