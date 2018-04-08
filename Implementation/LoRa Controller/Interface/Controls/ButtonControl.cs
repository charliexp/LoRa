using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class ButtonControl : BaseControl
    {
        #region Constructors
        public ButtonControl(string name) : base(name)
		{
			Field = new Button
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
        #endregion

        #region Public methods
        public override void Draw(int index)
		{
			Field.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX,

													InterfaceConstants.GroupBoxFirstItemY +
													index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding));

			Field.TabIndex = index;
        }
        #endregion
    }
}
