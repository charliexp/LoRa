
using LoRa_Controller.Interface.Controls;
using System.Windows.Forms;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.GroupBoxes
{
	public class RadioNodeGroupBox : BaseNodeGroupBox
	{
		public TextBoxControl RSSI;
		public TextBoxControl SNR;

		public RadioNodeGroupBox(string name) : base(name)
		{
			RSSI = new TextBoxControl("RSSI", TextBoxControl.Type.Output);
			SNR = new TextBoxControl("SNR", TextBoxControl.Type.Output);

			statusControls.Add(RSSI);
			statusControls.Add(SNR);

			RSSI.label.Width = RSSI.label.Width / 2 - 2 * InterfaceConstants.ItemPadding;
			RSSI.field.Width = RSSI.field.Width / 2;
			SNR.label.Width = SNR.label.Width / 2 - 2 * InterfaceConstants.ItemPadding;
			SNR.field.Width = SNR.field.Width / 2;
		}
		
		public void UpdateRSSI(int value)
		{
			((TextBox)RSSI.field).Text = value.ToString();
		}

		public void UpdateSNR(int value)
		{
			((TextBox)SNR.field).Text = value.ToString();
		}
		
		public new void Draw(int groupBoxIndex)
		{
			if (Address == Address_master)
				LoRaControls.Clear();

			base.Draw(groupBoxIndex);
		}
	}
}
