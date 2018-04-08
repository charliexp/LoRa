

using System.Collections.Generic;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Device
{
	public class Message
    {
        #region Public enums
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

        #region Public constants
        public const int ParametersMaxSize = 4;
        /* Source + target + command + parameters + signal quality*/
        public const int MaxLength = 1 + 1 + 1 + ParametersMaxSize + 2;
        #endregion

        #region Private constants
        public const int Idx_sourceAddress = 0;
        public const int Idx_targetAddress = 1;
        public const int Idx_command = 2;
        public const int Idx_commandParameter = 3;
        public const int Idx_RSSI = ParametersMaxSize + 3;
        public const int Idx_SNR = ParametersMaxSize + 4;
        #endregion

        #region Public properties
        public byte source;
        public byte target;
        public CommandType command;
        public List<int> parameters;
        public byte rssi;
        public byte snr;
        public ResponseType Response
        {
            get { return (ResponseType) (parameters[0] >> 24); }
        }
        public byte[] ByteRepresentation
        {
            get
            {
                byte[] array = new byte[MaxLength];
                array[Idx_sourceAddress] = source;
                array[Idx_targetAddress] = target;
                array[Idx_command] = (byte)command;
                if (parameters.Count != 0)
                {
                    array[Idx_commandParameter + 0] = (byte)(parameters[0] >> 24);
                    array[Idx_commandParameter + 1] = (byte)(parameters[0] >> 16);
                    array[Idx_commandParameter + 2] = (byte)(parameters[0] >> 8);
                    array[Idx_commandParameter + 3] = (byte)(parameters[0] >> 0);
                }

                return array;
            }
        }
        #endregion

        #region Private constructors
        private Message()
        {
            source = 0;
            target = 0;
            command = CommandType.Invalid;
            parameters = new List<int>();
            rssi = 0;
            snr = 0;
        }
        #endregion

        #region Public constructors
        public Message(byte[] byteRepresentation) : this()
        {
            source = byteRepresentation[Idx_sourceAddress];
            target = byteRepresentation[Idx_targetAddress];
            command = (CommandType)byteRepresentation[Idx_command];
            parameters.Add( byteRepresentation[Idx_commandParameter + 0] << 24  |
                            byteRepresentation[Idx_commandParameter + 1] << 16  |
                            byteRepresentation[Idx_commandParameter + 2] << 8   |
                            byteRepresentation[Idx_commandParameter + 3]);
            rssi = byteRepresentation[Idx_RSSI];
            snr = byteRepresentation[Idx_SNR];
        }
        public Message(List<byte> byteRepresentation) : this()
        {
            source = byteRepresentation[Idx_sourceAddress];
            target = byteRepresentation[Idx_targetAddress];
            command = (CommandType)byteRepresentation[Idx_command];
            parameters.Add(byteRepresentation[Idx_commandParameter + 0] << 24 |
                            byteRepresentation[Idx_commandParameter + 1] << 16 |
                            byteRepresentation[Idx_commandParameter + 2] << 8 |
                            byteRepresentation[Idx_commandParameter + 3]);
            rssi = byteRepresentation[Idx_RSSI];
            snr = byteRepresentation[Idx_SNR];
        }
        public Message(int target, CommandType command, int parameter) : this()
        {
            source = (int)AddressType.PC;
            this.target = (byte)target;
            this.command = command;
            parameters.Add(parameter);
        }
        public Message(CommandType command, int parameter) : this()
        {
            source = (int)AddressType.PC;
            target = (int)AddressType.General;
            this.command = command;
            parameters.Add(parameter);
        }
        public Message(CommandType command) : this()
        {
            source = (int)AddressType.PC;
            target = (int)AddressType.General;
            this.command = command;
        }
        #endregion
    }
}
