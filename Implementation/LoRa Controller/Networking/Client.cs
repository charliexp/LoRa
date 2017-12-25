using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using LoRa_Controller.Networking;
using LoRa_Controller.Connection;
using System.Threading.Tasks;

public class Client : TcpClient
{
	#region Constructors
	public Client()
	{
	}
	#endregion
	
	#region Private constants
	private const int port = 11000;
	#endregion
	
	public void Open()
	{
		Connect("localhost", port);
	}
	
	public Task<bool> SendCharAsync(byte[] byteToSend)
	{
		throw new NotImplementedException();
	}

	public Task<bool> ReadCharAsync(byte[] receiveBuffer)
	{
		throw new NotImplementedException();
	}
}