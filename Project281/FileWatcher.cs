using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project281
{
    //Delegate
    public delegate void delFileChange(string typeOfChange, FileSystemEventArgs e);

    internal class FileWatcher: IMonitorService
    {
        //Events
        public event delFileChange onFileChange;

        //Field
        string directoryPath;
        FileSystemWatcher watcher;

        //Property
        public string DirectoryPath 
        { 
            get => directoryPath;
            set 
            { 
                if (Directory.Exists(value))
                {
                    directoryPath = value;
                    watcher.Path = value;
                } else
                {
                    throw new DirectoryNotFoundException($"The specified directory does not exist: {value}");
                }
            }
        }

        //Constuctor
        public FileWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            this.watcher = watcher;
        }

        public FileWatcher(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);
            this.watcher = watcher;
        }

        //Methods
        public void StartMonitoring()
        {
            watcher.Changed += HandleFileEvent;
            watcher.Renamed += HandleFileEvent;
            watcher.Deleted += HandleFileEvent;
            watcher.Created += HandleFileEvent;

            watcher.NotifyFilter = NotifyFilters.Attributes      // file attributes (e.g., read-only flag) changed
                                 | NotifyFilters.CreationTime    // creation date changed
                                 | NotifyFilters.DirectoryName   // folder renamed
                                 | NotifyFilters.FileName        // file renamed
                                 | NotifyFilters.LastAccess      // last opened time changed
                                 | NotifyFilters.LastWrite       // content modified
                                 | NotifyFilters.Security;       // permissions changed

            watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            watcher.EnableRaisingEvents = false;
        }

        private void HandleFileEvent(object sender, FileSystemEventArgs e)
        {
            onFileChange?.Invoke(e.ChangeType.ToString(), e);
        }
    }
}
