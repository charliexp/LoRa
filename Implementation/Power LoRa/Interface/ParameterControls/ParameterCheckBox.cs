using Power_LoRa.Connection.Messages;
using Power_LoRa.Node;
using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Interface.Node.ParameterControls
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
            Connection.Messages.Message message = new Connection.Messages.Message(parameter, value);

            if (!remotelyChanged)
				await Program.connectionHandler.WriteAsync(new Frame(((BaseNodeGroupBox)Field.Parent.Parent.Parent.Parent).Address, message));
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
