using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class ButtonControl : BaseControl
	{
		public ButtonControl(string name) : base(name)
		{
			field = new Button
			{
				Margin = new Padding(InterfaceConstants.ItemPadding),
				Size = new System.Drawing.Size(InterfaceConstants.InputWidth, InterfaceConstants.InputHeight),
				Name = name.Replace(" ", "") + "Field",
				Text = name,
				Width = InterfaceConstants.LabelWidth +
						InterfaceConstants.ButtonWidth +
						InterfaceConstants.ItemPadding,
			};
		}

		public override void Draw(int index)
		{
			field.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX,

													InterfaceConstants.GroupBoxFirstItemY +
													index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding));

			field.TabIndex = index;
		}
	}
}
