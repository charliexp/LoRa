namespace Power_LoRa.Node
{
	public class RadioNode : BaseNode
    {
        #region Properties
        public int RSSI { get; private set; }
        public int SNR { get; private set; }
        public bool Connected { get; set; }
        #endregion

        #region Constructors
        public RadioNode(byte address) : base()
		{
			Address = address;
			Connected = true;
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
