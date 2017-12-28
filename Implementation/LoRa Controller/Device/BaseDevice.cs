using LoRa_Controller.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public abstract class BaseDevice
	{
		#region Constructors
		public BaseDevice()
		{
			nodeType = NodeType.Unknown;
		}
		#endregion

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

		public enum NodeType
		{
			Unknown,
			Master,
			Beacon,
		}
		#endregion

		#region Public constants
		public const int MasterDeviceAddress = 1;
		public const int CommandMaxLength = 7;
		#endregion

		#region Public variables
		public static IConnectionHandler connectionHandler;
		public NodeType nodeType;
		#endregion

		#region Private variables
		private int address;
		#endregion

		#region Public properties
		public int Address
		{
			get { return address; }
			set
			{
				address = value;
				if (address == MasterDeviceAddress)
					nodeType = NodeType.Master;
				else
					nodeType = NodeType.Beacon;
			}
		}

		public abstract bool Connected
		{
			get;
		}
		#endregion

		#region Public methods
		public static void SendGeneralCommand(byte[] command)
		{
			for (int i = 0; i < CommandMaxLength; i++)
				connectionHandler.WriteByte(command[i]);
		}

		public static void SendGeneralCommand(Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = 0;
			commandBytes[2] = Convert.ToByte(command);
			SendGeneralCommand(commandBytes);
		}

		public static void SendGeneralCommand(Commands command, int value)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = 0;
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
				connectionHandler.WriteByte(command[i]);
		}

		public void SendCommand(Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = (byte) address;
			commandBytes[2] = Convert.ToByte(command);
			SendCommand(commandBytes);
		}

		public void SendCommand(Commands command, int value)
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
				await (connectionHandler.SendByteAsync(command[i]));
		}

		public async Task SendCommandAsync(Commands command)
		{
			byte[] commandBytes = new byte[7];
			commandBytes[1] = (byte)address;
			commandBytes[2] = Convert.ToByte(command);
			await SendCommandAsync(commandBytes);
		}

		public async Task SendCommandAsync(Commands command, int value)
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
