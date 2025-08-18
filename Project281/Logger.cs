using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project281_Ryno_File
{
    internal class Logger : ILogger
    {
        private readonly string logFilePath;

        public Logger(string filePath)
        {
            logFilePath = filePath;
        }

        public void LogChange(string typeOfChange, FileSystemEventArgs e)
        {
            string logMessage = $"[{DateTime.Now}] {typeOfChange}: {e.FullPath}";
            Console.WriteLine(logMessage);

            try
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"[Logger Error] {ioEx.Message}");
            }
        }
    }
}
