using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class SpinBoxControl : LabeledControl
	{
		protected ValueChanged valueChangedCallback;

		public SpinBoxControl(string name, int minValue, int maxValue, int defaultValue) : base(name)
		{
			field = new NumericUpDown
			{
				Margin = field.Margin,
				Name = field.Name,
				Size = field.Size,

		};
			((System.ComponentModel.ISupportInitialize)field).BeginInit();
			((NumericUpDown)field).Maximum = new decimal(new int[] { maxValue, 0, 0, 0 });
			((NumericUpDown)field).Minimum = new decimal(new int[] { minValue, 0, 0, 0 });
			((NumericUpDown)field).Value = new decimal(new int[] { defaultValue, 0, 0, 0 });
			((NumericUpDown)field).ValueChanged += new EventHandler(ValueChanged);
			((System.ComponentModel.ISupportInitialize)field).EndInit();
		}
		
		private async void ValueChanged(object sender, EventArgs e)
		{
			if (valueChangedCallback != null)
				await valueChangedCallback(Decimal.ToInt32(((NumericUpDown)sender).Value));
		}
	}
}
