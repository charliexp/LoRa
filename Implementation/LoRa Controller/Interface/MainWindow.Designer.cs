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
			this.logGroupBox.SuspendLayout();
			this.radioConnectionGroupBox.SuspendLayout();
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
			this.logGroupBox.Location = new System.Drawing.Point(718, 374);
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
			this.radioConnectionGroupBox.Location = new System.Drawing.Point(718, 67);
			this.radioConnectionGroupBox.Margin = new System.Windows.Forms.Padding(4);
			this.radioConnectionGroupBox.Name = "radioConnectionGroupBox";
			this.radioConnectionGroupBox.Padding = new System.Windows.Forms.Padding(4);
			this.radioConnectionGroupBox.Size = new System.Drawing.Size(259, 124);
			this.radioConnectionGroupBox.TabIndex = 10;
			this.radioConnectionGroupBox.TabStop = false;
			this.radioConnectionGroupBox.Text = "Radio Connection";
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1026, 744);
			this.Controls.Add(this.radioConnectionGroupBox);
			this.Controls.Add(this.logGroupBox);
			this.Enabled = false;
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
	}
}

