using Power_LoRa.Settings;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Power_LoRa.Connection
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
			//try
			{
				tcpClient.Connect(ipAddress, port);
				baseStream = tcpClient.GetStream();
			}
			//catch
			{

			}
		}
		public override void Close()
		{
			tcpClient.Close();
        }
        public override void WriteBytes(byte[] data)
        {
            baseStream.Write(data, 0, 1);
        }
        public async override Task WriteBytesAsync(byte[] data)
        {
            await baseStream.WriteAsync(data, 0, 1);
        }
        public override byte[] ReadBytes(int numberOfBytes)
        {
            byte[] receiveBuffer = new byte[numberOfBytes];
            baseStream.Read(receiveBuffer, 0, numberOfBytes);
            return receiveBuffer;
        }
        public async override Task<byte[]> ReadBytesAsync(int numberOfBytes)
        {
            byte[] receiveBuffer = new byte[numberOfBytes];
            await baseStream.ReadAsync(receiveBuffer, 0, numberOfBytes);
            return receiveBuffer;
        }
        #endregion
    }
}
