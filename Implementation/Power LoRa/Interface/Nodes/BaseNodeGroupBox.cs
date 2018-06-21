using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Node.ParameterControls;
using Power_LoRa.Node;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Frame;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Interface.Nodes
{
	public class BaseNodeGroupBox : GroupBox
    {
        #region Private variables
        private TableLayoutPanel mainLayout;
        private Graph graph;
        private TableLayoutPanel secondLayout;
        private TableLayoutPanel powerReadingLayout;
        private TableLayoutPanel powerSetupLayout;

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
        public ParameterSpinBox TransmissionRate { get; set; }
        public TextBoxControl LastReadingTime { get; set; }
        public TextBoxControl ActiveEnergy { get; set; }
        public TextBoxControl ReactiveEnergy { get; set; }
        public TextBoxControl ActivePower { get; set; }
        public TextBoxControl ReactivePower { get; set; }
        public TextBoxControl ApparentPower { get; set; }
        public TextBoxControl PowerFactor { get; set; }

        #endregion

        #region Constructors
        public BaseNodeGroupBox(EventHandler setAddressEvent, string name) : base()
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
            LastReadingTime = new TextBoxControl("LastReading", TextBoxControl.Type.Output)
            {
                Value = "00:00:00"
            };
            ActiveEnergy = new TextBoxControl("ActiveEnergy", "kWh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            ReactiveEnergy = new TextBoxControl("ReactiveEnergy", "kVArh", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            ActivePower = new TextBoxControl("ActivePower", "kW", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            ReactivePower = new TextBoxControl("ReactivePower", "kVAr", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            ApparentPower = new TextBoxControl("ApparentPower", "kVA", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            PowerFactor = new TextBoxControl("PowerFactor", TextBoxControl.Type.Output)
            {
                Value = "1"
            };
            TransmissionRate = new ParameterSpinBox(CommandType.TransmissionRate, BaseNode.MinTransmissionRate, BaseNode.MaxTransmissionRate, 5);
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
            powerReadingLayout.Controls.Add(LastReadingTime.Label);
            powerReadingLayout.Controls.Add(LastReadingTime.Field);
            powerReadingLayout.Controls.Add(ActiveEnergy.Label);
            powerReadingLayout.Controls.Add(ActiveEnergy.Field);
            powerReadingLayout.Controls.Add(ReactiveEnergy.Label);
            powerReadingLayout.Controls.Add(ReactiveEnergy.Field);
            powerReadingLayout.Controls.Add(ActivePower.Label);
            powerReadingLayout.Controls.Add(ActivePower.Field);
            powerReadingLayout.Controls.Add(ReactivePower.Label);
            powerReadingLayout.Controls.Add(ReactivePower.Field);
            powerReadingLayout.Controls.Add(ApparentPower.Label);
            powerReadingLayout.Controls.Add(ApparentPower.Field);
            powerReadingLayout.Controls.Add(PowerFactor.Label);
            powerReadingLayout.Controls.Add(PowerFactor.Field);
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
            powerSetupLayout.Controls.Add(TransmissionRate.Label);
            powerSetupLayout.Controls.Add(TransmissionRate.Field);
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
            
            setAddress.Field.Click += new EventHandler(setAddressEvent);
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

        #region Public methods
        public void UpdateInterface(BaseControl sender, double value)
        {
            UpdateInterface(sender, value.ToString("0.##"));
        }
        public void UpdateInterface(BaseControl sender, int value)
        {
            if (sender.GetType() == TransmissionRate.GetType())
                ((ParameterSpinBox)sender).SetValue(value);
            else
                UpdateInterface(sender, value.ToString());
        }
        public void UpdateInterface(BaseControl sender, string value)
        {
            if (sender.GetType() == LastReadingTime.GetType())
                ((TextBoxControl)sender).Value = value;
        }
        #endregion
    }
}
