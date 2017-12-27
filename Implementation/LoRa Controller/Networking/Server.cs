using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoRa_Controller.Device;

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

		public void Send(string data)
		{
			data += "\n\r";
			_clients.RemoveAll(client => client.Connected == false);

			foreach (TcpClient client in _clients)
			{
				try
				{
					client.GetStream().Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
				}
				catch (System.IO.IOException)
				{
				}
			}
		}

		public byte[] Receive()
		{
			byte[] data = new byte[DeviceHandler.CommandMaxLength];
			data[0] = (byte)DeviceHandler.Commands.Invalid;
			_clients.RemoveAll(client => client.Connected == false);

			foreach (TcpClient client in _clients)
				if (client.Available == data.Length)
					client.GetStream().Read(data, 0, data.Length);

			return data;
		}

		public async Task SendAsync(string data)
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

		public async Task<byte[]> ReceiveAsync()
		{
			byte[] data = new byte[DeviceHandler.CommandMaxLength];
			data[0] = (byte) DeviceHandler.Commands.Invalid;
			_clients.RemoveAll(client => client.Connected == false);

			foreach (TcpClient client in _clients)
				if (client.Available == data.Length)
					await client.GetStream().ReadAsync(data, 0, data.Length);

			return data;
		}
		#endregion
	}
}
