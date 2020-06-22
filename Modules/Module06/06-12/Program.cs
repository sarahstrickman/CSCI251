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
                var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                {
                    ReceiveTimeout = 5000
                };
                client.Connect(host, 5080);
                // tell the server we are looking for the time
                byte[] b = new byte[0];
                client.Send(b);

                var response = new byte[1024];
                var ep = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
                
                var result = client.ReceiveFrom(response, ref ep);
              
                var theTime = System.Text.Encoding.ASCII.GetString(response);
                System.Console.WriteLine(theTime);
            
            }
            catch (Exception e)
            {
             
                //System.Console.WriteLine(e);
                System.Console.WriteLine("Unable to connect to the server");
            }
   
        }
    }

}
