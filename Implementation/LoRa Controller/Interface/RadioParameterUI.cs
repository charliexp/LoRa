using System.Windows.Forms;

namespace LoRa_Controller.Interface
{
	public class RadioParameterUI
	{
		public string Name;
		public Label Label;
		public Control Input;

		public RadioParameterUI(string name, GroupBox parent, Control control)
		{
			Name = name;
			Label = new Label();
			Input = control;

			Label.Parent = parent;
			Label.AutoSize = true;
			Label.Location = new System.Drawing.Point(Constants.LabelLocationX,
																Constants.GroupBoxFirstItemY +
																1 * (Constants.InputHeight + 2 * Constants.ItemPadding) +
																Constants.LabelToBoxOffset);
			Label.Margin = new Padding(Constants.ItemPadding, 0, Constants.ItemPadding, 0);
			Label.Name = Name + "Label";
			Label.Size = new System.Drawing.Size(Constants.LabelWidth, Constants.LabelHeight);
			Label.Text = Name;

			Input.Parent = parent;
			Input.Location = new System.Drawing.Point(Constants.LabelLocationX +
																Constants.LabelWidth +
																2 * Constants.ItemPadding,

																Constants.GroupBoxFirstItemY +
																1 * (Constants.InputHeight + Constants.ItemPadding) +
																Constants.LabelToBoxOffset);
			Input.Margin = new Padding(Constants.ItemPadding);
			Input.Name = Name + "Input";
			Input.Size = new System.Drawing.Size(Constants.InputWidth, Constants.InputHeight);

			if (control is ComboBox)
			{
				((ComboBox) Input).DropDownStyle = ComboBoxStyle.DropDownList;
				((ComboBox)Input).FormattingEnabled = true;
				((ComboBox)Input).Sorted = true;
			}
		}
	}
}
