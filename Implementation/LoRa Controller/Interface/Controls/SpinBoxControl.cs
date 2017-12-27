using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class SpinBoxControl : BaseControl
	{
		protected ValueChangedAsync valueChangedCallbackAsync;
		protected ValueChanged valueChangedCallback;

		private SpinBoxControl(string name) : base(name)
		{
			field = new NumericUpDown
			{
				Margin = field.Margin,
				Name = field.Name,
				Size = field.Size,
			};
			((System.ComponentModel.ISupportInitialize)field).BeginInit();
			((System.ComponentModel.ISupportInitialize)field).EndInit();
		}

		public SpinBoxControl(string name, ValueChanged callback) : this(name)
		{
			valueChangedCallback = callback;
			((NumericUpDown)field).ValueChanged += new EventHandler(SelectedIndexChanged);
		}

		public SpinBoxControl(string name, ValueChangedAsync callback) : this(name)
		{
			valueChangedCallbackAsync = callback;
			((NumericUpDown)field).ValueChanged += new EventHandler(SelectedIndexChangedAsync);
		}

		private void SelectedIndexChanged(object sender, EventArgs e)
		{
			valueChangedCallback(((ComboBox)sender).SelectedIndex);
		}

		private async void SelectedIndexChangedAsync(object sender, EventArgs e)
		{
			await valueChangedCallbackAsync(Decimal.ToInt32(((NumericUpDown)sender).Value));
		}
	}
}
