using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Logger
{
    public partial class Form1 : Form
    {
        DeviceHandler deviceHandler;
        Logger logger;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comPortComboBox.Items.AddRange(DeviceHandler.getAvailablePorts());
            comPortComboBox.SelectedIndex = 0;
            comPortComboBox.SelectedIndexChanged += comPortComboBox_SelectedIndexChanged;
            Application.ApplicationExit += new EventHandler(this.onApplicationExit);
        }

        private async void comPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If previously connected
            if (deviceHandler != null && deviceHandler.serialConnected)
            {
                logger.finish();
                await Task.Factory.StartNew(() => deviceHandler.closePort(), TaskCreationOptions.LongRunning);
            }

            logger = new Logger("log_", "dd.MM.yyyy", "txt");
            deviceHandler = new DeviceHandler((string)((ComboBox)sender).SelectedItem);
            //If serial connection succeeded
            if (deviceHandler.serialConnected)
            {
                updateConnectionStatus("Connected to board");
            }

            while (deviceHandler.serialConnected)
            {
                updateInterface(await deviceHandler.ReceiveDataAsync());
            }
        }

        public void onApplicationExit(object sender, EventArgs e)
        {
            if (deviceHandler != null && deviceHandler.serialConnected)
                deviceHandler.closePort();

            if (logger != null)
                logger.finish();
        }
        
        public void updateConnectionStatus(string value)
        {
            if (InvokeRequired && !IsDisposed)
            {
                this.Invoke(new Action<string>(updateConnectionStatus), new object[] { value });
                return;
            }
        }
        
        public void updateInterface(List<string> receivedData)
        {
            while (logListBox.Items.Count + receivedData.Count > 12)
                logListBox.Items.RemoveAt(0);
            logListBox.Items.AddRange(receivedData.ToArray());
            logListBox.TopIndex = logListBox.Items.Count - 1;

            if (deviceHandler.radioConnected)
            {
                rssiTextBox.Text = deviceHandler.RSSI;
                snrTextBox.Text = deviceHandler.SNR;
                connectionStatusLabel.Text = "LoRa device connected";
                if (deviceHandler.receiveTimeout)
                    errorsTextBox.Text = deviceHandler.errors.ToString();
                else
                    logger.write(deviceHandler.RSSI + ", " + deviceHandler.SNR);
            }
            else
            {
                snrTextBox.Text = "";
                rssiTextBox.Text = "";
                connectionStatusLabel.Text = "LoRa device disconnected";
                errorsTextBox.Text = "";
                logger.write("error");
            }
        }
    }
}
