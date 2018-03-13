using LoRa_Controller.Interface.Controls;
using System.Collections.Generic;

namespace LoRa_Controller.Interface.Node.GroupBoxes
{
	public class DirectNodeGroupBox : BaseNodeGroupBox
	{
		public TextBoxControl NodeType;
		public ButtonControl CheckBeacons;
		public ButtonControl SetAddress;

		public DirectNodeGroupBox(string name) : base(name)
		{
			NodeType = new TextBoxControl("NodeType", TextBoxControl.Type.Output);
			CheckBeacons = new ButtonControl("Check Beacons");
			SetAddress = new ButtonControl("Set Address");
			
			statusControls.Add(NodeType);
			statusControls.Add(SetAddress);
			statusControls.Add(CheckBeacons);
		}
	}
}
