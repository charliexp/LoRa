
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

		Task<bool> SendCharAsync(byte[] byteToSend);

		Task<bool> ReadCharAsync(byte[] receiveBuffer);
		#endregion
	}
}
