﻿using LoRa_Controller.Interface.Controls;
using System.Threading.Tasks;
using static LoRa_Controller.Device.BaseDevice;

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
			if (Program.DirectDevice != null)
				await Program.DirectDevice.SendCommandAsync(parameter, value);
		}
	}
}