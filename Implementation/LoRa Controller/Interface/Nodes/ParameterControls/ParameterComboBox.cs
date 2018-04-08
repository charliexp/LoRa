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
        #region Private variables
        private CommandType parameter;
		private bool remotelyChanged;
        #endregion

        #region Constructors
        public ParameterComboBox(CommandType parameter, List<string> values, int defaultIndex) : base(parameter.ToString(), values, defaultIndex)
		{
			this.parameter = parameter;
			valueChangedDelegate = ParameterChangedCallback;
			remotelyChanged = false;
        }
        #endregion

        #region Private methods
        private async Task ParameterChangedCallback(int value)
        {
            Device.Message message = new Device.Message(((BaseNodeGroupBox)Field.Parent).Address, parameter, value);

            if (!remotelyChanged)
                await Program.connectionHandler.WriteAsync(message);
			else
				remotelyChanged = false;
        }
        #endregion

        #region Public methods
        public void SetValue(int value)
		{
			remotelyChanged = true;
			((ComboBox)Field).SelectedIndex = value;
        }
        #endregion
    }
}
