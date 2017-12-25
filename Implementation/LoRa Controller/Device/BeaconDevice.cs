using LoRa_Controller.Device;
using System.Collections.Generic;

namespace LoRa_Controller
{
    class BeaconDevice : DeviceHandler
	{

		#region Constructors
		public BeaconDevice()
		{
		}

		public BeaconDevice(string comPortName) : base(ConnectionType.Serial, comPortName)
		{
		}

		public BeaconDevice(DeviceHandler deviceHandler)
		{
			Address = deviceHandler.Address;
		}
		#endregion

		#region Protected methods
		protected override void ParseData(string receivedData)
		{
			base.ParseData(receivedData);
			if (receivedData.Contains("Asked"))
			{
				_errors = 0;
			}
		}
		#endregion
	}
}