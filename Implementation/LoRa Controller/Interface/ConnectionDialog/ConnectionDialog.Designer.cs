namespace LoRa_Controller.Interface.ConnectionDialog
{
	partial class ConnectionDialog
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
			this.MessageLabel = new System.Windows.Forms.Label();
			this.SerialRadioButton = new System.Windows.Forms.RadioButton();
			this.RemoteRadioButton = new System.Windows.Forms.RadioButton();
			this.OKButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// MessageLabel
			// 
			this.MessageLabel.AutoSize = true;
			this.MessageLabel.Location = new System.Drawing.Point(12, 9);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(289, 17);
			this.MessageLabel.TabIndex = 0;
			this.MessageLabel.Text = "Please choose how to connect to the device.";
			// 
			// SerialRadioButton
			// 
			this.SerialRadioButton.AutoSize = true;
			this.SerialRadioButton.Checked = true;
			this.SerialRadioButton.Location = new System.Drawing.Point(15, 29);
			this.SerialRadioButton.Name = "SerialRadioButton";
			this.SerialRadioButton.Size = new System.Drawing.Size(65, 21);
			this.SerialRadioButton.TabIndex = 1;
			this.SerialRadioButton.TabStop = true;
			this.SerialRadioButton.Text = "Serial";
			this.SerialRadioButton.UseVisualStyleBackColor = true;
			this.SerialRadioButton.CheckedChanged += new System.EventHandler(this.SerialRadioButton_CheckedChanged);
			// 
			// RemoteRadioButton
			// 
			this.RemoteRadioButton.AutoSize = true;
			this.RemoteRadioButton.Location = new System.Drawing.Point(87, 29);
			this.RemoteRadioButton.Name = "RemoteRadioButton";
			this.RemoteRadioButton.Size = new System.Drawing.Size(78, 21);
			this.RemoteRadioButton.TabIndex = 2;
			this.RemoteRadioButton.Text = "Remote";
			this.RemoteRadioButton.UseVisualStyleBackColor = true;
			this.RemoteRadioButton.CheckedChanged += new System.EventHandler(this.RemoteRadioButton_CheckedChanged);
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(145, 181);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(75, 23);
			this.OKButton.TabIndex = 4;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// ConnectionDialog
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(455, 300);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.RemoteRadioButton);
			this.Controls.Add(this.SerialRadioButton);
			this.Controls.Add(this.MessageLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConnectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Connection";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label MessageLabel;
		private System.Windows.Forms.RadioButton SerialRadioButton;
		private System.Windows.Forms.RadioButton RemoteRadioButton;
		private System.Windows.Forms.Button OKButton;
	}
}