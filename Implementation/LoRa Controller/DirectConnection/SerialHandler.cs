using LoRa_Controller.Settings;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.DirectConnection
{
	class SerialHandler : BaseConnectionHandler
    {
        #region Private variables
        private SerialPort serialPort;
        #endregion

        #region Properties
        public override bool Connected
        {
            get { return serialPort.IsOpen; }
        }
        public string PortName
        {
            get
            {
                return serialPort.PortName;
            }
            set
            {
                serialPort.PortName = value;
                SettingHandler.COMPort.Value = serialPort.PortName;
            }
        }
        #endregion

        #region Constructors
        public SerialHandler(string portName)
		{
			serialPort = new SerialPort
			{
				BaudRate = 115200,
				Parity = Parity.None,
				DataBits = 8,
				StopBits = StopBits.One,
				Handshake = Handshake.None
			};
			PortName = portName;
		}
		#endregion

		#region Public methods
		public override void Open()
		{
			try
			{
				serialPort.Open();
				serialPort.DiscardInBuffer();
			}
			catch
			{
                //TODO: when does this fail?
			}
		}
		public override void Close()
		{
			serialPort.Close();
		}
		public override void WriteByte(byte data)
		{
			serialPort.BaseStream.Write(new byte[] { data }, 0, 1);
		}
		public async override Task WriteByteAsync(byte data)
		{
			await serialPort.BaseStream.WriteAsync(new byte[] { data }, 0, 1);
        }
        public override byte ReadByte()
        {
            byte[] receiveBuffer = new byte[1];
            serialPort.BaseStream.Read(receiveBuffer, 0, 1);
            return receiveBuffer[0];
        }
        public async override Task<byte> ReadByteAsync()
		{
			byte[] receiveBuffer = new byte[1];
			await serialPort.BaseStream.ReadAsync(receiveBuffer, 0, 1);
			return receiveBuffer[0];
		}
		#endregion
	}
}
