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

    public delegate void delSecurity(string reason, string path);

    internal class FileWatcher : IMonitorService
    {
        public event delFileChange onFileChange;
        public event delSecurity onSecurity;

        string directoryPath;
        FileSystemWatcher watcher;
        Thread mainThread;
        volatile bool _running;

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

        public FileWatcher()
        {
            directoryPath = Directory.GetCurrentDirectory();
        }

        public FileWatcher(string path)
        {
            DirectoryPath = path;
        }

        public void StartMonitoring()
        {
            _running = true;
            mainThread = new Thread(startMethod)
            {
                IsBackground = true
            };
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

            while (_running)
                Thread.Sleep(200);
        }

        public void StopMonitoring()
        {
            _running = false;

            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;

                watcher.Changed -= HandleFileEvent;
                watcher.Renamed -= HandleFileEvent;
                watcher.Deleted -= HandleFileEvent;
                watcher.Created -= HandleFileEvent;

                watcher.Dispose();
                watcher = null;
            }

            if (mainThread != null && mainThread.IsAlive)
                mainThread.Join();
        }

        private void HandleFileEvent(object sender, FileSystemEventArgs e)
        {
            onFileChange?.Invoke(e.ChangeType.ToString(), e);

            if (e.ChangeType == WatcherChangeTypes.Deleted)
                onSecurity?.Invoke("Deletion detected", e.FullPath);

            if (e is RenamedEventArgs r &&
                string.Equals(Path.GetExtension(r.FullPath), ".exe", StringComparison.OrdinalIgnoreCase))
            {
                onSecurity?.Invoke("Renamed to .exe", r.FullPath);
            }
        }
    }
}
