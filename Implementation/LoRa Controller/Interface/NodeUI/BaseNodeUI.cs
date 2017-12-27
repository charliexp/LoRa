using System.Windows.Forms;

namespace LoRa_Controller.Interface.NodeUI
{
	public abstract class BaseNodeUI
	{
		public GroupBox groupBox;
		public RadioSettingsUI radioParameters;

		protected const int groupBoxWidth = 2 * Constants.LabelLocationX +
											4 * Constants.ItemPadding +
											Constants.LabelWidth +
											Constants.InputWidth;

		public BaseNodeUI()
		{
			groupBox = new GroupBox();
			radioParameters = new RadioSettingsUI();
		}

		public abstract void Draw();
	}
}
