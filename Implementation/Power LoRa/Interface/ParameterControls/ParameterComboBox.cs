using LoRa_Controller.Connection.Messages;
using LoRa_Controller.Device;
using LoRa_Controller.Interface.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Connection.Messages.Message;

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
			ValueChanged = ParameterChangedCallback;
			remotelyChanged = false;
        }
        #endregion

        #region Private methods
        private async Task ParameterChangedCallback(byte value)
        {
            Connection.Messages.Message message = new Connection.Messages.Message(parameter, value);

            if (!remotelyChanged)
                await Program.connectionHandler.WriteAsync(new Frame(((BaseNodeGroupBox)Field.Parent.Parent.Parent.Parent).Address, message));
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
