using System.Drawing;
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
				Size = new Size(InterfaceConstants.LabelWidth +
                                InterfaceConstants.ButtonWidth +
                                InterfaceConstants.ItemPadding,
                                
                                InterfaceConstants.InputHeight),
				Name = name.Replace(" ", "") + "Field",
				Text = name,
			};
        }
        #endregion

        #region Public methods
        public override void Draw(int index)
		{
			Field.Location = new Point( InterfaceConstants.LabelLocationX,

										InterfaceConstants.GroupBoxFirstItemY +
										index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding));

			Field.TabIndex = index;
        }
        #endregion
    }
}
