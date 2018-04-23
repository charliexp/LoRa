
using LoRa_Controller.Interface.Controls;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Device
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

			//radioParameters.Add(RSSI);
            //radioParameters.Add(SNR);

            AddControlsToLayout();
        }
        #endregion

        #region Public methods
        public void Draw(int groupBoxIndex)
        {
            /*if (Address == (int)AddressType.Master)
                radioParameters.Clear();*/
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
