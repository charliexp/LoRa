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
		protected ValueChanged checkChangedCallback;

		public CheckBoxControl(string name, bool defaultState) : base(name)
		{
			field = new CheckBox
			{
				Margin = field.Margin,
				Name = field.Name,
				Size = field.Size,
			};
			((CheckBox)field).CheckAlign = System.Drawing.ContentAlignment.TopRight;
			((CheckBox)field).CheckState = defaultState?CheckState.Checked : CheckState.Unchecked;
			((CheckBox)field).CheckStateChanged += new EventHandler(CheckChanged);
		}
		
		private async void CheckChanged(object sender, EventArgs e)
		{
			if (checkChangedCallback != null)
				await checkChangedCallback(((((CheckBox)sender).CheckState == CheckState.Checked) ? 1 : 0));
		}
	}
}
