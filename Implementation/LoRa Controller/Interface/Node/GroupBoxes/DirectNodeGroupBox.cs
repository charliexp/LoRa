using LoRa_Controller.Interface.Controls;
using System.Collections.Generic;

namespace LoRa_Controller.Interface.Node.GroupBoxes
{
	public class DirectNodeGroupBox : BaseNodeGroupBox
	{
		public TextBoxControl NodeType;

		public DirectNodeGroupBox(string name) : base(name)
		{
			NodeType = new TextBoxControl("NodeType", TextBoxControl.Type.Output);

			List<BaseControl> newControls = new List<BaseControl>
			{
				Status,
				NodeType
			};
			newControls.AddRange(controls.GetRange(1, controls.Count - 1));
			controls = newControls;
		}
	}
}
