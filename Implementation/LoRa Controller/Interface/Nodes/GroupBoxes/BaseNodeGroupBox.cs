using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.ParameterControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static LoRa_Controller.Device.BaseDevice;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.GroupBoxes
{
	public abstract class BaseNodeGroupBox : GroupBox
	{
		public int Address
		{
			get
			{
				return Int32.Parse(((TextBox)AddressControl.field).Text);
			}
			set
			{
				((TextBox)AddressControl.field).Text = value.ToString();
			}
		}

		public TextBoxControl Status;
		public TextBoxControl AddressControl;
		public ParameterComboBox Bandwidth;
		public ParameterSpinBox OutputPower;
		public ParameterSpinBox SpreadingFactor;
		public ParameterComboBox CodingRate;
		public ParameterSpinBox RxSymTimeout;
		public ParameterSpinBox RxMsTimeout;
		public ParameterSpinBox TxTimeout;
		public ParameterSpinBox PreambleSize;
		public ParameterSpinBox PayloadMaxSize;
		public ParameterCheckBox VariablePayload;
		public ParameterCheckBox PerformCRC;

		public List<BaseControl> statusControls;
		public List<BaseControl> LoRaControls;

		public BaseNodeGroupBox(string name) : base()
		{
			Status = new TextBoxControl("Status", TextBoxControl.Type.Output);
			AddressControl = new TextBoxControl("Address", TextBoxControl.Type.Input);
			Bandwidth = new ParameterComboBox(Commands.Bandwidth, new List<string> { "125 kHz", "250 kHz", "500 kHz" }, 0);
			OutputPower = new ParameterSpinBox(Commands.OutputPower, 1, 14, 14);
			SpreadingFactor = new ParameterSpinBox(Commands.SpreadingFactor, 7, 12, 12);
			CodingRate = new ParameterComboBox(Commands.CodingRate, new List<string> { "4/5", "4/6", "4/7", "4/8" }, 3);
			RxSymTimeout = new ParameterSpinBox(Commands.RxSymTimeout, 1, 30, 5);
			RxMsTimeout = new ParameterSpinBox(Commands.RxMsTimeout, 1, 10000, 5000);
			TxTimeout = new ParameterSpinBox(Commands.TxTimeout, 1, 10000, 5000);
			PreambleSize = new ParameterSpinBox(Commands.PreambleSize, 2, 30, 8);
			PayloadMaxSize = new ParameterSpinBox(Commands.PayloadMaxSize, 1, 64, 64);
			VariablePayload = new ParameterCheckBox(Commands.VariablePayload, true);
			PerformCRC = new ParameterCheckBox(Commands.PerformCRC, true);

			statusControls = new List<BaseControl>
			{
				Status,
				AddressControl,
			};

			LoRaControls = new List<BaseControl>
			{
				Bandwidth,
				OutputPower,
				SpreadingFactor,
				CodingRate,
				RxSymTimeout,
				RxMsTimeout,
				TxTimeout,
				PreambleSize,
				PayloadMaxSize,
				VariablePayload,
				PerformCRC
			};
			
			Name = name.Replace(" ", "") + "GroupBox";
			Text = name;
			Margin = new Padding(InterfaceConstants.ItemPadding);
			Padding = new Padding(InterfaceConstants.ItemPadding);
			TabStop = false;
		}
		
		public void Draw(int groupBoxIndex)
		{
			int controlIndex = 0;
			
			SuspendLayout();
			
			foreach (BaseControl control in statusControls)
			{
				control.Draw(controlIndex++);
				if (control is LabeledControl)
					Controls.Add(((LabeledControl)control).label);
				Controls.Add(control.field);
			}

			foreach (BaseControl control in LoRaControls)
			{
				control.Draw(controlIndex++);
				if (control is LabeledControl)
					Controls.Add(((LabeledControl)control).label);
				Controls.Add(control.field);
			}

			Width = 2 * InterfaceConstants.LabelLocationX +
				InterfaceConstants.LabelWidth +
				InterfaceConstants.InputWidth +
				InterfaceConstants.ItemPadding;
			Height = InterfaceConstants.GroupBoxFirstItemY +
				Controls.Count * InterfaceConstants.InputHeight +
				(Controls.Count - 1) * InterfaceConstants.ItemPadding +
				InterfaceConstants.GroupBoxLastItemY;

			Location = new Point(InterfaceConstants.GroupBoxLocationX +
				groupBoxIndex * (Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);

			ResumeLayout(true);
		}

		public void UpdateConnectedStatus(bool connected)
		{
			if (connected)
			{
				((TextBox)Status.field).Text = "Connected";
				((TextBox)Status.field).BackColor = Color.LightGreen;
			}
			else
			{
				((TextBox)Status.field).Text = "Disconnected";
				((TextBox)Status.field).BackColor = Color.PaleVioletRed;
			}
		}
	}
}
