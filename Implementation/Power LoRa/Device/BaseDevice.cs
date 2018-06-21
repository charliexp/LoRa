using Power_LoRa.Interface.Nodes;
using System;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Message;

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
        public BaseNodeGroupBox GroupBox { get; private set; }
        public byte Address
        {
            get { return address; }
            set
            {
                address = value;
                GroupBox.Address = value;
                //TODO remove
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
            GroupBox = new BaseNodeGroupBox(SetNewAddress, "Directly Connected Node");
        }
        #endregion

        #region Public methods
        public void SetNewAddress(object sender, EventArgs e)
        {
            BaseNodeGroupBox parentGroupBox = (BaseNodeGroupBox)((Button)sender).Parent.Parent.Parent.Parent;
            parentGroupBox.Address = parentGroupBox.NewAddress;
            Address = parentGroupBox.Address;
            Program.Write(new Connection.Messages.Message(CommandType.SetAddress, parentGroupBox.Address));
        }
        #endregion
    }
}
