using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var p1 = new Program();
            p1.listen();
            for (var i = 0; i < 1000; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(DateTime.Now.ToString());
            }
            Console.ReadKey();
        }
 
        private async void listen () 
        {

            var listener = new TcpListener(IPAddress.Any, 1313);
            listener.Start();
            for(;;) {
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine(client.Connected);
                var netStream = client.GetStream();
                var data = DateTime.Now.ToString();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                netStream.Write(msg, 0, msg.Length);
            }
        }
    }

}
