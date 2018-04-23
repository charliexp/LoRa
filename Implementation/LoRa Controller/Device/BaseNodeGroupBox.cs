using LoRa_Controller.Interface;
using LoRa_Controller.Interface.Controls;
using LoRa_Controller.Interface.Node.ParameterControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.Device
{
	public abstract class BaseNodeGroupBox : GroupBox
    {
        #region Private variables
        private TableLayoutPanel mainLayout;
        private Graph graph;
        private TableLayoutPanel secondLayout;
        private TableLayoutPanel powerReadingLayout;
        private FlowLayoutPanel powerIOLayout;

        private TextBoxControl reactivePower;
        private CheckBox hasMeter;
        private List<CheckBox> outputs;
        private ButtonControl addPowerOutput;
        #endregion

        #region Protected variables
        protected TableLayoutPanel radioLayout;
        #endregion

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
            };

            graph = new Graph("Graph");
            reactivePower = new TextBoxControl("ReactivePower", "KVAR", TextBoxControl.Type.Output)
            {
                Value = "0"
            };
            hasMeter = new CheckBox()
            {
                Text = "Has meter",
            };
            outputs = new List<CheckBox>();
            outputs.Add(new CheckBox()
            {
                Text = "1234 KVAR",
            });
            addPowerOutput = new ButtonControl("Add output");
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
            
            mainLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                Name = name + "MainLayout",
                Location = new Point(InterfaceConstants.ItemPadding, InterfaceConstants.GroupBoxFirstItemY),
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            secondLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 2,
                Name = name + "SecondLayout",
            };
            secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            secondLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
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
            powerIOLayout = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Name = name + "PowerIOLayout",
            };

            Controls.Add(mainLayout);
            mainLayout.Controls.Add(graph);
            mainLayout.Controls.Add(secondLayout);
            secondLayout.Controls.Add(powerReadingLayout);
            secondLayout.Controls.Add(powerIOLayout);
            secondLayout.Controls.Add(horizontalSeparator);
            secondLayout.Controls.Add(radioLayout);
            secondLayout.SetRowSpan(powerIOLayout, 3);
            powerReadingLayout.Controls.Add(reactivePower.Label);
            powerReadingLayout.Controls.Add(reactivePower.Field);
            radioLayout.Controls.Add(AddressControl.Label);
            radioLayout.Controls.Add(AddressControl.Field);
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
            radioLayout.Controls.Add(PerformCRC.Field);
            powerIOLayout.Controls.Add(hasMeter);
            foreach (CheckBox output in outputs)
                powerIOLayout.Controls.Add(output);
            powerIOLayout.Controls.Add(addPowerOutput.Field);

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Name = name.Replace(" ", "") + "GroupBox";
            Text = name;
            TabStop = false;
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
