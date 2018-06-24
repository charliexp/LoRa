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
        private const int ChartHeight = 43;
        private const int ChartWidth = 73;
        #endregion

        #region Private variables
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel chartsLayout;
        private TableLayoutPanel regulatorLayout;
        private TableLayoutPanel readingsLayout;

        private ButtonControl configureButton;
        private ButtonControl resetButton;
            
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
            }
        }
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
                Legend = "LeftLegend",
                LegendText = "Active energy (kWh)",
            };
            reactiveEnergy = new Series
            {
                Color = Color.Blue,
                YAxisType = AxisType.Secondary,
                Legend = "RightLegend",
                LegendText = "Reactive energy (kVARh)",
            };
            activePower = new Series
            {
                Color = Color.Green,
                YAxisType = AxisType.Primary,
                Legend = "LeftLegend",
                LegendText = "Active power (kW)",
            };
            reactivePower = new Series
            {
                Color = Color.Blue,
                YAxisType = AxisType.Secondary,
                Legend = "RightLegend",
                LegendText = "Reactive power (kVAR)",
            };
            apparentPower = new Series
            {
                Color = Color.Red,
                YAxisType = AxisType.Primary,
                Legend = "LeftLegend",
                LegendText = "Apparent power (kVA)",
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
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.DateTime;
            }
            foreach (Series series in powerChartSeries)
            {
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.DateTime;
            }

            EnergyChart = new ChartControl(energyChartSeries, "Energy")
            {
                Size = new Size(ChartWidth, ChartHeight),
            };
            PowerChart = new ChartControl(powerChartSeries, "Power")
            {
                Size = new Size(ChartWidth, ChartHeight),
            };
            LastReadingTime = new TextBoxControl(this, "LastReading", TextBoxControl.Type.Output);
            ActiveEnergy = new ChartableControl(this, EnergyChart, activeEnergy, "ActiveEnergy", "kWh");
            ReactiveEnergy = new ChartableControl(this, EnergyChart, reactiveEnergy, "ReactiveEnergy", "kVArh");
            ActivePower = new ChartableControl(this, PowerChart, activePower, "ActivePower", "kW");
            ReactivePower = new ChartableControl(this, PowerChart, reactivePower, "ReactivePower", "kVAr");
            ApparentPower = new ChartableControl(this, PowerChart, apparentPower, "ApparentPower", "kVA");
            PowerFactor = new TextBoxControl(this, "PowerFactor", TextBoxControl.Type.Output);
            Outputs = new List<ParameterCheckBox>();
            resetButton = new ButtonControl(this, "Reset");
            configureButton = new ButtonControl(this, "Configure");

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
                Name = name + "ChartsLayout",
            };
            regulatorLayout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Name = name + "RegulatorLayout",
            };
            readingsLayout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Name = name + "ReadingsLayout",
            };
            regulatorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            regulatorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            readingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            readingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Controls.Add(mainLayout);
            mainLayout.Controls.Add(readingsLayout);
            mainLayout.Controls.Add(regulatorLayout);
            mainLayout.Controls.Add(chartsLayout);
            chartsLayout.Controls.Add(EnergyChart);
            chartsLayout.Controls.Add(PowerChart);
            regulatorLayout.Controls.Add(LastReadingTime.Label);
            regulatorLayout.Controls.Add(LastReadingTime.Field);
            readingsLayout.Controls.Add(PowerFactor.Label);
            readingsLayout.Controls.Add(PowerFactor.Field);
            readingsLayout.Controls.Add(ActivePower.Text.Label);
            readingsLayout.Controls.Add(ActivePower.Text.Field);
            readingsLayout.Controls.Add(ReactivePower.Text.Label);
            readingsLayout.Controls.Add(ReactivePower.Text.Field);
            chartsLayout.Controls.Add(resetButton.Field);
            chartsLayout.Controls.Add(configureButton.Field);

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = name.Replace(" ", "") + "GroupBox";
            Text = name;
            TabStop = false;

            EnergyChart.Click += new EventHandler(ShowAsBigChart);
            PowerChart.Click += new EventHandler(ShowAsBigChart);
            resetButton.Field.Click += new EventHandler(ResetClicked);
            configureButton.Field.Click += new EventHandler(ConfigureClicked);
        }
        #endregion

        #region Private methods
        private void ResetClicked(object sender, EventArgs e)
        {

        }
        private void ConfigureClicked(object sender, EventArgs e)
        {

        }
        /*
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
        }*/
        private void ShowAsBigChart(object sender, EventArgs e)
        {
            if (((ChartControl) sender).Equals(EnergyChart))
            {
                readingsLayout.Controls.Remove(ActivePower.Text.Label);
                readingsLayout.Controls.Remove(ActivePower.Text.Field);
                readingsLayout.Controls.Remove(ReactivePower.Text.Label);
                readingsLayout.Controls.Remove(ReactivePower.Text.Field);
                readingsLayout.Controls.Add(ActiveEnergy.Text.Label);
                readingsLayout.Controls.Add(ActiveEnergy.Text.Field);
                readingsLayout.Controls.Add(ReactiveEnergy.Text.Label);
                readingsLayout.Controls.Add(ReactiveEnergy.Text.Field);
            }
            else
            {
                readingsLayout.Controls.Remove(ActiveEnergy.Text.Label);
                readingsLayout.Controls.Remove(ActiveEnergy.Text.Field);
                readingsLayout.Controls.Remove(ReactiveEnergy.Text.Label);
                readingsLayout.Controls.Remove(ReactiveEnergy.Text.Field);
                readingsLayout.Controls.Add(ActivePower.Text.Label);
                readingsLayout.Controls.Add(ActivePower.Text.Field);
                readingsLayout.Controls.Add(ReactivePower.Text.Label);
                readingsLayout.Controls.Add(ReactivePower.Text.Field);
            }
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
                {
                    ParameterCheckBox output = new ParameterCheckBox(this, CommandType.SetCompensator, compensator.Type.ToString() + " " + compensator.Value + " " + Compensator.MeasureUnit, compensator.Position, false);
                    Outputs.Add(output);
                    regulatorLayout.Controls.Add(output.Label);
                    regulatorLayout.Controls.Add(output.Field);
                    output.Field.Dock = DockStyle.Left;
                }
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
