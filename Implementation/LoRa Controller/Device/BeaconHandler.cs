using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Timers;

namespace LoRa_Controller
{
    class BeaconHandler
    {
        #region Private variables
        public SerialHandler _serialDevice;
        private byte _address;
        #endregion

        #region Public enums
        #endregion

        #region Public properties
		#endregion

		#region Constructors
		public BeaconHandler(byte address)
        {
            _address = address;
		}
		#endregion

		#region Private methods
		#endregion

		#region Public methods
		public async Task SendCommandAsync(MasterHandler.Commands command, byte value)
        {
            await (_serialDevice.SendCharAsync(new byte[] { _address }));
            await (_serialDevice.SendCharAsync(new byte[] { Convert.ToByte(command) } ));
            await (_serialDevice.SendCharAsync(new byte[] { value }));
            await (_serialDevice.SendCharAsync(new byte[] { 0 }));
            await (_serialDevice.SendCharAsync(new byte[] { 0 }));
            await (_serialDevice.SendCharAsync(new byte[] { 0 }));
        }

        public async Task SendCommandAsync(MasterHandler.Commands command, int value)
        {
            await (_serialDevice.SendCharAsync(new byte[] { _address }));
            await (_serialDevice.SendCharAsync(new byte[] { Convert.ToByte(command) }));
            await (_serialDevice.SendCharAsync(new byte[] { (byte)(value >> 24) }));
            await (_serialDevice.SendCharAsync(new byte[] { (byte)(value >> 16) }));
            await (_serialDevice.SendCharAsync(new byte[] { (byte)(value >> 8) }));
            await (_serialDevice.SendCharAsync(new byte[] { (byte)value }));
        }
		#endregion
	}
}