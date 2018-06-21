﻿using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class SpinBoxControl : LabeledControl
    {
        #region Constructors
        public SpinBoxControl(string name, int minValue, int maxValue, int defaultValue) : base(name)
		{
			Field = new NumericUpDown
            {
                Dock = Field.Dock,
                Margin = Field.Margin,
				Name = Field.Name,
                Size = Field.Size,
            };
			((System.ComponentModel.ISupportInitialize)Field).BeginInit();
			((NumericUpDown)Field).Maximum = new decimal(new int[] { maxValue, 0, 0, 0 });
			((NumericUpDown)Field).Minimum = new decimal(new int[] { minValue, 0, 0, 0 });
			((NumericUpDown)Field).Value = new decimal(new int[] { defaultValue, 0, 0, 0 });
			((NumericUpDown)Field).ValueChanged += new EventHandler(IndexChanged);
			((System.ComponentModel.ISupportInitialize)Field).EndInit();
        }
        #endregion

        #region Public methods
        private async void IndexChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				await ValueChanged(Decimal.ToByte(((NumericUpDown)sender).Value));
        }
        #endregion
    }
}