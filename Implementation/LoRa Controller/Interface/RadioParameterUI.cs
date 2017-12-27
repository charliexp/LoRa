using LoRa_Controller.Device;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface
{
	public class RadioParameterUI
	{
		public static int Count;
		public Commands Parameter;
		public Label Label;
		public Control Field;

		public RadioParameterUI(Commands parameter, GroupBox parent, Control control)
		{
			Parameter = parameter;
			Label = new Label();
			Field = control;

			Label.Parent = parent;
			Label.AutoSize = true;
			Label.Location = new System.Drawing.Point(Constants.LabelLocationX,

													Constants.GroupBoxFirstItemY +
													Count * (Constants.InputHeight + 2 * Constants.ItemPadding) +
													Constants.LabelToBoxOffset);
			Label.Margin = new Padding(Constants.ItemPadding, 0, Constants.ItemPadding, 0);
			Label.Name = Parameter.ToString() + "Label";
			Label.Size = new System.Drawing.Size(Constants.LabelWidth, Constants.LabelHeight);

			string[] words = Regex.Matches(Parameter.ToString(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
											.OfType<Match>()
											.Select(m => m.Value)
											.ToArray();
			Label.Text = string.Join(" ", words);

			Field.Parent = parent;
			Field.Location = new System.Drawing.Point(Constants.LabelLocationX +
													Constants.LabelWidth +
													2 * Constants.ItemPadding,

													Constants.GroupBoxFirstItemY +
													Count * (Constants.InputHeight + 2 * Constants.ItemPadding));
			Field.Margin = new Padding(Constants.ItemPadding);
			Field.Name = Parameter.ToString() + "Input";
			Field.Size = new System.Drawing.Size(Constants.InputWidth, Constants.InputHeight);

			if (control is ComboBox)
			{
				Field.TabIndex = Count;
				((ComboBox)Field).DropDownStyle = ComboBoxStyle.DropDownList;
				((ComboBox)Field).FormattingEnabled = true;
				((ComboBox)Field).Sorted = true;
				((ComboBox)Field).SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
			}
			else if (control is NumericUpDown)
			{
				Field.TabIndex = Count;
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

			Count++;
		}

		private async void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(Parameter, (byte)((ComboBox)sender).SelectedIndex);
		}

		private async void NumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(Parameter, (byte)(((NumericUpDown)sender).Value));
		}
		
		private async void CheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(Parameter, (byte)((((CheckBox)sender).CheckState == CheckState.Checked) ? 1 : 0));
		}
	}
}
