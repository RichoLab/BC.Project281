using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project281_Ryno_File
{
    internal class AlertHandler
    {
        public void DisplayAlert(string typeOfChange, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ALERT] {typeOfChange} detected -> {e.FullPath}");
            Console.ResetColor();
        }
    }
}
