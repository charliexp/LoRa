using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public class RadioDevice : BaseDevice
	{
		#region Constructors
		public RadioDevice(int address) : base()
		{
			Address = address;
		}
		#endregion

		#region Private variables
		private int rssi;
		private int snr;
		private bool connected;
		#endregion

		#region Public properties
		public int RSSI
		{
			get { return rssi; }
		}

		public int SNR
		{
			get { return snr; }
		}

		public bool Connected
		{
			get { return connected; }
		}
		#endregion

		#region Public methods
		public void updateSignalQuality(string line)
		{
			String tempString = line.Remove(line.IndexOf(' '));

			if (tempString.Length != 0)
			{
				tempString = tempString.Substring(line.IndexOf('=') + 1);
				rssi = Int32.Parse(tempString);
			}
			tempString = line.Substring(line.LastIndexOf('=') + 1);
			snr = Int32.Parse(tempString);
		}
		#endregion
	}
}
