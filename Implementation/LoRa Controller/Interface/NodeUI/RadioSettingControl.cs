using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.NodeUI
{
	public class RadioSettingControl
	{
		public Commands Parameter;
		public Label Label;
		public Control Field;

		public RadioSettingControl(Commands parameter, Control control)
		{
			Parameter = parameter;
			Label = new Label();
			Field = control;
			
			Label.AutoSize = true;
			Label.Margin = new Padding(Constants.ItemPadding, 0, Constants.ItemPadding, 0);
			Label.Name = Parameter.ToString() + "Label";
			Label.Size = new System.Drawing.Size(Constants.LabelWidth, Constants.LabelHeight);

			string[] words = Regex.Matches(Parameter.ToString(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
											.OfType<Match>()
											.Select(m => m.Value)
											.ToArray();
			Label.Text = string.Join(" ", words);
			
			Field.Margin = new Padding(Constants.ItemPadding);
			Field.Name = Parameter.ToString() + "Input";
			Field.Size = new System.Drawing.Size(Constants.InputWidth, Constants.InputHeight);

			if (control is ComboBox)
			{
				((ComboBox)Field).DropDownStyle = ComboBoxStyle.DropDownList;
				((ComboBox)Field).FormattingEnabled = true;
				((ComboBox)Field).Sorted = true;
				((ComboBox)Field).SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
			}
			else if (control is NumericUpDown)
			{
				((System.ComponentModel.ISupportInitialize)Field).BeginInit();
				((NumericUpDown)Field).ValueChanged += new EventHandler(NumericUpDown_ValueChanged);
				((System.ComponentModel.ISupportInitialize)Field).EndInit();
			}
			else if (control is CheckBox)
			{
				((CheckBox)Field).CheckAlign = System.Drawing.ContentAlignment.TopRight;
				((CheckBox)Field).CheckStateChanged += new EventHandler(CheckBox_CheckStateChanged);
			}
			else if (control is TextBox)
			{
				((TextBox)Field).BackColor = System.Drawing.Color.White;
			}
		}

		public void Draw(int index)
		{
			Label.Location = new System.Drawing.Point(Constants.LabelLocationX,

													Constants.GroupBoxFirstItemY +
													index * (Constants.InputHeight + 2 * Constants.ItemPadding) +
													Constants.LabelToBoxOffset);
			Field.Location = new System.Drawing.Point(Constants.LabelLocationX +
													Constants.LabelWidth +
													2 * Constants.ItemPadding,

													Constants.GroupBoxFirstItemY +
													index * (Constants.InputHeight + 2 * Constants.ItemPadding));

			Field.TabIndex = index;
		}

		private async void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(Parameter, ((ComboBox)sender).SelectedIndex);
		}

		private async void NumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(Parameter, Decimal.ToInt32(((NumericUpDown)sender).Value));
		}
		
		private async void CheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(Parameter, ((((CheckBox)sender).CheckState == CheckState.Checked) ? 1 : 0));
		}
	}
}
