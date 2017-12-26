namespace LoRa_Controller.Interface
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.logListBox = new System.Windows.Forms.ListBox();
			this.logGroupBox = new System.Windows.Forms.GroupBox();
			this.changeLogFolderButton = new System.Windows.Forms.Button();
			this.logFolderTextBox = new System.Windows.Forms.TextBox();
			this.logFolderLabel = new System.Windows.Forms.Label();
			this.currentErrorsTextBox = new System.Windows.Forms.TextBox();
			this.currentErrorsLabel = new System.Windows.Forms.Label();
			this.snrTextBox = new System.Windows.Forms.TextBox();
			this.snrLabel = new System.Windows.Forms.Label();
			this.rssiTextBox = new System.Windows.Forms.TextBox();
			this.rssiLabel = new System.Windows.Forms.Label();
			this.radioStatusTextBox = new System.Windows.Forms.TextBox();
			this.radioStatusLabel = new System.Windows.Forms.Label();
			this.totalErrorsTextBox = new System.Windows.Forms.TextBox();
			this.totalErrorsLabel = new System.Windows.Forms.Label();
			this.radioConnectionGroupBox = new System.Windows.Forms.GroupBox();
			this.beaconSettingsGroupBox = new System.Windows.Forms.GroupBox();
			this.beaconRxMsTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconRxMsTimeoutLabel = new System.Windows.Forms.Label();
			this.beaconTxTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconTxTimeoutLabel = new System.Windows.Forms.Label();
			this.beaconRxSymTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconRxSymTimeoutLabel = new System.Windows.Forms.Label();
			this.beaconCrcCheckBox = new System.Windows.Forms.CheckBox();
			this.beaconVariablePayloadCheckBox = new System.Windows.Forms.CheckBox();
			this.beaconPayloadNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconPayloadLabel = new System.Windows.Forms.Label();
			this.beaconPreambleNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconPreambleLabel = new System.Windows.Forms.Label();
			this.beaconCodingRateLabel = new System.Windows.Forms.Label();
			this.beaconCodingRateComboBox = new System.Windows.Forms.ComboBox();
			this.beaconSpreadingFactorNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconSpreadingFactorLabel = new System.Windows.Forms.Label();
			this.beaconOutputPowerNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beaconOutputPowerLabel = new System.Windows.Forms.Label();
			this.beaconBandwidthLabel = new System.Windows.Forms.Label();
			this.beaconBandwidthComboBox = new System.Windows.Forms.ComboBox();
			this.logGroupBox.SuspendLayout();
			this.radioConnectionGroupBox.SuspendLayout();
			this.beaconSettingsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.beaconRxMsTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconTxTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconRxSymTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconPayloadNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconPreambleNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconSpreadingFactorNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconOutputPowerNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// logListBox
			// 
			this.logListBox.FormattingEnabled = true;
			this.logListBox.ItemHeight = 16;
			this.logListBox.Location = new System.Drawing.Point(8, 91);
			this.logListBox.Margin = new System.Windows.Forms.Padding(4);
			this.logListBox.Name = "logListBox";
			this.logListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.logListBox.Size = new System.Drawing.Size(239, 196);
			this.logListBox.TabIndex = 0;
			// 
			// logGroupBox
			// 
			this.logGroupBox.Controls.Add(this.changeLogFolderButton);
			this.logGroupBox.Controls.Add(this.logFolderTextBox);
			this.logGroupBox.Controls.Add(this.logListBox);
			this.logGroupBox.Controls.Add(this.logFolderLabel);
			this.logGroupBox.Enabled = false;
			this.logGroupBox.Location = new System.Drawing.Point(339, 412);
			this.logGroupBox.Margin = new System.Windows.Forms.Padding(4);
			this.logGroupBox.Name = "logGroupBox";
			this.logGroupBox.Padding = new System.Windows.Forms.Padding(4);
			this.logGroupBox.Size = new System.Drawing.Size(259, 299);
			this.logGroupBox.TabIndex = 4;
			this.logGroupBox.TabStop = false;
			this.logGroupBox.Text = "Log";
			// 
			// changeLogFolderButton
			// 
			this.changeLogFolderButton.Location = new System.Drawing.Point(147, 19);
			this.changeLogFolderButton.Margin = new System.Windows.Forms.Padding(4);
			this.changeLogFolderButton.Name = "changeLogFolderButton";
			this.changeLogFolderButton.Size = new System.Drawing.Size(100, 28);
			this.changeLogFolderButton.TabIndex = 14;
			this.changeLogFolderButton.Text = "Change";
			this.changeLogFolderButton.UseVisualStyleBackColor = true;
			this.changeLogFolderButton.Click += new System.EventHandler(this.ChangeLogFolderButton_Click);
			// 
			// logFolderTextBox
			// 
			this.logFolderTextBox.Location = new System.Drawing.Point(8, 59);
			this.logFolderTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.logFolderTextBox.Name = "logFolderTextBox";
			this.logFolderTextBox.Size = new System.Drawing.Size(239, 22);
			this.logFolderTextBox.TabIndex = 13;
			// 
			// logFolderLabel
			// 
			this.logFolderLabel.AutoSize = true;
			this.logFolderLabel.Location = new System.Drawing.Point(8, 30);
			this.logFolderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.logFolderLabel.Name = "logFolderLabel";
			this.logFolderLabel.Size = new System.Drawing.Size(76, 17);
			this.logFolderLabel.TabIndex = 12;
			this.logFolderLabel.Text = "Log Folder";
			// 
			// currentErrorsTextBox
			// 
			this.currentErrorsTextBox.Location = new System.Drawing.Point(64, 88);
			this.currentErrorsTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.currentErrorsTextBox.Name = "currentErrorsTextBox";
			this.currentErrorsTextBox.Size = new System.Drawing.Size(68, 22);
			this.currentErrorsTextBox.TabIndex = 5;
			// 
			// currentErrorsLabel
			// 
			this.currentErrorsLabel.AutoSize = true;
			this.currentErrorsLabel.Location = new System.Drawing.Point(8, 91);
			this.currentErrorsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.currentErrorsLabel.Name = "currentErrorsLabel";
			this.currentErrorsLabel.Size = new System.Drawing.Size(47, 17);
			this.currentErrorsLabel.TabIndex = 4;
			this.currentErrorsLabel.Text = "Errors";
			// 
			// snrTextBox
			// 
			this.snrTextBox.Location = new System.Drawing.Point(188, 56);
			this.snrTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.snrTextBox.Name = "snrTextBox";
			this.snrTextBox.Size = new System.Drawing.Size(59, 22);
			this.snrTextBox.TabIndex = 3;
			// 
			// snrLabel
			// 
			this.snrLabel.AutoSize = true;
			this.snrLabel.Location = new System.Drawing.Point(143, 59);
			this.snrLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.snrLabel.Name = "snrLabel";
			this.snrLabel.Size = new System.Drawing.Size(37, 17);
			this.snrLabel.TabIndex = 2;
			this.snrLabel.Text = "SNR";
			// 
			// rssiTextBox
			// 
			this.rssiTextBox.Location = new System.Drawing.Point(64, 56);
			this.rssiTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.rssiTextBox.Name = "rssiTextBox";
			this.rssiTextBox.Size = new System.Drawing.Size(71, 22);
			this.rssiTextBox.TabIndex = 1;
			// 
			// rssiLabel
			// 
			this.rssiLabel.AutoSize = true;
			this.rssiLabel.Location = new System.Drawing.Point(8, 59);
			this.rssiLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.rssiLabel.Name = "rssiLabel";
			this.rssiLabel.Size = new System.Drawing.Size(39, 17);
			this.rssiLabel.TabIndex = 0;
			this.rssiLabel.Text = "RSSI";
			// 
			// radioStatusTextBox
			// 
			this.radioStatusTextBox.Location = new System.Drawing.Point(64, 23);
			this.radioStatusTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.radioStatusTextBox.Name = "radioStatusTextBox";
			this.radioStatusTextBox.Size = new System.Drawing.Size(183, 22);
			this.radioStatusTextBox.TabIndex = 9;
			// 
			// radioStatusLabel
			// 
			this.radioStatusLabel.AutoSize = true;
			this.radioStatusLabel.Location = new System.Drawing.Point(8, 26);
			this.radioStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.radioStatusLabel.Name = "radioStatusLabel";
			this.radioStatusLabel.Size = new System.Drawing.Size(48, 17);
			this.radioStatusLabel.TabIndex = 8;
			this.radioStatusLabel.Text = "Status";
			// 
			// totalErrorsTextBox
			// 
			this.totalErrorsTextBox.Location = new System.Drawing.Point(188, 88);
			this.totalErrorsTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.totalErrorsTextBox.Name = "totalErrorsTextBox";
			this.totalErrorsTextBox.Size = new System.Drawing.Size(59, 22);
			this.totalErrorsTextBox.TabIndex = 11;
			// 
			// totalErrorsLabel
			// 
			this.totalErrorsLabel.AutoSize = true;
			this.totalErrorsLabel.Location = new System.Drawing.Point(140, 91);
			this.totalErrorsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.totalErrorsLabel.Name = "totalErrorsLabel";
			this.totalErrorsLabel.Size = new System.Drawing.Size(40, 17);
			this.totalErrorsLabel.TabIndex = 10;
			this.totalErrorsLabel.Text = "Total";
			// 
			// radioConnectionGroupBox
			// 
			this.radioConnectionGroupBox.Controls.Add(this.radioStatusTextBox);
			this.radioConnectionGroupBox.Controls.Add(this.radioStatusLabel);
			this.radioConnectionGroupBox.Controls.Add(this.currentErrorsLabel);
			this.radioConnectionGroupBox.Controls.Add(this.snrTextBox);
			this.radioConnectionGroupBox.Controls.Add(this.snrLabel);
			this.radioConnectionGroupBox.Controls.Add(this.currentErrorsTextBox);
			this.radioConnectionGroupBox.Controls.Add(this.rssiTextBox);
			this.radioConnectionGroupBox.Controls.Add(this.totalErrorsTextBox);
			this.radioConnectionGroupBox.Controls.Add(this.totalErrorsLabel);
			this.radioConnectionGroupBox.Controls.Add(this.rssiLabel);
			this.radioConnectionGroupBox.Enabled = false;
			this.radioConnectionGroupBox.Location = new System.Drawing.Point(632, 467);
			this.radioConnectionGroupBox.Margin = new System.Windows.Forms.Padding(4);
			this.radioConnectionGroupBox.Name = "radioConnectionGroupBox";
			this.radioConnectionGroupBox.Padding = new System.Windows.Forms.Padding(4);
			this.radioConnectionGroupBox.Size = new System.Drawing.Size(259, 124);
			this.radioConnectionGroupBox.TabIndex = 10;
			this.radioConnectionGroupBox.TabStop = false;
			this.radioConnectionGroupBox.Text = "Radio Connection";
			// 
			// beaconSettingsGroupBox
			// 
			this.beaconSettingsGroupBox.Controls.Add(this.beaconRxMsTimeoutNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconRxMsTimeoutLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconTxTimeoutNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconTxTimeoutLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconRxSymTimeoutNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconRxSymTimeoutLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconCrcCheckBox);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconVariablePayloadCheckBox);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconPayloadNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconPayloadLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconPreambleNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconPreambleLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconCodingRateLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconCodingRateComboBox);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconSpreadingFactorNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconSpreadingFactorLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconOutputPowerNumericUpDown);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconOutputPowerLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconBandwidthLabel);
			this.beaconSettingsGroupBox.Controls.Add(this.beaconBandwidthComboBox);
			this.beaconSettingsGroupBox.Enabled = false;
			this.beaconSettingsGroupBox.Location = new System.Drawing.Point(655, 66);
			this.beaconSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4);
			this.beaconSettingsGroupBox.Name = "beaconSettingsGroupBox";
			this.beaconSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4);
			this.beaconSettingsGroupBox.Size = new System.Drawing.Size(263, 374);
			this.beaconSettingsGroupBox.TabIndex = 31;
			this.beaconSettingsGroupBox.TabStop = false;
			this.beaconSettingsGroupBox.Text = "Beacon Settings";
			// 
			// beaconRxMsTimeoutNumericUpDown
			// 
			this.beaconRxMsTimeoutNumericUpDown.Location = new System.Drawing.Point(138, 184);
			this.beaconRxMsTimeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconRxMsTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.beaconRxMsTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beaconRxMsTimeoutNumericUpDown.Name = "beaconRxMsTimeoutNumericUpDown";
			this.beaconRxMsTimeoutNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconRxMsTimeoutNumericUpDown.TabIndex = 30;
			this.beaconRxMsTimeoutNumericUpDown.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.beaconRxMsTimeoutNumericUpDown.ValueChanged += new System.EventHandler(this.RxMsTimeoutNumericUpDown_ValueChanged);
			// 
			// beaconRxMsTimeoutLabel
			// 
			this.beaconRxMsTimeoutLabel.AutoSize = true;
			this.beaconRxMsTimeoutLabel.Location = new System.Drawing.Point(9, 186);
			this.beaconRxMsTimeoutLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconRxMsTimeoutLabel.Name = "beaconRxMsTimeoutLabel";
			this.beaconRxMsTimeoutLabel.Size = new System.Drawing.Size(111, 17);
			this.beaconRxMsTimeoutLabel.TabIndex = 29;
			this.beaconRxMsTimeoutLabel.Text = "Rx Timeout (ms)";
			// 
			// beaconTxTimeoutNumericUpDown
			// 
			this.beaconTxTimeoutNumericUpDown.Location = new System.Drawing.Point(138, 214);
			this.beaconTxTimeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconTxTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.beaconTxTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beaconTxTimeoutNumericUpDown.Name = "beaconTxTimeoutNumericUpDown";
			this.beaconTxTimeoutNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconTxTimeoutNumericUpDown.TabIndex = 28;
			this.beaconTxTimeoutNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.beaconTxTimeoutNumericUpDown.ValueChanged += new System.EventHandler(this.TxTimeoutNumericUpDown_ValueChanged);
			// 
			// beaconTxTimeoutLabel
			// 
			this.beaconTxTimeoutLabel.AutoSize = true;
			this.beaconTxTimeoutLabel.Location = new System.Drawing.Point(9, 216);
			this.beaconTxTimeoutLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconTxTimeoutLabel.Name = "beaconTxTimeoutLabel";
			this.beaconTxTimeoutLabel.Size = new System.Drawing.Size(110, 17);
			this.beaconTxTimeoutLabel.TabIndex = 27;
			this.beaconTxTimeoutLabel.Text = "Tx Timeout (ms)";
			// 
			// beaconRxSymTimeoutNumericUpDown
			// 
			this.beaconRxSymTimeoutNumericUpDown.Location = new System.Drawing.Point(137, 154);
			this.beaconRxSymTimeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconRxSymTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.beaconRxSymTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beaconRxSymTimeoutNumericUpDown.Name = "beaconRxSymTimeoutNumericUpDown";
			this.beaconRxSymTimeoutNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconRxSymTimeoutNumericUpDown.TabIndex = 26;
			this.beaconRxSymTimeoutNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.beaconRxSymTimeoutNumericUpDown.ValueChanged += new System.EventHandler(this.RxSymTimeoutNumericUpDown_ValueChanged);
			// 
			// beaconRxSymTimeoutLabel
			// 
			this.beaconRxSymTimeoutLabel.AutoSize = true;
			this.beaconRxSymTimeoutLabel.Location = new System.Drawing.Point(8, 156);
			this.beaconRxSymTimeoutLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconRxSymTimeoutLabel.Name = "beaconRxSymTimeoutLabel";
			this.beaconRxSymTimeoutLabel.Size = new System.Drawing.Size(118, 17);
			this.beaconRxSymTimeoutLabel.TabIndex = 25;
			this.beaconRxSymTimeoutLabel.Text = "Rx Timeout (sym)";
			// 
			// beaconCrcCheckBox
			// 
			this.beaconCrcCheckBox.AutoSize = true;
			this.beaconCrcCheckBox.Checked = true;
			this.beaconCrcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.beaconCrcCheckBox.Location = new System.Drawing.Point(9, 338);
			this.beaconCrcCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.beaconCrcCheckBox.Name = "beaconCrcCheckBox";
			this.beaconCrcCheckBox.Size = new System.Drawing.Size(112, 21);
			this.beaconCrcCheckBox.TabIndex = 24;
			this.beaconCrcCheckBox.Text = "Perform CRC";
			this.beaconCrcCheckBox.UseVisualStyleBackColor = true;
			this.beaconCrcCheckBox.CheckedChanged += new System.EventHandler(this.CrcCheckBox_CheckedChanged);
			// 
			// beaconVariablePayloadCheckBox
			// 
			this.beaconVariablePayloadCheckBox.AutoSize = true;
			this.beaconVariablePayloadCheckBox.Checked = true;
			this.beaconVariablePayloadCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.beaconVariablePayloadCheckBox.Location = new System.Drawing.Point(9, 310);
			this.beaconVariablePayloadCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.beaconVariablePayloadCheckBox.Name = "beaconVariablePayloadCheckBox";
			this.beaconVariablePayloadCheckBox.Size = new System.Drawing.Size(168, 21);
			this.beaconVariablePayloadCheckBox.TabIndex = 23;
			this.beaconVariablePayloadCheckBox.Text = "Variable Payload Size";
			this.beaconVariablePayloadCheckBox.UseVisualStyleBackColor = true;
			this.beaconVariablePayloadCheckBox.CheckedChanged += new System.EventHandler(this.VariablePayloadCheckBox_CheckedChanged);
			// 
			// beaconPayloadNumericUpDown
			// 
			this.beaconPayloadNumericUpDown.Location = new System.Drawing.Point(138, 278);
			this.beaconPayloadNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconPayloadNumericUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.beaconPayloadNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beaconPayloadNumericUpDown.Name = "beaconPayloadNumericUpDown";
			this.beaconPayloadNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconPayloadNumericUpDown.TabIndex = 22;
			this.beaconPayloadNumericUpDown.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.beaconPayloadNumericUpDown.ValueChanged += new System.EventHandler(this.PayloadNumericUpDown_ValueChanged);
			// 
			// beaconPayloadLabel
			// 
			this.beaconPayloadLabel.AutoSize = true;
			this.beaconPayloadLabel.Location = new System.Drawing.Point(9, 280);
			this.beaconPayloadLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconPayloadLabel.Name = "beaconPayloadLabel";
			this.beaconPayloadLabel.Size = new System.Drawing.Size(119, 17);
			this.beaconPayloadLabel.TabIndex = 21;
			this.beaconPayloadLabel.Text = "Payload Max Size";
			// 
			// beaconPreambleNumericUpDown
			// 
			this.beaconPreambleNumericUpDown.Location = new System.Drawing.Point(138, 246);
			this.beaconPreambleNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconPreambleNumericUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.beaconPreambleNumericUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.beaconPreambleNumericUpDown.Name = "beaconPreambleNumericUpDown";
			this.beaconPreambleNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconPreambleNumericUpDown.TabIndex = 20;
			this.beaconPreambleNumericUpDown.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.beaconPreambleNumericUpDown.ValueChanged += new System.EventHandler(this.PreambleNumericUpDown_ValueChanged);
			// 
			// beaconPreambleLabel
			// 
			this.beaconPreambleLabel.AutoSize = true;
			this.beaconPreambleLabel.Location = new System.Drawing.Point(9, 248);
			this.beaconPreambleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconPreambleLabel.Name = "beaconPreambleLabel";
			this.beaconPreambleLabel.Size = new System.Drawing.Size(99, 17);
			this.beaconPreambleLabel.TabIndex = 19;
			this.beaconPreambleLabel.Text = "Preamble Size";
			// 
			// beaconCodingRateLabel
			// 
			this.beaconCodingRateLabel.AutoSize = true;
			this.beaconCodingRateLabel.Location = new System.Drawing.Point(8, 124);
			this.beaconCodingRateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconCodingRateLabel.Name = "beaconCodingRateLabel";
			this.beaconCodingRateLabel.Size = new System.Drawing.Size(86, 17);
			this.beaconCodingRateLabel.TabIndex = 17;
			this.beaconCodingRateLabel.Text = "Coding Rate";
			// 
			// beaconCodingRateComboBox
			// 
			this.beaconCodingRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.beaconCodingRateComboBox.FormattingEnabled = true;
			this.beaconCodingRateComboBox.Items.AddRange(new object[] {
            "4/5",
            "4/6",
            "4/7",
            "4/8"});
			this.beaconCodingRateComboBox.Location = new System.Drawing.Point(137, 121);
			this.beaconCodingRateComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.beaconCodingRateComboBox.Name = "beaconCodingRateComboBox";
			this.beaconCodingRateComboBox.Size = new System.Drawing.Size(116, 24);
			this.beaconCodingRateComboBox.Sorted = true;
			this.beaconCodingRateComboBox.TabIndex = 18;
			// 
			// beaconSpreadingFactorNumericUpDown
			// 
			this.beaconSpreadingFactorNumericUpDown.Location = new System.Drawing.Point(137, 89);
			this.beaconSpreadingFactorNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconSpreadingFactorNumericUpDown.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.beaconSpreadingFactorNumericUpDown.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
			this.beaconSpreadingFactorNumericUpDown.Name = "beaconSpreadingFactorNumericUpDown";
			this.beaconSpreadingFactorNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconSpreadingFactorNumericUpDown.TabIndex = 16;
			this.beaconSpreadingFactorNumericUpDown.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.beaconSpreadingFactorNumericUpDown.ValueChanged += new System.EventHandler(this.SpreadingFactorNumericUpDown_ValueChanged);
			// 
			// beaconSpreadingFactorLabel
			// 
			this.beaconSpreadingFactorLabel.AutoSize = true;
			this.beaconSpreadingFactorLabel.Location = new System.Drawing.Point(8, 91);
			this.beaconSpreadingFactorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconSpreadingFactorLabel.Name = "beaconSpreadingFactorLabel";
			this.beaconSpreadingFactorLabel.Size = new System.Drawing.Size(113, 17);
			this.beaconSpreadingFactorLabel.TabIndex = 15;
			this.beaconSpreadingFactorLabel.Text = "Spreading factor";
			// 
			// beaconOutputPowerNumericUpDown
			// 
			this.beaconOutputPowerNumericUpDown.Location = new System.Drawing.Point(137, 57);
			this.beaconOutputPowerNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
			this.beaconOutputPowerNumericUpDown.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
			this.beaconOutputPowerNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beaconOutputPowerNumericUpDown.Name = "beaconOutputPowerNumericUpDown";
			this.beaconOutputPowerNumericUpDown.Size = new System.Drawing.Size(117, 22);
			this.beaconOutputPowerNumericUpDown.TabIndex = 14;
			this.beaconOutputPowerNumericUpDown.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
			this.beaconOutputPowerNumericUpDown.ValueChanged += new System.EventHandler(this.OutputPowerNumericUpDown_ValueChanged);
			// 
			// beaconOutputPowerLabel
			// 
			this.beaconOutputPowerLabel.AutoSize = true;
			this.beaconOutputPowerLabel.Location = new System.Drawing.Point(8, 59);
			this.beaconOutputPowerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconOutputPowerLabel.Name = "beaconOutputPowerLabel";
			this.beaconOutputPowerLabel.Size = new System.Drawing.Size(94, 17);
			this.beaconOutputPowerLabel.TabIndex = 13;
			this.beaconOutputPowerLabel.Text = "Output Power";
			// 
			// beaconBandwidthLabel
			// 
			this.beaconBandwidthLabel.AutoSize = true;
			this.beaconBandwidthLabel.Location = new System.Drawing.Point(8, 27);
			this.beaconBandwidthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.beaconBandwidthLabel.Name = "beaconBandwidthLabel";
			this.beaconBandwidthLabel.Size = new System.Drawing.Size(73, 17);
			this.beaconBandwidthLabel.TabIndex = 10;
			this.beaconBandwidthLabel.Text = "Bandwidth";
			// 
			// beaconBandwidthComboBox
			// 
			this.beaconBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.beaconBandwidthComboBox.FormattingEnabled = true;
			this.beaconBandwidthComboBox.Items.AddRange(new object[] {
            "125 kHz",
            "250 kHz",
            "500 kHz"});
			this.beaconBandwidthComboBox.Location = new System.Drawing.Point(137, 23);
			this.beaconBandwidthComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.beaconBandwidthComboBox.Name = "beaconBandwidthComboBox";
			this.beaconBandwidthComboBox.Size = new System.Drawing.Size(116, 24);
			this.beaconBandwidthComboBox.Sorted = true;
			this.beaconBandwidthComboBox.TabIndex = 12;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1026, 744);
			this.Controls.Add(this.beaconSettingsGroupBox);
			this.Controls.Add(this.radioConnectionGroupBox);
			this.Controls.Add(this.logGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.Name = "MainWindow";
			this.Text = "LoRa Controller";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.logGroupBox.ResumeLayout(false);
			this.logGroupBox.PerformLayout();
			this.radioConnectionGroupBox.ResumeLayout(false);
			this.radioConnectionGroupBox.PerformLayout();
			this.beaconSettingsGroupBox.ResumeLayout(false);
			this.beaconSettingsGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.beaconRxMsTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconTxTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconRxSymTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconPayloadNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconPreambleNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconSpreadingFactorNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beaconOutputPowerNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox logListBox;
		private System.Windows.Forms.GroupBox logGroupBox;
		private System.Windows.Forms.Label rssiLabel;
		private System.Windows.Forms.TextBox snrTextBox;
		private System.Windows.Forms.Label snrLabel;
		private System.Windows.Forms.TextBox rssiTextBox;
		private System.Windows.Forms.TextBox currentErrorsTextBox;
		private System.Windows.Forms.Label currentErrorsLabel;
		private System.Windows.Forms.TextBox radioStatusTextBox;
		private System.Windows.Forms.Label radioStatusLabel;
		private System.Windows.Forms.TextBox totalErrorsTextBox;
		private System.Windows.Forms.Label totalErrorsLabel;
		private System.Windows.Forms.GroupBox radioConnectionGroupBox;
		private System.Windows.Forms.Button changeLogFolderButton;
		private System.Windows.Forms.TextBox logFolderTextBox;
		private System.Windows.Forms.Label logFolderLabel;
		private System.Windows.Forms.GroupBox beaconSettingsGroupBox;
		private System.Windows.Forms.NumericUpDown beaconRxMsTimeoutNumericUpDown;
		private System.Windows.Forms.Label beaconRxMsTimeoutLabel;
		private System.Windows.Forms.NumericUpDown beaconTxTimeoutNumericUpDown;
		private System.Windows.Forms.Label beaconTxTimeoutLabel;
		private System.Windows.Forms.NumericUpDown beaconRxSymTimeoutNumericUpDown;
		private System.Windows.Forms.Label beaconRxSymTimeoutLabel;
		private System.Windows.Forms.CheckBox beaconCrcCheckBox;
		private System.Windows.Forms.CheckBox beaconVariablePayloadCheckBox;
		private System.Windows.Forms.NumericUpDown beaconPayloadNumericUpDown;
		private System.Windows.Forms.Label beaconPayloadLabel;
		private System.Windows.Forms.NumericUpDown beaconPreambleNumericUpDown;
		private System.Windows.Forms.Label beaconPreambleLabel;
		private System.Windows.Forms.Label beaconCodingRateLabel;
		private System.Windows.Forms.ComboBox beaconCodingRateComboBox;
		private System.Windows.Forms.NumericUpDown beaconSpreadingFactorNumericUpDown;
		private System.Windows.Forms.Label beaconSpreadingFactorLabel;
		private System.Windows.Forms.NumericUpDown beaconOutputPowerNumericUpDown;
		private System.Windows.Forms.Label beaconOutputPowerLabel;
		private System.Windows.Forms.Label beaconBandwidthLabel;
		private System.Windows.Forms.ComboBox beaconBandwidthComboBox;
	}
}

