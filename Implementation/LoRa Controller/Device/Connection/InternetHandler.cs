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

		public InternetHandler(string IPAddress)
		{
			_IPAddress = IPAddress;
		}
		#endregion

		#region Private variables
		private string _IPAddress = "127.0.0.1";
		private int _port = 13000;
		#endregion

		#region Public properties
		public string IPAddress
		{
			get { return _IPAddress; }
			set { _IPAddress = value; }
		}

		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}

		public List<string> Parameters
		{
			set
			{
				_IPAddress = value[0];
				_port = Int32.Parse(value[1]);
				Settings.Save(Settings.IPAddress, _IPAddress);
				Settings.Save(Settings.TCPPort, _port.ToString());
			}
		}
		#endregion

		#region Public methods
		public void Open()
		{
			try
			{
				Connect(_IPAddress, _port);
			}
			catch
			{

			}
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
