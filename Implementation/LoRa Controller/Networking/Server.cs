using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoRa_Controller.Device;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;
using LoRa_Controller.DirectConnection;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.Networking
{
	public class Server : TcpListener
	{
		#region Constructors
		public Server() : base(IPAddress.Parse(_IPAddress), _port)
		{
			clients = new List<TcpClient>();
		}
		#endregion

		#region Private constants
		private const string _IPAddress = "127.0.0.1";
		private const int _port = 13000;
		#endregion

		#region Private variables
		private List<TcpClient> clients;
		#endregion

		#region Private methods
		private void ClientConnected(IAsyncResult ar)
		{
			clients.Add(EndAcceptTcpClient(ar));
			BeginAcceptTcpClient(new AsyncCallback(ClientConnected), this);
		}
		#endregion

		#region Public methods
		public new void Start()
		{
			try
			{
				base.Start();
				BeginAcceptTcpClient(new AsyncCallback(ClientConnected), this);
			}
			catch (SocketException)
			{
                //TODO: when does this fail?
			}
		}

		public void Write(string data)
		{
			data += "\n\r";
			clients.RemoveAll(client => client.Connected == false);

			foreach (TcpClient client in clients)
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

		public Message Read()
        {
            byte[] receivedData = new byte[MaxLength];
            
            clients.RemoveAll(client => client.Connected == false);
            foreach (TcpClient client in clients)
                if (client.Available == MaxLength)
                    client.GetStream().Read(receivedData, 0, MaxLength);

            return new Message(receivedData);
		}

		public async Task WriteAsync(string data)
		{
			data += "\n\r";
			clients.RemoveAll(client => client.Connected == false);

			foreach(TcpClient client in clients)
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

		public async Task<byte[]> ReadAsync()
		{
			byte[] data = new byte[MaxLength];
			data[0] = (byte) CommandType.Invalid;
			clients.RemoveAll(client => client.Connected == false);

			foreach (TcpClient client in clients)
				if (client.Available == data.Length)
					await client.GetStream().ReadAsync(data, 0, data.Length);

			return data;
		}
		#endregion
	}
}
