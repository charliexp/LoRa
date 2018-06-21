using LoRa_Controller.Connection.Messages;
using System;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Log
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
            Columns.Add("RSSI");
            Columns.Add("SNR");
        }
        #endregion

        #region Public methods
        public void Write(Frame frame)
        {
            if (Enabled)
            {
                ListViewItem item = new ListViewItem(frame.Timestamp.ToString("HH:mm:ss.fff"));

                item.SubItems.Add(frame.EndDevice.ToString());
                item.SubItems.Add(frame.Messages[0].Command.ToString());
                item.SubItems.Add(frame.Messages[0].PrintableArgument);
                item.SubItems.Add((-frame.RSSI).ToString());
                item.SubItems.Add(frame.SNR.ToString());
                
                Items.Add(item);
                if (Items.Count > maxEntries)
                    Items.RemoveAt(0);
                TopItem = Items[Items.Count - 1];

                AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
        #endregion
    }
}
