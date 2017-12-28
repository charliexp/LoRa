using LoRa_Controller.Interface.Controls;
using System.Threading.Tasks;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

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
			if (Program.directDevice != null)
				await Program.connectionHandler.SendCommandAsync(Program.directDevice.Address, parameter, value);
		}
	}
}
