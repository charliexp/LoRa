using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface
{
	public partial class ConnectionTypeDialog : Form
	{
		public ConnectionType connectionType;

		public ConnectionTypeDialog()
		{
			InitializeComponent();
		}

		private void LocalButton_Click(object sender, System.EventArgs e)
		{
			connectionType = ConnectionType.Serial;
		}

		private void RemoteButton_Click(object sender, System.EventArgs e)
		{
			connectionType = ConnectionType.Internet;
		}
	}
}
