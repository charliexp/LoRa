using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoRa_Controller.Device.DeviceHandler;

namespace LoRa_Controller.Interface.NodeUI
{
	public class RadioSettingsUI
	{
		public RadioSettingControl Bandwidth;
		public RadioSettingControl NodeType;
		public RadioSettingControl OutputPower;
		public RadioSettingControl SpreadingFactor;
		public RadioSettingControl CodingRate;
		public RadioSettingControl RxSymTimeout;
		public RadioSettingControl RxMsTimeout;
		public RadioSettingControl TxTimeout;
		public RadioSettingControl PreambleSize;
		public RadioSettingControl PayloadMaxSize;
		public RadioSettingControl VariablePayload;
		public RadioSettingControl PerformCRC;

		public List<RadioSettingControl> list;

		public RadioSettingsUI()
		{
			NodeType = new RadioSettingControl(Commands.NodeType, new TextBox());
			Bandwidth = new RadioSettingControl(Commands.Bandwidth, new ComboBox());
			OutputPower = new RadioSettingControl(Commands.OutputPower, new NumericUpDown());
			SpreadingFactor = new RadioSettingControl(Commands.SpreadingFactor, new NumericUpDown());
			CodingRate = new RadioSettingControl(Commands.CodingRate, new ComboBox());
			RxSymTimeout = new RadioSettingControl(Commands.RxSymTimeout, new NumericUpDown());
			RxMsTimeout = new RadioSettingControl(Commands.RxMsTimeout, new NumericUpDown());
			TxTimeout = new RadioSettingControl(Commands.TxTimeout, new NumericUpDown());
			PreambleSize = new RadioSettingControl(Commands.PreambleSize, new NumericUpDown());
			PayloadMaxSize = new RadioSettingControl(Commands.PayloadMaxSize, new NumericUpDown());
			VariablePayload = new RadioSettingControl(Commands.VariablePayload, new CheckBox());
			PerformCRC = new RadioSettingControl(Commands.PerformCRC, new CheckBox());

			list = new List<RadioSettingControl>
			{
				NodeType,
				Bandwidth,
				OutputPower,
				SpreadingFactor,
				CodingRate,
				RxSymTimeout,
				RxMsTimeout,
				TxTimeout,
				PreambleSize,
				PayloadMaxSize,
				VariablePayload,
				PerformCRC
			};

			((TextBox)NodeType.Field).ReadOnly = true;
			NodeType.Field.TabStop = false;

			((ComboBox)Bandwidth.Field).Items.AddRange(new object[] { "125 kHz", "250 kHz", "500 kHz" });
			((ComboBox)Bandwidth.Field).SelectedIndex = 0;

			((NumericUpDown)OutputPower.Field).Maximum = new decimal(new int[] { 14, 0, 0, 0 });
			((NumericUpDown)OutputPower.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)OutputPower.Field).Value = new decimal(new int[] { 14, 0, 0, 0 });

			((NumericUpDown)SpreadingFactor.Field).Maximum = new decimal(new int[] { 12, 0, 0, 0 });
			((NumericUpDown)SpreadingFactor.Field).Minimum = new decimal(new int[] { 7, 0, 0, 0 });
			((NumericUpDown)SpreadingFactor.Field).Value = new decimal(new int[] { 12, 0, 0, 0 });

			((ComboBox)CodingRate.Field).Items.AddRange(new object[] { "4/5", "4/6", "4/7", "4/8" });
			((ComboBox)CodingRate.Field).SelectedIndex = 3;

			((NumericUpDown)RxSymTimeout.Field).Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			((NumericUpDown)RxSymTimeout.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)RxSymTimeout.Field).Value = new decimal(new int[] { 5, 0, 0, 0 });

			((NumericUpDown)RxMsTimeout.Field).Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			((NumericUpDown)RxMsTimeout.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)RxMsTimeout.Field).Value = new decimal(new int[] { 3000, 0, 0, 0 });

			((NumericUpDown)TxTimeout.Field).Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			((NumericUpDown)TxTimeout.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)TxTimeout.Field).Value = new decimal(new int[] { 1000, 0, 0, 0 });

			((NumericUpDown)PreambleSize.Field).Maximum = new decimal(new int[] { 30, 0, 0, 0 });
			((NumericUpDown)PreambleSize.Field).Minimum = new decimal(new int[] { 2, 0, 0, 0 });
			((NumericUpDown)PreambleSize.Field).Value = new decimal(new int[] { 8, 0, 0, 0 });

			((NumericUpDown)PayloadMaxSize.Field).Maximum = new decimal(new int[] { 64, 0, 0, 0 });
			((NumericUpDown)PayloadMaxSize.Field).Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			((NumericUpDown)PayloadMaxSize.Field).Value = new decimal(new int[] { 64, 0, 0, 0 });

			((CheckBox)VariablePayload.Field).CheckState = CheckState.Checked;

			((CheckBox)PerformCRC.Field).CheckState = CheckState.Checked;
		}
	}
}
