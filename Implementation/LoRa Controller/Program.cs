using LoRa_Controller.Interface;
using System;
using System.Windows.Forms;

namespace LoRa_Controller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			ConnectionTypeDialog connectionTypeDialog = new ConnectionTypeDialog();
			DialogResult result = connectionTypeDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				MainInterface mainInterface = new MainInterface();
				mainInterface.ConnectionType = connectionTypeDialog.connectionType;

				Application.Run(mainInterface);
			}
        }
    }
}
