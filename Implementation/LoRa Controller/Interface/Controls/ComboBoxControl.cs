using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class ComboBoxControl : BaseControl
	{
		protected ValueChangedAsync indexChangedCallbackAsync;
		protected ValueChanged indexChangedCallback;

		private ComboBoxControl(string name) : base(name)
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
		}

		public ComboBoxControl(string name, ValueChanged callback) : this(name)
		{
			indexChangedCallback = callback;
			((ComboBox)field).SelectedIndexChanged += new EventHandler(SelectedIndexChanged);
		}

		public ComboBoxControl(string name, ValueChangedAsync callback) : this(name)
		{
			indexChangedCallbackAsync = callback;
			((ComboBox)field).SelectedIndexChanged += new EventHandler(SelectedIndexChangedAsync);
		}

		private void SelectedIndexChanged(object sender, EventArgs e)
		{
			indexChangedCallback(((ComboBox)sender).SelectedIndex);
		}

		private async void SelectedIndexChangedAsync(object sender, EventArgs e)
		{
			await indexChangedCallbackAsync(((ComboBox)sender).SelectedIndex);
		}
	}
}
