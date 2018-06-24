using Power_LoRa.Connection.Messages;
using Power_LoRa.Interface;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Power_LoRa.Log
{
	public class LogGroupBox : GroupBox
    {
        #region Private variables
        public TableLayoutPanel layout;
        #endregion

        #region Properties
        public LogListView List { get; private set; }
        public Button ChangeFolderButton { get; private set; }
        public TextBox FolderTextBox { get; private set; }
        public Label FolderLabel { get; private set; }
        #endregion

        #region Constructors
        public LogGroupBox() : base()
        {
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = "logGroupBox";
            Text = "Log";

            layout = new TableLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Name = "layout",
                ColumnCount = 3,
            };
            FolderLabel = new Label
            {
                AutoSize = true,
                Name = "logFolderLabel",
                Text = "Log Folder",
            };
            FolderTextBox = new TextBox
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Name = "logFolderTextBox",
            };
            ChangeFolderButton = new Button
            {
                AutoSize = true,
                Name = "changeLogFolderButton",
                Text = "Change",
            };
            List = new LogListView
            {
                AutoSize = true,
                Dock = DockStyle.Fill,
                Name = "logListView",
            };
            List.Height = InterfaceConstants.ListHeaderHeight +
                InterfaceConstants.ListItemHeight * LogListView.maxVisibleEntries +
                InterfaceConstants.ItemPadding;

            Controls.Add(layout);
            layout.Controls.Add(List);
            layout.Controls.Add(FolderLabel);
            layout.Controls.Add(FolderTextBox);
            layout.Controls.Add(ChangeFolderButton);
            layout.SetColumnSpan(List, 3);
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            ChangeFolderButton.Click += new EventHandler(ChangeLogFolderButton_Click);
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
		public void Update(Frame frame)
		{
            List.Write(frame);
		}
        #endregion
    }
}
