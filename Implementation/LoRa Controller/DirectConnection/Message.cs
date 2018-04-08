

namespace LoRa_Controller.Device
{
	public class Message
	{
		public enum CommandType
		{
			IsPresent = 0x10,

			GetAddress = 0x20,
			SetAddress = 0x21,

			Bandwidth = 0x40,
			OutputPower = 0x41,
			SpreadingFactor = 0x42,
			CodingRate = 0x43,
			RxSymTimeout = 0x44,
			RxMsTimeout = 0x45,
			TxTimeout = 0x46,
			PreambleSize = 0x47,
			PayloadMaxSize = 0x48,
			VariablePayload = 0x49,
			PerformCRC = 0x4a,

			Invalid = 0
		}
	}
}
