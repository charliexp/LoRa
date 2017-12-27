using System;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.NodeUI
{
	public class RadioConnectedUI : BaseNodeUI
	{
		private string name;
		private int indexInUI;
		private Label statusLabel;
		private TextBox statusTextBox;
		private Label rssiLabel;
		private TextBox snrTextBox;
		private Label snrLabel;
		private TextBox rssiTextBox;

		public RadioConnectedUI(string name, int indexInUI) : base()
		{
			this.name = name;
			this.indexInUI = indexInUI;
			statusLabel = new Label();
			statusTextBox = new TextBox();
			snrLabel = new Label();
			snrTextBox = new TextBox();
			rssiLabel = new Label();
			rssiTextBox = new TextBox();

			//statusLabel.Parent = groupBox;
			statusLabel.AutoSize = true;
			statusLabel.Margin = new Padding(Constants.ItemPadding, 0, Constants.ItemPadding, 0);
			statusLabel.Name = name.Replace(" ", string.Empty) + "StatusLabel";
			statusLabel.Size = new System.Drawing.Size(Constants.LabelWidth, Constants.LabelHeight);
			statusLabel.Text = "Status";

			//statusTextBox.Parent = groupBox;
			statusTextBox.Margin = new Padding(Constants.ItemPadding);
			statusTextBox.Name = name.Replace(" ", string.Empty) + "StatusTextBox";
			statusTextBox.Size = new System.Drawing.Size(Constants.InputWidth, Constants.InputHeight);
		}

		public override void Draw()
		{
			int index = 0;

			groupBox.SuspendLayout();

			radioParameters.NodeType.Draw(index++);
			groupBox.Controls.Add(radioParameters.NodeType.Label);
			groupBox.Controls.Add(radioParameters.NodeType.Field);
			/*
			DrawExtraElements(ref index);
			groupBox.Controls.Add(statusLabel);
			groupBox.Controls.Add(statusTextBox);

			foreach (RadioSettingControl control in radioParameters.list.GetRange(0, radioParameters.list.Count -1))
			{
				control.Draw(index++);
				groupBox.Controls.Add(control.Label);
				groupBox.Controls.Add(control.Field);
			}
			*/
			groupBox.Location = new System.Drawing.Point(indexInUI * (Constants.GroupBoxLocationX + groupBoxWidth),
														
														Constants.GroupBoxLocationY);
			groupBox.Margin = new Padding(Constants.ItemPadding);
			groupBox.Name = name.Replace(" ", string.Empty) + "GroupBox";
			groupBox.Padding = new Padding(Constants.ItemPadding);
			groupBox.TabStop = false;

			groupBox.Size = new System.Drawing.Size(groupBoxWidth,

													Constants.GroupBoxFirstItemY +
													index * Constants.InputHeight +
													(index - 1) * 2 * Constants.ItemPadding +
													Constants.LabelLocationY);
			groupBox.Text = "Radio device";

			groupBox.ResumeLayout(false);
			groupBox.PerformLayout();
		}

		private void DrawExtraElements(ref int startIndex)
		{
			statusLabel.Location = new System.Drawing.Point(Constants.LabelLocationX,

													Constants.GroupBoxFirstItemY +
													startIndex * (Constants.InputHeight + 2 * Constants.ItemPadding) +
													Constants.LabelToBoxOffset);
			statusTextBox.Location = new System.Drawing.Point(Constants.LabelLocationX +
													Constants.LabelWidth +
													2 * Constants.ItemPadding,

													Constants.GroupBoxFirstItemY +
													startIndex * (Constants.InputHeight + 2 * Constants.ItemPadding));
			startIndex++;
		}
	}
}
