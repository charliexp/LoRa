using LoRa_Controller.Interface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterCheckBox : CheckBoxControl
	{
		private Commands parameter;

		public ParameterCheckBox(Commands parameter) : base(parameter.ToString(), null)
		{
			this.parameter = parameter;
			checkedChangedCallbackAsync = ParameterChangedCallbackAsync;
		}

		private async Task ParameterChangedCallbackAsync(int value)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(parameter, value);
		}
	}
}
