using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Log
{
	public class LogGroupBox : GroupBox
	{
		public LogListView listView;
		public ListBox listBox;
		public Button changeFolderButton;
		public TextBox FolderTextBox;
		public Label FolderLabel;

		private const int logMaxEntries = 12;

		public LogGroupBox() : base()
		{
			listView = new LogListView();
			listBox = new ListBox();
			changeFolderButton = new Button();
			FolderTextBox = new TextBox();
			FolderLabel = new Label();

			// 
			// logListBox
			// 
			listBox.ItemHeight = 16;
			listBox.Location = new Point(8, 91);
			listBox.Margin = new Padding(4, 4, 4, 4);
			listBox.Name = "logListBox";
			listBox.SelectionMode = SelectionMode.None;
			listBox.Size = new Size(300, listBox.ItemHeight * logMaxEntries);
			listBox.TabIndex = 0;

			listView.Location = new Point(InterfaceConstants.LabelLocationX, 91 + listBox.Size.Height);
			listView.Margin = new Padding(4, 4, 4, 4);
			listView.Name = "logListView";
			listView.Size = new Size(300, 16 * logMaxEntries);
			// 
			// logGroupBox
			// 
			Controls.Add(changeFolderButton);
			Controls.Add(FolderTextBox);
			Controls.Add(listBox);
			Controls.Add(FolderLabel);
			Controls.Add(listView);

			AutoSize = true;
			Name = "logGroupBox";
			Text = "Log";
			Margin = new Padding(InterfaceConstants.ItemPadding);
			Padding = new Padding(InterfaceConstants.ItemPadding);
			Size = new Size(256, 299);
			// 
			// changeLogFolderButton
			// 
			changeFolderButton.Location = new Point(148, 19);
			changeFolderButton.Margin = new Padding(4, 4, 4, 4);
			changeFolderButton.Name = "changeLogFolderButton";
			changeFolderButton.Size = new Size(100, 28);
			changeFolderButton.TabIndex = 14;
			changeFolderButton.Text = "Change";
			changeFolderButton.UseVisualStyleBackColor = true;
			changeFolderButton.Click += new System.EventHandler(ChangeLogFolderButton_Click);
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
				listBox.Items.Add(data);
				if (listBox.Items.Count > logMaxEntries)
					listBox.Items.RemoveAt(0);
				listBox.TopIndex = listBox.Items.Count - 1;
			}
		}

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

	}
}
