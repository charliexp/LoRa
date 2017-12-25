using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
{
	class InternetHandler : TcpClient, IConnectionHandler
	{
		#region Private constants
		private const string _IPAddress = "127.0.0.1";
		private const int _port = 13000;
		#endregion
		
		#region Public methods
		public void Open()
		{
			Connect(_IPAddress, _port);
		}

		public async Task<bool> SendCharAsync(byte[] byteToSend)
		{
			try
			{
				await GetStream().WriteAsync(byteToSend, 0, 1);
				return true;
			}
			catch (System.IO.IOException)
			{
				return false;
			}
		}

		public async Task<bool> ReadCharAsync(byte[] receiveBuffer)
		{
			try
			{
				await GetStream().ReadAsync(receiveBuffer, 0, 1);
				return true;
			}
			catch (System.IO.IOException)
			{
				return false;
			}
		}
		#endregion
	}
}
