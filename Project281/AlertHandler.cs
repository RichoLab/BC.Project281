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

        public void DisplaySecurity(string reason, string path = null)
        {
            try { Console.Beep(); } catch { }
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            if (string.IsNullOrWhiteSpace(path))
                Console.WriteLine($"[SECURITY] {reason}");
            else
                Console.WriteLine($"[SECURITY] {reason} -> {path}");
            Console.ForegroundColor = prev;
        }
    }
}
