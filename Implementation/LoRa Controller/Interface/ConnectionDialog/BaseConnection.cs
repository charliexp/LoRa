using System.Collections.Generic;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.ConnectionDialog
{
	public abstract class BaseConnection
    {
        #region Properties
        public List<Label> ParameterLabels { get; private set; }
        public List<Control> ParameterBoxes { get; private set; }
        #endregion

        #region Constructors
        public BaseConnection()
		{
			ParameterLabels = new List<Label>();
			ParameterBoxes = new List<Control>();
		}
		#endregion
	}
}
