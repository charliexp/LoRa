using System.Windows.Forms;

namespace Power_LoRa.Interface.Controls
{
	public class ButtonControl : BaseControl
    {
        #region Constructors
        public ButtonControl(string name) : base(name)
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
