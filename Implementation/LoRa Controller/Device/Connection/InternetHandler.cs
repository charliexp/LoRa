using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
{
	class InternetHandler : TcpClient, IConnectionHandler
	{
		#region Constructors
		public InternetHandler()
		{

		}

		public InternetHandler(string IPAddress, int port)
		{
			_IPAddress = IPAddress;
			_port = port;
		}
		#endregion

		#region Private variables
		private string _IPAddress = "127.0.0.1";
		private int _port = 13000;
		private NetworkStream baseStream;
		#endregion

		#region Public properties
		public string IPAddress
		{
			get
			{
				return _IPAddress;
			}
			set
			{
				_IPAddress = value;
				SettingHandler.IPAddress.Value = _IPAddress;
			}
		}

		public int Port
		{
			get
			{
				return _port;
			}
			set
			{
				_port = value;
				SettingHandler.IPAddress.Value = _port;
			}
		}
		#endregion

		#region Public methods
		public void Open()
		{
			try
			{
				Connect(_IPAddress, _port);
				baseStream = GetStream();
			}
			catch
			{

			}
		}

		public void WriteByte(byte data)
		{
			baseStream.Write(new byte[] { data }, 0, 1);
		}

		public byte ReadByte()
		{
			byte[] receiveBuffer = new byte[1];
			baseStream.Read(receiveBuffer, 0, 1);
			return receiveBuffer[0];
		}

		public async Task SendByteAsync(byte data)
		{
			await baseStream.WriteAsync(new byte[] { data }, 0, 1);
		}

		public async Task<byte> ReadByteAsync()
		{
			byte[] receiveBuffer = new byte[1];
			await baseStream.ReadAsync(receiveBuffer, 0, 1);
			return receiveBuffer[0];
		}
		#endregion
	}
}
