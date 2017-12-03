using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.Timers;

namespace LoRa_Controller
{
    class MasterHandler : DeviceHandler
	{
		#region Private constants
		private const uint NotConnectedErrorThreshold = 10;
		#endregion

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

		public MasterHandler(DeviceHandler deviceHandler)
		{
			Address = deviceHandler.Address;
		}
		#endregion

		#region Protected methods
		protected override void ParseData(string receivedData)
		{
			base.ParseData(receivedData);
			if (receivedData.Contains("ACK"))
			{
				_errors = 0;
				_hasBeaconConnected = true;

				_beaconHandler = new BeaconHandler();
				_beaconHandler.Address = 2;
				//_beaconHandler.Address = receivedData.Substring(;
			}
			else if (receivedData.Contains("not responding"))
			{
				_errors++;
				_totalErrors++;

				if (_errors >= NotConnectedErrorThreshold)
					_hasBeaconConnected = false;
			}
			if (!_hasBeaconConnected)
			{
				_errors = 0;
			}
		}
		#endregion
	}
}