using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetScorpion.Common
{
    public enum LogType
    {
        Info,
        Error,
        Success
    }
    public class Tools
    {
        public void Crawl(string Path)
        {

        }

        public static void writeLog(string text, LogType logType)
        {
            ConsoleColor currentColor = Console.ForegroundColor;

            if (logType == LogType.Error)
            {
                Console.Write($"[{DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm")}] ");

                Console.ForegroundColor = ConsoleColor.Red;

                Console.Write("ERROR ");

                Console.ForegroundColor = currentColor;

                Console.WriteLine(text);
            }
            if (logType == LogType.Info)
            {
                Console.Write($"[{DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm")}] ");

                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.Write("INFO ");

                Console.ForegroundColor = currentColor;

                Console.WriteLine(text);
            }
            if (logType == LogType.Success)
            {
                Console.Write($"[{DateTime.UtcNow.ToString("dd-MM-yyyy hh:mm")}] ");

                Console.ForegroundColor = ConsoleColor.Green;

                Console.Write("SUCCESS ");

                Console.ForegroundColor = currentColor;

                Console.WriteLine(text);
            }
        }

        public static string readConfig(string key)
        {
            foreach (string line in File.ReadAllLines("config.ini"))
            {
                if (line.Split(':')[0] == key)
                {
                    return line.Split(new char[] { ':' }, 2, StringSplitOptions.None)[1];
                }
            }
            return "false";
        }

        public static bool writeConfig(string key, string value)
        {
            foreach (string line in File.ReadAllLines("config.ini"))
            {
                string currentLine = line;

                if (line.Split(':')[0] == key)
                {
                    string newLine = key + ":" + value;

                    File.WriteAllText("config.ini", File.ReadAllText("config.ini").Replace(currentLine, newLine));
                    
                    return true;
                }
            }

            return false;
        }

        public static string getLocalIpAdress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
