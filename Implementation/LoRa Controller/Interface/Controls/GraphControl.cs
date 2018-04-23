using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
    public class Graph : PictureBox
    {
        #region Constructors
        public Graph(string name) : base()
        {
            Name = name.Replace(" ", "") + "Field";
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill;
        }
        #endregion
    }
}
