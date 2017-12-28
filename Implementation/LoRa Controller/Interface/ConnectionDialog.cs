using LoRa_Controller.Interface.Connection;
using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static LoRa_Controller.Device.DirectDevice;

namespace LoRa_Controller.Interface
{
	public partial class ConnectionDialog : Form
	{
		private BaseConnection ConnectionUI;

		public ConnectionDialog()
		{
			InitializeComponent();
			SerialRadioButton_CheckedChanged(SerialRadioButton, new EventArgs());
		}

		private void SerialRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (SerialRadioButton.Checked)
			{
				SuspendLayout();

				if (ConnectionUI != null)
				{
					ConnectionUI.ParameterLabels[0].Dispose();
					ConnectionUI.ParameterBoxes[0].Dispose();
					ConnectionUI.ParameterLabels[1].Dispose();
					ConnectionUI.ParameterBoxes[1].Dispose();
				}
				
				ConnectionUI = new SerialConnection();
				((ComboBox)ConnectionUI.ParameterBoxes[0]).SelectedIndexChanged += new EventHandler(PortComboBox_SelectedIndexChanged);

				Controls.Add(ConnectionUI.ParameterLabels[0]);
				Controls.Add(ConnectionUI.ParameterBoxes[0]);

				OKButton.Location = new System.Drawing.Point(InterfaceConstants.MessageLabelMaxWidth -
															InterfaceConstants.ButtonWidth,

															2 * InterfaceConstants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionUI.ParameterBoxes[0].Height +
															InterfaceConstants.ItemPadding);
				ClientSize = new System.Drawing.Size(InterfaceConstants.MessageLabelMaxWidth,

													2 * InterfaceConstants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													ConnectionUI.ParameterBoxes[0].Height +
													OKButton.Height +
													3 * InterfaceConstants.ItemPadding);

				ResumeLayout(false);
				PerformLayout();
			}
		}

		private void RemoteRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (RemoteRadioButton.Checked)
			{
				SuspendLayout();

				if (ConnectionUI != null)
				{
					ConnectionUI.ParameterLabels[0].Dispose();
					ConnectionUI.ParameterBoxes[0].Dispose();
				}
				
				ConnectionUI = new InternetConnection();
				((TextBox)ConnectionUI.ParameterBoxes[0]).TextChanged += new EventHandler(IPTextBox_TextChanged);
				((TextBox)ConnectionUI.ParameterBoxes[1]).TextChanged += new EventHandler(PortTextBox_TextChanged);

				Controls.Add(ConnectionUI.ParameterLabels[0]);
				Controls.Add(ConnectionUI.ParameterBoxes[0]);
				Controls.Add(ConnectionUI.ParameterLabels[1]);
				Controls.Add(ConnectionUI.ParameterBoxes[1]);

				OKButton.Location = new System.Drawing.Point(InterfaceConstants.MessageLabelMaxWidth -
															InterfaceConstants.ButtonWidth,

															2 * InterfaceConstants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionUI.ParameterBoxes[0].Height +
															ConnectionUI.ParameterBoxes[1].Height +
															2 * InterfaceConstants.ItemPadding);
				ClientSize = new System.Drawing.Size(InterfaceConstants.MessageLabelMaxWidth,

													2 * InterfaceConstants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													ConnectionUI.ParameterBoxes[0].Height +
													ConnectionUI.ParameterBoxes[1].Height +
													OKButton.Height +
													4 * InterfaceConstants.ItemPadding);

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
			ConnectionType connectionType;
			List<string> parameters;
			parameters = new List<string>();

			if (SerialRadioButton.Checked)
			{
				connectionType = ConnectionType.Serial;
				parameters.Add((string)(((ComboBox)ConnectionUI.ParameterBoxes[0]).SelectedItem));
				SettingHandler.COMPort.Value = parameters[0];
			}
			else
			{
				connectionType = ConnectionType.Internet;
				parameters.Add(((TextBox)ConnectionUI.ParameterBoxes[0]).Text);
				parameters.Add(((TextBox)ConnectionUI.ParameterBoxes[1]).Text);
				SettingHandler.IPAddress.Value = parameters[0];
				SettingHandler.TCPPort.Value = parameters[1];
			}
			Close();
			Program.StartConnection(connectionType, parameters);
		}
	}
}
