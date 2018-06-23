using Power_LoRa.Interface.Controls;
using Power_LoRa.Interface.Node.ParameterControls;
using Power_LoRa.Node;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Power_LoRa.Connection.Messages.Frame;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Interface.Nodes
{
	public class BaseNodeGroupBox : GroupBox
    {
        #region Private constants
        private const int ChartWidth = 70;
        #endregion

        #region Private variables
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel chartsLayout;
        private TableLayoutPanel regulatorLayout;

        private TableLayoutPanel readings;
        private ButtonControl addPowerOutput;

        private TextBoxControl addressControl;
        private ButtonControl checkIfPresent;
        private ButtonControl setAddress;

        private Series activeEnergy;
        private Series reactiveEnergy;
        private Series activePower;
        private Series reactivePower;
        private Series apparentPower;

        private byte address;
        #endregion

        #region Protected variables
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
        public ChartableControl ActiveEnergy { get; set; }
        public ChartableControl ReactiveEnergy { get; set; }
        public ChartableControl ActivePower { get; set; }
        public ChartableControl ReactivePower { get; set; }
        public ChartableControl ApparentPower { get; set; }
        public TextBoxControl PowerFactor { get; set; }
        public List<ParameterCheckBox> Outputs { get; set; }
        public ChartControl EnergyChart { get; private set; }
        public ChartControl PowerChart { get; private set; }
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
            List<Label> verticalSeparators = new List<Label>
            {
                new Label
                {
                    AutoSize = false,
                    Width = 2,
                    BorderStyle = BorderStyle.Fixed3D,
                    Dock = DockStyle.Fill,
                },
                new Label
                {
                    AutoSize = false,
                    Width = 2,
                    BorderStyle = BorderStyle.Fixed3D,
                    Dock = DockStyle.Fill,
                }
            };

            activeEnergy = new Series
            {
                Color = Color.Green,
                YAxisType = AxisType.Primary,
            };
            reactiveEnergy = new Series
            {
                Color = Color.Blue,
                YAxisType = AxisType.Secondary,
            };
            activePower = new Series
            {
                Color = Color.Green,
                YAxisType = AxisType.Primary,
            };
            reactivePower = new Series
            {
                Color = Color.Blue,
                YAxisType = AxisType.Secondary,
            };
            apparentPower = new Series
            {
                Color = Color.Red,
                YAxisType = AxisType.Primary,
            };
            List<Series> energyChartSeries = new List<Series>
            {
                activeEnergy,
                reactiveEnergy
            };
            List<Series> powerChartSeries = new List<Series>
            {
                activePower,
                reactivePower,
                apparentPower
            };

            foreach (Series series in energyChartSeries)
            {
                series.IsVisibleInLegend = true;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.DateTime;
            }
            foreach (Series series in powerChartSeries)
            {
                series.IsVisibleInLegend = true;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.DateTime;
            }

            EnergyChart = new ChartControl(energyChartSeries, "Energies Chart")
            {
                Size = new Size(ChartWidth, ChartWidth),
            };
            PowerChart = new ChartControl(powerChartSeries, "Powers Chart")
            {
                Size = new Size(ChartWidth, ChartWidth),
            };
            LastReadingTime = new TextBoxControl(this, "LastReading", TextBoxControl.Type.Output)
            {
                Value = "00:00:00"
            };
            //ActiveEnergy = new ChartableControl(this, EnergyChart, activeEnergy, "ActiveEnergy", "kWh");
            //ReactiveEnergy = new ChartableControl(this, EnergyChart, reactiveEnergy, "ReactiveEnergy", "kVArh");
            ActivePower = new ChartableControl(this, PowerChart, activePower, "ActivePower", "kW");
            ReactivePower = new ChartableControl(this, PowerChart, reactivePower, "ReactivePower", "kVAr");
            ApparentPower = new ChartableControl(this, PowerChart, apparentPower, "ApparentPower", "kVA");
            PowerFactor = new TextBoxControl(this, "PowerFactor", TextBoxControl.Type.Output)
            {
                Value = "1"
            };
            TransmissionRate = new ParameterSpinBox(this, CommandType.TransmissionRate, BaseNode.MinTransmissionRate, BaseNode.MaxTransmissionRate, 5);
            Outputs = new List<ParameterCheckBox>();
            addPowerOutput = new ButtonControl(this, "Add output");
            checkIfPresent = new ButtonControl(this, "Check if present");
            addressControl = new TextBoxControl(this, "Address", TextBoxControl.Type.Input);
            setAddress = new ButtonControl(this, "Set Address");

            mainLayout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 3,
                Name = name + "MainLayout",
                Location = new Point(InterfaceConstants.ItemPadding, InterfaceConstants.GroupBoxFirstItemY),
            };
            chartsLayout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Name = name + "PowerLayout",
            };
            chartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            regulatorLayout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Name = name + "SetupLayout",
            };
            regulatorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            regulatorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            readings = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Name = name + "PowerReadingLayout",
            };
            readings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            readings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Controls.Add(mainLayout);
            //mainLayout.Controls.Add(verticalSeparators[0]);
            mainLayout.Controls.Add(readings);
            mainLayout.Controls.Add(regulatorLayout);
            //mainLayout.Controls.Add(verticalSeparators[1]);
            mainLayout.Controls.Add(chartsLayout);
            /*mainLayout.SetRowSpan(verticalSeparators[0], 3);
            mainLayout.SetRowSpan(verticalSeparators[1], 3);
            mainLayout.SetColumn(verticalSeparators[0], 1);
            mainLayout.SetColumn(verticalSeparators[0], 3);
            mainLayout.SetColumn(regulatorLayout, 0);
            mainLayout.SetColumn(powerReadingLayout, 2);
            mainLayout.SetColumn(powerLayout, 4);
            */
            chartsLayout.Controls.Add(EnergyChart);
            chartsLayout.Controls.Add(PowerChart);

            readings.Controls.Add(LastReadingTime.Label);
            readings.Controls.Add(LastReadingTime.Field);
            //powerReadingLayout.Controls.Add(ActiveEnergy.Text.Label);
            //powerReadingLayout.Controls.Add(ActiveEnergy.Text.Field);
            //powerReadingLayout.Controls.Add(ReactiveEnergy.Text.Label);
            //powerReadingLayout.Controls.Add(ReactiveEnergy.Text.Field);
            readings.Controls.Add(ActivePower.Text.Label);
            readings.Controls.Add(ActivePower.Text.Field);
            readings.Controls.Add(ReactivePower.Text.Label);
            readings.Controls.Add(ReactivePower.Text.Field);
            //powerReadingLayout.Controls.Add(ApparentPower.Text.Label);
            //powerReadingLayout.Controls.Add(ApparentPower.Text.Field);
            regulatorLayout.Controls.Add(PowerFactor.Label);
            regulatorLayout.Controls.Add(PowerFactor.Field);
            /*regulatorLayout.Controls.Add(addressControl.Label);
            regulatorLayout.Controls.Add(addressControl.Field);
            regulatorLayout.Controls.Add(checkIfPresent.Field);
            regulatorLayout.Controls.Add(setAddress.Field);
            regulatorLayout.Controls.Add(TransmissionRate.Label);
            regulatorLayout.Controls.Add(TransmissionRate.Field);*/
            /*foreach (ParameterCheckBox output in Outputs)
            {
                regulatorLayout.Controls.Add(output.Label);
                regulatorLayout.Controls.Add(output.Field);
            }*/

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = name.Replace(" ", "") + "GroupBox";
            Text = name;
            TabStop = false;

            EnergyChart.Click += new EventHandler(ShowAsBigChart);
            PowerChart.Click += new EventHandler(ShowAsBigChart);
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
        private void ShowAsBigChart(object sender, EventArgs e)
        {
            //Replace power fields with energies
            Program.SetBigChartData((ChartControl) sender);
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
                    Outputs.Add(new ParameterCheckBox(this, CommandType.SetCompensator, compensator.Value + " " + Compensator.MeasureUnit, compensator.Position, false));

                regulatorLayout.Controls.Remove(addPowerOutput.Field);
                foreach (ParameterCheckBox output in Outputs)
                {
                    regulatorLayout.Controls.Add(output.Label);
                    regulatorLayout.Controls.Add(output.Field);
                }
                //regulatorLayout.Controls.Add(addPowerOutput.Field);
            }
        }
        public void UpdateInterface(BaseControl target, double value)
        {
            UpdateInterface(target, value.ToString("0.##"));
        }
        public void UpdateInterface(ChartableControl target, DataPoint value)
        {
            target.LastValue = value;
            Program.mainWindow.BigChart.ResetAutoValues();
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
