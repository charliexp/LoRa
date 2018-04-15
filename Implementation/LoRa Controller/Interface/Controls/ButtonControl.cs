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
				Name = name.Replace(" ", "") + "Field",
				Text = name,
			};
        }
        #endregion
    }
}
