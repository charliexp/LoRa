using System.Windows.Forms;

namespace LoRa_Controller.Interface.NodeUI
{
	public class DirectlyConnectedUI : BaseNodeUI
	{
		public DirectlyConnectedUI() : base()
		{
		}
		
		public override void Draw()
		{
			int index = 0;

			groupBox.SuspendLayout();
			
			foreach (RadioSettingControl control in radioParameters.list)
			{
				control.Draw(index++);
				groupBox.Controls.Add(control.Label);
				groupBox.Controls.Add(control.Field);
			}

			groupBox.Location = new System.Drawing.Point(Constants.GroupBoxLocationX, Constants.GroupBoxLocationY);
			groupBox.Margin = new Padding(Constants.ItemPadding);
			groupBox.Name = "DirectlyConnectedGroupBox";
			groupBox.Padding = new Padding(Constants.ItemPadding);
			groupBox.TabStop = false;

			groupBox.Size = new System.Drawing.Size(groupBoxWidth,

													Constants.GroupBoxFirstItemY +
													index * Constants.InputHeight +
													(index - 1) * 2 * Constants.ItemPadding +
													Constants.LabelLocationY);
			groupBox.Text = "Directly connected device";

			groupBox.ResumeLayout(false);
			groupBox.PerformLayout();
		}
	}
}
