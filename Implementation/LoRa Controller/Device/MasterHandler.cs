using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.Timers;

namespace LoRa_Controller
{
    class MasterHandler : DeviceHandler
	{
        #region Private variables
		private bool _hasBeaconConnected;
		private DeviceHandler _beaconHandler;
        #endregion
		
		#region Public properties
		public bool HasBeaconConnected
		{
			get { return _hasBeaconConnected; }
		}

		public DeviceHandler BeaconHandler
		{
			get { return _beaconHandler; }
		}
		#endregion

		#region Constructors
		public MasterHandler(string comPortName) : base(comPortName)
		{
			_hasBeaconConnected = false;
		}
		#endregion

		#region Private methods
		private void CheckRadioErrors(Object source, ElapsedEventArgs e)
		{
			if (_errors - _oldErrors >= 2)
				_hasBeaconConnected = false;
			_oldErrors = _errors;
		}

		protected override void ParseData(string receivedData)
		{
			base.ParseData(receivedData);
			if (receivedData.Contains("Beacon ACK"))
			{
				_errors = 0;
				_hasBeaconConnected = true;

				_beaconHandler = new BeaconHandler();
				_beaconHandler.Address = 2;
			}
			else if (receivedData.Contains("not responding"))
			{
				if (_errors != 4)
					_hasBeaconConnected = false;
				_errors++;
				_totalErrors++;
			}
			if (!_hasBeaconConnected)
			{
				_errors = 0;
			}
		}
		#endregion
	}
}