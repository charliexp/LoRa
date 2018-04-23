using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class CheckBoxControl : LabeledControl
    {
        #region Constructors
        public CheckBoxControl(string name, bool defaultState) : base(name)
		{
			Field = new CheckBox
            {
                Dock = Field.Dock,
                Margin = Field.Margin,
				Name = Field.Name,
                Size = Field.Size,
			};
			((CheckBox)Field).CheckAlign = System.Drawing.ContentAlignment.TopRight;
			((CheckBox)Field).CheckState = defaultState?CheckState.Checked : CheckState.Unchecked;
			((CheckBox)Field).CheckStateChanged += new EventHandler(CheckChanged);
        }
        #endregion

        #region Private methods
        private async void CheckChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				await ValueChanged(((((CheckBox)sender).CheckState == CheckState.Checked) ? 1 : 0));
        }
        #endregion
    }
}
