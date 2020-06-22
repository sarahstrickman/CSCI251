using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace ConsoleApplication
{
    public class Program
    {
        public static Object myLock;
        public static void Main(string[] args)
        {
            var p1 = new Program();
            p1.listen();
            Console.ReadKey();
        }

        public async void listen () 
        {

            var listener = new TcpListener(IPAddress.Any, 1313);
            listener.Start();
            for(;;) {
                
                var client = await listener.AcceptTcpClientAsync();
                new Task(async () =>
                {
                    var netStream = client.GetStream();
                    var data = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                    var msg = System.Text.Encoding.ASCII.GetBytes(data);
                    await netStream.WriteAsync(msg, 0, msg.Length);
                    Thread.Sleep(10000);
                    Console.WriteLine("Done sleeping");
                }).Start();
                
            }
        }
    }

}
