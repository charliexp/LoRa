using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace LoRa_Logger
{
    class DeviceHandler
    {
        SerialPort serialPort;
        System.Timers.Timer connectionChecker;
        public int errors;
        private int oldErrors;
        public bool radioMaster;
        public bool radioConnected;
        public bool serialConnected;
        public string RSSI;
        public string SNR;
        public bool receiveTimeout;

        public DeviceHandler(string portName)
        {
            serialPort = new SerialPort();
            serialPort.BaudRate = 115200;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = 1200;
            serialPort.WriteTimeout = 1000;
            serialPort.PortName = portName;
            serialPort.Open();
            serialPort.DiscardInBuffer();

            serialConnected = true;
            errors = 0;
            oldErrors = 0;

            connectionChecker = new System.Timers.Timer(5000);
            connectionChecker.Elapsed += checkErrors;
        }

        public void closePort()
        {
            serialPort.Close();

            while (serialPort.IsOpen) ;
            serialConnected = false;
        }

        public static string[] getAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        private void checkErrors(Object source, ElapsedEventArgs e)
        {
            if (errors - oldErrors >= 5)
                radioConnected = false;
        }

        public async Task<List<string>> ReceiveDataAsync()
        {
            List<string> receivedData = new List<string>();
            string receivedLine = "";
            int receivedByte = 0;

            while (!receivedLine.Contains("txDone"))
            {
                while (serialPort.IsOpen && !receivedLine.Contains("\r"))
                {
                    try
                    {
                        receivedByte = await Task.Run(serialPort.ReadChar);
                    }
                    catch (System.IO.IOException)
                    {
                        return receivedData;
                    }
                    catch (TimeoutException)
                    {
                        return receivedData;
                    }
                    if (receivedByte > 0)
                        receivedLine += Convert.ToChar(receivedByte);
                }
                receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

                if (receivedLine.Contains("PING"))
                    radioMaster = true;
                else if (receivedLine.Contains("PONG"))
                    radioMaster = false;
                else if (receivedLine.Contains("Rssi") && receivedLine.Contains(","))
                {
                    RSSI = receivedLine.Remove(receivedLine.IndexOf(','));
                    if (RSSI.Length != 0)
                        RSSI = RSSI.Substring(receivedLine.IndexOf('-'));
                    SNR = receivedLine.Substring(receivedLine.LastIndexOf('=') + 1);
                    radioConnected = true;
                }
                else if (receivedLine.Contains("OnRxTimeout"))
                {
                    if (!connectionChecker.Enabled)
                    {
                        oldErrors = errors;
                        connectionChecker.Start();
                    }
                    receiveTimeout = true;
                    errors++;
                }
                if (!radioConnected)
                {
                    errors = 0;
                }

                receivedData.Add(receivedLine);
            }

            return receivedData;
        }
    }
}