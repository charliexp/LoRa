using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterComboBox : ComboBoxControl
	{
		private Command parameter;

		public ParameterComboBox(Command parameter, List<string> values, int defaultIndex) : base(parameter.ToString(), values, defaultIndex)
		{
			this.parameter = parameter;
			indexChangedCallback = ParameterChangedCallback;
		}
		
		private async Task ParameterChangedCallback(int value)
		{
			await Program.connectionHandler.SendCommandAsync(((BaseNodeGroupBox)field.Parent).Address, parameter, value);
		}
	}
}
