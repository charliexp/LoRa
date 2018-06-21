using LoRa_Controller.Settings;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
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
				Handshake = Handshake.None,
                ReadTimeout = SerialPort.InfiniteTimeout,
			};
			PortName = portName;
		}
		#endregion

		#region Public methods
		public override void Open()
		{
			serialPort.Open();
			serialPort.DiscardInBuffer();
		}
		public override void Close()
		{
			serialPort.Close();
		}
        public override void WriteBytes(byte[] data)
        {
            serialPort.BaseStream.Write(data, 0, data.Length);
        }
        public async override Task WriteBytesAsync(byte[] data)
        {
            await serialPort.BaseStream.WriteAsync(data, 0, data.Length);
        }
        public override byte[] ReadBytes(int numberOfBytes)
        {
            byte[] receiveBuffer = new byte[numberOfBytes];
            serialPort.BaseStream.Read(receiveBuffer, 0, numberOfBytes);
            return receiveBuffer;
        }
        public async override Task<byte[]> ReadBytesAsync(int numberOfBytes)
        {
            byte[] receiveBuffer = new byte[numberOfBytes];
            await serialPort.BaseStream.ReadAsync(receiveBuffer, 0, numberOfBytes);
            return receiveBuffer;
        }
        #endregion
    }
}
