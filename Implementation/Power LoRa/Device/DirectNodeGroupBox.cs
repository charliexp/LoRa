using Power_LoRa.Interface.Controls;
using System.Windows.Forms;
using static Power_LoRa.Device.BaseDevice;

namespace Power_LoRa.Device
{
	public class DirectNodeGroupBox : BaseNodeGroupBox
    {
        #region Properties
		public ButtonControl SetAddress;
        #endregion

        #region Constructors
        public DirectNodeGroupBox(string name) : base(name)
        {
            SetAddress = new ButtonControl("Set Address");

            radioLayout.Controls.Add(SetAddress.Field);

            AddControlsToLayout();
        }
        #endregion
    }
}
