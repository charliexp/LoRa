using System.Drawing;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public abstract class BaseControl
    {
        #region Properties
        public Control Field { get; protected set; }
        #endregion

        #region Constructors
        public BaseControl(string name)
		{
			Field = new Control
			{
				Name = name.Replace(" ", "") + "Field",
                //Size = new Size(InterfaceConstants.InputHeight, InterfaceConstants.InputWidth),
			};
        }
        #endregion
    }
}
