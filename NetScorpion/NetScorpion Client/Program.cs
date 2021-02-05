using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetScorpion.Common;
using NetScorpion_Client;

namespace NetScorpion_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.setEvents();

            trytoconnect:
            try
            {
                Tools.writeLog("Connecting...", LogType.Info);
                Network.client.Connect();
            }
            catch
            {
                Tools.writeLog($"Server did not respond. Trying to re-connect in {Network.ReconnectSecond} seconds...", LogType.Error);

                Thread.Sleep(Network.ReconnectSecond * 1000); //sleep Network.ReconnectSecond seconds

                goto trytoconnect;
            }

            Console.ReadLine();
        }
    }
}
