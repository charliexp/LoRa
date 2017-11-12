﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRa_Controller
{
    class Logger
    {
		private string _folder;
		public string fileName;
        private StreamWriter streamWriter;
        private string dateFormat;
        private bool _isOpen = false;

		public string Folder
		{
			get { return _folder; }
			set
			{
				_folder = value;
                if (_isOpen)
                    start();
            }
		}

        public string Path
        {
            get { return _folder + "\\" + fileName; }
        }

        public Logger()
        {
            dateFormat = "HH:mm:ss.fff";
            fileName = "log_" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
        }

        public Logger(string fileNamePrefix)
        {
            dateFormat = "HH:mm:ss.fff";
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy")+ ".txt";
        }

        public Logger(string fileNamePrefix, string fileFormat)
        {
            dateFormat = "HH:mm:ss.fff";
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy") + "." + fileFormat;
        }

        public Logger(string fileNamePrefix, string dateFormat, string fileFormat)
        {
            this.dateFormat = dateFormat;
            fileName = fileNamePrefix + DateTime.Now.ToString(dateFormat) + "." + fileFormat;
        }
        
        public async Task write(string data)
        {
            //await streamWriter.WriteLineAsync(DateTime.Now.ToString("HH:mm:ss.fff") + ": " + data);
        }

        public bool isOpen()
        {
            return _isOpen;
        }

        public void start()
        {/*
            streamWriter = File.AppendText(_folder + "\\" + fileName);
			streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Log started");
            _isOpen = true;*/
		}

        public void finish()
        {/*
			streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Log finished");
            if (streamWriter != null)
                streamWriter.Close();
            _isOpen = false;*/
        }
    }
}
