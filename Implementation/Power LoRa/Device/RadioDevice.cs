namespace Power_LoRa.Device
{
	public class RadioDevice : BaseDevice
    {
        #region Properties
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
