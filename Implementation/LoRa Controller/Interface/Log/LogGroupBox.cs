using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Log
{
	public class LogGroupBox : GroupBox
    {
        #region Private constants
        private const int maxEntries = 12;
        #endregion

        #region Properties
        public LogListView ListView { get; private set; }
		public ListBox ListBox { get; private set; }
        public Button ChangeFolderButton { get; private set; }
        public TextBox FolderTextBox { get; private set; }
        public Label FolderLabel { get; private set; }
        #endregion

        #region Constructors
        public LogGroupBox() : base()
		{
			ListView = new LogListView();
			ListBox = new ListBox();
			ChangeFolderButton = new Button();
			FolderTextBox = new TextBox();
			FolderLabel = new Label();

			// 
			// logListBox
			// 
			ListBox.ItemHeight = 16;
			ListBox.Location = new Point(8, 91);
			ListBox.Margin = new Padding(4, 4, 4, 4);
			ListBox.Name = "logListBox";
			ListBox.SelectionMode = SelectionMode.None;
			ListBox.Size = new Size(300, ListBox.ItemHeight * maxEntries);
			ListBox.TabIndex = 0;

			ListView.Location = new Point(InterfaceConstants.LabelLocationX, 91 + ListBox.Size.Height);
			ListView.Margin = new Padding(4, 4, 4, 4);
			ListView.Name = "logListView";
			ListView.Size = new Size(300, 16 * maxEntries);
			// 
			// logGroupBox
			// 
			Controls.Add(ChangeFolderButton);
			Controls.Add(FolderTextBox);
			Controls.Add(ListBox);
			Controls.Add(FolderLabel);
			Controls.Add(ListView);

			AutoSize = true;
			Name = "logGroupBox";
			Text = "Log";
			Margin = new Padding(InterfaceConstants.ItemPadding);
			Padding = new Padding(InterfaceConstants.ItemPadding);
			Size = new Size(256, 299);
			// 
			// changeLogFolderButton
			// 
			ChangeFolderButton.Location = new Point(148, 19);
			ChangeFolderButton.Margin = new Padding(4, 4, 4, 4);
			ChangeFolderButton.Name = "changeLogFolderButton";
			ChangeFolderButton.Size = new Size(100, 28);
			ChangeFolderButton.TabIndex = 14;
			ChangeFolderButton.Text = "Change";
			ChangeFolderButton.UseVisualStyleBackColor = true;
			ChangeFolderButton.Click += new System.EventHandler(ChangeLogFolderButton_Click);
			// 
			// logFolderTextBox
			// 
			FolderTextBox.Location = new Point(8, 59);
			FolderTextBox.Margin = new Padding(4, 4, 4, 4);
			FolderTextBox.Name = "logFolderTextBox";
			FolderTextBox.Size = new Size(239, 22);
			FolderTextBox.TabIndex = 13;
			// 
			// logFolderLabel
			// 
			FolderLabel.AutoSize = true;
			FolderLabel.Location = new Point(8, 30);
			FolderLabel.Margin = new Padding(4, 0, 4, 0);
			FolderLabel.Name = "logFolderLabel";
			FolderLabel.Size = new Size(76, 17);
			FolderLabel.TabIndex = 12;
			FolderLabel.Text = "Log Folder";
        }
        #endregion

        #region Private methods
        private void ChangeLogFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog;

            folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select the folder to store the logs.",
                ShowNewFolderButton = true,
            };

            if (Directory.Exists(FolderTextBox.Text))
                folderBrowserDialog.SelectedPath = FolderTextBox.Text;
            else
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Program.logger.Folder = folderBrowserDialog.SelectedPath;
                FolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        #endregion

        #region Public methods
        public void Draw(int groupBoxIndex)
		{
			SuspendLayout();
			
			Location = new Point(InterfaceConstants.GroupBoxLocationX +
				groupBoxIndex * (Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);

			ResumeLayout(true);
		}
		public void UpdateLog(string data)
		{
			if (Enabled)
			{
				ListBox.Items.Add(data);
				if (ListBox.Items.Count > maxEntries)
					ListBox.Items.RemoveAt(0);
				ListBox.TopIndex = ListBox.Items.Count - 1;
			}
		}
        #endregion
    }
}
