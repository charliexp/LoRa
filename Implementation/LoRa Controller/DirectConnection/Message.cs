

using System;
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
            HasMeter = 0x22,
            SetContact = 0x23,
            ChangeActuator = 0x24,

            LastReading = 0x30,
            ActivePower = 0x31,
            InductivePower = 0x32,
            CapacitivePower = 0x33,
            ReactivePower = 0x34,
            ApparentPower = 0x35,
            PowerFactor = 0x36,
            /*
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
            */
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
        private const int Idx_paramLength = 3;
        private const int Idx_parameter = 4;
        private const int Idx_RSSI = ParameterMaxSize + 3;
        private const int Idx_SNR = ParameterMaxSize + 4;
        private const int ParameterMaxSize = 4;
        #endregion

        #region Public constants
        /* Source + target + command + parameters + signal quality*/
        public const int MaxLength = 1 + 1 + 1 + ParameterMaxSize + 2;
        #endregion

        #region Properties
        public DateTime Timestamp { get; private set; }
        public byte Source { get; private set; }
        public byte Target { get; private set; }
        public CommandType Command { get; private set; }
        public byte Length { get; private set; }
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
                byte[] array = new byte[MaxLength - 2];
                array[Idx_sourceAddress] = Source;
                array[Idx_targetAddress] = Target;
                array[Idx_command] = (byte)Command;
                array[Idx_paramLength] = Length;
                if (Parameters.Count != 0)
                {
                    array[Idx_parameter + 0] = (byte)(Parameters[0] >> 24);
                    array[Idx_parameter + 1] = (byte)(Parameters[0] >> 16);
                    array[Idx_parameter + 2] = (byte)(Parameters[0] >> 8);
                    array[Idx_parameter + 3] = (byte)(Parameters[0] >> 0);
                }

                return array;
            }
        }
        #endregion
        
        #region Constructors
        private Message()
        {
            Timestamp = DateTime.Now;
            Source = 0;
            Target = 0;
            Length = 0;
            Command = CommandType.Invalid;
            Parameters = new List<int>();
            RSSI = 0;
            SNR = 0;
        }
        public Message(byte[] byteRepresentation) : this()
        {
            int parameter = 0;
            int i = 0;

            Source = byteRepresentation[Idx_sourceAddress];
            Target = byteRepresentation[Idx_targetAddress];
            Command = (CommandType)byteRepresentation[Idx_command];
            Length = byteRepresentation[Idx_paramLength];
            
            while (i < Length)
                parameter = parameter << 8 + byteRepresentation[Idx_parameter + i++];
            Parameters.Add(parameter);
            if (Source != (byte) AddressType.PC && Target != (byte) AddressType.PC)
            {
                RSSI = byteRepresentation[Idx_RSSI];
                SNR = byteRepresentation[Idx_SNR];
            }
        }
        public Message(List<byte> byteRepresentation) : this(byteRepresentation.ToArray())
        {
        }
        public Message(int target, CommandType command, int parameter) : this()
        {
            Source = (byte)AddressType.PC;
            Target = (byte)target;
            Command = command;
            Length = 4;
            Parameters.Add(parameter);
        }
        public Message(CommandType command, int parameter) : this()
        {
            Source = (byte)AddressType.PC;
            Target = (byte)AddressType.General;
            Command = command;
            Length = 4;
            Parameters.Add(parameter);
        }
        public Message(CommandType command) : this()
        {
            Source = (byte)AddressType.PC;
            Target = (byte)AddressType.General;
            Length = 0;
            Command = command;
        }
        #endregion
    }
}
