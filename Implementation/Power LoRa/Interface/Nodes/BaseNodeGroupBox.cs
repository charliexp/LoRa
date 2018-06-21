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
        private TableLayoutPanel powerReadingLayout;
        private TableLayoutPanel powerSetupLayout;

        private Graph graph;

        private ParameterCheckBox hasMeter;
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
        public List<ParameterCheckBox> Outputs { get; set; }
        #endregion

        #region Constructors
        public BaseNodeGroupBox(EventHandler setAddressEvent, EventHandler isPresentEvent, string name) : base()
        {
            List<Label> horizontalSeparators = new List<Label>
            {
                new Label
                {
                    AutoSize = false,
                    Height = 2,
                    BorderStyle = BorderStyle.Fixed3D,
                    Dock = DockStyle.Fill,
                },
                new Label
                {
                    AutoSize = false,
                    Height = 2,
                    BorderStyle = BorderStyle.Fixed3D,
                    Dock = DockStyle.Fill,
                }
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
            hasMeter = new ParameterCheckBox(CommandType.HasMeter, true);
            Outputs = new List<ParameterCheckBox>();
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
            mainLayout.Controls.Add(radioLayout);
            mainLayout.Controls.Add(horizontalSeparators[0]);
            mainLayout.Controls.Add(graph);
            mainLayout.Controls.Add(powerReadingLayout);
            mainLayout.Controls.Add(horizontalSeparators[1]);
            mainLayout.Controls.Add(powerSetupLayout);
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
            foreach (ParameterCheckBox output in Outputs)
            {
                powerSetupLayout.Controls.Add(output.Label);
                powerSetupLayout.Controls.Add(output.Field);
            }

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = name.Replace(" ", "") + "GroupBox";
            Text = name;
            TabStop = false;
            
            setAddress.Field.Click += new EventHandler(setAddressEvent);
            checkIfPresent.Field.Click += new EventHandler(isPresentEvent);
            addressControl.Field.TextChanged += new EventHandler(AddressFieldChanged);
        }
        #endregion

        #region Private methods
        private void AddressFieldChanged(object sender, EventArgs e)
        {
            try
            {
                NewAddress = Byte.Parse(((TextBox)addressControl.Field).Text);
                //TODO: check for used addresses
                if (NewAddress != Address &&
                    NewAddress != (byte) AddressType.Broadcast &&
                    NewAddress != (byte) AddressType.PC)
                    setAddress.Field.Enabled = true;
                else
                    setAddress.Field.Enabled = false;
            }
            catch
            {
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
        public void UpdateInterface(object target, List<Compensator> value)
        {
            if (target.GetType() == Outputs.GetType())
            {
                Outputs = new List<ParameterCheckBox>();

                foreach(Compensator compensator in value)
                    Outputs.Add(new ParameterCheckBox(CommandType.SetCompensator, compensator.Value + " " + Compensator.MeasureUnit, compensator.Position, false));

                powerSetupLayout.Controls.Remove(addPowerOutput.Field);
                foreach (ParameterCheckBox output in Outputs)
                {
                    powerSetupLayout.Controls.Add(output.Label);
                    powerSetupLayout.Controls.Add(output.Field);
                }
                powerSetupLayout.Controls.Add(addPowerOutput.Field);
            }
        }
        public void UpdateInterface(BaseControl target, double value)
        {
            UpdateInterface(target, value.ToString("0.##"));
        }
        public void UpdateInterface(BaseControl target, int value)
        {
            if (target.GetType() == TransmissionRate.GetType())
                ((ParameterSpinBox)target).SetValue(value);
            else
                UpdateInterface(target, value.ToString());
        }
        public void UpdateInterface(BaseControl target, string value)
        {
            if (target.GetType() == LastReadingTime.GetType())
                ((TextBoxControl)target).Value = value;
        }
        #endregion
    }
}
