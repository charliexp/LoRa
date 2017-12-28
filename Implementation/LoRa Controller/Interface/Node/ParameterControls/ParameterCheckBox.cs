using LoRa_Controller.Interface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterCheckBox : CheckBoxControl
	{
		private Commands parameter;

		public ParameterCheckBox(Commands parameter, bool defaultState) : base(parameter.ToString(), defaultState)
		{
			this.parameter = parameter;
			checkChangedCallback = ParameterChangedCallback;
		}

		private async Task ParameterChangedCallback(int value)
		{
			if (Program.DirectDevice != null)
				await Program.DirectDevice.SendCommandAsync(parameter, value);
		}
	}
}
