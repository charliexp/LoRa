using LoRa_Controller.Device;
using LoRa_Controller.Interface.Controls;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.Interface.Node.ParameterControls
{
	public class ParameterCheckBox : CheckBoxControl
    {
        #region Private variables
        private CommandType parameter;
		private bool remotelyChanged;
        #endregion

        #region Constructors
        public ParameterCheckBox(CommandType parameter, bool defaultState) : base(parameter.ToString(), defaultState)
		{
			this.parameter = parameter;
            ValueChanged = ParameterChangedCallback;
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
        public void SetValue(bool value)
		{
			remotelyChanged = true;
			((CheckBox)Field).Checked = value;
        }
        #endregion
    }
}
