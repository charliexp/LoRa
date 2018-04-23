using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class ComboBoxControl : LabeledControl
    {
        //TODO: intermediary class between labeledcontrol and those with value changed delegate
        #region Constructors
        public ComboBoxControl(string name, List<string> values, int defaultIndex) : base(name)
		{
			Field = new ComboBox
			{
				Margin = Field.Margin,
				Name = Field.Name,
                Dock = Field.Dock,
                DropDownStyle = ComboBoxStyle.DropDownList,
				FormattingEnabled = true,
                Size = Field.Size,
                Sorted = true
			};
			((ComboBox)Field).Items.AddRange(values.ToArray());
			((ComboBox)Field).SelectedIndex = defaultIndex;
			((ComboBox)Field).SelectedIndexChanged += new EventHandler(SelectedIndexChanged);
        }
        #endregion

        #region Private methods
        private async void SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				await ValueChanged(((ComboBox)sender).SelectedIndex);
        }
        #endregion
    }
}
