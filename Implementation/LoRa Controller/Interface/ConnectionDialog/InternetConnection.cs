using LoRa_Controller.Settings;
using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.ConnectionDialog
{
	public class InternetConnection : BaseConnection
    {
        #region Constructors
        public InternetConnection() : base()
		{

			TextBox IPTextBox = new TextBox();
			ParameterBoxes.Add(IPTextBox);
			TextBox portTextBox = new TextBox();
			ParameterBoxes.Add(portTextBox);

			Label IPLabel = new Label();
			ParameterLabels.Add(IPLabel);
			Label portLabel = new Label();
			ParameterLabels.Add(portLabel);

			IPLabel.AutoSize = true;
			IPLabel.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX,
														InterfaceConstants.LabelLocationY +
														InterfaceConstants.LabelHeight +
														InterfaceConstants.RadioButtonHeight +
														InterfaceConstants.LabelToBoxOffset);
			IPLabel.Margin = new Padding(4, 0, 4, 0);
			IPLabel.Name = "IPLabel";
			IPLabel.Size = new System.Drawing.Size(34, InterfaceConstants.LabelHeight);
			IPLabel.Text = "IP";
			
			IPTextBox.Location = new System.Drawing.Point(InterfaceConstants.MessageLabelMaxWidth -
															InterfaceConstants.InputWidth -
															InterfaceConstants.WindowMarginX -
															2 * InterfaceConstants.ItemPadding,

															InterfaceConstants.LabelLocationY +
															InterfaceConstants.LabelHeight +
															InterfaceConstants.RadioButtonHeight);
			IPTextBox.Margin = new Padding(4);
			IPTextBox.Name = "IPTextBox";
			IPTextBox.Size = new System.Drawing.Size(InterfaceConstants.InputWidth, InterfaceConstants.InputHeight);
			IPTextBox.TabIndex = 2;
			IPTextBox.Text = (string) SettingHandler.IPAddress.Value;

			portLabel.AutoSize = true;
			portLabel.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX,

														InterfaceConstants.LabelLocationY +
														InterfaceConstants.LabelHeight +
														InterfaceConstants.RadioButtonHeight +
														InterfaceConstants.InputHeight +
														InterfaceConstants.ItemPadding +
														InterfaceConstants.LabelToBoxOffset);
			portLabel.Margin = new Padding(4, 0, 4, 0);
			portLabel.Name = "portLabel";
			portLabel.Size = new System.Drawing.Size(34, 17);
			portLabel.Text = "Port";

			portTextBox.Location = new System.Drawing.Point(InterfaceConstants.MessageLabelMaxWidth -
															InterfaceConstants.InputWidth -
															InterfaceConstants.WindowMarginX -
															2 * InterfaceConstants.ItemPadding,

															InterfaceConstants.LabelLocationY +
															InterfaceConstants.LabelHeight +
															InterfaceConstants.RadioButtonHeight +
															InterfaceConstants.ItemPadding +
															InterfaceConstants.InputHeight);
			portTextBox.Margin = new Padding(4);
			portTextBox.Name = "portTextBox";
			portTextBox.Size = new System.Drawing.Size(InterfaceConstants.InputWidth, InterfaceConstants.InputHeight);
			portTextBox.TabIndex = 3;
			portTextBox.Text = SettingHandler.TCPPort.Value.ToString();
        }
        #endregion
    }
}
