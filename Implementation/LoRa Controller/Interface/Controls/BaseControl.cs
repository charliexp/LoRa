using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public delegate Task ValueChanged(int index);

	public abstract class BaseControl
	{
		public Control field;

		public BaseControl(string name)
		{
			field = new Control
			{
				Margin = new Padding(InterfaceConstants.ItemPadding),
				Name = name.Replace(" ", "") + "Field",
				Size = new System.Drawing.Size(InterfaceConstants.InputWidth, InterfaceConstants.InputHeight)
			};
		}

		public abstract void Draw(int index);
	}
}
