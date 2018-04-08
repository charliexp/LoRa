using System.Threading.Tasks;
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
				Margin = new Padding(InterfaceConstants.ItemPadding),
				Name = name.Replace(" ", "") + "Field",
				Size = new System.Drawing.Size(InterfaceConstants.InputWidth, InterfaceConstants.InputHeight)
			};
        }
        #endregion

        #region Public methods
        public abstract void Draw(int index);
        #endregion
    }
}
