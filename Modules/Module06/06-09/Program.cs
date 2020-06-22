using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var p1 = new Program();
            p1.server();
            while (true)
            {
                Thread.Sleep(1000);
            }
            //System.Console.WriteLine("Usage: dotnet run");
    
        }

        public async void server() 
        {
            try {
                
                var client = new UdpClient(5080);
                for (;;) {
                    var result = await client.ReceiveAsync();
                    var ep = result.RemoteEndPoint;
                    System.Console.WriteLine(ep.Address + " "+ ep.Port);
                    var data = DateTime.Now.ToString();
                    var msg = System.Text.Encoding.ASCII.GetBytes(data);
                    // need to receive data so we know who to send it to
                    Thread.Sleep(10000);
                    var i = await client.SendAsync(msg, msg.Length, ep);
                    System.Console.WriteLine(i + " bytes sent"); 
                }
            }
            catch (Exception e)
            {   
                System.Console.WriteLine(e);
                System.Console.WriteLine("UDP Client failed");
            }
   
        }
    }

}
