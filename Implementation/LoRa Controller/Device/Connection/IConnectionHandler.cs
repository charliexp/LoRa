
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
{
	public interface IConnectionHandler
	{
		#region Properties
		bool Connected
		{
			get;
		}
		#endregion

		#region Methods
		void Open();

		void Close();

		void SendChar(byte[] byteToSend);

		void ReadChar(byte[] receiveBuffer);

		Task SendCharAsync(byte[] byteToSend);

		Task ReadCharAsync(byte[] receiveBuffer);
		#endregion
	}
}
