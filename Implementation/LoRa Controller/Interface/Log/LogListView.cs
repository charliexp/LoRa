using System.Windows.Forms;

namespace LoRa_Controller.Interface.Log
{
	public class LogListView : ListView
    {
        #region Constructors
        public LogListView() : base()
		{
			View = View.Details;
			AllowColumnReorder = false;
			FullRowSelect = true;
			Sorting = SortOrder.None;

			// Create three items and three sets of subitems for each item.
			ListViewItem item1 = new ListViewItem("item1", 0);
			// Place a check mark next to the item.
			item1.Checked = true;
			item1.SubItems.Add("1");
			item1.SubItems.Add("2");
			item1.SubItems.Add("3");
			ListViewItem item2 = new ListViewItem("item2", 1);
			item2.SubItems.Add("4");
			item2.SubItems.Add("5");
			item2.SubItems.Add("6");
			ListViewItem item3 = new ListViewItem("item3", 0);
			// Place a check mark next to the item.
			item3.Checked = true;
			item3.SubItems.Add("7");
			item3.SubItems.Add("8");
			item3.SubItems.Add("9");

			// Create columns for the items and subitems.
			// Width of -2 indicates auto-size.
			Columns.Add("Item Column", -2, HorizontalAlignment.Left);
			Columns.Add("Column 2", -2, HorizontalAlignment.Left);
			Columns.Add("Column 3", -2, HorizontalAlignment.Left);
			Columns.Add("Column 4", -2, HorizontalAlignment.Center);

			//Add the items to the ListView.
			Items.AddRange(new ListViewItem[] { item1, item2, item3 });
        }
        #endregion
    }
}
