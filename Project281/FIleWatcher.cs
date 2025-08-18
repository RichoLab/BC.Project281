using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project281_Ryno_File
{
    public delegate void delFileChange(string typeOfChange, FileSystemEventArgs e);

    internal class FileWatcher : IMonitorService
    {
        // Events
        public event delFileChange onFileChange;

        // Fields
        string directoryPath;
        FileSystemWatcher watcher;
        Thread mainThread;

        // Property
        public string DirectoryPath
        {
            get => directoryPath;
            set
            {
                if (Directory.Exists(value))
                {
                    directoryPath = value;
                }
                else
                {
                    throw new DirectoryNotFoundException($"The specified directory does not exist: {value}");
                }
            }
        }

        // Constructor
        public FileWatcher()
        {
            directoryPath = Directory.GetCurrentDirectory();
        }

        public FileWatcher(string path)
        {
            DirectoryPath = path;
        }

        // Methods
        public void StartMonitoring()
        {
            mainThread = new Thread(startMethod);
            mainThread.Start();
        }

        void startMethod()
        {
            watcher = new FileSystemWatcher(directoryPath);
            watcher.Changed += HandleFileEvent;
            watcher.Renamed += HandleFileEvent;
            watcher.Deleted += HandleFileEvent;
            watcher.Created += HandleFileEvent;

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security;

            watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            watcher.EnableRaisingEvents = false;
            if (mainThread != null && mainThread.IsAlive)
                mainThread.Abort();
        }

        private void HandleFileEvent(object sender, FileSystemEventArgs e)
        {
            onFileChange?.Invoke(e.ChangeType.ToString(), e);
        }
    }
}
