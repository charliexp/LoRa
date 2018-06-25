using Power_LoRa.Connection.Messages;
using Power_LoRa.Node;
using Power_LoRa.Settings;
using System;
using System.IO;
using System.Threading.Tasks;
using static Power_LoRa.Connection.Messages.Frame;

namespace Power_LoRa.Log
{
    public class Logger
    {
        #region Private constants
        private const int LinesRequiredToSaveFile = 10;
        #endregion

        #region Private variables
        private string fileName;
        private string folder;
        private int linesWritten;
        private StreamWriter streamWriter;
        #endregion
        
        #region Properties
        public string Folder
		{
			get { return folder; }
			set
			{
				folder = value;
                Interface.FolderTextBox.Text = folder;
                SettingHandler.LogFolder.Value = folder;
                if (IsOpen)
				{
					Finish();
					Start();
				}
            }
		}
        public string Path
        {
            get { return folder + "\\" + fileName; }
        }
        public bool IsOpen { get; private set; }
        public LogGroupBox Interface { get; private set; }
        #endregion

        #region Constructors
        public Logger()
        {
            IsOpen = false;
			folder = (string) SettingHandler.LogFolder.Value;
            fileName = "log_" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
            Interface = new LogGroupBox();
            Interface.FolderTextBox.Text = folder;
        }
        public Logger(string fileNamePrefix) : this()
        {
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy")+ ".txt";
        }
        public Logger(string fileNamePrefix, string fileFormat) : this()
		{
            fileName = fileNamePrefix + DateTime.Now.ToString("dd.MM.yyyy") + "." + fileFormat;
        }
        #endregion

        #region Public methods
        public void Write(string message)
        {
            try
            {
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ", " + message + ",");
                linesWritten++;
            }
            catch (ObjectDisposedException)
            {

            }
            if (linesWritten == LinesRequiredToSaveFile)
            {
                linesWritten = 0;
                streamWriter.Flush();
            }
        }
        public void Write(Frame frame)
        {
            string printableFrame = frame.EndDevice + ", ";
            
            foreach(Message message in frame.Messages)
            {
                printableFrame += message.Command + ", " + message.PrintableArgument + ", ";
            }
            if (frame.EndDevice == (byte) AddressType.PC)
                printableFrame += frame.RSSI + ", ";

            Interface.Update(frame);
            Write(printableFrame);
		}
		public async Task WriteAsync(string data)
        {
			try
			{
				await streamWriter.WriteLineAsync(DateTime.Now.ToString("HH:mm:ss.fff") + ", " + data + ",");
				linesWritten++;
			}
			catch (ObjectDisposedException)
			{

			}
			if (linesWritten == LinesRequiredToSaveFile)
			{
				linesWritten = 0;
				await streamWriter.FlushAsync();
			}
        }
        public void Start() 
        {
			streamWriter = File.AppendText(folder + "\\" + fileName);
			Write("Log started");
            IsOpen = true;
			linesWritten = 0;
		}
        public void Finish()
		{
			Write("Log finished");
            if (streamWriter != null)
                streamWriter.Close();
            IsOpen = false;
        }
        #endregion
    }
}
