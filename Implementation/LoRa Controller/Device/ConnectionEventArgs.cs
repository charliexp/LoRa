using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public class ConnectionEventArgs : EventArgs
	{
		public bool Connected { get; set; }
		public bool DisconnectedOnPurpose { get; set; }
	}
}
