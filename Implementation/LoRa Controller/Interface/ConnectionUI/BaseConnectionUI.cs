using System.Collections.Generic;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.ConnectionUI
{
	public abstract class BaseConnectionUI
	{
		#region Constructors
		public BaseConnectionUI()
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
