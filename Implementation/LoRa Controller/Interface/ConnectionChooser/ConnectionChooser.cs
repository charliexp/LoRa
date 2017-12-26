using System.Collections.Generic;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.ConnectionChooser
{
	public abstract class ConnectionChooser
	{
		#region Constructors
		public ConnectionChooser()
		{
			ParameterLabels = new List<Label>();
			ParameterBoxes = new List<Control>();
		}
		#endregion

		#region Public variables
		public List<Label> ParameterLabels;
		public List<Control> ParameterBoxes;
		#endregion
	}
}
