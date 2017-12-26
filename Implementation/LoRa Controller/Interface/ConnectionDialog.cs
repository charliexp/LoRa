using LoRa_Controller.Interface.ConnectionChooser;
using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface
{
	public partial class ConnectionDialog : Form
	{
		public ConnectionType ConnectionType;
		public List<string> Parameters;
		private ConnectionChooser.ConnectionChooser ConnectionInterface;

		public ConnectionDialog()
		{
			Parameters = new List<string>();
			InitializeComponent();
		}

		private void SerialRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (SerialRadioButton.Checked)
			{
				SuspendLayout();

				if (ConnectionInterface != null)
				{
					ConnectionInterface.ParameterLabels[0].Dispose();
					ConnectionInterface.ParameterBoxes[0].Dispose();
					ConnectionInterface.ParameterLabels[1].Dispose();
					ConnectionInterface.ParameterBoxes[1].Dispose();
				}

				ConnectionType = ConnectionType.Serial;
				ConnectionInterface = new SerialConnectionChooser();
				((ComboBox)ConnectionInterface.ParameterBoxes[0]).SelectedIndexChanged += new EventHandler(PortComboBox_SelectedIndexChanged);

				Controls.Add(ConnectionInterface.ParameterLabels[0]);
				Controls.Add(ConnectionInterface.ParameterBoxes[0]);
				OKButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															2 * Constants.ButtonWidth -
															Constants.LabelLocationX +
															Constants.InterfaceItemPadding,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															Constants.InterfaceItemPadding);

				ExitButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															Constants.ButtonWidth,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															Constants.InterfaceItemPadding);
				ClientSize = new System.Drawing.Size(Constants.LabelMaxWidth,

													2 * Constants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													ConnectionInterface.ParameterBoxes[0].Height +
													OKButton.Height +
													3 * Constants.InterfaceItemPadding);

				ResumeLayout(false);
				PerformLayout();
			}
		}

		private void RemoteRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (RemoteRadioButton.Checked)
			{
				SuspendLayout();

				if (ConnectionInterface != null)
				{
					ConnectionInterface.ParameterLabels[0].Dispose();
					ConnectionInterface.ParameterBoxes[0].Dispose();
				}

				ConnectionType = ConnectionType.Internet;
				ConnectionInterface = new InternetConnectionChooser();
				((TextBox)ConnectionInterface.ParameterBoxes[0]).TextChanged += new EventHandler(IPTextBox_TextChanged);
				((TextBox)ConnectionInterface.ParameterBoxes[1]).TextChanged += new EventHandler(PortTextBox_TextChanged);

				Controls.Add(ConnectionInterface.ParameterLabels[0]);
				Controls.Add(ConnectionInterface.ParameterBoxes[0]);
				Controls.Add(ConnectionInterface.ParameterLabels[1]);
				Controls.Add(ConnectionInterface.ParameterBoxes[1]);

				OKButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															2 * Constants.ButtonWidth -
															Constants.LabelLocationX +
															Constants.InterfaceItemPadding,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															ConnectionInterface.ParameterBoxes[1].Height +
															2 * Constants.InterfaceItemPadding);

				ExitButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															Constants.ButtonWidth,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															ConnectionInterface.ParameterBoxes[1].Height +
															2 * Constants.InterfaceItemPadding);
				ClientSize = new System.Drawing.Size(Constants.LabelMaxWidth,

													2 * Constants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													ConnectionInterface.ParameterBoxes[0].Height +
													ConnectionInterface.ParameterBoxes[1].Height +
													OKButton.Height +
													4 * Constants.InterfaceItemPadding);

				ResumeLayout(false);
				PerformLayout();
			}
		}

		private void PortComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OKButton.Enabled = true;
		}

		private void IPTextBox_TextChanged(object sender, EventArgs e)
		{
			if (((TextBox)sender).Text.Split(new char[] { '.' }).Length == 4)
				OKButton.Enabled = true;
			else
				OKButton.Enabled = false;
		}

		private void PortTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Int32.TryParse(((TextBox)sender).Text, out int port))
				OKButton.Enabled = true;
			else
				OKButton.Enabled = false;
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			if (SerialRadioButton.Checked)
			{
				Parameters.Add((string)(((ComboBox)ConnectionInterface.ParameterBoxes[0]).SelectedItem));
				SettingHandler.COMPort.Value = Parameters[0];
			}
			else if (RemoteRadioButton.Checked)
			{
				Parameters.Add(((TextBox)ConnectionInterface.ParameterBoxes[0]).Text);
				Parameters.Add(((TextBox)ConnectionInterface.ParameterBoxes[1]).Text);
				SettingHandler.IPAddress.Value = Parameters[0];
				SettingHandler.TCPPort.Value = Parameters[1];
			}
		}
	}
}
