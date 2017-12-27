using System.Collections.Generic;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Connection
{
	public abstract class BaseConnection
	{
		#region Constructors
		public BaseConnection()
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
