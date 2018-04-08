using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.ParameterControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static LoRa_Controller.Device.Message;
using static LoRa_Controller.DirectConnection.BaseConnectionHandler;

namespace LoRa_Controller.Interface.Node.GroupBoxes
{
	public abstract class BaseNodeGroupBox : GroupBox
    {
        #region Properties
        public int Address
		{
			get
			{
				return Int32.Parse(((TextBox)AddressControl.Field).Text);
			}
			set
			{
				((TextBox)AddressControl.Field).Text = value.ToString();
			}
		}
        //TODO: set to properties with field value handled in get/set. Also in derived classes
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
        #endregion

        #region Constructors
        public BaseNodeGroupBox(string name) : base()
		{
			Status = new TextBoxControl("Status", TextBoxControl.Type.Output);
			AddressControl = new TextBoxControl("Address", TextBoxControl.Type.Input);
			Bandwidth = new ParameterComboBox(CommandType.Bandwidth, new List<string> { "125 kHz", "250 kHz", "500 kHz" }, 0);
			OutputPower = new ParameterSpinBox(CommandType.OutputPower, 1, 14, 14);
			SpreadingFactor = new ParameterSpinBox(CommandType.SpreadingFactor, 7, 12, 12);
			CodingRate = new ParameterComboBox(CommandType.CodingRate, new List<string> { "4/5", "4/6", "4/7", "4/8" }, 3);
			RxSymTimeout = new ParameterSpinBox(CommandType.RxSymTimeout, 1, 30, 5);
			RxMsTimeout = new ParameterSpinBox(CommandType.RxMsTimeout, 1, 10000, 5000);
			TxTimeout = new ParameterSpinBox(CommandType.TxTimeout, 1, 10000, 5000);
			PreambleSize = new ParameterSpinBox(CommandType.PreambleSize, 2, 30, 8);
			PayloadMaxSize = new ParameterSpinBox(CommandType.PayloadMaxSize, 1, 64, 64);
			VariablePayload = new ParameterCheckBox(CommandType.VariablePayload, true);
			PerformCRC = new ParameterCheckBox(CommandType.PerformCRC, true);

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

			AutoSize = true;
			Name = name.Replace(" ", "") + "GroupBox";
			Text = name;
			Margin = new Padding(InterfaceConstants.ItemPadding);
			Padding = new Padding(InterfaceConstants.ItemPadding);
			TabStop = false;
        }
        #endregion

        #region Public methods
        public void Draw(int groupBoxIndex)
		{
			int controlIndex = 0;
			
			SuspendLayout();

			foreach (BaseControl control in statusControls)
			{
				control.Draw(controlIndex++);
				if (control is LabeledControl)
					Controls.Add(((LabeledControl)control).label);
				Controls.Add(control.Field);
			}

			foreach (BaseControl control in LoRaControls)
			{
				control.Draw(controlIndex++);
				if (control is LabeledControl)
					Controls.Add(((LabeledControl)control).label);
				Controls.Add(control.Field);
			}
            //TODO: use flow layout so draws are no longer needed
			Location = new Point(InterfaceConstants.GroupBoxLocationX +
				groupBoxIndex * (Width + InterfaceConstants.GroupBoxLocationX),
				InterfaceConstants.GroupBoxLocationY);

			ResumeLayout(true);
		}

		public void UpdateConnectedStatus(bool connected)
		{
			if (connected)
			{
				((TextBox)Status.Field).Text = "Connected";
				((TextBox)Status.Field).BackColor = Color.LightGreen;
			}
			else
			{
				((TextBox)Status.Field).Text = "Disconnected";
				((TextBox)Status.Field).BackColor = Color.PaleVioletRed;
			}
        }
        #endregion
    }
}
