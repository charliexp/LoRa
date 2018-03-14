using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Threading.Tasks;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterCheckBox : CheckBoxControl
	{
		private Command parameter;

		public ParameterCheckBox(Command parameter, bool defaultState) : base(parameter.ToString(), defaultState)
		{
			this.parameter = parameter;
			checkChangedCallback = ParameterChangedCallback;
		}

		private async Task ParameterChangedCallback(int value)
		{
			await Program.connectionHandler.SendCommandAsync(((BaseNodeGroupBox)field.Parent).Address, parameter, value);
		}
	}
}
