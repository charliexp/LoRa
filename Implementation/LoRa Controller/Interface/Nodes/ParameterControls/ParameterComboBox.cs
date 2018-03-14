using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.GroupBoxes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterComboBox : ComboBoxControl
	{
		private Command parameter;
		private bool remotelyChanged;

		public ParameterComboBox(Command parameter, List<string> values, int defaultIndex) : base(parameter.ToString(), values, defaultIndex)
		{
			this.parameter = parameter;
			indexChangedCallback = ParameterChangedCallback;
			remotelyChanged = false;
		}
		
		private async Task ParameterChangedCallback(int value)
		{
			if (!remotelyChanged)
				await Program.connectionHandler.SendCommandAsync(((BaseNodeGroupBox)field.Parent).Address, parameter, value);
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
