using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class TextBoxControl : LabeledControl
    {
        #region Types
        public enum Type
		{
			Input,
			Output
        }
        #endregion

        #region Constructors
        public TextBoxControl(string name, Type type) : base(name)
		{
			Field = new TextBox
			{
				Margin = Field.Margin,
				Name = Field.Name,
				Size = Field.Size,
			};

			if (type == Type.Output)
			{
				((TextBox)Field).ReadOnly = true;
				Field.TabStop = false;
			}

			((TextBox)Field).BackColor = System.Drawing.Color.White;
        }
        #endregion
    }
}
