using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.DirectConnection
{
	public class InternetConnectionInterface : DirectConnectionInterface
	{
		public InternetConnectionInterface() : base()
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
			IPLabel.Location = new System.Drawing.Point(Constants.LabelLocationX,
														Constants.LabelLocationY +
														Constants.LabelHeight +
														Constants.RadioButtonHeight +
														Constants.LabelToBoxOffset);
			IPLabel.Margin = new Padding(4, 0, 4, 0);
			IPLabel.Name = "IPLabel";
			IPLabel.Size = new System.Drawing.Size(34, Constants.LabelHeight);
			IPLabel.Text = "IP";
			
			IPTextBox.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															Constants.TextBoxWidth -
															Constants.WindowMarginX -
															2 * Constants.InterfaceItemPadding,

															Constants.LabelLocationY +
															Constants.LabelHeight +
															Constants.RadioButtonHeight);
			IPTextBox.Margin = new Padding(4);
			IPTextBox.Name = "IPTextBox";
			IPTextBox.Size = new System.Drawing.Size(Constants.TextBoxWidth, Constants.TextBoxHeight);
			IPTextBox.TabIndex = 2;
			IPTextBox.Text = "127.0.0.1";

			portLabel.AutoSize = true;
			portLabel.Location = new System.Drawing.Point(Constants.LabelLocationX,

														Constants.LabelLocationY +
														Constants.LabelHeight +
														Constants.RadioButtonHeight +
														Constants.TextBoxHeight +
														Constants.InterfaceItemPadding +
														Constants.LabelToBoxOffset);
			portLabel.Margin = new Padding(4, 0, 4, 0);
			portLabel.Name = "portLabel";
			portLabel.Size = new System.Drawing.Size(34, 17);
			portLabel.Text = "Port";

			portTextBox.Location = new System.Drawing.Point(Constants.LabelMaxWidth -
															Constants.TextBoxWidth -
															Constants.WindowMarginX -
															2 * Constants.InterfaceItemPadding,

															Constants.LabelLocationY +
															Constants.LabelHeight +
															Constants.RadioButtonHeight +
															Constants.InterfaceItemPadding +
															Constants.TextBoxHeight);
			portTextBox.Margin = new Padding(4);
			portTextBox.Name = "portTextBox";
			portTextBox.Size = new System.Drawing.Size(Constants.TextBoxWidth, Constants.TextBoxHeight);
			portTextBox.TabIndex = 3;
			portTextBox.Text = "13000";
		}
	}
}
