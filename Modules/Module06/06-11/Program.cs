using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1) {
                var host = args[0];
                var p1 = new Program();
                p1.getData(host);
                Console.ReadKey();
            } else {
                System.Console.WriteLine("Usage: dotnet run <host>");
            }      
        }

        public async void getData(string host) 
        {
            try {
                var client = new UdpClient();

                // tell the server we are looking for the time
                byte[] b = new byte[0];
                await client.SendAsync(b, 0, host, 5080);
                
                var t = new Timer((state) =>
                {
                    throw new SocketException(10060);
                }, null, 5000, Timeout.Infinite);

                var result = await client.ReceiveAsync();
                t.Dispose();
                var theTime = System.Text.Encoding.ASCII.GetString(result.Buffer);
                System.Console.WriteLine(theTime);
            
            }
            catch (Exception e)
            {
             
                System.Console.WriteLine(e);
                System.Console.WriteLine("Unable to connect to the server");
            }
   
        }
    }

}
