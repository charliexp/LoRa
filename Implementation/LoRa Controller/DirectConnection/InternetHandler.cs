using LoRa_Controller.Settings;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoRa_Controller.DirectConnection
{
	class InternetHandler : BaseConnectionHandler
    {
        #region Private variables
        private TcpClient tcpClient;
        private string ipAddress = "127.0.0.1";
        private int port = 13000;
        private NetworkStream baseStream;
        #endregion

        #region Properties
        public string IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
                SettingHandler.IPAddress.Value = ipAddress;
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                SettingHandler.IPAddress.Value = port;
            }
        }
        public override bool Connected
        {
            get
            {
                return tcpClient.Connected;
            }
        }
        #endregion

        #region Constructors
        public InternetHandler(string IPAddress, int port)
		{
			ipAddress = IPAddress;
			this.port = port;
            tcpClient = new TcpClient();
        }
		#endregion

		#region Public methods
		public override void Open()
		{
			try
			{
				tcpClient.Connect(ipAddress, port);
				baseStream = tcpClient.GetStream();
			}
			catch
			{

			}
		}
		public override void Close()
		{
			tcpClient.Close();
		}
		public override void WriteByte(byte data)
		{
			baseStream.Write(new byte[] { data }, 0, 1);
		}
		public async override Task WriteByteAsync(byte data)
		{
			await baseStream.WriteAsync(new byte[] { data }, 0, 1);
        }
        public override byte ReadByte()
        {
            byte[] receiveBuffer = new byte[1];
            baseStream.Read(receiveBuffer, 0, 1);
            return receiveBuffer[0];
        }
        public async override Task<byte> ReadByteAsync()
		{
			byte[] receiveBuffer = new byte[1];
			await baseStream.ReadAsync(receiveBuffer, 0, 1);
			return receiveBuffer[0];
		}
		#endregion
	}
}
