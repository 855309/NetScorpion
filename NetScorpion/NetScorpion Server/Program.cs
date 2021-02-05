using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetScorpion.Common;

namespace NetScorpion_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("config.ini"))
            {
                prepareConfig();
            }

            Terminal.Start();
        }

        public static void prepareConfig()
        {
            File.WriteAllText("config.ini", $"ip:{Tools.getLocalIpAdress()}\r\nport:{Network.ServerPORT}");
        }
    }
}
