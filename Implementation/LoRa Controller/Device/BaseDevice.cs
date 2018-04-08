using LoRa_Controller.DirectConnection;

namespace LoRa_Controller.Device
{
    public abstract class BaseDevice
    {
        #region Public enums
        public enum NodeType
        {
            Unknown,
            Master,
            Beacon,
        }

        public enum AddressType
        {
            General = 0,
            Master = 1,
            Beacon = 2,
            PC = 0xff,
        }
        #endregion

        #region Public variables
        public NodeType nodeType;
        #endregion

        #region Public properties
        public int Address
        {
            get { return address; }
            set
            {
                address = value;
                if (address == (int)AddressType.Master)
                    nodeType = NodeType.Master;
                else if (address == (int)AddressType.General)
                    nodeType = NodeType.Unknown;
                else
                    nodeType = NodeType.Beacon;
            }
        }
        #endregion

        #region Private variables
        private int address;
        #endregion

        #region Constructors
        public BaseDevice()
        {
            nodeType = NodeType.Unknown;
        }
        #endregion
	}
}
