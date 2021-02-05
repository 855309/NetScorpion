using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace NetScorpion_Server
{
    public class Network
    {
        public static int ServerPORT = 2865;

        public static WatsonTcpServer server;

        public static void setEvents()
        {
            server.Events.ClientConnected += clientConnected;
            server.Events.ClientDisconnected += clientDisconnected;
            server.Events.MessageReceived += messageReceived;
        }

        #region Client Events

        public static void clientConnected(object sender, ConnectionEventArgs args)
        {

        }

        public static void clientDisconnected(object sender, DisconnectionEventArgs args)
        {

        }

        public static void messageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Mesage from client " + args.IpPort + ": " + Encoding.Default.GetString(args.Data));
        }

        #endregion
    }
}
