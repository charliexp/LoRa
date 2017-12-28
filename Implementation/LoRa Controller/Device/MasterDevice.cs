using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.Timers;

namespace LoRa_Controller
{
    class MasterDevice : DirectDevice
	{
		#region Private constants
		private const uint NotConnectedErrorThreshold = 10;
		#endregion

		#region Private variables
		private bool _hasBeaconConnected;
		private DirectDevice _beaconHandler;
        #endregion
		
		#region Public properties
		public bool HasBeaconConnected
		{
			get { return _hasBeaconConnected; }
		}

		public DirectDevice BeaconHandler
		{
			get { return _beaconHandler; }
		}
		#endregion

		#region Constructors
		public MasterDevice(string comPortName) : base(ConnectionType.Serial, comPortName)
		{
			_hasBeaconConnected = false;
		}

		public MasterDevice(DirectDevice deviceHandler)
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
				errors = 0;
				_hasBeaconConnected = true;

				_beaconHandler = new BeaconDevice();
				_beaconHandler.Address = Byte.Parse(receivedData.Remove(receivedData.LastIndexOf(' ')).Substring(receivedData.IndexOf(' ') + 1));
			}
			else if (receivedData.Contains("not responding"))
			{
				errors++;
				totalErrors++;

				if (errors >= NotConnectedErrorThreshold)
				{
					_hasBeaconConnected = false;
					rssi = 0;
					snr = 0;
				}
			}
			if (!_hasBeaconConnected)
			{
				errors = 0;
			}
		}
		#endregion
	}
}