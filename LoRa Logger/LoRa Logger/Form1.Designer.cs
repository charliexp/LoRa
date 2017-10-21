namespace LoRa_Logger
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
            this.connectionStatusLabel = new System.Windows.Forms.Label();
            this.logGroupBox = new System.Windows.Forms.GroupBox();
            this.signalQualityGroupBox = new System.Windows.Forms.GroupBox();
            this.errorsTextBox = new System.Windows.Forms.TextBox();
            this.errorsLabel = new System.Windows.Forms.Label();
            this.snrTextBox = new System.Windows.Forms.TextBox();
            this.snrLabel = new System.Windows.Forms.Label();
            this.rssiTextBox = new System.Windows.Forms.TextBox();
            this.rssiLabel = new System.Windows.Forms.Label();
            this.logGroupBox.SuspendLayout();
            this.signalQualityGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // logListBox
            // 
            this.logListBox.FormattingEnabled = true;
            this.logListBox.Location = new System.Drawing.Point(6, 19);
            this.logListBox.Name = "logListBox";
            this.logListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.logListBox.Size = new System.Drawing.Size(227, 160);
            this.logListBox.TabIndex = 0;
            // 
            // comPortComboBox
            // 
            this.comPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comPortComboBox.FormattingEnabled = true;
            this.comPortComboBox.Location = new System.Drawing.Point(72, 10);
            this.comPortComboBox.Name = "comPortComboBox";
            this.comPortComboBox.Size = new System.Drawing.Size(121, 21);
            this.comPortComboBox.Sorted = true;
            this.comPortComboBox.TabIndex = 1;
            // 
            // comPortLabel
            // 
            this.comPortLabel.AutoSize = true;
            this.comPortLabel.Location = new System.Drawing.Point(13, 13);
            this.comPortLabel.Name = "comPortLabel";
            this.comPortLabel.Size = new System.Drawing.Size(53, 13);
            this.comPortLabel.TabIndex = 2;
            this.comPortLabel.Text = "COM Port";
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.AutoSize = true;
            this.connectionStatusLabel.Location = new System.Drawing.Point(6, 16);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(92, 13);
            this.connectionStatusLabel.TabIndex = 3;
            this.connectionStatusLabel.Text = "Connection status";
            // 
            // logGroupBox
            // 
            this.logGroupBox.Controls.Add(this.logListBox);
            this.logGroupBox.Location = new System.Drawing.Point(12, 37);
            this.logGroupBox.Name = "logGroupBox";
            this.logGroupBox.Size = new System.Drawing.Size(252, 197);
            this.logGroupBox.TabIndex = 4;
            this.logGroupBox.TabStop = false;
            this.logGroupBox.Text = "Log";
            // 
            // signalQualityGroupBox
            // 
            this.signalQualityGroupBox.Controls.Add(this.errorsTextBox);
            this.signalQualityGroupBox.Controls.Add(this.errorsLabel);
            this.signalQualityGroupBox.Controls.Add(this.connectionStatusLabel);
            this.signalQualityGroupBox.Controls.Add(this.snrTextBox);
            this.signalQualityGroupBox.Controls.Add(this.snrLabel);
            this.signalQualityGroupBox.Controls.Add(this.rssiTextBox);
            this.signalQualityGroupBox.Controls.Add(this.rssiLabel);
            this.signalQualityGroupBox.Location = new System.Drawing.Point(270, 37);
            this.signalQualityGroupBox.Name = "signalQualityGroupBox";
            this.signalQualityGroupBox.Size = new System.Drawing.Size(200, 160);
            this.signalQualityGroupBox.TabIndex = 5;
            this.signalQualityGroupBox.TabStop = false;
            this.signalQualityGroupBox.Text = "Status";
            // 
            // errorsTextBox
            // 
            this.errorsTextBox.Location = new System.Drawing.Point(44, 84);
            this.errorsTextBox.Name = "errorsTextBox";
            this.errorsTextBox.Size = new System.Drawing.Size(100, 20);
            this.errorsTextBox.TabIndex = 5;
            // 
            // errorsLabel
            // 
            this.errorsLabel.AutoSize = true;
            this.errorsLabel.Location = new System.Drawing.Point(6, 87);
            this.errorsLabel.Name = "errorsLabel";
            this.errorsLabel.Size = new System.Drawing.Size(34, 13);
            this.errorsLabel.TabIndex = 4;
            this.errorsLabel.Text = "Errors";
            // 
            // snrTextBox
            // 
            this.snrTextBox.Location = new System.Drawing.Point(44, 58);
            this.snrTextBox.Name = "snrTextBox";
            this.snrTextBox.Size = new System.Drawing.Size(100, 20);
            this.snrTextBox.TabIndex = 3;
            // 
            // snrLabel
            // 
            this.snrLabel.AutoSize = true;
            this.snrLabel.Location = new System.Drawing.Point(6, 61);
            this.snrLabel.Name = "snrLabel";
            this.snrLabel.Size = new System.Drawing.Size(30, 13);
            this.snrLabel.TabIndex = 2;
            this.snrLabel.Text = "SNR";
            // 
            // rssiTextBox
            // 
            this.rssiTextBox.Location = new System.Drawing.Point(44, 32);
            this.rssiTextBox.Name = "rssiTextBox";
            this.rssiTextBox.Size = new System.Drawing.Size(100, 20);
            this.rssiTextBox.TabIndex = 1;
            // 
            // rssiLabel
            // 
            this.rssiLabel.AutoSize = true;
            this.rssiLabel.Location = new System.Drawing.Point(6, 35);
            this.rssiLabel.Name = "rssiLabel";
            this.rssiLabel.Size = new System.Drawing.Size(32, 13);
            this.rssiLabel.TabIndex = 0;
            this.rssiLabel.Text = "RSSI";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 345);
            this.Controls.Add(this.signalQualityGroupBox);
            this.Controls.Add(this.logGroupBox);
            this.Controls.Add(this.comPortLabel);
            this.Controls.Add(this.comPortComboBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.logGroupBox.ResumeLayout(false);
            this.signalQualityGroupBox.ResumeLayout(false);
            this.signalQualityGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.ComboBox comPortComboBox;
        private System.Windows.Forms.Label comPortLabel;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.GroupBox logGroupBox;
        private System.Windows.Forms.GroupBox signalQualityGroupBox;
        private System.Windows.Forms.Label rssiLabel;
        private System.Windows.Forms.TextBox snrTextBox;
        private System.Windows.Forms.Label snrLabel;
        private System.Windows.Forms.TextBox rssiTextBox;
        private System.Windows.Forms.TextBox errorsTextBox;
        private System.Windows.Forms.Label errorsLabel;
    }
}

