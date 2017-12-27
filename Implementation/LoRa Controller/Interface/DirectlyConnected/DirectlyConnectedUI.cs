using System;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.DirectlyConnected
{
	public class DirectlyConnectedUI
	{
		public GroupBox GroupBox;

		public Label NodeTypeLabel;
		public Label CodingRateLabel;
		public Label SpreadingFactorLabel;
		public Label OutputPowerLabel;
		public Label BandwidthLabel;
		public Label TxTimeoutLabel;
		public Label RxMsTimeoutLabel;
		public Label RxSymTimeoutLabel;
		public Label PayloadLabel;
		public Label PreambleLabel;

		public TextBox NodeTypeTextBox;
		public ComboBox BandwidthComboBox;
		public ComboBox CodingRateComboBox;
		public NumericUpDown SpreadingFactorNumericUpDown;
		public NumericUpDown OutputPowerNumericUpDown;
		public NumericUpDown TxTimeoutNumericUpDown;
		public NumericUpDown RxMsTimeoutNumericUpDown;
		public NumericUpDown RxSymTimeoutNumericUpDown;
		public NumericUpDown PayloadNumericUpDown;
		public NumericUpDown PreambleNumericUpDown;
		public CheckBox VariablePayloadCheckBox;
		public CheckBox CrcCheckBox;

		public DirectlyConnectedUI()
		{
			GroupBox = new GroupBox();

			BandwidthLabel = new Label();
			OutputPowerLabel = new Label();
			CodingRateLabel = new Label();
			SpreadingFactorLabel = new Label();
			PayloadLabel = new Label();
			PreambleLabel = new Label();
			TxTimeoutLabel = new Label();
			RxMsTimeoutLabel = new Label();
			RxSymTimeoutLabel = new Label();
			NodeTypeLabel = new Label();

			NodeTypeTextBox = new TextBox();
			RxMsTimeoutNumericUpDown = new NumericUpDown();
			TxTimeoutNumericUpDown = new NumericUpDown();
			RxSymTimeoutNumericUpDown = new NumericUpDown();
			CrcCheckBox = new CheckBox();
			VariablePayloadCheckBox = new CheckBox();
			PayloadNumericUpDown = new NumericUpDown();
			PreambleNumericUpDown = new NumericUpDown();
			CodingRateComboBox = new ComboBox();
			SpreadingFactorNumericUpDown = new NumericUpDown();
			OutputPowerNumericUpDown = new NumericUpDown();
			BandwidthComboBox = new ComboBox();

			GroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(RxMsTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(TxTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(RxSymTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(PayloadNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(PreambleNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(SpreadingFactorNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(OutputPowerNumericUpDown)).BeginInit();
			
			GroupBox.Controls.Add(NodeTypeLabel);
			GroupBox.Controls.Add(NodeTypeTextBox);
			GroupBox.Controls.Add(RxMsTimeoutNumericUpDown);
			GroupBox.Controls.Add(RxMsTimeoutLabel);
			GroupBox.Controls.Add(TxTimeoutNumericUpDown);
			GroupBox.Controls.Add(TxTimeoutLabel);
			GroupBox.Controls.Add(RxSymTimeoutNumericUpDown);
			GroupBox.Controls.Add(RxSymTimeoutLabel);
			GroupBox.Controls.Add(CrcCheckBox);
			GroupBox.Controls.Add(VariablePayloadCheckBox);
			GroupBox.Controls.Add(PayloadNumericUpDown);
			GroupBox.Controls.Add(PayloadLabel);
			GroupBox.Controls.Add(PreambleNumericUpDown);
			GroupBox.Controls.Add(PreambleLabel);
			GroupBox.Controls.Add(CodingRateLabel);
			GroupBox.Controls.Add(CodingRateComboBox);
			GroupBox.Controls.Add(SpreadingFactorNumericUpDown);
			GroupBox.Controls.Add(SpreadingFactorLabel);
			GroupBox.Controls.Add(OutputPowerNumericUpDown);
			GroupBox.Controls.Add(OutputPowerLabel);
			GroupBox.Controls.Add(BandwidthLabel);
			GroupBox.Controls.Add(BandwidthComboBox);
			
			GroupBox.Location = new System.Drawing.Point(13, 13);
			GroupBox.Margin = new Padding(4);
			GroupBox.Name = "GroupBox";
			GroupBox.Padding = new Padding(4);
			GroupBox.Size = new System.Drawing.Size(263, 435);
			GroupBox.TabStop = false;
			GroupBox.Text = "Connected device";
			
			NodeTypeTextBox.Location = new System.Drawing.Point(139, 23);
			NodeTypeTextBox.Margin = new Padding(4);
			NodeTypeTextBox.Name = "NodeTypeTextBox";
			NodeTypeTextBox.ReadOnly = true;
			NodeTypeTextBox.BackColor = System.Drawing.Color.White;
			NodeTypeTextBox.Size = new System.Drawing.Size(116, 22);
			NodeTypeTextBox.TabIndex = 13;

			NodeTypeLabel.AutoSize = true;
			NodeTypeLabel.Location = new System.Drawing.Point(8, 26);
			NodeTypeLabel.Margin = new Padding(4, 0, 4, 0);
			NodeTypeLabel.Name = "NodeTypeLabel";
			NodeTypeLabel.Size = new System.Drawing.Size(48, 17);
			NodeTypeLabel.TabIndex = 12;
			NodeTypeLabel.Text = "NodeType";
			// 
			// RxMsTimeoutNumericUpDown
			// 
			RxMsTimeoutNumericUpDown.Location = new System.Drawing.Point(140, 214);
			RxMsTimeoutNumericUpDown.Margin = new Padding(4);
			RxMsTimeoutNumericUpDown.Maximum = new decimal(new int[] {
			10000,
			0,
			0,
			0});
			RxMsTimeoutNumericUpDown.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			RxMsTimeoutNumericUpDown.Name = "RxMsTimeoutNumericUpDown";
			RxMsTimeoutNumericUpDown.Size = new System.Drawing.Size(117, 22);
			RxMsTimeoutNumericUpDown.TabIndex = 30;
			RxMsTimeoutNumericUpDown.Value = new decimal(new int[] {
			3000,
			0,
			0,
			0});
			RxMsTimeoutNumericUpDown.ValueChanged += new System.EventHandler(RxMsTimeoutNumericUpDown_ValueChanged);
			// 
			// RxMsTimeoutLabel
			// 
			RxMsTimeoutLabel.AutoSize = true;
			RxMsTimeoutLabel.Location = new System.Drawing.Point(8, 216);
			RxMsTimeoutLabel.Margin = new Padding(4, 0, 4, 0);
			RxMsTimeoutLabel.Name = "RxMsTimeoutLabel";
			RxMsTimeoutLabel.Size = new System.Drawing.Size(111, 17);
			RxMsTimeoutLabel.TabIndex = 29;
			RxMsTimeoutLabel.Text = "Rx Timeout (ms)";
			// 
			// TxTimeoutNumericUpDown
			// 
			TxTimeoutNumericUpDown.Location = new System.Drawing.Point(140, 244);
			TxTimeoutNumericUpDown.Margin = new Padding(4);
			TxTimeoutNumericUpDown.Maximum = new decimal(new int[] {
			10000,
			0,
			0,
			0});
			TxTimeoutNumericUpDown.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			TxTimeoutNumericUpDown.Name = "TxTimeoutNumericUpDown";
			TxTimeoutNumericUpDown.Size = new System.Drawing.Size(117, 22);
			TxTimeoutNumericUpDown.TabIndex = 28;
			TxTimeoutNumericUpDown.Value = new decimal(new int[] {
			1000,
			0,
			0,
			0});
			TxTimeoutNumericUpDown.ValueChanged += new System.EventHandler(TxTimeoutNumericUpDown_ValueChanged);
			// 
			// TxTimeoutLabel
			// 
			TxTimeoutLabel.AutoSize = true;
			TxTimeoutLabel.Location = new System.Drawing.Point(8, 246);
			TxTimeoutLabel.Margin = new Padding(4, 0, 4, 0);
			TxTimeoutLabel.Name = "TxTimeoutLabel";
			TxTimeoutLabel.Size = new System.Drawing.Size(110, 17);
			TxTimeoutLabel.TabIndex = 27;
			TxTimeoutLabel.Text = "Tx Timeout (ms)";
			// 
			// RxSymTimeoutNumericUpDown
			// 
			RxSymTimeoutNumericUpDown.Location = new System.Drawing.Point(139, 184);
			RxSymTimeoutNumericUpDown.Margin = new Padding(4);
			RxSymTimeoutNumericUpDown.Maximum = new decimal(new int[] {
			30,
			0,
			0,
			0});
			RxSymTimeoutNumericUpDown.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			RxSymTimeoutNumericUpDown.Name = "RxSymTimeoutNumericUpDown";
			RxSymTimeoutNumericUpDown.Size = new System.Drawing.Size(117, 22);
			RxSymTimeoutNumericUpDown.TabIndex = 26;
			RxSymTimeoutNumericUpDown.Value = new decimal(new int[] {
			5,
			0,
			0,
			0});
			RxSymTimeoutNumericUpDown.ValueChanged += new System.EventHandler(RxSymTimeoutNumericUpDown_ValueChanged);
			// 
			// RxSymTimeoutLabel
			// 
			RxSymTimeoutLabel.AutoSize = true;
			RxSymTimeoutLabel.Location = new System.Drawing.Point(8, 186);
			RxSymTimeoutLabel.Margin = new Padding(4, 0, 4, 0);
			RxSymTimeoutLabel.Name = "RxSymTimeoutLabel";
			RxSymTimeoutLabel.Size = new System.Drawing.Size(118, 17);
			RxSymTimeoutLabel.TabIndex = 25;
			RxSymTimeoutLabel.Text = "Rx Timeout (sym)";
			// 
			// CrcCheckBox
			// 
			CrcCheckBox.AutoSize = true;
			CrcCheckBox.Checked = true;
			CrcCheckBox.CheckState = CheckState.Checked;
			CrcCheckBox.Location = new System.Drawing.Point(8, 367);
			CrcCheckBox.Margin = new Padding(4);
			CrcCheckBox.Name = "CrcCheckBox";
			CrcCheckBox.Size = new System.Drawing.Size(112, 21);
			CrcCheckBox.TabIndex = 24;
			CrcCheckBox.Text = "Perform CRC";
			CrcCheckBox.UseVisualStyleBackColor = true;
			CrcCheckBox.CheckedChanged += new System.EventHandler(CrcCheckBox_CheckedChanged);
			// 
			// VariablePayloadCheckBox
			// 
			VariablePayloadCheckBox.AutoSize = true;
			VariablePayloadCheckBox.Checked = true;
			VariablePayloadCheckBox.CheckState = CheckState.Checked;
			VariablePayloadCheckBox.Location = new System.Drawing.Point(8, 338);
			VariablePayloadCheckBox.Margin = new Padding(4);
			VariablePayloadCheckBox.Name = "VariablePayloadCheckBox";
			VariablePayloadCheckBox.Size = new System.Drawing.Size(168, 21);
			VariablePayloadCheckBox.TabIndex = 23;
			VariablePayloadCheckBox.Text = "Variable Payload Size";
			VariablePayloadCheckBox.UseVisualStyleBackColor = true;
			VariablePayloadCheckBox.CheckedChanged += new System.EventHandler(VariablePayloadCheckBox_CheckedChanged);
			// 
			// PayloadNumericUpDown
			// 
			PayloadNumericUpDown.Location = new System.Drawing.Point(140, 308);
			PayloadNumericUpDown.Margin = new Padding(4);
			PayloadNumericUpDown.Maximum = new decimal(new int[] {
			64,
			0,
			0,
			0});
			PayloadNumericUpDown.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			PayloadNumericUpDown.Name = "PayloadNumericUpDown";
			PayloadNumericUpDown.Size = new System.Drawing.Size(117, 22);
			PayloadNumericUpDown.TabIndex = 22;
			PayloadNumericUpDown.Value = new decimal(new int[] {
			64,
			0,
			0,
			0});
			PayloadNumericUpDown.ValueChanged += new System.EventHandler(PayloadNumericUpDown_ValueChanged);
			// 
			// PayloadLabel
			// 
			PayloadLabel.AutoSize = true;
			PayloadLabel.Location = new System.Drawing.Point(8, 310);
			PayloadLabel.Margin = new Padding(4, 0, 4, 0);
			PayloadLabel.Name = "PayloadLabel";
			PayloadLabel.Size = new System.Drawing.Size(119, 17);
			PayloadLabel.TabIndex = 21;
			PayloadLabel.Text = "Payload Max Size";
			// 
			// PreambleNumericUpDown
			// 
			PreambleNumericUpDown.Location = new System.Drawing.Point(140, 276);
			PreambleNumericUpDown.Margin = new Padding(4);
			PreambleNumericUpDown.Maximum = new decimal(new int[] {
			30,
			0,
			0,
			0});
			PreambleNumericUpDown.Minimum = new decimal(new int[] {
			2,
			0,
			0,
			0});
			PreambleNumericUpDown.Name = "PreambleNumericUpDown";
			PreambleNumericUpDown.Size = new System.Drawing.Size(117, 22);
			PreambleNumericUpDown.TabIndex = 20;
			PreambleNumericUpDown.Value = new decimal(new int[] {
			8,
			0,
			0,
			0});
			PreambleNumericUpDown.ValueChanged += new System.EventHandler(PreambleNumericUpDown_ValueChanged);
			// 
			// PreambleLabel
			// 
			PreambleLabel.AutoSize = true;
			PreambleLabel.Location = new System.Drawing.Point(8, 278);
			PreambleLabel.Margin = new Padding(4, 0, 4, 0);
			PreambleLabel.Name = "PreambleLabel";
			PreambleLabel.Size = new System.Drawing.Size(99, 17);
			PreambleLabel.TabIndex = 19;
			PreambleLabel.Text = "Preamble Size";
			// 
			// CodingRateLabel
			// 
			CodingRateLabel.AutoSize = true;
			CodingRateLabel.Location = new System.Drawing.Point(8, 154);
			CodingRateLabel.Margin = new Padding(4, 0, 4, 0);
			CodingRateLabel.Name = "CodingRateLabel";
			CodingRateLabel.Size = new System.Drawing.Size(86, 17);
			CodingRateLabel.TabIndex = 17;
			CodingRateLabel.Text = "Coding Rate";
			// 
			// CodingRateComboBox
			// 
			CodingRateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			CodingRateComboBox.FormattingEnabled = true;
			CodingRateComboBox.Items.AddRange(new object[] {
			"4/5",
			"4/6",
			"4/7",
			"4/8"});
			CodingRateComboBox.Location = new System.Drawing.Point(139, 151);
			CodingRateComboBox.Margin = new Padding(4);
			CodingRateComboBox.Name = "CodingRateComboBox";
			CodingRateComboBox.Size = new System.Drawing.Size(116, 24);
			CodingRateComboBox.Sorted = true;
			CodingRateComboBox.TabIndex = 18;
			// 
			// SpreadingFactorNumericUpDown
			// 
			SpreadingFactorNumericUpDown.Location = new System.Drawing.Point(139, 119);
			SpreadingFactorNumericUpDown.Margin = new Padding(4);
			SpreadingFactorNumericUpDown.Maximum = new decimal(new int[] {
			12,
			0,
			0,
			0});
			SpreadingFactorNumericUpDown.Minimum = new decimal(new int[] {
			7,
			0,
			0,
			0});
			SpreadingFactorNumericUpDown.Name = "SpreadingFactorNumericUpDown";
			SpreadingFactorNumericUpDown.Size = new System.Drawing.Size(117, 22);
			SpreadingFactorNumericUpDown.TabIndex = 16;
			SpreadingFactorNumericUpDown.Value = new decimal(new int[] {
			12,
			0,
			0,
			0});
			SpreadingFactorNumericUpDown.ValueChanged += new System.EventHandler(SpreadingFactorNumericUpDown_ValueChanged);
			// 
			// SpreadingFactorLabel
			// 
			SpreadingFactorLabel.AutoSize = true;
			SpreadingFactorLabel.Location = new System.Drawing.Point(8, 121);
			SpreadingFactorLabel.Margin = new Padding(4, 0, 4, 0);
			SpreadingFactorLabel.Name = "SpreadingFactorLabel";
			SpreadingFactorLabel.Size = new System.Drawing.Size(117, 17);
			SpreadingFactorLabel.TabIndex = 15;
			SpreadingFactorLabel.Text = "Spreading Factor";
			// 
			// OutputPowerNumericUpDown
			// 
			OutputPowerNumericUpDown.Location = new System.Drawing.Point(138, 85);
			OutputPowerNumericUpDown.Margin = new Padding(4);
			OutputPowerNumericUpDown.Maximum = new decimal(new int[] {
			14,
			0,
			0,
			0});
			OutputPowerNumericUpDown.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			OutputPowerNumericUpDown.Name = "OutputPowerNumericUpDown";
			OutputPowerNumericUpDown.Size = new System.Drawing.Size(117, 22);
			OutputPowerNumericUpDown.TabIndex = 14;
			OutputPowerNumericUpDown.Value = new decimal(new int[] {
			14,
			0,
			0,
			0});
			OutputPowerNumericUpDown.ValueChanged += new System.EventHandler(OutputPowerNumericUpDown_ValueChanged);
			// 
			// OutputPowerLabel
			// 
			OutputPowerLabel.AutoSize = true;
			OutputPowerLabel.Location = new System.Drawing.Point(8, 87);
			OutputPowerLabel.Margin = new Padding(4, 0, 4, 0);
			OutputPowerLabel.Name = "OutputPowerLabel";
			OutputPowerLabel.Size = new System.Drawing.Size(94, 17);
			OutputPowerLabel.TabIndex = 13;
			OutputPowerLabel.Text = "Output Power";
			// 
			// BandwidthLabel
			// 
			BandwidthLabel.AutoSize = true;
			BandwidthLabel.Location = new System.Drawing.Point(8, 57);
			BandwidthLabel.Margin = new Padding(4, 0, 4, 0);
			BandwidthLabel.Name = "BandwidthLabel";
			BandwidthLabel.Size = new System.Drawing.Size(73, 17);
			BandwidthLabel.TabIndex = 10;
			BandwidthLabel.Text = "Bandwidth";
			// 
			// BandwidthComboBox
			// 
			BandwidthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			BandwidthComboBox.FormattingEnabled = true;
			BandwidthComboBox.Items.AddRange(new object[] {
			"125 kHz",
			"250 kHz",
			"500 kHz"});
			BandwidthComboBox.Location = new System.Drawing.Point(139, 53);
			BandwidthComboBox.Margin = new Padding(4);
			BandwidthComboBox.Name = "BandwidthComboBox";
			BandwidthComboBox.Size = new System.Drawing.Size(116, 24);
			BandwidthComboBox.Sorted = true;
			BandwidthComboBox.TabIndex = 12;
			
			BandwidthComboBox.SelectedIndex = 0;
			CodingRateComboBox.SelectedIndex = 3;
			BandwidthComboBox.SelectedIndexChanged += new EventHandler(BandwidthComboBox_SelectedIndexChanged);
			CodingRateComboBox.SelectedIndexChanged += new EventHandler(CodingRateComboBox_SelectedIndexChanged);


			GroupBox.ResumeLayout(false);
			GroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(RxMsTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(TxTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(RxSymTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(PayloadNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(PreambleNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(SpreadingFactorNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(OutputPowerNumericUpDown)).EndInit();
		}

		private async void BandwidthComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.Bandwidth, (byte)((ComboBox)sender).SelectedIndex);
		}

		private async void CodingRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.CodingRate, (byte)(((ComboBox)sender).SelectedIndex + 1));
		}

		private async void OutputPowerNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.OutputPower, (byte)(((NumericUpDown)sender).Value));
		}

		private async void SpreadingFactorNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.SpreadingFactor, (byte)(((NumericUpDown)sender).Value));
		}

		private async void RxSymTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.RxSymTimeout, (byte)(((NumericUpDown)sender).Value));
		}

		private async void RxMsTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.RxMsTimeout, (int)(((NumericUpDown)sender).Value));
		}
		private async void TxTimeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.TxTimeout, (int)(((NumericUpDown)sender).Value));
		}

		private async void PreambleNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.PreambleSize, (byte)(((NumericUpDown)sender).Value));
		}

		private async void PayloadNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.PayloadMaxSize, (byte)(((NumericUpDown)sender).Value));
		}

		private async void VariablePayloadCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.VariablePayload, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}

		private async void CrcCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			await Program.DeviceHandler.SendCommandAsync(Commands.PerformCRC, (byte)(((CheckBox)sender).Checked ? 1 : 0));
		}
	}
}
