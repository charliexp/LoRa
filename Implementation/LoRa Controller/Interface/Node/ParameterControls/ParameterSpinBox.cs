using LoRa_Controller.Interface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterSpinBox : SpinBoxControl
	{
		private Commands parameter;

		public ParameterSpinBox(Commands parameter, int minValue, int maxValue, int defaultValue) : base(parameter.ToString(), minValue, maxValue, defaultValue)
		{
			this.parameter = parameter;
			valueChangedCallback = ParameterChangedCallback;
		}

		private async Task ParameterChangedCallback(int value)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(parameter, value);
		}
	}
}
