using System;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Interface.Log
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
			Columns.Add("Source");
			Columns.Add("Target");
			Columns.Add("Command");
            Columns.Add("Parameter");
            Columns.Add("RSSI");
            Columns.Add("SNR");
        }
        #endregion

        #region Public methods
        public void Write(Device.Message message)
        {
            if (Enabled)
            {
                ListViewItem item = new ListViewItem(message.Timestamp.ToString("HH:mm:ss.fff"));
                string source;
                string target;

                if (message.Source < (byte)AddressType.Beacon || message.Source == (byte)AddressType.PC)
                    source = Enum.GetName(typeof(AddressType), message.Source);
                else
                    source = "Beacon " + message.Source.ToString();
                if (message.Target < (byte)AddressType.Beacon || message.Target == (byte)AddressType.PC)
                    target = Enum.GetName(typeof(AddressType), message.Target);
                else
                    target = "Beacon " + message.Target.ToString();

                item.SubItems.Add(source);
                item.SubItems.Add(target);
                item.SubItems.Add(message.Command.ToString());
                item.SubItems.Add(message.Parameters[0].ToString());
                item.SubItems.Add((-message.RSSI).ToString());
                item.SubItems.Add(message.SNR.ToString());

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
