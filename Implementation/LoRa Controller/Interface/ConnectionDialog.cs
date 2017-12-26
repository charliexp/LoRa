using LoRa_Controller.Interface.DirectConnection;
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
		private DirectConnectionInterface ConnectionInterface;

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
					Parameters.Clear();
				}

				ConnectionType = ConnectionType.Serial;
				ConnectionInterface = new SerialConnectionInterface();
				((ComboBox)ConnectionInterface.ParameterBoxes[0]).SelectedIndexChanged += new EventHandler(PortComboBox_SelectedIndexChanged);
				Parameters.Add("");

				Controls.Add(ConnectionInterface.ParameterLabels[0]);
				Controls.Add(ConnectionInterface.ParameterBoxes[0]);
				OKButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															2 * Constants.ButtonWidth -
															Constants.WindowMarginX,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															Constants.InterfaceItemPadding);

				ExitButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															Constants.ButtonWidth -
															Constants.WindowMarginX,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															Constants.InterfaceItemPadding);
				ClientSize = new System.Drawing.Size(2 * Constants.WindowMarginX +
													MessageLabel.Width,

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
					Parameters.Clear();
				}

				ConnectionType = ConnectionType.Internet;
				ConnectionInterface = new InternetConnectionInterface();
				((TextBox)ConnectionInterface.ParameterBoxes[0]).TextChanged += new EventHandler(IPTextBox_TextChanged);
				((TextBox)ConnectionInterface.ParameterBoxes[1]).TextChanged += new EventHandler(PortTextBox_TextChanged);
				Parameters.Add(((TextBox)ConnectionInterface.ParameterBoxes[0]).Text);
				Parameters.Add(((TextBox)ConnectionInterface.ParameterBoxes[1]).Text);

				Controls.Add(ConnectionInterface.ParameterLabels[0]);
				Controls.Add(ConnectionInterface.ParameterBoxes[0]);
				Controls.Add(ConnectionInterface.ParameterLabels[1]);
				Controls.Add(ConnectionInterface.ParameterBoxes[1]);

				OKButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															2 * Constants.ButtonWidth -
															Constants.WindowMarginX,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															ConnectionInterface.ParameterBoxes[1].Height +
															Constants.InterfaceItemPadding);

				ExitButton.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															Constants.ButtonWidth -
															Constants.WindowMarginX,

															2 * Constants.WindowMarginY +
															MessageLabel.Height +
															SerialRadioButton.Height +
															ConnectionInterface.ParameterBoxes[0].Height +
															ConnectionInterface.ParameterBoxes[1].Height +
															Constants.InterfaceItemPadding);
				ClientSize = new System.Drawing.Size(2 * Constants.WindowMarginX +
													MessageLabel.Width,

													2 * Constants.WindowMarginY +
													MessageLabel.Height +
													SerialRadioButton.Height +
													ConnectionInterface.ParameterBoxes[0].Height +
													ConnectionInterface.ParameterBoxes[1].Height +
													OKButton.Height +
													3 * Constants.InterfaceItemPadding);

				ResumeLayout(false);
				PerformLayout();
			}
		}

		private void PortComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Parameters[0] = (string)((ComboBox)sender).SelectedItem;
		}

		private void IPTextBox_TextChanged(object sender, EventArgs e)
		{
			Parameters[0] = ((TextBox)sender).Text;
		}

		private void PortTextBox_TextChanged(object sender, EventArgs e)
		{
			Parameters[1] = ((TextBox)sender).Text;
		}
	}
}
