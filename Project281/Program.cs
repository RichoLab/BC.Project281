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

            Console.Write("Enter directory path to monitor: ");
            string path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Directory.GetCurrentDirectory();
            }

            try
            {
                FileWatcher watcher = new FileWatcher(path);
                Logger logger = new Logger("log.txt");
                AlertHandler alertHandler = new AlertHandler();

                watcher.onFileChange += logger.LogChange;
                watcher.onFileChange += alertHandler.DisplayAlert;
                watcher.onSecurity   += alertHandler.DisplaySecurity;

                watcher.StartMonitoring();
                Console.WriteLine($"Monitoring started on: {path}");
                Console.WriteLine("Press 'q' to quit...\n");
                Console.WriteLine("Tip: Press 'A' for admin (view last 10 log lines).");


                while (Console.ReadKey(true).Key != ConsoleKey.Q) 
                {
                    Console.WriteLine("Press 'A' for admin, any other key to continue...");
                    var k2 = Console.ReadKey(true).Key;
                    if (k2 == ConsoleKey.A)
                    {
                        Console.Write("Admin password: ");
                        string pwd = Console.ReadLine(); 
                        if (AdminAuth.Verify(pwd))
                        {
                            string logPath = "log.txt"; 
                            if (File.Exists(logPath))
                            {
                                var lines = File.ReadAllLines(logPath);
                                int start = Math.Max(0, lines.Length - 10);
                                Console.WriteLine("---- Last 10 log lines ----");
                                for (int i = start; i < lines.Length; i++)
                                    Console.WriteLine(lines[i]);
                                Console.WriteLine("---------------------------");
                            }
                            else
                            {
                                Console.WriteLine("No log file yet.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Access denied.");
                        }
                    }
                }

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

