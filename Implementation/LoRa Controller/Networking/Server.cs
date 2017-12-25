using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.Networking
{
	public class Server : TcpListener
	{
		#region Constructors
		public Server() : base(IPAddress.Parse(_IPAddress), _port)
		{
			_clients = new List<TcpClient>();
		}
		#endregion

		#region Private constants
		private const string _IPAddress = "127.0.0.1";
		private const int _port = 13000;
		#endregion

		#region Private variables
		private List<TcpClient> _clients;
		#endregion

		#region Private methods
		private void ClientConnected(IAsyncResult ar)
		{
			_clients.Add(EndAcceptTcpClient(ar));
			BeginAcceptTcpClient(new AsyncCallback(ClientConnected), this);
		}
		#endregion

		#region Public methods
		public new void Start()
		{
			base.Start();
			BeginAcceptTcpClient(new AsyncCallback(ClientConnected), this);
		}

		public async Task Send(string data)
		{
			data += "\n\r";
			_clients.RemoveAll(client => client.Connected == false);

			foreach(TcpClient client in _clients)
			{
				try
				{
					await client.GetStream().WriteAsync(Encoding.ASCII.GetBytes(data), 0, data.Length);
				}
				catch (System.IO.IOException)
				{
				}
			}
		}
		#endregion
	}
}
