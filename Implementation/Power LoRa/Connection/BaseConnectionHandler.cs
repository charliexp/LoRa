using LoRa_Controller.Connection.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
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
        public void WriteByte(byte byteToSend)
        {
            WriteBytes(new byte[] { byteToSend });
        }
        public async Task WriteByteAsync(byte byteToSend)
        {
            await WriteBytesAsync(new byte[] { byteToSend });
        }
        public byte ReadByte()
        {
            return ReadBytes(1)[0];
        }
        public async Task<byte> ReadByteAsync()
        {
            return (await ReadBytesAsync(1))[0];
        }
        public abstract void WriteBytes(byte[] bytesToSend);
        public abstract Task WriteBytesAsync(byte[] bytesToSend);
        public abstract byte[] ReadBytes(int numberOfBytes);
        public abstract Task<byte[]> ReadBytesAsync(int numberOfBytes);

        public void Write(Message message)
        {
            Write(new Frame(message));
        }
        public async Task WriteAsync(Message message)
        {
            await WriteAsync(new Frame(message));
        }
        public void Write(Frame frame)
        {
            WriteBytes(frame.ToArray());
        }
        public async Task WriteAsync(Frame frame)
        {
            await WriteBytesAsync(frame.ToArray());
        }
        public Frame Read()
        {
            int expectedLength;
            List<byte> frame = new List<byte>();

            /* Get frame header */
            try
            {
                frame.AddRange(ReadBytes(Frame.HeaderSize));
            }
            catch
            {
                Close();
                return null;
            }

            /* Get entire frame based on its length */
            expectedLength = Frame.LengthFromArray(frame.ToArray()) - Frame.HeaderSize;
            try
            {
                frame.AddRange(ReadBytes(expectedLength));
            }
            catch
            {
                Close();
                return null;
            }
            return new Frame(frame.ToArray());
        }
        public async Task<Frame> ReadAsync()
        {
            int argLength;
            List<byte> header = new List<byte>();
            List<byte> argument = new List<byte>();

            /* Get frame header + message header */
            while (Connected && (header.Count < Frame.HeaderSize + Message.HeaderSize))
            {
                try
                {
                    header.Add(await ReadByteAsync());
                }
                catch
                {
                    Close();
                }
            }

            /* Get message argument based on its length */
            argLength = Message.ArgLengthFromArray(header.ToArray());
            while (Connected && argument.Count < argLength)
            {
                try
                {
                    argument.Add(await ReadByteAsync());
                }
                catch
                {
                    Close();
                }
            }
            header.AddRange(argument);

            return new Frame(header.ToArray());
        }
        #endregion
    }
}
