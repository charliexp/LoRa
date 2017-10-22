using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Timers;

namespace LoRa_Controller
{
    class DeviceHandler
    {
        #region Private variables
        private SerialHandler _serialDevice;
		private bool _isConnectedToBoard;
		
        private Timer _connectionChecker;
        private uint _oldErrors;
        private uint _errors;
        private uint _totalErrors;
        private bool _isMaster;
        private bool _isRadioConnected;
        private int _rssi;
        private int _snr;
        private bool _receiveTimeout;
        #endregion

        #region Public properties
        public uint Errors
        {
            get { return _errors; }
        }

        public uint TotalErrors
        {
            get { return _totalErrors; }
        }

        public bool IsMaster
        {
            get { return _isMaster; }
        }

        public bool IsRadioConnected
        {
            get { return _isRadioConnected; }
        }

        public int RSSI
        {
            get { return _rssi; }
        }

        public int SNR
        {
            get { return _snr; }
        }

        public bool ReceiveTimeout
        {
            get { return _receiveTimeout; }
        }

		public bool IsConnectedToBoard
		{
			get { return _isConnectedToBoard; }
		}

		public EventHandler<ConnectionEventArgs> ConnectedToBoard
		{
			set { _serialDevice.ConnectSucceeded += value; }
		}

		public EventHandler<ConnectionEventArgs> UnableToConnectToBoard
		{
			set { _serialDevice.ConnectFailed += value; }
		}

		public EventHandler<ConnectionEventArgs> DisconnectedFromBoard
		{
			set { _serialDevice.Disconnected += value; }
		}
		#endregion

		#region Constructors
		public DeviceHandler(string comPortName)
        {
			_serialDevice = new SerialHandler(comPortName);
			_serialDevice.ConnectSucceeded += UpdateBoardConnectionStatus;
			_serialDevice.ConnectFailed += UpdateBoardConnectionStatus;
			_serialDevice.Disconnected += UpdateBoardConnectionStatus;

			_errors = 0;
			_oldErrors = 0;
			_totalErrors = 0;


			_connectionChecker = new Timer(5000);
			_connectionChecker.Elapsed += checkErrors;
		}
		#endregion

		#region Private methods
		private void UpdateBoardConnectionStatus(object sender, ConnectionEventArgs e)
		{
			_isConnectedToBoard = e.Connected;
			if (!_isConnectedToBoard)
				_isRadioConnected = false;
		}

		private void checkErrors(Object source, ElapsedEventArgs e)
		{
			if (_errors - _oldErrors >= 4)
				_isRadioConnected = false;
			_oldErrors = _errors;
		}
		#endregion

		#region Public methods
		public void ConnectToBoard()
		{
			_serialDevice.Open();
		}

        public static string[] getAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public async Task<List<string>> ReceiveDataAsync()
        {
            List<string> receivedData = new List<string>();
            string receivedLine = "";

            while (_serialDevice.IsOpen && !receivedLine.Contains("txDone"))
            {
                receivedLine = "";
                while (_serialDevice.IsOpen && !receivedLine.Contains("\r"))
                {
                    byte[] receivedByte = new byte[1];

					if (await _serialDevice.ReadCharAsync(receivedByte))
					{
						if (receivedByte[0] > 0)
							receivedLine += Convert.ToChar(receivedByte[0]);
					}
                }
                receivedLine = receivedLine.TrimEnd(new char[] { '\n', '\r' });

                if (receivedLine.Contains("PING"))
                    _isMaster = true;
                else if (receivedLine.Contains("PONG"))
                    _isMaster = false;
                else if (receivedLine.Contains("Rssi") && receivedLine.Contains(","))
                {
					String tempString = receivedLine.Remove(receivedLine.IndexOf(' '));

                    if (tempString.Length != 0)
					{
						tempString = tempString.Substring(receivedLine.IndexOf('=') + 1);
						_rssi = Int16.Parse(tempString);
					}
					tempString = receivedLine.Substring(receivedLine.LastIndexOf('=') + 1);
					_snr = Int16.Parse(tempString);
                    _isRadioConnected = true;
                }
                else if (receivedLine.Contains("OnRxTimeout"))
                {
                    if (!_connectionChecker.Enabled)
                        _connectionChecker.Start();
                    _receiveTimeout = true;
                    _errors++;
					_totalErrors++;
                }
                if (!_isRadioConnected)
                {
                    _errors = 0;
                }

                receivedData.Add(receivedLine);
            }

            return receivedData;
        }
		#endregion
	}
}