using LoRa_Controller.Device;
using System.Collections.Generic;

namespace LoRa_Controller
{
    class BeaconHandler : DeviceHandler
	{

		#region Constructors
		public BeaconHandler()
		{
		}
		public BeaconHandler(string comPortName) : base(comPortName)
		{
		}
		#endregion

		#region Private methods
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