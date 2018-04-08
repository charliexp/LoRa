using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterComboBox : ComboBoxControl
	{
		private CommandType parameter;
		private bool remotelyChanged;

		public ParameterComboBox(CommandType parameter, List<string> values, int defaultIndex) : base(parameter.ToString(), values, defaultIndex)
		{
			this.parameter = parameter;
			indexChangedCallback = ParameterChangedCallback;
			remotelyChanged = false;
		}
		
		private async Task ParameterChangedCallback(int value)
        {
            Device.Message message = new Device.Message(((BaseNodeGroupBox)field.Parent).Address, parameter, value);

            if (!remotelyChanged)
                await Program.connectionHandler.WriteAsync(message);
			else
				remotelyChanged = false;
		}

		public void SetValue(int value)
		{
			remotelyChanged = true;
			((ComboBox)field).SelectedIndex = value;
		}
	}
}
