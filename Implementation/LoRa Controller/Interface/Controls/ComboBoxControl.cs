using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class ComboBoxControl : LabeledControl
	{
		protected ValueChanged indexChangedCallback;

		public ComboBoxControl(string name, List<string> values, int defaultIndex) : base(name)
		{
			field = new ComboBox
			{
				Margin = field.Margin,
				Name = field.Name,
				Size = field.Size,
				DropDownStyle = ComboBoxStyle.DropDownList,
				FormattingEnabled = true,
				Sorted = true
			};
			((ComboBox)field).Items.AddRange(values.ToArray());
			((ComboBox)field).SelectedIndex = defaultIndex;
			((ComboBox)field).SelectedIndexChanged += new EventHandler(SelectedIndexChanged);
		}
		
		private async void SelectedIndexChanged(object sender, EventArgs e)
		{
			if (indexChangedCallback != null)
				await indexChangedCallback(((ComboBox)sender).SelectedIndex);
		}
	}
}
