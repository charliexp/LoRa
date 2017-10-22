using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	class SerialHandler : SerialPort
	{
		#region Constructors
		public SerialHandler(string portName)
		{
			BaudRate = 115200;
			Parity = Parity.None;
			DataBits = 8;
			StopBits = StopBits.One;
			Handshake = Handshake.None;
			ReadTimeout = 1200;
			WriteTimeout = 1000;
			PortName = portName;
		}
		#endregion

		#region Protected methods
		protected virtual void OnConnect(ConnectionEventArgs eventArgs)
		{
			if (eventArgs.Connected)
				ConnectSucceeded?.Invoke(this, eventArgs);
			else
				ConnectFailed?.Invoke(this, eventArgs);
		}
		protected virtual void OnDisconnect(ConnectionEventArgs eventArgs)
		{
			Disconnected?.Invoke(this, eventArgs);
		}
		#endregion

		#region Public methods
		public new void Open()
		{
			ConnectionEventArgs connectionEventArgs = new ConnectionEventArgs();
			try
			{
				base.Open();
				connectionEventArgs.Connected = true;
				DiscardInBuffer();
			}
			catch
			{
				connectionEventArgs.Connected = false;
			}
			finally
			{
				OnConnect(connectionEventArgs);
			}
		}

		public new void Close()
		{
			ConnectionEventArgs connectionEventArgs = new ConnectionEventArgs();
			connectionEventArgs.Connected = false;
			connectionEventArgs.DisconnectedOnPurpose = true;
			base.Close();
			OnDisconnect(connectionEventArgs);
		}

		public async Task<bool> ReadCharAsync(byte[] receiveBuffer)
		{
			try
			{
				await BaseStream.ReadAsync(receiveBuffer, 0, 1);
				return true;
			}
			catch (System.IO.IOException)
			{
				ConnectionEventArgs connectionEventArgs = new ConnectionEventArgs();
				connectionEventArgs.Connected = false;
				connectionEventArgs.DisconnectedOnPurpose = false;
				OnDisconnect(connectionEventArgs);
				return false;
			}
		}
		#endregion

		#region Public events
		public event EventHandler<ConnectionEventArgs> ConnectSucceeded;
		public event EventHandler<ConnectionEventArgs> ConnectFailed;
		public event EventHandler<ConnectionEventArgs> Disconnected;
		#endregion
	}
}
