using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Threading.Tasks;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterSpinBox : SpinBoxControl
	{
		private Command parameter;

		public ParameterSpinBox(Command parameter, int minValue, int maxValue, int defaultValue) : base(parameter.ToString(), minValue, maxValue, defaultValue)
		{
			this.parameter = parameter;
			valueChangedCallback = ParameterChangedCallback;
		}

		private async Task ParameterChangedCallback(int value)
		{
			await Program.connectionHandler.SendCommandAsync(((BaseNodeGroupBox)field.Parent).Address, parameter, value);
		}
	}
}
