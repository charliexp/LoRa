
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

		public new void Draw(int groupBoxIndex)
		{
			int controlIndex = 0;
			
			SuspendLayout();

			Status.Draw(controlIndex++);
			Controls.Add(Status.label);
			Controls.Add(Status.field);

			RSSI.Draw(controlIndex);
			Controls.Add(RSSI.label);
			Controls.Add(RSSI.field);

			SNR.Draw(controlIndex++);
			SNR.label.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX +
				InterfaceConstants.LabelWidth / 2 +
				InterfaceConstants.InputWidth / 2 +
				InterfaceConstants.ItemPadding,
				SNR.label.Location.Y);
			SNR.field.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX +
				InterfaceConstants.LabelWidth / 2 +
				InterfaceConstants.InputWidth / 2 +
				InterfaceConstants.LabelWidth / 2 +
				InterfaceConstants.ItemPadding,
				SNR.field.Location.Y);
			Controls.Add(SNR.label);
			Controls.Add(SNR.field);

			foreach (BaseControl control in controls.GetRange(3, controls.Count - 3))
			{
				control.Draw(controlIndex++);
				Controls.Add(control.label);
				Controls.Add(control.field);
			}

			Width = 2 * InterfaceConstants.LabelLocationX +
				InterfaceConstants.LabelWidth +
				InterfaceConstants.InputWidth +
				InterfaceConstants.ItemPadding;
			Height = InterfaceConstants.GroupBoxFirstItemY +
				(Controls.Count / 2 - 1) * InterfaceConstants.InputHeight +
				((Controls.Count / 2) - 2) * InterfaceConstants.ItemPadding +
				InterfaceConstants.GroupBoxLastItemY;

			Location = new System.Drawing.Point(InterfaceConstants.GroupBoxLocationX +
				groupBoxIndex * (Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);

			ResumeLayout(true);
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
