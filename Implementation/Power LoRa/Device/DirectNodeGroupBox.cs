using LoRa_Controller.Interface.Controls;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Device
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
