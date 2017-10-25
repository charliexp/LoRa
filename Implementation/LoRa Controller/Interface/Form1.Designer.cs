namespace LoRa_Controller
{
    partial class Form1
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
			this.comPortComboBox = new System.Windows.Forms.ComboBox();
			this.comPortLabel = new System.Windows.Forms.Label();
			this.logGroupBox = new System.Windows.Forms.GroupBox();
			this.changeLogFolderButton = new System.Windows.Forms.Button();
			this.logFolderTextBox = new System.Windows.Forms.TextBox();
			this.logFolderLabel = new System.Windows.Forms.Label();
			this.serialConnectionGroupBox = new System.Windows.Forms.GroupBox();
			this.serialStatusTextBox = new System.Windows.Forms.TextBox();
			this.serialStatusLabel = new System.Windows.Forms.Label();
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
			this.radioSettingsGroupBox = new System.Windows.Forms.GroupBox();
			this.txTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.txTimeoutLabel = new System.Windows.Forms.Label();
			this.rxTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.rxTimeoutLabel = new System.Windows.Forms.Label();
			this.crcCheckBox = new System.Windows.Forms.CheckBox();
			this.variablePayloadCheckBox = new System.Windows.Forms.CheckBox();
			this.payloadNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.payloadLabel = new System.Windows.Forms.Label();
			this.preambleNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.preambleLabel = new System.Windows.Forms.Label();
			this.codingRateLabel = new System.Windows.Forms.Label();
			this.codingRateComboBox = new System.Windows.Forms.ComboBox();
			this.spreadingFactorNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.spreadingFactorLabel = new System.Windows.Forms.Label();
			this.outputPowerNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.outputPowerLabel = new System.Windows.Forms.Label();
			this.bandwidthLabel = new System.Windows.Forms.Label();
			this.bandwidthComboBox = new System.Windows.Forms.ComboBox();
			this.logGroupBox.SuspendLayout();
			this.serialConnectionGroupBox.SuspendLayout();
			this.radioConnectionGroupBox.SuspendLayout();
			this.radioSettingsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rxTimeoutNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.payloadNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.preambleNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spreadingFactorNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.outputPowerNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// logListBox
			// 
			this.logListBox.FormattingEnabled = true;
			this.logListBox.Location = new System.Drawing.Point(6, 74);
			this.logListBox.Name = "logListBox";
			this.logListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.logListBox.Size = new System.Drawing.Size(201, 160);
			this.logListBox.TabIndex = 0;
			// 
			// comPortComboBox
			// 
			this.comPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comPortComboBox.FormattingEnabled = true;
			this.comPortComboBox.Location = new System.Drawing.Point(69, 19);
			this.comPortComboBox.Name = "comPortComboBox";
			this.comPortComboBox.Size = new System.Drawing.Size(138, 21);
			this.comPortComboBox.Sorted = true;
			this.comPortComboBox.TabIndex = 1;
			this.comPortComboBox.DropDown += new System.EventHandler(this.ComPortComboBox_DropDown);
			this.comPortComboBox.SelectedIndexChanged += new System.EventHandler(this.ComPortComboBox_SelectedIndexChanged);
			// 
			// comPortLabel
			// 
			this.comPortLabel.AutoSize = true;
			this.comPortLabel.Location = new System.Drawing.Point(6, 22);
			this.comPortLabel.Name = "comPortLabel";
			this.comPortLabel.Size = new System.Drawing.Size(53, 13);
			this.comPortLabel.TabIndex = 2;
			this.comPortLabel.Text = "COM Port";
			// 
			// logGroupBox
			// 
			this.logGroupBox.Controls.Add(this.changeLogFolderButton);
			this.logGroupBox.Controls.Add(this.logFolderTextBox);
			this.logGroupBox.Controls.Add(this.logListBox);
			this.logGroupBox.Controls.Add(this.logFolderLabel);
			this.logGroupBox.Enabled = false;
			this.logGroupBox.Location = new System.Drawing.Point(12, 205);
			this.logGroupBox.Name = "logGroupBox";
			this.logGroupBox.Size = new System.Drawing.Size(217, 243);
			this.logGroupBox.TabIndex = 4;
			this.logGroupBox.TabStop = false;
			this.logGroupBox.Text = "Log";
			// 
			// changeLogFolderButton
			// 
			this.changeLogFolderButton.Location = new System.Drawing.Point(132, 19);
			this.changeLogFolderButton.Name = "changeLogFolderButton";
			this.changeLogFolderButton.Size = new System.Drawing.Size(75, 23);
			this.changeLogFolderButton.TabIndex = 14;
			this.changeLogFolderButton.Text = "Change";
			this.changeLogFolderButton.UseVisualStyleBackColor = true;
			this.changeLogFolderButton.Click += new System.EventHandler(this.ChangeLogFolderButton_Click);
			// 
			// logFolderTextBox
			// 
			this.logFolderTextBox.Location = new System.Drawing.Point(6, 48);
			this.logFolderTextBox.Name = "logFolderTextBox";
			this.logFolderTextBox.Size = new System.Drawing.Size(201, 20);
			this.logFolderTextBox.TabIndex = 13;
			// 
			// logFolderLabel
			// 
			this.logFolderLabel.AutoSize = true;
			this.logFolderLabel.Location = new System.Drawing.Point(6, 24);
			this.logFolderLabel.Name = "logFolderLabel";
			this.logFolderLabel.Size = new System.Drawing.Size(57, 13);
			this.logFolderLabel.TabIndex = 12;
			this.logFolderLabel.Text = "Log Folder";
			// 
			// serialConnectionGroupBox
			// 
			this.serialConnectionGroupBox.Controls.Add(this.serialStatusTextBox);
			this.serialConnectionGroupBox.Controls.Add(this.serialStatusLabel);
			this.serialConnectionGroupBox.Controls.Add(this.comPortComboBox);
			this.serialConnectionGroupBox.Controls.Add(this.comPortLabel);
			this.serialConnectionGroupBox.Location = new System.Drawing.Point(12, 12);
			this.serialConnectionGroupBox.Name = "serialConnectionGroupBox";
			this.serialConnectionGroupBox.Size = new System.Drawing.Size(217, 77);
			this.serialConnectionGroupBox.TabIndex = 5;
			this.serialConnectionGroupBox.TabStop = false;
			this.serialConnectionGroupBox.Text = "Serial connection";
			// 
			// serialStatusTextBox
			// 
			this.serialStatusTextBox.Location = new System.Drawing.Point(69, 46);
			this.serialStatusTextBox.Name = "serialStatusTextBox";
			this.serialStatusTextBox.Size = new System.Drawing.Size(138, 20);
			this.serialStatusTextBox.TabIndex = 7;
			// 
			// serialStatusLabel
			// 
			this.serialStatusLabel.AutoSize = true;
			this.serialStatusLabel.Location = new System.Drawing.Point(6, 49);
			this.serialStatusLabel.Name = "serialStatusLabel";
			this.serialStatusLabel.Size = new System.Drawing.Size(37, 13);
			this.serialStatusLabel.TabIndex = 6;
			this.serialStatusLabel.Text = "Status";
			// 
			// currentErrorsTextBox
			// 
			this.currentErrorsTextBox.Location = new System.Drawing.Point(69, 71);
			this.currentErrorsTextBox.Name = "currentErrorsTextBox";
			this.currentErrorsTextBox.Size = new System.Drawing.Size(50, 20);
			this.currentErrorsTextBox.TabIndex = 5;
			// 
			// currentErrorsLabel
			// 
			this.currentErrorsLabel.AutoSize = true;
			this.currentErrorsLabel.Location = new System.Drawing.Point(6, 74);
			this.currentErrorsLabel.Name = "currentErrorsLabel";
			this.currentErrorsLabel.Size = new System.Drawing.Size(41, 13);
			this.currentErrorsLabel.TabIndex = 4;
			this.currentErrorsLabel.Text = "Current";
			// 
			// snrTextBox
			// 
			this.snrTextBox.Location = new System.Drawing.Point(162, 45);
			this.snrTextBox.Name = "snrTextBox";
			this.snrTextBox.Size = new System.Drawing.Size(45, 20);
			this.snrTextBox.TabIndex = 3;
			// 
			// snrLabel
			// 
			this.snrLabel.AutoSize = true;
			this.snrLabel.Location = new System.Drawing.Point(126, 48);
			this.snrLabel.Name = "snrLabel";
			this.snrLabel.Size = new System.Drawing.Size(30, 13);
			this.snrLabel.TabIndex = 2;
			this.snrLabel.Text = "SNR";
			// 
			// rssiTextBox
			// 
			this.rssiTextBox.Location = new System.Drawing.Point(69, 45);
			this.rssiTextBox.Name = "rssiTextBox";
			this.rssiTextBox.Size = new System.Drawing.Size(50, 20);
			this.rssiTextBox.TabIndex = 1;
			// 
			// rssiLabel
			// 
			this.rssiLabel.AutoSize = true;
			this.rssiLabel.Location = new System.Drawing.Point(6, 48);
			this.rssiLabel.Name = "rssiLabel";
			this.rssiLabel.Size = new System.Drawing.Size(32, 13);
			this.rssiLabel.TabIndex = 0;
			this.rssiLabel.Text = "RSSI";
			// 
			// radioStatusTextBox
			// 
			this.radioStatusTextBox.Location = new System.Drawing.Point(69, 19);
			this.radioStatusTextBox.Name = "radioStatusTextBox";
			this.radioStatusTextBox.Size = new System.Drawing.Size(138, 20);
			this.radioStatusTextBox.TabIndex = 9;
			// 
			// radioStatusLabel
			// 
			this.radioStatusLabel.AutoSize = true;
			this.radioStatusLabel.Location = new System.Drawing.Point(6, 22);
			this.radioStatusLabel.Name = "radioStatusLabel";
			this.radioStatusLabel.Size = new System.Drawing.Size(37, 13);
			this.radioStatusLabel.TabIndex = 8;
			this.radioStatusLabel.Text = "Status";
			// 
			// totalErrorsTextBox
			// 
			this.totalErrorsTextBox.Location = new System.Drawing.Point(162, 71);
			this.totalErrorsTextBox.Name = "totalErrorsTextBox";
			this.totalErrorsTextBox.Size = new System.Drawing.Size(45, 20);
			this.totalErrorsTextBox.TabIndex = 11;
			// 
			// totalErrorsLabel
			// 
			this.totalErrorsLabel.AutoSize = true;
			this.totalErrorsLabel.Location = new System.Drawing.Point(125, 74);
			this.totalErrorsLabel.Name = "totalErrorsLabel";
			this.totalErrorsLabel.Size = new System.Drawing.Size(31, 13);
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
			this.radioConnectionGroupBox.Location = new System.Drawing.Point(12, 95);
			this.radioConnectionGroupBox.Name = "radioConnectionGroupBox";
			this.radioConnectionGroupBox.Size = new System.Drawing.Size(217, 101);
			this.radioConnectionGroupBox.TabIndex = 10;
			this.radioConnectionGroupBox.TabStop = false;
			this.radioConnectionGroupBox.Text = "Radio Connection";
			// 
			// radioSettingsGroupBox
			// 
			this.radioSettingsGroupBox.Controls.Add(this.txTimeoutNumericUpDown);
			this.radioSettingsGroupBox.Controls.Add(this.txTimeoutLabel);
			this.radioSettingsGroupBox.Controls.Add(this.rxTimeoutNumericUpDown);
			this.radioSettingsGroupBox.Controls.Add(this.rxTimeoutLabel);
			this.radioSettingsGroupBox.Controls.Add(this.crcCheckBox);
			this.radioSettingsGroupBox.Controls.Add(this.variablePayloadCheckBox);
			this.radioSettingsGroupBox.Controls.Add(this.payloadNumericUpDown);
			this.radioSettingsGroupBox.Controls.Add(this.payloadLabel);
			this.radioSettingsGroupBox.Controls.Add(this.preambleNumericUpDown);
			this.radioSettingsGroupBox.Controls.Add(this.preambleLabel);
			this.radioSettingsGroupBox.Controls.Add(this.codingRateLabel);
			this.radioSettingsGroupBox.Controls.Add(this.codingRateComboBox);
			this.radioSettingsGroupBox.Controls.Add(this.spreadingFactorNumericUpDown);
			this.radioSettingsGroupBox.Controls.Add(this.spreadingFactorLabel);
			this.radioSettingsGroupBox.Controls.Add(this.outputPowerNumericUpDown);
			this.radioSettingsGroupBox.Controls.Add(this.outputPowerLabel);
			this.radioSettingsGroupBox.Controls.Add(this.bandwidthLabel);
			this.radioSettingsGroupBox.Controls.Add(this.bandwidthComboBox);
			this.radioSettingsGroupBox.Enabled = false;
			this.radioSettingsGroupBox.Location = new System.Drawing.Point(235, 12);
			this.radioSettingsGroupBox.Name = "radioSettingsGroupBox";
			this.radioSettingsGroupBox.Size = new System.Drawing.Size(197, 436);
			this.radioSettingsGroupBox.TabIndex = 10;
			this.radioSettingsGroupBox.TabStop = false;
			this.radioSettingsGroupBox.Text = "Device Settings";
			// 
			// txTimeoutNumericUpDown
			// 
			this.txTimeoutNumericUpDown.Location = new System.Drawing.Point(103, 151);
			this.txTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.txTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.txTimeoutNumericUpDown.Name = "txTimeoutNumericUpDown";
			this.txTimeoutNumericUpDown.Size = new System.Drawing.Size(88, 20);
			this.txTimeoutNumericUpDown.TabIndex = 28;
			this.txTimeoutNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// txTimeoutLabel
			// 
			this.txTimeoutLabel.AutoSize = true;
			this.txTimeoutLabel.Location = new System.Drawing.Point(6, 153);
			this.txTimeoutLabel.Name = "txTimeoutLabel";
			this.txTimeoutLabel.Size = new System.Drawing.Size(82, 13);
			this.txTimeoutLabel.TabIndex = 27;
			this.txTimeoutLabel.Text = "Tx Timeout (ms)";
			// 
			// rxTimeoutNumericUpDown
			// 
			this.rxTimeoutNumericUpDown.Location = new System.Drawing.Point(103, 125);
			this.rxTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.rxTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.rxTimeoutNumericUpDown.Name = "rxTimeoutNumericUpDown";
			this.rxTimeoutNumericUpDown.Size = new System.Drawing.Size(88, 20);
			this.rxTimeoutNumericUpDown.TabIndex = 26;
			this.rxTimeoutNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// rxTimeoutLabel
			// 
			this.rxTimeoutLabel.AutoSize = true;
			this.rxTimeoutLabel.Location = new System.Drawing.Point(6, 127);
			this.rxTimeoutLabel.Name = "rxTimeoutLabel";
			this.rxTimeoutLabel.Size = new System.Drawing.Size(88, 13);
			this.rxTimeoutLabel.TabIndex = 25;
			this.rxTimeoutLabel.Text = "Rx Timeout (sym)";
			// 
			// crcCheckBox
			// 
			this.crcCheckBox.AutoSize = true;
			this.crcCheckBox.Checked = true;
			this.crcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.crcCheckBox.Location = new System.Drawing.Point(6, 252);
			this.crcCheckBox.Name = "crcCheckBox";
			this.crcCheckBox.Size = new System.Drawing.Size(87, 17);
			this.crcCheckBox.TabIndex = 24;
			this.crcCheckBox.Text = "Perform CRC";
			this.crcCheckBox.UseVisualStyleBackColor = true;
			// 
			// variablePayloadCheckBox
			// 
			this.variablePayloadCheckBox.AutoSize = true;
			this.variablePayloadCheckBox.Checked = true;
			this.variablePayloadCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.variablePayloadCheckBox.Location = new System.Drawing.Point(6, 229);
			this.variablePayloadCheckBox.Name = "variablePayloadCheckBox";
			this.variablePayloadCheckBox.Size = new System.Drawing.Size(128, 17);
			this.variablePayloadCheckBox.TabIndex = 23;
			this.variablePayloadCheckBox.Text = "Variable Payload Size";
			this.variablePayloadCheckBox.UseVisualStyleBackColor = true;
			// 
			// payloadNumericUpDown
			// 
			this.payloadNumericUpDown.Location = new System.Drawing.Point(103, 203);
			this.payloadNumericUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.payloadNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.payloadNumericUpDown.Name = "payloadNumericUpDown";
			this.payloadNumericUpDown.Size = new System.Drawing.Size(88, 20);
			this.payloadNumericUpDown.TabIndex = 22;
			this.payloadNumericUpDown.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
			// 
			// payloadLabel
			// 
			this.payloadLabel.AutoSize = true;
			this.payloadLabel.Location = new System.Drawing.Point(6, 205);
			this.payloadLabel.Name = "payloadLabel";
			this.payloadLabel.Size = new System.Drawing.Size(91, 13);
			this.payloadLabel.TabIndex = 21;
			this.payloadLabel.Text = "Payload Max Size";
			// 
			// preambleNumericUpDown
			// 
			this.preambleNumericUpDown.Location = new System.Drawing.Point(103, 177);
			this.preambleNumericUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.preambleNumericUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.preambleNumericUpDown.Name = "preambleNumericUpDown";
			this.preambleNumericUpDown.Size = new System.Drawing.Size(88, 20);
			this.preambleNumericUpDown.TabIndex = 20;
			this.preambleNumericUpDown.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			// 
			// preambleLabel
			// 
			this.preambleLabel.AutoSize = true;
			this.preambleLabel.Location = new System.Drawing.Point(6, 179);
			this.preambleLabel.Name = "preambleLabel";
			this.preambleLabel.Size = new System.Drawing.Size(74, 13);
			this.preambleLabel.TabIndex = 19;
			this.preambleLabel.Text = "Preamble Size";
			// 
			// codingRateLabel
			// 
			this.codingRateLabel.AutoSize = true;
			this.codingRateLabel.Location = new System.Drawing.Point(6, 101);
			this.codingRateLabel.Name = "codingRateLabel";
			this.codingRateLabel.Size = new System.Drawing.Size(66, 13);
			this.codingRateLabel.TabIndex = 17;
			this.codingRateLabel.Text = "Coding Rate";
			// 
			// codingRateComboBox
			// 
			this.codingRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.codingRateComboBox.FormattingEnabled = true;
			this.codingRateComboBox.Items.AddRange(new object[] {
            "4/5",
            "4/6",
            "4/7",
            "4/8"});
			this.codingRateComboBox.Location = new System.Drawing.Point(103, 98);
			this.codingRateComboBox.Name = "codingRateComboBox";
			this.codingRateComboBox.Size = new System.Drawing.Size(88, 21);
			this.codingRateComboBox.Sorted = true;
			this.codingRateComboBox.TabIndex = 18;
			// 
			// spreadingFactorNumericUpDown
			// 
			this.spreadingFactorNumericUpDown.Location = new System.Drawing.Point(103, 72);
			this.spreadingFactorNumericUpDown.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
			this.spreadingFactorNumericUpDown.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.spreadingFactorNumericUpDown.Name = "spreadingFactorNumericUpDown";
			this.spreadingFactorNumericUpDown.Size = new System.Drawing.Size(88, 20);
			this.spreadingFactorNumericUpDown.TabIndex = 16;
			this.spreadingFactorNumericUpDown.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
			// 
			// spreadingFactorLabel
			// 
			this.spreadingFactorLabel.AutoSize = true;
			this.spreadingFactorLabel.Location = new System.Drawing.Point(6, 74);
			this.spreadingFactorLabel.Name = "spreadingFactorLabel";
			this.spreadingFactorLabel.Size = new System.Drawing.Size(85, 13);
			this.spreadingFactorLabel.TabIndex = 15;
			this.spreadingFactorLabel.Text = "Spreading factor";
			// 
			// outputPowerNumericUpDown
			// 
			this.outputPowerNumericUpDown.Location = new System.Drawing.Point(103, 46);
			this.outputPowerNumericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.outputPowerNumericUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
			this.outputPowerNumericUpDown.Name = "outputPowerNumericUpDown";
			this.outputPowerNumericUpDown.Size = new System.Drawing.Size(88, 20);
			this.outputPowerNumericUpDown.TabIndex = 14;
			this.outputPowerNumericUpDown.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
			// 
			// outputPowerLabel
			// 
			this.outputPowerLabel.AutoSize = true;
			this.outputPowerLabel.Location = new System.Drawing.Point(6, 48);
			this.outputPowerLabel.Name = "outputPowerLabel";
			this.outputPowerLabel.Size = new System.Drawing.Size(72, 13);
			this.outputPowerLabel.TabIndex = 13;
			this.outputPowerLabel.Text = "Output Power";
			// 
			// bandwidthLabel
			// 
			this.bandwidthLabel.AutoSize = true;
			this.bandwidthLabel.Location = new System.Drawing.Point(6, 22);
			this.bandwidthLabel.Name = "bandwidthLabel";
			this.bandwidthLabel.Size = new System.Drawing.Size(57, 13);
			this.bandwidthLabel.TabIndex = 10;
			this.bandwidthLabel.Text = "Bandwidth";
			// 
			// bandwidthComboBox
			// 
			this.bandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.bandwidthComboBox.FormattingEnabled = true;
			this.bandwidthComboBox.Items.AddRange(new object[] {
            "125 kHz",
            "250 kHz",
            "500 kHz"});
			this.bandwidthComboBox.Location = new System.Drawing.Point(103, 19);
			this.bandwidthComboBox.Name = "bandwidthComboBox";
			this.bandwidthComboBox.Size = new System.Drawing.Size(88, 21);
			this.bandwidthComboBox.Sorted = true;
			this.bandwidthComboBox.TabIndex = 12;
			this.bandwidthComboBox.SelectedIndexChanged += new System.EventHandler(this.BandwidthComboBox_SelectedIndexChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(440, 460);
			this.Controls.Add(this.radioSettingsGroupBox);
			this.Controls.Add(this.radioConnectionGroupBox);
			this.Controls.Add(this.serialConnectionGroupBox);
			this.Controls.Add(this.logGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "LoRa Controller";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.logGroupBox.ResumeLayout(false);
			this.logGroupBox.PerformLayout();
			this.serialConnectionGroupBox.ResumeLayout(false);
			this.serialConnectionGroupBox.PerformLayout();
			this.radioConnectionGroupBox.ResumeLayout(false);
			this.radioConnectionGroupBox.PerformLayout();
			this.radioSettingsGroupBox.ResumeLayout(false);
			this.radioSettingsGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rxTimeoutNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.payloadNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.preambleNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spreadingFactorNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.outputPowerNumericUpDown)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.ComboBox comPortComboBox;
        private System.Windows.Forms.Label comPortLabel;
        private System.Windows.Forms.GroupBox logGroupBox;
        private System.Windows.Forms.GroupBox serialConnectionGroupBox;
        private System.Windows.Forms.Label rssiLabel;
        private System.Windows.Forms.TextBox snrTextBox;
        private System.Windows.Forms.Label snrLabel;
        private System.Windows.Forms.TextBox rssiTextBox;
        private System.Windows.Forms.TextBox currentErrorsTextBox;
        private System.Windows.Forms.Label currentErrorsLabel;
		private System.Windows.Forms.TextBox radioStatusTextBox;
		private System.Windows.Forms.Label radioStatusLabel;
		private System.Windows.Forms.TextBox serialStatusTextBox;
		private System.Windows.Forms.Label serialStatusLabel;
		private System.Windows.Forms.TextBox totalErrorsTextBox;
		private System.Windows.Forms.Label totalErrorsLabel;
		private System.Windows.Forms.GroupBox radioConnectionGroupBox;
		private System.Windows.Forms.GroupBox radioSettingsGroupBox;
		private System.Windows.Forms.Label codingRateLabel;
		private System.Windows.Forms.ComboBox codingRateComboBox;
		private System.Windows.Forms.NumericUpDown spreadingFactorNumericUpDown;
		private System.Windows.Forms.Label spreadingFactorLabel;
		private System.Windows.Forms.NumericUpDown outputPowerNumericUpDown;
		private System.Windows.Forms.Label outputPowerLabel;
		private System.Windows.Forms.Label bandwidthLabel;
		private System.Windows.Forms.ComboBox bandwidthComboBox;
		private System.Windows.Forms.NumericUpDown txTimeoutNumericUpDown;
		private System.Windows.Forms.Label txTimeoutLabel;
		private System.Windows.Forms.NumericUpDown rxTimeoutNumericUpDown;
		private System.Windows.Forms.Label rxTimeoutLabel;
		private System.Windows.Forms.CheckBox crcCheckBox;
		private System.Windows.Forms.CheckBox variablePayloadCheckBox;
		private System.Windows.Forms.NumericUpDown payloadNumericUpDown;
		private System.Windows.Forms.Label payloadLabel;
		private System.Windows.Forms.NumericUpDown preambleNumericUpDown;
		private System.Windows.Forms.Label preambleLabel;
		private System.Windows.Forms.Button changeLogFolderButton;
		private System.Windows.Forms.TextBox logFolderTextBox;
		private System.Windows.Forms.Label logFolderLabel;
	}
}

