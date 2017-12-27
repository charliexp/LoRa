using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public class CheckBoxControl : BaseControl
	{
		protected ValueChangedAsync checkedChangedCallbackAsync;
		protected ValueChanged checkedChangedCallback;

		public CheckBoxControl(string name) : base(name)
		{
			field = new CheckBox
			{
				Margin = field.Margin,
				Name = field.Name,
				Size = field.Size,
			};
			((CheckBox)field).CheckAlign = System.Drawing.ContentAlignment.TopRight;
		}

		public CheckBoxControl(string name, ValueChanged callback) : this(name)
		{
			checkedChangedCallback = callback;
			((CheckBox)field).CheckStateChanged += new EventHandler(SelectedIndexChanged);
		}

		public CheckBoxControl(string name, ValueChangedAsync callback) : this(name)
		{
			checkedChangedCallbackAsync = callback;
			((CheckBox)field).CheckStateChanged += new EventHandler(SelectedIndexChangedAsync);
		}

		private void SelectedIndexChanged(object sender, EventArgs e)
		{
			checkedChangedCallback(((ComboBox)sender).SelectedIndex);
		}

		private async void SelectedIndexChangedAsync(object sender, EventArgs e)
		{
			await checkedChangedCallbackAsync(((((CheckBox)sender).CheckState == CheckState.Checked) ? 1 : 0));
		}
	}
}
