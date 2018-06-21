using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoRa_Controller.Connection.Messages;
using static LoRa_Controller.Connection.Messages.Message;

namespace LoRa_Controller.Networking
{
	public class Server : TcpListener
    {
        #region Private constants
        private const string _IPAddress = "127.0.0.1";
        private const int _port = 13000;
        #endregion

        #region Private variables
        private List<TcpClient> clients;
        #endregion

        #region Constructors
        public Server() : base(IPAddress.Parse(_IPAddress), _port)
		{
			clients = new List<TcpClient>();
		}
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
			base.Start();
			BeginAcceptTcpClient(new AsyncCallback(ClientConnected), this);
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
        //TODO: switch to frame
        public Frame Read()
        {/*
            byte[] receivedData = new byte[MaxSize];

            clients.RemoveAll(client => client.Connected == false);
            foreach (TcpClient client in clients)
                if (client.Available == MaxSize)
                    client.GetStream().Read(receivedData, 0, MaxSize);
                    
            return new Message(receivedData);*/
            return null;
        }
        public async Task<byte[]> ReadAsync()
        {/*
			byte[] data = new byte[MaxSize];
			clients.RemoveAll(client => client.Connected == false);

			foreach (TcpClient client in clients)
				if (client.Available == data.Length)
					await client.GetStream().ReadAsync(data, 0, data.Length);
                    
			return data;*/
            return null;
        }
		#endregion
	}
}
