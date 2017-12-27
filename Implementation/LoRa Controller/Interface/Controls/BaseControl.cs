using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public delegate Task ValueChangedAsync(int index);
	public delegate void ValueChanged(int index);

	public abstract class BaseControl
	{
		public Label label;
		public Control field;

		public BaseControl(string name)
		{
			label = new Label();
			field = new Control();

			label.AutoSize = true;
			label.Margin = new Padding(InterfaceConstants.ItemPadding, 0, InterfaceConstants.ItemPadding, 0);
			label.Name = name.Replace(" ", "") + "Label";
			label.Size = new System.Drawing.Size(InterfaceConstants.LabelWidth, InterfaceConstants.LabelHeight);

			string[] words = Regex.Matches(name.ToString(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
											.OfType<Match>()
											.Select(m => m.Value)
											.ToArray();
			label.Text = string.Join(" ", words);

			field.Margin = new Padding(InterfaceConstants.ItemPadding);
			field.Name = name.Replace(" ", "") + "Field";
			field.Size = new System.Drawing.Size(InterfaceConstants.InputWidth, InterfaceConstants.InputHeight);
		}

		public void Draw(int index)
		{
			label.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX,

													InterfaceConstants.GroupBoxFirstItemY +
													index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding) +
													InterfaceConstants.LabelToBoxOffset);
			field.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX +
													InterfaceConstants.LabelWidth +
													InterfaceConstants.ItemPadding,

													InterfaceConstants.GroupBoxFirstItemY +
													index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding));

			field.TabIndex = index;
		}
	}
}
