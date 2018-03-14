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
			this.logListBox.Location = new System.Drawing.Point(6, 73);
			this.logListBox.Name = "logListBox";
			this.logListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.logListBox.Size = new System.Drawing.Size(192, 147);
			this.logListBox.TabIndex = 0;
			// 
			// logGroupBox
			// 
			this.logGroupBox.AutoSize = true;
			this.logGroupBox.Controls.Add(this.changeLogFolderButton);
			this.logGroupBox.Controls.Add(this.logFolderTextBox);
			this.logGroupBox.Controls.Add(this.logListBox);
			this.logGroupBox.Controls.Add(this.logFolderLabel);
			this.logGroupBox.Location = new System.Drawing.Point(12, 12);
			this.logGroupBox.Name = "logGroupBox";
			this.logGroupBox.Size = new System.Drawing.Size(204, 239);
			this.logGroupBox.TabIndex = 4;
			this.logGroupBox.TabStop = false;
			this.logGroupBox.Text = "Log";
			// 
			// changeLogFolderButton
			// 
			this.changeLogFolderButton.Location = new System.Drawing.Point(118, 15);
			this.changeLogFolderButton.Name = "changeLogFolderButton";
			this.changeLogFolderButton.Size = new System.Drawing.Size(80, 22);
			this.changeLogFolderButton.TabIndex = 14;
			this.changeLogFolderButton.Text = "Change";
			this.changeLogFolderButton.UseVisualStyleBackColor = true;
			this.changeLogFolderButton.Click += new System.EventHandler(this.ChangeLogFolderButton_Click);
			// 
			// logFolderTextBox
			// 
			this.logFolderTextBox.Location = new System.Drawing.Point(6, 47);
			this.logFolderTextBox.Name = "logFolderTextBox";
			this.logFolderTextBox.Size = new System.Drawing.Size(192, 20);
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
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(269, 288);
			this.Controls.Add(this.logGroupBox);
			this.Enabled = false;
			this.Name = "MainWindow";
			this.Text = "LoRa Controller";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.logGroupBox.ResumeLayout(false);
			this.logGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox logListBox;
		private System.Windows.Forms.GroupBox logGroupBox;
		private System.Windows.Forms.Button changeLogFolderButton;
		private System.Windows.Forms.TextBox logFolderTextBox;
		private System.Windows.Forms.Label logFolderLabel;
	}
}

