using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.DirectConnection
{
	public class SerialConnectionInterface : DirectConnectionInterface
	{
		public SerialConnectionInterface() : base()
		{
			ComboBox portComboBox = new ComboBox();
			ParameterBoxes.Add(portComboBox);

			Label portLabel = new Label();
			ParameterLabels.Add(portLabel);
			
			portLabel.AutoSize = true;
			portLabel.Location = new System.Drawing.Point(Constants.LabelLocationX,
														Constants.LabelLocationY +
														Constants.LabelHeight +
														Constants.RadioButtonHeight +
														Constants.LabelToBoxOffset);
			portLabel.Margin = new Padding(4, 0, 4, 0);
			portLabel.Name = "portLabel";
			portLabel.Size = new System.Drawing.Size(34, 17);
			portLabel.Text = "Port";

			portComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			portComboBox.FormattingEnabled = true;
			portComboBox.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															portLabel.Width -
															Constants.ComboBoxWidth,

															Constants.LabelLocationY +
															Constants.LabelHeight +
															Constants.RadioButtonHeight);
			portComboBox.Margin = new Padding(4);
			portComboBox.Name = "portComboBox";
			portComboBox.Size = new System.Drawing.Size(Constants.ComboBoxWidth, Constants.ComboBoxHeight);
			portComboBox.Sorted = true;
			portComboBox.TabIndex = 1;
			portComboBox.DropDown += new EventHandler(PortComboBox_DropDown);
		}

		private void PortComboBox_DropDown(object sender, EventArgs e)
		{
			ComboBox comboBox = ((ComboBox)sender);
			string[] comPortsList = MasterDevice.getAvailablePorts();

			comboBox.Items.Clear();
			foreach (string port in comPortsList)
				if (!comboBox.Items.Contains(port))
					comboBox.Items.Add(port);
		}
	}
}
