namespace LoRa_Controller.Interface
{
	partial class ConnectionTypeDialog
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
			this.LocalButton = new System.Windows.Forms.Button();
			this.RemoteButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LocalButton
			// 
			this.LocalButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.LocalButton.Location = new System.Drawing.Point(12, 63);
			this.LocalButton.Name = "LocalButton";
			this.LocalButton.Size = new System.Drawing.Size(75, 23);
			this.LocalButton.TabIndex = 0;
			this.LocalButton.Text = "Local";
			this.LocalButton.UseVisualStyleBackColor = true;
			this.LocalButton.Click += new System.EventHandler(this.LocalButton_Click);
			// 
			// RemoteButton
			// 
			this.RemoteButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.RemoteButton.Location = new System.Drawing.Point(93, 63);
			this.RemoteButton.Name = "RemoteButton";
			this.RemoteButton.Size = new System.Drawing.Size(75, 23);
			this.RemoteButton.TabIndex = 1;
			this.RemoteButton.Text = "Remote";
			this.RemoteButton.UseVisualStyleBackColor = true;
			this.RemoteButton.Click += new System.EventHandler(this.RemoteButton_Click);
			// 
			// CancelButton
			// 
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.CancelButton.Location = new System.Drawing.Point(226, 63);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(75, 23);
			this.CancelButton.TabIndex = 2;
			this.CancelButton.Text = "Exit";
			this.CancelButton.UseVisualStyleBackColor = true;
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
			// ConnectionTypeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(326, 136);
			this.ControlBox = false;
			this.Controls.Add(this.MessageLabel);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.RemoteButton);
			this.Controls.Add(this.LocalButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ConnectionTypeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Connection";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button LocalButton;
		private System.Windows.Forms.Button RemoteButton;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Label MessageLabel;
	}
}