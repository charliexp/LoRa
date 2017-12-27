using LoRa_Controller.Interface.Controls;
using System.Threading.Tasks;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterComboBox : ComboBoxControl
	{
		private Commands parameter;

		public ParameterComboBox(Commands parameter) : base(parameter.ToString(), null)
		{
			this.parameter = parameter;
			indexChangedCallbackAsync = ParameterChangedCallbackAsync;
		}
		
		private async Task ParameterChangedCallbackAsync(int value)
		{
			if (Program.DeviceHandler != null)
				await Program.DeviceHandler.SendCommandAsync(parameter, value);
		}
	}
}
