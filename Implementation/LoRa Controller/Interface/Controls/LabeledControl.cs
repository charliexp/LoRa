using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public abstract class LabeledControl : BaseControl
    {
        #region Types
        protected delegate Task ValueChangedDelegate(int index);
        #endregion

        #region Protected variables
        protected ValueChangedDelegate valueChangedDelegate;
        #endregion

        #region Properties
        public Label Label { get; protected set; }
        #endregion

        #region Constructors
        public LabeledControl(string name) : base(name)
		{
            Label = new Label
            {
                Name = name.Replace(" ", "") + "Label",
                TextAlign = ContentAlignment.MiddleLeft,
			};

			string[] words = Regex.Matches(name.ToString(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
											.OfType<Match>()
											.Select(m => m.Value)
											.ToArray();
			Label.Text = string.Join(" ", words);
        }
        #endregion
    }
}
