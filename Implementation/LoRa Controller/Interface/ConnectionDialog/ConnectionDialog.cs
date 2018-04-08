using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.ConnectionDialog
{
	public partial class ConnectionDialog : Form
    {
        #region Private variables
        private BaseConnection baseConnection;
        #endregion

        #region Constructors
        public ConnectionDialog()
		{
			InitializeComponent();
			SerialRadioButton_CheckedChanged(SerialRadioButton, new EventArgs());
        }
        #endregion

        #region Private methods
        private void SerialRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (SerialRadioButton.Checked)
			{
				SuspendLayout();

				if (baseConnection != null)
				{
					baseConnection.ParameterLabels[0].Dispose();
					baseConnection.ParameterBoxes[0].Dispose();
					baseConnection.ParameterLabels[1].Dispose();
					baseConnection.ParameterBoxes[1].Dispose();
				}
				
				baseConnection = new SerialConnection();
				((ComboBox)baseConnection.ParameterBoxes[0]).SelectedIndexChanged += new EventHandler(PortComboBox_SelectedIndexChanged);

				Controls.Add(baseConnection.ParameterLabels[0]);
				Controls.Add(baseConnection.ParameterBoxes[0]);

				OKButton.Location = new System.Drawing.Point(InterfaceConstants.MessageLabelMaxWidth -
															InterfaceConstants.ButtonWidth,

															2 * InterfaceConstants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															baseConnection.ParameterBoxes[0].Height +
															InterfaceConstants.ItemPadding);
				ClientSize = new System.Drawing.Size(InterfaceConstants.MessageLabelMaxWidth,

													2 * InterfaceConstants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													baseConnection.ParameterBoxes[0].Height +
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

				if (baseConnection != null)
				{
					baseConnection.ParameterLabels[0].Dispose();
					baseConnection.ParameterBoxes[0].Dispose();
				}
				
				baseConnection = new InternetConnection();
				((TextBox)baseConnection.ParameterBoxes[0]).TextChanged += new EventHandler(IPTextBox_TextChanged);
				((TextBox)baseConnection.ParameterBoxes[1]).TextChanged += new EventHandler(PortTextBox_TextChanged);

				Controls.Add(baseConnection.ParameterLabels[0]);
				Controls.Add(baseConnection.ParameterBoxes[0]);
				Controls.Add(baseConnection.ParameterLabels[1]);
				Controls.Add(baseConnection.ParameterBoxes[1]);

				OKButton.Location = new System.Drawing.Point(InterfaceConstants.MessageLabelMaxWidth -
															InterfaceConstants.ButtonWidth,

															2 * InterfaceConstants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															baseConnection.ParameterBoxes[0].Height +
															baseConnection.ParameterBoxes[1].Height +
															2 * InterfaceConstants.ItemPadding);
				ClientSize = new System.Drawing.Size(InterfaceConstants.MessageLabelMaxWidth,

													2 * InterfaceConstants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													baseConnection.ParameterBoxes[0].Height +
													baseConnection.ParameterBoxes[1].Height +
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
				parameters.Add((string)(((ComboBox)baseConnection.ParameterBoxes[0]).SelectedItem));
				SettingHandler.COMPort.Value = parameters[0];
			}
			else
			{
				connectionType = ConnectionType.Internet;
				parameters.Add(((TextBox)baseConnection.ParameterBoxes[0]).Text);
				parameters.Add(((TextBox)baseConnection.ParameterBoxes[1]).Text);
				SettingHandler.IPAddress.Value = parameters[0];
				SettingHandler.TCPPort.Value = parameters[1];
			}
			Close();
			Program.StartConnection(connectionType, parameters);
        }
        #endregion
    }
}
