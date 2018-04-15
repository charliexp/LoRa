using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Log
{
	public class LogGroupBox : GroupBox
    {
        #region Private variables
        public FlowLayoutPanel layout;
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
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = "logGroupBox";
            Text = "Log";

            layout = new FlowLayoutPanel
            {
                AutoSize = true,
                Name = "layout",
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(InterfaceConstants.ItemPadding, InterfaceConstants.GroupBoxFirstItemY),
            };
            FolderLabel = new Label
            {
                AutoSize = true,
                Name = "logFolderLabel",
                Text = "Log Folder",
                Anchor = AnchorStyles.Left,
            };
            FolderTextBox = new TextBox
            {
                AutoSize = true,
                Name = "logFolderTextBox",
            };
            ChangeFolderButton = new Button
            {
                Name = "changeLogFolderButton",
                Text = "Change",
                Anchor = AnchorStyles.Right,
            };
            List = new LogListView
            {
                Name = "logListView",
            };
            List.Size = new Size(   List.Columns[0].Width +
                                    List.Columns[1].Width +
                                    List.Columns[2].Width +
                                    List.Columns[3].Width +
                                    List.Columns[4].Width +
                                    List.Columns[5].Width +
                                    List.Columns[6].Width,

                                    InterfaceConstants.ListHeaderHeight +
                                    InterfaceConstants.ListItemHeight * LogListView.maxVisibleEntries +
                                    InterfaceConstants.ItemPadding);

            Controls.Add(layout);
            layout.Controls.Add(FolderLabel);
            layout.Controls.Add(FolderTextBox);
            layout.Controls.Add(ChangeFolderButton);
            layout.SetFlowBreak(ChangeFolderButton, true);
            layout.Controls.Add(List);

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
		public void Update(Device.Message message)
		{
            List.Write(message);
		}
        #endregion
    }
}
