using LoRa_Controller.Settings;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
{
	class SerialHandler : SerialPort , IConnectionHandler
	{
		#region Constructors
		public SerialHandler(string portName)
		{
			BaudRate = 115200;
			Parity = Parity.None;
			DataBits = 8;
			StopBits = StopBits.One;
			Handshake = Handshake.None;
			PortName = portName;
		}
		#endregion
		
		#region Public properties
		public bool Connected
		{
			get { return IsOpen; }
		}

		public new string PortName
		{
			get
			{
				return base.PortName;
			}
			set
			{
				base.PortName = value;
				SettingHandler.COMPort.Value = base.PortName;
			}
		}
		#endregion

		#region Public methods
		public new void Open()
		{
			try
			{
				base.Open();
				DiscardInBuffer();
			}
			catch
			{
			}
		}

		public new void Close()
		{
			base.Close();
		}

		public void WriteByte(byte data)
		{
			BaseStream.Write(new byte[] { data }, 0, 1);
		}

		public byte ReadByte()
		{
			byte[] receiveBuffer = new byte[1];
			BaseStream.Read(receiveBuffer, 0, 1);
			return receiveBuffer[0];
		}

		public async Task SendByteAsync(byte data)
		{
			await BaseStream.WriteAsync(new byte[] { data }, 0, 1);
		}

		public async Task<byte> ReadByteAsync()
		{
			byte[] receiveBuffer = new byte[1];
			await BaseStream.ReadAsync(receiveBuffer, 0, 1);
			return receiveBuffer[0];
		}
		#endregion
	}
}
