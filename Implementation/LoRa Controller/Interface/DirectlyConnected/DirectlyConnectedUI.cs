using LoRa_Controller.Device;
using System;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.DirectlyConnected
{
	public class DirectlyConnectedUI
	{
		public GroupBox GroupBox;
		public RadioParameterUI Bandwidth;
		public RadioParameterUI NodeType;
		public RadioParameterUI OutputPower;
		public RadioParameterUI SpreadingFactor;
		public RadioParameterUI CodingRate;
		public RadioParameterUI RxSymTimeout;
		public RadioParameterUI RxMsTimeout;
		public RadioParameterUI TxTimeout;
		public RadioParameterUI PreambleSize;
		public RadioParameterUI PayloadMaxSize;
		public RadioParameterUI VariablePayload;
		public RadioParameterUI PerformCRC;

		public DirectlyConnectedUI()
		{
			GroupBox = new GroupBox();
			NodeType = new RadioParameterUI(Commands.NodeType, GroupBox, new TextBox());
			Bandwidth = new RadioParameterUI(Commands.Bandwidth, GroupBox, new ComboBox());
			OutputPower = new RadioParameterUI(Commands.OutputPower, GroupBox, new NumericUpDown());
			SpreadingFactor = new RadioParameterUI(Commands.SpreadingFactor, GroupBox, new NumericUpDown());
			CodingRate = new RadioParameterUI(Commands.CodingRate, GroupBox, new ComboBox());
			RxSymTimeout = new RadioParameterUI(Commands.RxSymTimeout, GroupBox, new NumericUpDown());
			RxMsTimeout = new RadioParameterUI(Commands.RxMsTimeout, GroupBox, new NumericUpDown());
			TxTimeout = new RadioParameterUI(Commands.TxTimeout, GroupBox, new NumericUpDown());
			PreambleSize = new RadioParameterUI(Commands.PreambleSize, GroupBox, new NumericUpDown());
			PayloadMaxSize = new RadioParameterUI(Commands.PayloadMaxSize, GroupBox, new NumericUpDown());
			VariablePayload = new RadioParameterUI(Commands.VariablePayload, GroupBox, new CheckBox());
			PerformCRC = new RadioParameterUI(Commands.PerformCRC, GroupBox, new CheckBox());

			GroupBox.SuspendLayout();

			GroupBox.Controls.Add(NodeType.Label);
			GroupBox.Controls.Add(NodeType.Field);
			GroupBox.Controls.Add(Bandwidth.Label);
			GroupBox.Controls.Add(Bandwidth.Field);
			GroupBox.Controls.Add(OutputPower.Label);
			GroupBox.Controls.Add(OutputPower.Field);
			GroupBox.Controls.Add(CodingRate.Label);
			GroupBox.Controls.Add(CodingRate.Field);
			GroupBox.Controls.Add(RxSymTimeout.Label);
			GroupBox.Controls.Add(RxSymTimeout.Field);
			GroupBox.Controls.Add(RxMsTimeout.Label);
			GroupBox.Controls.Add(RxMsTimeout.Field);
			GroupBox.Controls.Add(TxTimeout.Label);
			GroupBox.Controls.Add(TxTimeout.Field);
			GroupBox.Controls.Add(PreambleSize.Label);
			GroupBox.Controls.Add(PreambleSize.Field);
			GroupBox.Controls.Add(PayloadMaxSize.Label);
			GroupBox.Controls.Add(PayloadMaxSize.Field);
			GroupBox.Controls.Add(VariablePayload.Field);
			GroupBox.Controls.Add(PerformCRC.Field);


			((TextBox)NodeType.Field).ReadOnly = true;
			NodeType.Field.TabStop = false;
			
			((ComboBox)Bandwidth.Field).Items.AddRange(new object[] { "125 kHz", "250 kHz", "500 kHz" });
			((ComboBox)Bandwidth.Field).SelectedIndex = 0;

			((NumericUpDown)OutputPower.Field).Maximum = new decimal(new int[] { 14, 0, 0, 0 });
			((NumericUpDown)OutputPower.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)OutputPower.Field).Value = new decimal(new int[] { 14, 0, 0, 0 });

			((NumericUpDown)SpreadingFactor.Field).Maximum = new decimal(new int[] { 12, 0, 0, 0 });
			((NumericUpDown)SpreadingFactor.Field).Minimum = new decimal(new int[] { 7, 0, 0, 0 });
			((NumericUpDown)SpreadingFactor.Field).Value = new decimal(new int[] { 12, 0, 0, 0 });

			((ComboBox)CodingRate.Field).Items.AddRange(new object[] { "4/5", "4/6", "4/7", "4/8" });
			((ComboBox)CodingRate.Field).SelectedIndex = 3;

			((NumericUpDown)RxSymTimeout.Field).Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			((NumericUpDown)RxSymTimeout.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)RxSymTimeout.Field).Value = new decimal(new int[] { 5, 0, 0, 0 });

			((NumericUpDown)RxMsTimeout.Field).Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			((NumericUpDown)RxMsTimeout.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)RxMsTimeout.Field).Value = new decimal(new int[] { 3000, 0, 0, 0 });

			((NumericUpDown)TxTimeout.Field).Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			((NumericUpDown)TxTimeout.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)TxTimeout.Field).Value = new decimal(new int[] { 1000, 0, 0, 0 });

			((NumericUpDown)PreambleSize.Field).Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			((NumericUpDown)PreambleSize.Field).Minimum = new decimal(new int[] { 2, 0, 0, 0 });
			((NumericUpDown)PreambleSize.Field).Value = new decimal(new int[] { 8, 0, 0, 0 });

			((NumericUpDown)PayloadMaxSize.Field).Maximum = new decimal(new int[] { 64, 0, 0, 0 });
			((NumericUpDown)PayloadMaxSize.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)PayloadMaxSize.Field).Value = new decimal(new int[] { 64, 0, 0, 0 });

			((CheckBox)VariablePayload.Field).CheckState = CheckState.Checked;

			((CheckBox)PerformCRC.Field).CheckState = CheckState.Checked;

			GroupBox.Location = new System.Drawing.Point(Constants.GroupBoxLocationX, Constants.GroupBoxLocationY);
			GroupBox.Margin = new Padding(Constants.ItemPadding);
			GroupBox.Name = "GroupBox";
			GroupBox.Padding = new Padding(Constants.ItemPadding);
			GroupBox.Size = new System.Drawing.Size(2 * Constants.LabelLocationX +
													4 * Constants.ItemPadding +
													Constants.LabelWidth +
													Constants.InputWidth,
													
													Constants.GroupBoxFirstItemY +
													RadioParameterUI.Count * Constants.InputHeight +
													(RadioParameterUI.Count - 1) * 2 * Constants.ItemPadding +
													Constants.LabelLocationY);
			GroupBox.TabStop = false;
			GroupBox.Text = "Connected device";
			

			GroupBox.ResumeLayout(false);
			GroupBox.PerformLayout();
		}
	}
}
