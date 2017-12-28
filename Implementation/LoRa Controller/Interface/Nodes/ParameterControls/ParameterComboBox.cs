using LoRa_Controller.Interface.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterComboBox : ComboBoxControl
	{
		private Commands parameter;

		public ParameterComboBox(Commands parameter, List<string> values, int defaultIndex) : base(parameter.ToString(), values, defaultIndex)
		{
			this.parameter = parameter;
			indexChangedCallback = ParameterChangedCallback;
		}
		
		private async Task ParameterChangedCallback(int value)
		{
			await Program.connectionHandler.SendCommandAsync(Program.directDevice.Address, parameter, value);
		}
	}
}
