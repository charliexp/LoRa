using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.ParameterControls;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.Node
{
	public class NodeGroupBox : GroupBox
	{
		public TextBoxControl NodeType;
		public TextBoxControl Status;
		public ParameterComboBox Bandwidth;
		public ParameterSpinBox OutputPower;
		public ParameterSpinBox SpreadingFactor;
		public ParameterComboBox CodingRate;
		public ParameterSpinBox RxSymTimeout;
		public ParameterSpinBox RxMsTimeout;
		public ParameterSpinBox TxTimeout;
		public ParameterSpinBox PreambleSize;
		public ParameterSpinBox PayloadMaxSize;
		public ParameterCheckBox VariablePayload;
		public ParameterCheckBox PerformCRC;

		public List<BaseControl> controls;
		
		public NodeGroupBox(string name) : base()
		{
			NodeType = new TextBoxControl("NodeType", TextBoxControl.Type.Output);
			Status = new TextBoxControl("Status", TextBoxControl.Type.Output);
			Bandwidth = new ParameterComboBox(Commands.Bandwidth, new List<string> { "125 kHz", "250 kHz", "500 kHz" }, 0);
			OutputPower = new ParameterSpinBox(Commands.OutputPower, 1, 14, 14);
			SpreadingFactor = new ParameterSpinBox(Commands.SpreadingFactor, 7, 12, 12);
			CodingRate = new ParameterComboBox(Commands.CodingRate, new List<string> { "4/5", "4/6", "4/7", "4/8" }, 3);
			RxSymTimeout = new ParameterSpinBox(Commands.RxSymTimeout, 1, 30, 5);
			RxMsTimeout = new ParameterSpinBox(Commands.RxMsTimeout, 1, 10000, 3000);
			TxTimeout = new ParameterSpinBox(Commands.TxTimeout, 1, 10000, 2000);
			PreambleSize = new ParameterSpinBox(Commands.PreambleSize, 2, 30, 8);
			PayloadMaxSize = new ParameterSpinBox(Commands.PayloadMaxSize, 1, 64, 64);
			VariablePayload = new ParameterCheckBox(Commands.VariablePayload, true);
			PerformCRC = new ParameterCheckBox(Commands.PerformCRC, true);

			controls = new List<BaseControl>
			{
				NodeType,
				Status,
				Bandwidth,
				OutputPower,
				SpreadingFactor,
				CodingRate,
				RxSymTimeout,
				RxMsTimeout,
				TxTimeout,
				PreambleSize,
				PayloadMaxSize,
				VariablePayload,
				PerformCRC
			};
			
			Name = name.Replace(" ", "") + "GroupBox";
			Text = name;
			Margin = new Padding(InterfaceConstants.ItemPadding);
			Padding = new Padding(InterfaceConstants.ItemPadding);
			TabStop = false;
		}
		
		public void Draw(int groupBoxIndex)
		{
			int controlIndex = 0;

			Enabled = true;

			SuspendLayout();
			
			foreach (BaseControl control in controls)
			{
				control.Draw(controlIndex++);
				Controls.Add(control.label);
				Controls.Add(control.field);
			}

			Width = 2 * InterfaceConstants.LabelLocationX +
				InterfaceConstants.LabelWidth +
				InterfaceConstants.InputWidth +
				InterfaceConstants.ItemPadding;
			Height = InterfaceConstants.GroupBoxFirstItemY +
				(Controls.Count / 2) * InterfaceConstants.InputHeight +
				((Controls.Count / 2) - 1) * InterfaceConstants.ItemPadding +
				InterfaceConstants.GroupBoxLastItemY;

			Location = new System.Drawing.Point(InterfaceConstants.GroupBoxLocationX +
				groupBoxIndex * (Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);

			ResumeLayout(true);
		}
	}
}
