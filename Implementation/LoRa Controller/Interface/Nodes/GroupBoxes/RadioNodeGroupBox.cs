
using LoRa_Controller.Interface.Controls;
using System.Collections.Generic;
using System.Windows.Forms;

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

			List<BaseControl> newControls = new List<BaseControl>
			{
				Status,
				RSSI,
				SNR
			};

			RSSI.label.Width = RSSI.label.Width / 2 - 2 * InterfaceConstants.ItemPadding;
			RSSI.field.Width = RSSI.field.Width / 2;
			SNR.label.Width = SNR.label.Width / 2 - 2 * InterfaceConstants.ItemPadding;
			SNR.field.Width = SNR.field.Width / 2;
			newControls.AddRange(controls.GetRange(1, controls.Count - 1));
			controls = newControls;
		}
		
		public void UpdateRSSI(int value)
		{
			((TextBox)RSSI.field).Text = value.ToString();
		}

		public void UpdateSNR(int value)
		{
			((TextBox)SNR.field).Text = value.ToString();
		}

	}
}
