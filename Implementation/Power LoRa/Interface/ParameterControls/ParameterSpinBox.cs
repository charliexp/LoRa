using Power_LoRa.Connection.Messages;
using Power_LoRa.Device;
using Power_LoRa.Interface.Controls;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Interface.Node.ParameterControls
{
	public class ParameterSpinBox : SpinBoxControl
    {
        #region Private variables
        private CommandType parameter;
		private bool remotelyChanged;
        #endregion

        #region Constructors
        public ParameterSpinBox(CommandType parameter, int minValue, int maxValue, int defaultValue) : base(parameter.ToString(), minValue, maxValue, defaultValue)
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
			((NumericUpDown)Field).Value = value;
        }
        #endregion
    }
}
