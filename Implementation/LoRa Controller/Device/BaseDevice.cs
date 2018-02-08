using LoRa_Controller.DirectConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Device
{
	public abstract class BaseDevice
	{
		#region Constructors
		public BaseDevice()
		{
			nodeType = NodeType.Unknown;
		}
		#endregion

		#region Public enums
		public enum NodeType
		{
			Unknown,
			Master,
			Beacon,
		}
		#endregion

		#region Public constants
		public const int GeneralCallAddress = 0;
		public const int MasterDeviceAddress = 1;
		#endregion

		#region Public variables
		public NodeType nodeType;
		#endregion

		#region Private variables
		private int address;
		#endregion

		#region Public properties
		public int Address
		{
			get { return address; }
			set
			{
				address = value;
				if (address == MasterDeviceAddress)
					nodeType = NodeType.Master;
				else
					nodeType = NodeType.Beacon;
			}
		}
		#endregion
	}
}
