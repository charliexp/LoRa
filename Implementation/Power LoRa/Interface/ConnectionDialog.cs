using Power_LoRa.Interface.Controls;
using Power_LoRa.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using static Power_LoRa.Connection.BaseConnectionHandler;

namespace Power_LoRa.Interface
{
	public partial class ConnectionDialog : Form
    {
        #region Private variables
        private ComboBoxControl serialPortChooser;
        private Label message;
        private RadioButton serialRadioButton;
        private RadioButton remoteRadioButton;
        private Button okButton;
        private FlowLayoutPanel layout;
        private TableLayoutPanel parametersTable;
        private TextBoxControl ipChooser;
        private TextBoxControl tcpPortChooser;
        #endregion

        #region Constructors
        public ConnectionDialog()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponent();
            
            message = new Label
            {
                AutoSize = true,
                Name = "MessageLabel",
                Text = "Please choose how to connect to the device."
            };
            serialRadioButton = new RadioButton
            {
                AutoSize = true,
                Checked = true,
                Name = "SerialRadioButton",
                Text = "Serial",
            };
            remoteRadioButton = new RadioButton
            {
                AutoSize = true,
                Name = "RemoteRadioButton",
                Text = "Remote",
            };
            okButton = new Button
            {
                Anchor = AnchorStyles.Right,
                AutoSize = true,
                DialogResult = DialogResult.OK,
                Name = "OKButton",
                Text = "OK",
            };
            layout = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Name = "layout",
                Padding = new Padding(InterfaceConstants.ItemPadding),
                FlowDirection = FlowDirection.TopDown,
            };
            parametersTable = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Name = "parametersTable",
            };
            parametersTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            parametersTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            serialPortChooser = new ComboBoxControl(this, "COMPort", new List<string>(SerialPort.GetPortNames()), 0);
            ((ComboBox)serialPortChooser.Field).SelectedItem = SettingHandler.COMPort.Value;

            ipChooser = new TextBoxControl(this, "IP", TextBoxControl.Type.Input);
            ipChooser.Field.Text = (string)SettingHandler.IPAddress.Value;

            tcpPortChooser = new TextBoxControl(this, "TCPPort", TextBoxControl.Type.Input);
            tcpPortChooser.Field.Text = (string)SettingHandler.TCPPort.Value.ToString();

            Controls.Add(layout);

            layout.Controls.Add(message);
            layout.Controls.Add(serialRadioButton);
            layout.Controls.Add(remoteRadioButton);
            layout.Controls.Add(parametersTable);
            layout.Controls.Add(okButton);

            parametersTable.Controls.Add(serialPortChooser.Label);
            parametersTable.Controls.Add(serialPortChooser.Field);
            parametersTable.Controls.Add(ipChooser.Label);
            parametersTable.Controls.Add(ipChooser.Field);
            parametersTable.Controls.Add(tcpPortChooser.Label);
            parametersTable.Controls.Add(tcpPortChooser.Field);
            parametersTable.Controls.Add(okButton);
            parametersTable.SetColumnSpan(okButton, 2);
            serialPortChooser.Label.Dock = DockStyle.Top;
            serialPortChooser.Field.Dock = DockStyle.Fill;
            ipChooser.Label.Dock = DockStyle.Top;
            ipChooser.Field.Dock = DockStyle.Fill;
            tcpPortChooser.Label.Dock = DockStyle.Top;
            tcpPortChooser.Field.Dock = DockStyle.Fill;

            ((ComboBox)(serialPortChooser.Field)).DropDown += SerialPortSelecting;
            ((ComboBox)(serialPortChooser.Field)).SelectedIndexChanged += SerialPortSelected;
            ((TextBox)(ipChooser.Field)).TextChanged += IPEntered;
            ((TextBox)(tcpPortChooser.Field)).TextChanged += TCPPortEntered;
            serialRadioButton.CheckedChanged += new EventHandler(SerialRadioButton_CheckedChanged);
            remoteRadioButton.CheckedChanged += new EventHandler(RemoteRadioButton_CheckedChanged);
            okButton.Click += new EventHandler(OKButton_Click);

            AcceptButton = okButton;
            SerialRadioButton_CheckedChanged(serialRadioButton, new EventArgs());
        }
        #endregion

        #region Private methods
        private void SerialRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (serialRadioButton.Checked)
			{
                serialPortChooser.Visible = true;
                ipChooser.Visible = false;
                tcpPortChooser.Visible = false;
			}
		}
		private void RemoteRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (remoteRadioButton.Checked)
            {
                serialPortChooser.Visible = false;
                ipChooser.Visible = true;
                tcpPortChooser.Visible = true;
            }
        }
        private void SerialPortSelecting(object sender, EventArgs e)
        {
            ComboBox comboBox = ((ComboBox)sender);
            string[] comPortsList = SerialPort.GetPortNames();

            comboBox.Items.Clear();
            foreach (string port in comPortsList)
                if (!comboBox.Items.Contains(port))
                    comboBox.Items.Add(port);
        }
        private void SerialPortSelected(object sender, EventArgs e)
        {
            okButton.Enabled = true;
        }
        private void IPEntered(object sender, EventArgs e)
		{
			if (((TextBox)sender).Text.Split(new char[] { '.' }).Length == 4)
				okButton.Enabled = true;
			else
				okButton.Enabled = false;
		}
		private void TCPPortEntered(object sender, EventArgs e)
		{
			if (Int32.TryParse(((TextBox)sender).Text, out int port))
				okButton.Enabled = true;
			else
				okButton.Enabled = false;
		}
		private void OKButton_Click(object sender, EventArgs e)
		{
			ConnectionType connectionType;
			List<string> parameters = new List<string>();

			if (serialRadioButton.Checked)
			{
				connectionType = ConnectionType.Serial;
				parameters.Add((string)(((ComboBox)serialPortChooser.Field).SelectedItem));
				SettingHandler.COMPort.Value = parameters[0];
			}
			else
			{
				connectionType = ConnectionType.Internet;
				parameters.Add(((TextBox)ipChooser.Field).Text);
				parameters.Add(((TextBox)tcpPortChooser.Field).Text);
				SettingHandler.IPAddress.Value = parameters[0];
				SettingHandler.TCPPort.Value = parameters[1];
			}
			Close();
			Program.StartConnection(connectionType, parameters);
        }
        #endregion
    }
}
