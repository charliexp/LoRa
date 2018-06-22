using System.Windows.Forms;

namespace Power_LoRa.Interface.Controls
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

        #region Properties
        public string MeasureUnit { get; private set; }
        public string Value
        {
            get
            {
                return Field.Text;
            }
            set
            {
                Field.Text = value + " " + MeasureUnit;
            }
        }
        #endregion

        #region Constructors
        public TextBoxControl(Control container, string name, Type type) : base(container, name)
		{
            Field = new TextBox
            {
                Dock = Field.Dock,
				Margin = Field.Margin,
				Name = Field.Name,
                Size = Field.Size,
            };

			if (type == Type.Output)
			{
				((TextBox)Field).ReadOnly = true;
                ((TextBox)Field).TextAlign = HorizontalAlignment.Right;
                Field.TabStop = false;
			}

			((TextBox)Field).BackColor = System.Drawing.Color.White;
            MeasureUnit = null;
        }

        public TextBoxControl(Control container, string name, string measureUnit, Type type) : this(container, name, type)
        {
            MeasureUnit = measureUnit;
            ((TextBox)Field).TextAlign = HorizontalAlignment.Right;
        }
        #endregion
    }
}
