using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller
{
    class Logger
    {
		private string _path;
		public string fileName;
        private StreamWriter streamWriter;
        private string dateFormat;

		public string Path
		{
			get { return _path; }
			set
			{
				_path = value;
				start();
			}
		}

        public Logger()
        {
            dateFormat = "HH:mm:ss.fff";
            fileName = "log_" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
            start();
        }

        public Logger(string fileNamePrefix)
        {
            dateFormat = "HH:mm:ss.fff";
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy")+ ".txt";
            start();
        }

        public Logger(string fileNamePrefix, string fileFormat)
        {
            dateFormat = "HH:mm:ss.fff";
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy") + "." + fileFormat;
            start();
        }

        public Logger(string fileNamePrefix, string dateFormat, string fileFormat)
        {
            this.dateFormat = dateFormat;
            fileName = fileNamePrefix + DateTime.Now.ToString(dateFormat) + "." + fileFormat;
            start();
        }
        
        public async Task write(string data)
        {
            await streamWriter.WriteLineAsync(DateTime.Now.ToString("HH:mm:ss.fff") + ": " + data);
        }

        private void start()
        {
            streamWriter = File.AppendText(_path + "\\" + fileName);
			streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Log started");
		}

        public void finish()
        {
			streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Log finished");
            if (streamWriter != null)
                streamWriter.Close();
        }
    }
}
