using System.Windows.Forms;

namespace Power_LoRa.Interface.Controls
{
	public class ButtonControl : BaseControl
    {
        #region Constructors
        public ButtonControl(Control container, string name) : base(container, name)
		{
			Field = new Button
            {
                Dock = Field.Dock,
                Name = name.Replace(" ", "") + "Field",
				Text = name,
			};
        }
        #endregion
    }
}
