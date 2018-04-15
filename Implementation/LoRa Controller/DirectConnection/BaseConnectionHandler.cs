using LoRa_Controller.Device;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LoRa_Controller.Device.Message;

namespace LoRa_Controller.DirectConnection
{
	public abstract class BaseConnectionHandler
	{
		#region Types
		public enum Error
		{
			RADIO_RX_TIMEOUT = 0xf2,
			RADIO_RX_ERROR = 0xf3,
			RADIO_TX_TIMEOUT = 0xf5,
		};
		public enum ConnectionType
		{
			Serial,
			Internet
		}
        #endregion

		#region Properties
		public abstract bool Connected
		{
			get;
		}
        #endregion
        
        #region Public methods
        public abstract void Open();
        public abstract void Close();
        public abstract void WriteByte(byte byteToSend);
        public abstract Task WriteByteAsync(byte byteToSend);
        public abstract byte ReadByte();
        public abstract Task<byte> ReadByteAsync();

        public void Write(Message message)
        {
            byte[] array = message.ByteRepresentation;
            for (int i = 0; i < array.Length; i++)
                WriteByte(array[i]);
        }
        public async Task WriteAsync(Message message)
        {
            byte[] array = message.ByteRepresentation;
            for (int i = 0; i < array.Length; i++)
                await (WriteByteAsync(array[i]));
        }
        public Message Read()
        {
            List<byte> receivedData = new List<byte>();

            while (Connected && receivedData.Count != MaxLength)
            {
                try
                {
                    receivedData.Add(ReadByte());
                }
                catch
                {
                    //Close();
                }
            }

            return new Message(receivedData);
        }
        public async Task<Message> ReadAsync()
        {
            List<byte> receivedData = new List<byte>();

            while (Connected && receivedData.Count != MaxLength)
            {
                try
                {
                    receivedData.Add(await ReadByteAsync());
                }
                catch
                {
                    Close();
                }
            }

            return new Message(receivedData);
        }
        #endregion
    }
}
