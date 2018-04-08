using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterSpinBox : SpinBoxControl
	{
		private CommandType parameter;
		private bool remotelyChanged;

		public ParameterSpinBox(CommandType parameter, int minValue, int maxValue, int defaultValue) : base(parameter.ToString(), minValue, maxValue, defaultValue)
		{
			this.parameter = parameter;
			valueChangedCallback = ParameterChangedCallback;
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
			((NumericUpDown)field).Value = value;
		}
	}
}
