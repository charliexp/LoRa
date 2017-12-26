using LoRa_Controller.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller.Log
{
    public class Logger
    {
		private string _folder;
		public string fileName;
        private StreamWriter streamWriter;
        private string dateFormat;
        private bool _isOpen = false;
		private uint _linesWritten;

		private const uint LinesRequiredToSaveFile = 10;

		public string Folder
		{
			get { return _folder; }
			set
			{
				_folder = value;
				SettingHandler.LogFolder.Value = _folder;
                if (_isOpen)
				{
					finish();
					start();
				}
            }
		}

        public string Path
        {
            get { return _folder + "\\" + fileName; }
        }

        public Logger()
        {
			_folder = (string) SettingHandler.LogFolder.Value;
            dateFormat = "HH:mm:ss.fff";
            fileName = "log_" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
        }

        public Logger(string fileNamePrefix) : this()
        {
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy")+ ".txt";
        }

        public Logger(string fileNamePrefix, string fileFormat) : this()
		{
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy") + "." + fileFormat;
        }

        public Logger(string fileNamePrefix, string dateFormat, string fileFormat) : this()
		{
            dateFormat = dateFormat;
            fileName = fileNamePrefix + DateTime.Now.ToString(dateFormat) + "." + fileFormat;
        }
        
        public async Task write(string data)
        {
			try
			{
				await streamWriter.WriteLineAsync(DateTime.Now.ToString("HH:mm:ss.fff") + ": " + data);
				_linesWritten++;
			}
			catch (ObjectDisposedException)
			{

			}
			if (_linesWritten == LinesRequiredToSaveFile)
			{
				_linesWritten = 0;
				await streamWriter.FlushAsync();
			}
        }

        public bool isOpen()
        {
            return _isOpen;
        }

        public void start()
        {
            streamWriter = File.AppendText(_folder + "\\" + fileName);
			streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Log started");
            _isOpen = true;
			_linesWritten = 0;
		}

        public void finish()
		{
			streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Log finished");
            if (streamWriter != null)
                streamWriter.Close();
            _isOpen = false;
        }
	}
}
