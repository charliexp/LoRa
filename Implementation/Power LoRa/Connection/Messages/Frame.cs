using System;
using System.Collections.Generic;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Connection.Messages
{
    public class Frame
    {
        #region Types
        public enum AddressType
        {
            Broadcast = 0xAA,
            EndDevice = 1,
            PC = 0xff,
        }
        #endregion

        #region Private constants
        private const int MaxMessages = 1;
        private const int MaxSize = HeaderSize + MaxMessages * Message.MaxSize;

        private const int Idx_length = 0;
        private const int Idx_devAddr = 1;
        private const int Idx_firstMessage = HeaderSize;
        //private const int Idx_RSSI = HeaderSize + ArgMaxSize + 3;
        //private const int Idx_SNR = HeaderSize + ArgMaxSize + 4;
        #endregion

        #region Public constants
        public const int HeaderSize = 2;
        #endregion

        #region Properties
        public DateTime Timestamp { get; private set; }
        public byte EndDevice { get; private set; }
        public List<Message> Messages { get; set; }
        public byte RSSI { get; private set; }
        public byte SNR { get; private set; }
        #endregion

        #region Constructors
        private Frame()
        {
            Timestamp = DateTime.Now;
            Messages = new List<Message>();
            EndDevice = 0;
            RSSI = 0;
            SNR = 0;
        }
        public Frame(byte[] array) : this()
        {
            int i = Idx_firstMessage;
            EndDevice = array[Idx_devAddr];

            while(i < array.Length)
            {
                int messageArrayLength = Message.HeaderSize + array[i + Idx_argLength];
                byte[] messageArray = new byte[messageArrayLength];

                Array.Copy(array, i + Idx_command, messageArray, 0, messageArrayLength);
                Messages.Add(new Message(messageArray));

                i += Message.HeaderSize + Messages[Messages.Count - 1].RawArgument.Length;
            }
        }
        public Frame(byte endDevice, Message message) : this()
        {
            EndDevice = endDevice;
            Messages.Add(message);
        }
        public Frame(int endDevice, Message message) : this((byte) endDevice, message)
        {

        }
        public Frame(Message message) : this((byte) AddressType.PC, message)
        {
        }
        #endregion

        #region Public static methods
        public static byte LengthFromArray(byte[] array)
        {
            return array[Idx_length];
        }
        #endregion

        #region Public methods
        public byte[] ToArray()
        {
            byte length = HeaderSize;
            List<byte> list = new List<byte>
            {
                0,
                EndDevice
            };

            foreach (Message message in Messages)
            {
                list.Add((byte) message.Command);
                list.Add((byte) message.RawArgument.Length);
                list.AddRange(message.RawArgument);
                length += (byte)(Message.HeaderSize + message.RawArgument.Length);
            }
            list[Idx_length] = length;

            return list.ToArray();
        }
        #endregion
    }
}
