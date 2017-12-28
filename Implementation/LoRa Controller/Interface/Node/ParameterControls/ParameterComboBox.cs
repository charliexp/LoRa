using LoRa_Controller.Interface.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Device.DirectDevice;

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
			await Program.DirectDevice.SendCommandAsync(parameter, value);
		}
	}
}
