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
			this.logGroupBox.SuspendLayout();
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
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1026, 744);
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
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox logListBox;
		private System.Windows.Forms.GroupBox logGroupBox;
		private System.Windows.Forms.Button changeLogFolderButton;
		private System.Windows.Forms.TextBox logFolderTextBox;
		private System.Windows.Forms.Label logFolderLabel;
	}
}

