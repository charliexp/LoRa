using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public class RadioDevice : BaseDevice
    {
        #region Public properties
        public int RSSI { get; private set; }
        public int SNR { get; private set; }
        public bool Connected { get; set; }
        #endregion

        #region Constructors
        public RadioDevice(int address) : base()
		{
			Address = address;
			Connected = false;
		}

        #endregion

        #region Public methods
        public void UpdateSignalQuality(int rssi, int snr)
		{
			RSSI = rssi;
			SNR = snr;
		}
		#endregion
	}
}
