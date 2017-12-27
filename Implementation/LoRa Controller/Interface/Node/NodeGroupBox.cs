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
			Bandwidth = new ParameterComboBox(Commands.Bandwidth);
			OutputPower = new ParameterSpinBox(Commands.OutputPower);
			SpreadingFactor = new ParameterSpinBox(Commands.SpreadingFactor);
			CodingRate = new ParameterComboBox(Commands.CodingRate);
			RxSymTimeout = new ParameterSpinBox(Commands.RxSymTimeout);
			RxMsTimeout = new ParameterSpinBox(Commands.RxMsTimeout);
			TxTimeout = new ParameterSpinBox(Commands.TxTimeout);
			PreambleSize = new ParameterSpinBox(Commands.PreambleSize);
			PayloadMaxSize = new ParameterSpinBox(Commands.PayloadMaxSize);
			VariablePayload = new ParameterCheckBox(Commands.VariablePayload);
			PerformCRC = new ParameterCheckBox(Commands.PerformCRC);

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

			((ComboBox)Bandwidth.field).Items.AddRange(new object[] { "125 kHz", "250 kHz", "500 kHz" });
			((ComboBox)Bandwidth.field).SelectedIndex = 0;

			((NumericUpDown)OutputPower.field).Maximum = new decimal(new int[] { 14, 0, 0, 0 });
			((NumericUpDown)OutputPower.field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)OutputPower.field).Value = new decimal(new int[] { 14, 0, 0, 0 });

			((NumericUpDown)SpreadingFactor.field).Maximum = new decimal(new int[] { 12, 0, 0, 0 });
			((NumericUpDown)SpreadingFactor.field).Minimum = new decimal(new int[] { 7, 0, 0, 0 });
			((NumericUpDown)SpreadingFactor.field).Value = new decimal(new int[] { 12, 0, 0, 0 });

			((ComboBox)CodingRate.field).Items.AddRange(new object[] { "4/5", "4/6", "4/7", "4/8" });
			((ComboBox)CodingRate.field).SelectedIndex = 3;

			((NumericUpDown)RxSymTimeout.field).Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			((NumericUpDown)RxSymTimeout.field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)RxSymTimeout.field).Value = new decimal(new int[] { 5, 0, 0, 0 });

			((NumericUpDown)RxMsTimeout.field).Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			((NumericUpDown)RxMsTimeout.field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)RxMsTimeout.field).Value = new decimal(new int[] { 3000, 0, 0, 0 });

			((NumericUpDown)TxTimeout.field).Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			((NumericUpDown)TxTimeout.field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)TxTimeout.field).Value = new decimal(new int[] { 2000, 0, 0, 0 });

			((NumericUpDown)PreambleSize.field).Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			((NumericUpDown)PreambleSize.field).Minimum = new decimal(new int[] { 2, 0, 0, 0 });
			((NumericUpDown)PreambleSize.field).Value = new decimal(new int[] { 8, 0, 0, 0 });

			((NumericUpDown)PayloadMaxSize.field).Maximum = new decimal(new int[] { 64, 0, 0, 0 });
			((NumericUpDown)PayloadMaxSize.field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)PayloadMaxSize.field).Value = new decimal(new int[] { 64, 0, 0, 0 });

			((CheckBox)VariablePayload.field).CheckState = CheckState.Checked;

			((CheckBox)PerformCRC.field).CheckState = CheckState.Checked;

			Enabled = false;
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
