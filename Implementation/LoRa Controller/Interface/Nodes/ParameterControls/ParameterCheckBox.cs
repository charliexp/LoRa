using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterCheckBox : CheckBoxControl
	{
		private Command parameter;
		private bool remotelyChanged;

		public ParameterCheckBox(Command parameter, bool defaultState) : base(parameter.ToString(), defaultState)
		{
			this.parameter = parameter;
			checkChangedCallback = ParameterChangedCallback;
			remotelyChanged = false;
		}

		private async Task ParameterChangedCallback(int value)
		{
			if (!remotelyChanged)
				await Program.connectionHandler.SendCommandAsync(((BaseNodeGroupBox)field.Parent).Address, parameter, value);
			else
				remotelyChanged = false;
		}

		public void SetValue(bool value)
		{
			remotelyChanged = true;
			((CheckBox)field).Checked = value;
		}
	}
}
