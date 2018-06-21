﻿using Power_LoRa.Interface;
using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Node.ParameterControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Frame;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Device
{
	public abstract class BaseNodeGroupBox : GroupBox
    {
        #region Private variables
        private TableLayoutPanel mainLayout;
        private Graph graph;
        private TableLayoutPanel secondLayout;
        private TableLayoutPanel powerReadingLayout;
        private TableLayoutPanel powerSetupLayout;

        private TextBoxControl lastReadingTime;
        private TextBoxControl activePower;
        private TextBoxControl inductivePower;
        private TextBoxControl capacitivePower;
        private TextBoxControl reactivePower;
        private TextBoxControl apparentPower;
        private TextBoxControl powerFactor;

        private TextBoxControl transmissionRate;
        private ParameterCheckBox hasMeter;
        private List<CheckBox> outputs;
        private ButtonControl addPowerOutput;

        private ButtonControl checkIfPresent;
        private TextBoxControl addressControl;
        private ButtonControl setAddress;

        private byte address;
        #endregion

        #region Protected variables
        protected TableLayoutPanel radioLayout;
        #endregion

        #region Properties
        public byte NewAddress { get; set; }
        public byte Address
		{
			get
			{
				return address;
			}
			set
			{
                address = value;

                switch ((AddressType)address)
                {
                    case AddressType.Broadcast:
                        Text = "Gateway";
                        break;
                    default:
                        Text = "End device " + address.ToString();
                        break;
                }
                ((TextBox)addressControl.Field).Text = address.ToString();
			}
		}
        /*
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
		public ParameterCheckBox PerformCRC;*/
        #endregion

        #region Constructors
        public BaseNodeGroupBox(string name) : base()
        {
            Label horizontalSeparator = new Label
            {
                AutoSize = false,
                Height = 2,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            Label verticalSeparator = new Label
            {
                AutoSize = false,
                Width = 2,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };

            graph = new Graph("Graph");
            lastReadingTime = new TextBoxControl("LastReading", TextBoxControl.Type.Output)
            {
                Value = "00:00:00"
            };
            activePower = new TextBoxControl("ActivePower", "kWh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            inductivePower = new TextBoxControl("InductivePower", "kVArh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            capacitivePower = new TextBoxControl("CapacitivePower", "kVArh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            reactivePower = new TextBoxControl("ReactivePower", "kVArh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            apparentPower = new TextBoxControl("ApparentPower", "kWh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            powerFactor = new TextBoxControl("PowerFactor", TextBoxControl.Type.Output)
            {
                Value = "1"
            };
            transmissionRate = new TextBoxControl("TransmissionRate", "s", TextBoxControl.Type.Input)
            {
                Value = "5"
            };
            hasMeter = new ParameterCheckBox(CommandType.HasMeter, false);
            outputs = new List<CheckBox>
            {
                new CheckBox()
                {
                    Text = "50 kVArh",
                },
                new CheckBox()
                {
                    Text = "50 kVArh",
                }
            };
            addPowerOutput = new ButtonControl("Add output");
            checkIfPresent = new ButtonControl("Check if present");
            addressControl = new TextBoxControl("Address", TextBoxControl.Type.Input);
            setAddress = new ButtonControl("Set Address");

            mainLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                Name = name + "MainLayout",
                Location = new Point(InterfaceConstants.ItemPadding, InterfaceConstants.GroupBoxFirstItemY),
            };
            secondLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 3,
                Name = name + "SecondLayout",
            };
            secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49));
            secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 2));
            secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49));
            powerReadingLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                Name = name + "PowerReadingLayout",
            };
            powerReadingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            powerReadingLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            radioLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                Name = name + "RadioLayout",
            };
            radioLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            radioLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            powerSetupLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                Name = name + "PowerSetupLayout",
            };
            powerSetupLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            powerSetupLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Controls.Add(mainLayout);
            mainLayout.Controls.Add(graph);
            mainLayout.Controls.Add(secondLayout);
            secondLayout.Controls.Add(powerReadingLayout);
            secondLayout.Controls.Add(verticalSeparator);
            secondLayout.Controls.Add(radioLayout);
            secondLayout.Controls.Add(horizontalSeparator);
            secondLayout.Controls.Add(powerSetupLayout);
            secondLayout.SetRowSpan(verticalSeparator, 3);
            secondLayout.SetRowSpan(radioLayout, 3);
            powerReadingLayout.Controls.Add(lastReadingTime.Label);
            powerReadingLayout.Controls.Add(lastReadingTime.Field);
            powerReadingLayout.Controls.Add(activePower.Label);
            powerReadingLayout.Controls.Add(activePower.Field);
            powerReadingLayout.Controls.Add(inductivePower.Label);
            powerReadingLayout.Controls.Add(inductivePower.Field);
            powerReadingLayout.Controls.Add(capacitivePower.Label);
            powerReadingLayout.Controls.Add(capacitivePower.Field);
            powerReadingLayout.Controls.Add(reactivePower.Label);
            powerReadingLayout.Controls.Add(reactivePower.Field);
            powerReadingLayout.Controls.Add(apparentPower.Label);
            powerReadingLayout.Controls.Add(apparentPower.Field);
            powerReadingLayout.Controls.Add(powerFactor.Label);
            powerReadingLayout.Controls.Add(powerFactor.Field);
            radioLayout.Controls.Add(addressControl.Label);
            radioLayout.Controls.Add(addressControl.Field);
            radioLayout.Controls.Add(checkIfPresent.Field);
            radioLayout.Controls.Add(setAddress.Field);/*
            radioLayout.Controls.Add(Bandwidth.Label);
            radioLayout.Controls.Add(Bandwidth.Field);
            radioLayout.Controls.Add(OutputPower.Label);
            radioLayout.Controls.Add(OutputPower.Field);
            radioLayout.Controls.Add(SpreadingFactor.Label);
            radioLayout.Controls.Add(SpreadingFactor.Field);
            radioLayout.Controls.Add(CodingRate.Label);
            radioLayout.Controls.Add(CodingRate.Field);
            radioLayout.Controls.Add(RxSymTimeout.Label);
            radioLayout.Controls.Add(RxSymTimeout.Field);
            radioLayout.Controls.Add(RxMsTimeout.Label);
            radioLayout.Controls.Add(RxMsTimeout.Field);
            radioLayout.Controls.Add(TxTimeout.Label);
            radioLayout.Controls.Add(TxTimeout.Field);
            radioLayout.Controls.Add(PreambleSize.Label);
            radioLayout.Controls.Add(PreambleSize.Field);
            radioLayout.Controls.Add(PayloadMaxSize.Label);
            radioLayout.Controls.Add(PayloadMaxSize.Field);
            radioLayout.Controls.Add(VariablePayload.Label);
            radioLayout.Controls.Add(VariablePayload.Field);
            radioLayout.Controls.Add(PerformCRC.Label);
            radioLayout.Controls.Add(PerformCRC.Field);*/
            powerSetupLayout.Controls.Add(transmissionRate.Label);
            powerSetupLayout.Controls.Add(transmissionRate.Field);
            powerSetupLayout.Controls.Add(hasMeter.Label);
            powerSetupLayout.Controls.Add(hasMeter.Field);
            foreach (CheckBox output in outputs)
            {
                powerSetupLayout.Controls.Add(output);
                powerSetupLayout.SetColumnSpan(output, 2);
            }
            powerSetupLayout.Controls.Add(addPowerOutput.Field);

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = name.Replace(" ", "") + "GroupBox";
            Text = name;
            TabStop = false;

            setAddress.Field.Click += new EventHandler(Program.SetNewAddress);
            addressControl.Field.TextChanged += new EventHandler(AddressFieldChanged);
        }
        #endregion

        #region Private methods
        private void AddressFieldChanged(object sender, EventArgs e)
        {
            try
            {
                NewAddress = Byte.Parse(((TextBox)addressControl.Field).Text);
                if (NewAddress != Address &&
                    NewAddress != (byte) AddressType.Broadcast &&
                    NewAddress != (byte) AddressType.PC)
                    setAddress.Field.Enabled = true;
                else
                    setAddress.Field.Enabled = false;
            }
            catch
            {
                //TODO: invalid address
                setAddress.Field.Enabled = false;
            }
        }
        #endregion

        #region Protected methods
        protected void AddControlsToLayout()
        {/*
            foreach (BaseControl control in radioParameters)
            {
                if (control is LabeledControl)
                {
                    mainLayout.Controls.Add(((LabeledControl)control).Label);
                    ((LabeledControl)control).Label.Dock = DockStyle.Top;
                }
                mainLayout.Controls.Add(control.Field);
                control.Field.Dock = DockStyle.Fill;
            }*/
        }
        #endregion
    }
}
