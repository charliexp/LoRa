using System;
using System.Collections.Generic;
using static LoRa_Controller.Device.BaseDevice;

namespace LoRa_Controller.Connection.Messages
{
	public class Message
    {
        #region Types
        public enum ResponseType
        {
            ACK = 1,
            NAK = 0
        }
        public enum CommandType
		{
			IsPresent = 0x10,
            Resend = 0x11,
            Reset = 0x12,
            
			SetAddress = 0x20,
            HasMeter = 0x21,
            ChangeCompensator = 0x22,

            Acquisition = 0x30,
            Timestamp = 0x31,
            ActiveEnergy = 0x32,
            ReactiveEnergy = 0x33,
            ReactivePower = 0x34,
            SetCompensator = 0x35
		}
        #endregion

        #region Private constants
        private const int ArgMaxSize = 4;

        private const int Idx_argument = 2;
        private const int Idx_ack = 0;
        #endregion

        #region Public constants
        public const int Idx_command = 0;
        public const int Idx_argLength = 1;
        public const int HeaderSize = 2;
        public const int MaxSize = HeaderSize + ArgMaxSize;
        #endregion

        #region Properties
        public CommandType Command { get; private set; }
        public byte[] RawArgument
        {
            get
            {
                return rawArgument;
            }

            private set
            {
                int tempValue;

                rawArgument = value;
                if (value.Length != 0)
                    switch(Command)
                    {
                        case CommandType.IsPresent:
                            PrintableArgument = ((ResponseType)value[Idx_ack]).ToString();
                            break;
                        case CommandType.Timestamp:
                            PrintableArgument = rawArgument[0].ToString("D2") + ":" +
                                rawArgument[1].ToString("D2") + ":" +
                                rawArgument[2].ToString("D2");
                            break;
                        case CommandType.ActiveEnergy:
                            PrintableArgument = ((rawArgument[0] << 16) |
                                (rawArgument[1] << 8) |
                                (rawArgument[2])).ToString() + " kWh";
                            break;
                        case CommandType.ReactiveEnergy:
                            tempValue = (rawArgument[0] << 16) |
                                (rawArgument[1] << 8) |
                                (rawArgument[2]);
                            if ((tempValue & (1 << 23)) != 0)
                                tempValue |= 0xFF << 24;
                            PrintableArgument = tempValue.ToString("+#;-#;0") + " kVARh";
                            break;
                        case CommandType.ReactivePower:
                            tempValue = (rawArgument[0] << 16) |
                                (rawArgument[1] << 8) |
                                (rawArgument[2]);
                            if ((tempValue & (1 << 23)) != 0)
                                tempValue |= 0xFF << 24;
                            PrintableArgument = tempValue.ToString("+#;-#;0") + " kVAR";
                            break;
                    }
            }
        }
        public string PrintableArgument { get; private set; }
        #endregion
        
        #region Public static methods
        public static byte ArgLengthFromArray(byte[] array)
        {
            return array[Idx_argLength];
        }
        #endregion

        #region Private constants
        private byte[] rawArgument;
        #endregion

        #region Constructors
        private Message()
        {
        }
        public Message(CommandType command) : this()
        {
            Command = command;
            RawArgument = new byte[0];
        }
        public Message(byte[] array) : this((CommandType)array[Idx_command])
        {
            byte[] tempArray = new byte[array[Idx_argLength]];

            Array.Copy(array, Idx_argument, tempArray, 0, array[Idx_argLength]);
            RawArgument = tempArray;
        }
        public Message(List<byte> list) : this(list.ToArray())
        {

        }
        public Message(CommandType command, byte argument) : this(command)
        {
            RawArgument = new byte[1] { argument };
        }
        public Message(CommandType command, int argument) : this(command)
        {
            RawArgument = BitConverter.GetBytes(argument);
        }
        #endregion
    }
}
