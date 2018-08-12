using Power_LoRa.Connection.Messages;
using Power_LoRa.Interface;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Frame;

namespace Power_LoRa.Log
{
	public class LogListView : ListView
    {
        #region Public constants
        public const int maxVisibleEntries = 10;
        public const int maxEntries = 1000;
        #endregion

        #region Constructors
        public LogListView() : base()
        {
			View = View.Details;
			AllowColumnReorder = false;
			FullRowSelect = true;
			Sorting = SortOrder.None;

			Columns.Add("Timestamp");
			Columns.Add("Address");
			Columns.Add("Command");
            Columns.Add("Parameter");
            //Columns.Add("RSSI");
            //Columns.Add("SNR");

            Columns[0].Width = InterfaceConstants.ListLongColumnWidth;
            Columns[1].Width = InterfaceConstants.ListLongColumnWidth;
            Columns[2].Width = InterfaceConstants.ListLongColumnWidth * 2;
            Columns[3].Width = InterfaceConstants.ListLongColumnWidth;
        }
        #endregion

        #region Public methods
        public void Write(Frame frame)
        {
            if (Enabled)
            {
                foreach (Connection.Messages.Message message in frame.Messages)
                {
                    ListViewItem item = new ListViewItem(frame.Timestamp.ToString("HH:mm:ss"));

                    if (frame.EndDevice == (byte) AddressType.Broadcast)
                        item.SubItems.Add("Broadcast");
                    else
                        item.SubItems.Add(frame.EndDevice.ToString("X2"));
                    item.SubItems.Add(message.Command.ToString());
                    item.SubItems.Add(message.PrintableArgument);
                    item.SubItems.Add((-frame.RSSI).ToString());
                    item.SubItems.Add(frame.SNR.ToString());

                    Items.Add(item);
                }

                if (Items.Count > maxEntries)
                    Items.RemoveAt(0);
                TopItem = Items[Items.Count - 1];
            }
        }
        #endregion
    }
}
