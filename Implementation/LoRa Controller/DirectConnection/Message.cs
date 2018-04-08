

using System.Collections.Generic;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Device
{
	public class Message
    {
        #region Types
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
        public enum ResponseType
        {
            ACK = 1,
            NACK = 0xff
        };
        #endregion

        #region Private constants
        private const int Idx_sourceAddress = 0;
        private const int Idx_targetAddress = 1;
        private const int Idx_command = 2;
        private const int Idx_commandParameter = 3;
        private const int Idx_RSSI = ParametersMaxSize + 3;
        private const int Idx_SNR = ParametersMaxSize + 4;
        private const int ParametersMaxSize = 4;
        #endregion

        #region Public constants
        /* Source + target + command + parameters + signal quality*/
        public const int MaxLength = 1 + 1 + 1 + ParametersMaxSize + 2;
        #endregion

        #region Properties
        public byte Source { get; private set; }
        public byte Target { get; private set; }
        public CommandType Command { get; private set; }
        public List<int> Parameters { get; private set; }
        public byte RSSI { get; private set; }
        public byte SNR { get; private set; }
        public ResponseType Response
        {
            get { return (ResponseType) (Parameters[0] >> 24); }
        }
        public byte[] ByteRepresentation
        {
            get
            {
                byte[] array = new byte[MaxLength];
                array[Idx_sourceAddress] = Source;
                array[Idx_targetAddress] = Target;
                array[Idx_command] = (byte)Command;
                if (Parameters.Count != 0)
                {
                    array[Idx_commandParameter + 0] = (byte)(Parameters[0] >> 24);
                    array[Idx_commandParameter + 1] = (byte)(Parameters[0] >> 16);
                    array[Idx_commandParameter + 2] = (byte)(Parameters[0] >> 8);
                    array[Idx_commandParameter + 3] = (byte)(Parameters[0] >> 0);
                }

                return array;
            }
        }
        #endregion
        
        #region Constructors
        private Message()
        {
            Source = 0;
            Target = 0;
            Command = CommandType.Invalid;
            Parameters = new List<int>();
            RSSI = 0;
            SNR = 0;
        }
        public Message(byte[] byteRepresentation) : this()
        {
            Source = byteRepresentation[Idx_sourceAddress];
            Target = byteRepresentation[Idx_targetAddress];
            Command = (CommandType)byteRepresentation[Idx_command];
            Parameters.Add( byteRepresentation[Idx_commandParameter + 0] << 24  |
                            byteRepresentation[Idx_commandParameter + 1] << 16  |
                            byteRepresentation[Idx_commandParameter + 2] << 8   |
                            byteRepresentation[Idx_commandParameter + 3]);
            RSSI = byteRepresentation[Idx_RSSI];
            SNR = byteRepresentation[Idx_SNR];
        }
        public Message(List<byte> byteRepresentation) : this()
        {
            Source = byteRepresentation[Idx_sourceAddress];
            Target = byteRepresentation[Idx_targetAddress];
            Command = (CommandType)byteRepresentation[Idx_command];
            Parameters.Add(byteRepresentation[Idx_commandParameter + 0] << 24 |
                            byteRepresentation[Idx_commandParameter + 1] << 16 |
                            byteRepresentation[Idx_commandParameter + 2] << 8 |
                            byteRepresentation[Idx_commandParameter + 3]);
            RSSI = byteRepresentation[Idx_RSSI];
            SNR = byteRepresentation[Idx_SNR];
        }
        public Message(int target, CommandType command, int parameter) : this()
        {
            Source = (int)AddressType.PC;
            this.Target = (byte)target;
            this.Command = command;
            Parameters.Add(parameter);
        }
        public Message(CommandType command, int parameter) : this()
        {
            Source = (int)AddressType.PC;
            Target = (int)AddressType.General;
            this.Command = command;
            Parameters.Add(parameter);
        }
        public Message(CommandType command) : this()
        {
            Source = (int)AddressType.PC;
            Target = (int)AddressType.General;
            this.Command = command;
        }
        #endregion
    }
}
