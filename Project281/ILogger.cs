using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project281_Ryno_File
{
    internal interface ILogger
    {
        void LogChange(string typeOfChange, FileSystemEventArgs e);
    }
}
