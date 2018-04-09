using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Log
{
	public class LogGroupBox : GroupBox
    {
        #region Properties
        public LogListView List { get; private set; }
        public Button ChangeFolderButton { get; private set; }
        public TextBox FolderTextBox { get; private set; }
        public Label FolderLabel { get; private set; }
        #endregion

        #region Constructors
        public LogGroupBox() : base()
		{
            FolderLabel = new Label
            {
                Location = new Point(   InterfaceConstants.LabelLocationX,
                
                                        InterfaceConstants.GroupBoxFirstItemY +
                                        InterfaceConstants.LabelToBoxOffset),
                Margin = new Padding(InterfaceConstants.ItemPadding, 0, InterfaceConstants.ItemPadding, 0),
                Name = "logFolderLabel",
                Size = new Size(InterfaceConstants.ShortLabelWidth, InterfaceConstants.LabelHeight),
                Text = "Log Folder"
            };

            List = new LogListView
            {
                Location = new Point(InterfaceConstants.LabelLocationX,

                                        InterfaceConstants.GroupBoxFirstItemY +
                                        InterfaceConstants.InputHeight +
                                        InterfaceConstants.ItemPadding),
                Margin = new Padding(InterfaceConstants.ItemPadding, 0, InterfaceConstants.ItemPadding, 0),
                Name = "logListView",
                Size = new Size(500,
                                InterfaceConstants.ListHeaderHeight +
                                InterfaceConstants.ListItemHeight * LogListView.maxEntries +
                                InterfaceConstants.ItemPadding)
                                
            };

            FolderTextBox = new TextBox
            {
                Location = new Point(   InterfaceConstants.LabelLocationX +
                                        FolderLabel.Width +
                                        InterfaceConstants.ItemPadding,
                                                    
                                        InterfaceConstants.GroupBoxFirstItemY),
                Margin = new Padding(InterfaceConstants.ItemPadding, 0, InterfaceConstants.ItemPadding, 0),
                Name = "logFolderTextBox",
                Size = new Size(300, InterfaceConstants.InputHeight)
            };

            ChangeFolderButton = new Button
            {
                Location = new Point(   InterfaceConstants.LabelLocationX +
                                        FolderLabel.Width +
                                        FolderTextBox.Width +
                                        2 * InterfaceConstants.ItemPadding,

                                        InterfaceConstants.GroupBoxFirstItemY -
                                        InterfaceConstants.LabelToBoxOffset),
                Margin = new Padding(InterfaceConstants.ItemPadding, 0, InterfaceConstants.ItemPadding, 0),
                Name = "changeLogFolderButton",
                Size = new Size(InterfaceConstants.LabelWidth, InterfaceConstants.InputHeight),
                Text = "Change",
            };
            ChangeFolderButton.Click += new System.EventHandler(ChangeLogFolderButton_Click);

            Controls.Add(ChangeFolderButton);
			Controls.Add(FolderTextBox);
			Controls.Add(FolderLabel);
			Controls.Add(List);

			AutoSize = true;
			Name = "logGroupBox";
			Text = "Log";
			Margin = new Padding(InterfaceConstants.ItemPadding);
			Padding = new Padding(InterfaceConstants.ItemPadding);
			Size = new Size(256, 299);
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
		public void Update(Device.Message message)
		{
            List.Write(message);
		}
        #endregion
    }
}
