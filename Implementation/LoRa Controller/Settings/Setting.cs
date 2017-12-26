using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Settings
{
	class Setting
	{
		private string _name;
		private object _value;

		public Setting(string name)
		{
			_name = name;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				SettingHandler.Save(this);
			}
		}
	}
}
