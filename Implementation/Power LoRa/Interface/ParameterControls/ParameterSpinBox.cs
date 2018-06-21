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
	public class ParameterSpinBox : SpinBoxControl
    {
        #region Private variables
        private CommandType command;
		private bool remotelyChanged;
        #endregion

        #region Constructors
        public ParameterSpinBox(CommandType parameter, int minValue, int maxValue, int defaultValue) : base(parameter.ToString(), minValue, maxValue, defaultValue)
		{
			this.command = parameter;
            ValueChanged = ParameterChangedCallback;
			remotelyChanged = false;
        }
        #endregion

        #region Private methods
        private async Task ParameterChangedCallback(int value)
        {
            Connection.Messages.Message message = new Connection.Messages.Message(command, (Int16) value);

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
			((NumericUpDown)Field).Value = value;
        }
        #endregion
    }
}
