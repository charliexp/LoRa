using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Logger
{
    class Logger
    {
        public string Path
        {
            get;
        }

        private bool started;
        private StreamWriter streamWriter;
        private string dateFormat;

        public Logger()
        {
            dateFormat = "HH:mm:ss.fff";
            this.Path = "log_" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
            start();
        }

        public Logger(string pathPrefix)
        {
            dateFormat = "HH:mm:ss.fff";
            this.Path = pathPrefix + DateTime.Now.ToString("dd.MM.yyyy")+ ".txt";
            start();
        }

        public Logger(string pathPrefix, string fileFormat)
        {
            dateFormat = "HH:mm:ss.fff";
            this.Path = pathPrefix + DateTime.Now.ToString("dd.MM.yyyy") + "." + fileFormat;
            start();
        }

        public Logger(string pathPrefix, string dateFormat, string fileFormat)
        {
            this.dateFormat = dateFormat;
            this.Path = pathPrefix + DateTime.Now.ToString(dateFormat) + "." + fileFormat;
            start();
        }
        
        public void write(string data)
        {
            if (!started)
                started = true;
            streamWriter.WriteLineAsync(DateTime.Now.ToString("HH:mm:ss.fff") + ", " + data);
        }
        private void start()
        {
            started = false;
            streamWriter = File.AppendText(Path);
        }

        public void finish()
        {
            if (streamWriter != null)
                streamWriter.Close();
        }
    }
}
