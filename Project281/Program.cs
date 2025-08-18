using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project281_Ryno_File
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Smart Document Monitoring & Alert System ===");

            // Ask user for directory
            Console.Write("Enter directory path to monitor: ");
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Directory.GetCurrentDirectory();
            }

            try
            {
                // Initialize core objects
                FileWatcher watcher = new FileWatcher(path);
                Logger logger = new Logger("log.txt");
                AlertHandler alertHandler = new AlertHandler();

                // Subscribe events
                watcher.onFileChange += logger.LogChange;
                watcher.onFileChange += alertHandler.DisplayAlert;

                // Start monitoring
                watcher.StartMonitoring();
                Console.WriteLine($"Monitoring started on: {path}");
                Console.WriteLine("Press 'q' to quit...\n");

                while (Console.ReadKey(true).Key != ConsoleKey.Q) { }

                watcher.StopMonitoring();
                Console.WriteLine("Monitoring stopped. Goodbye!");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }
    }
}
