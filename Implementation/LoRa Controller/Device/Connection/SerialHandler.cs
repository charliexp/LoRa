﻿using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace LoRa_Controller.Connection
{
	class SerialHandler : SerialPort , IConnectionHandler
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

		#region Private variables
		private bool _isDisconnectedOnPurpose;
		#endregion
		
		#region Public properties
		public bool Connected
		{
			get { return IsOpen; }
		}

		public bool DisconnectedOnPurpose
		{
			get { return _isDisconnectedOnPurpose; }
		}
		#endregion

		#region Public methods
		public new void Open()
		{
			try
			{
				base.Open();
				DiscardInBuffer();
			}
			catch
			{
			}
		}

		public new void Close()
		{
			base.Close();
			_isDisconnectedOnPurpose = true;
		}

		public async Task<bool> SendCharAsync(byte[] byteToSend)
		{
			try
			{
				await BaseStream.WriteAsync(byteToSend, 0, 1);
				return true;
			}
			catch (System.IO.IOException)
			{
				_isDisconnectedOnPurpose = false;
				return false;
			}
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
				_isDisconnectedOnPurpose = false;
				return false;
			}
		}
		#endregion
	}
}
