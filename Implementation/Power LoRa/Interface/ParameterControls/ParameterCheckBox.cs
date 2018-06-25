using Power_LoRa.Connection.Messages;
using Power_LoRa.Node;
using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Message;
using System;

namespace Power_LoRa.Interface.Node.ParameterControls
{
	public class ParameterCheckBox : CheckBoxControl
    {
        #region Private variables
        private readonly CommandType command;
        private readonly byte argument;
		private bool remotelyChanged;
        #endregion

        #region Constructors
        public ParameterCheckBox(Control container, CommandType command, bool defaultState) : base(container, command.ToString(), defaultState)
		{
            argument = Byte.MaxValue;
			this.command = command;
            ValueChanged = ParameterChangedCallback;
			remotelyChanged = false;
        }
        public ParameterCheckBox(Control container, CommandType command, string text, byte argument, bool defaultState) : this(container, command, defaultState)
        {
            Label.Text = text;
            this.argument = argument;
        }
        #endregion

        #region Private methods
        private async Task ParameterChangedCallback(int value)
        {
            Connection.Messages.Message message;
            if (argument != Byte.MaxValue)
                message = new Connection.Messages.Message(command, (byte) ((argument << 4 |  value) & 0xFF));
            else
                message = new Connection.Messages.Message(command, (byte)value);

            if (!remotelyChanged)
                await Program.connectionHandler.WriteAsync(new Frame(((BaseNodeGroupBox)container).Address, message));
            else
				remotelyChanged = false;
        }
        #endregion

        #region Public methods
        public void SetValue(bool value)
		{
			//remotelyChanged = true;
			((CheckBox)Field).Checked = value;
        }
        #endregion
    }
}
