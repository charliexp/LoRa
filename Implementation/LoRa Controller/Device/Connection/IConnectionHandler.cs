
using System;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
{
	interface IConnectionHandler
	{
		#region Public properties
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
