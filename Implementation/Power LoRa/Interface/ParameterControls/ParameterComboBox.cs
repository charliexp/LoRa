using Power_LoRa.Connection.Messages;
using Power_LoRa.Node;
using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Nodes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Interface.Node.ParameterControls
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
        private async Task ParameterChangedCallback(int value)
        {
            Connection.Messages.Message message = new Connection.Messages.Message(parameter, value);

            if (!remotelyChanged)
                await Program.connectionHandler.WriteAsync(new Frame(((BaseNodeGroupBox)Field.Parent.Parent.Parent).Address, message));
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
