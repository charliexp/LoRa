namespace Power_LoRa.Device
{
    public abstract class BaseDevice
    {
        #region Types
        public enum NodeType
        {
            Gateway = 0,
            EndDevice,
            Unknown,
        }
        public enum AddressType
        {
            General = 0,
            Master = 1,
            Beacon = 2,
            PC = 0xff,
        }
        #endregion

        #region Private variables
        private byte address;
        #endregion

        #region Properties
        public byte Address
        {
            get { return address; }
            set
            {
                address = value;
                if (address == (int)AddressType.Master)
                    Type = NodeType.Gateway;
                else if (address == (int)AddressType.General)
                    Type = NodeType.Unknown;
                else
                    Type = NodeType.EndDevice;
            }
        }
        public NodeType Type { get; private set; }
        #endregion

        #region Constructors
        public BaseDevice()
        {
            Type = NodeType.Unknown;
        }
        #endregion
	}
}
