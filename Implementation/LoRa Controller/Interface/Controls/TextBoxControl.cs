using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class TextBoxControl : BaseControl
	{
		public enum Type
		{
			Input,
			Output
		}

		public TextBoxControl(string name, Type type) : base(name)
		{
			field = new TextBox
			{
				Margin = field.Margin,
				Name = field.Name,
				Size = field.Size,
			};

			if (type == Type.Output)
			{
				((TextBox)field).ReadOnly = true;
				field.TabStop = false;
			}

			((TextBox)field).BackColor = System.Drawing.Color.White;
		}
	}
}
