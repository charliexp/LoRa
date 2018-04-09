
using LoRa_Controller.Interface.Controls;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.GroupBoxes
{
	public class RadioNodeGroupBox : BaseNodeGroupBox
    {
        #region Properties
        public TextBoxControl RSSI;
		public TextBoxControl SNR;
        #endregion

        #region Constructors
        public RadioNodeGroupBox(string name) : base(name)
		{
			RSSI = new TextBoxControl("RSSI", TextBoxControl.Type.Output);
			SNR = new TextBoxControl("SNR", TextBoxControl.Type.Output);

			statusControls.Add(RSSI);
			statusControls.Add(SNR);

			RSSI.Label.Width = RSSI.Label.Width / 2 - 2 * InterfaceConstants.ItemPadding;
			RSSI.Field.Width = RSSI.Field.Width / 2;
			SNR.Label.Width = SNR.Label.Width / 2 - 2 * InterfaceConstants.ItemPadding;
			SNR.Field.Width = SNR.Field.Width / 2;
        }
        #endregion

        #region Public methods
        public new void Draw(int groupBoxIndex)
        {
            if (Address == (int)AddressType.Master)
                LoRaControls.Clear();

            base.Draw(groupBoxIndex);
        }
        public void UpdateRSSI(int value)
		{
			((TextBox)RSSI.Field).Text = value.ToString();
		}
        public void UpdateSNR(int value)
		{
			((TextBox)SNR.Field).Text = value.ToString();
        }
        #endregion
    }
}
