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
                var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                client.ReceiveTimeout = 5000;
                client.Connect(host, 5080);
                // tell the server we are looking for the time

                var sa = new SocketAsyncEventArgs {SendPacketsElements = new SendPacketsElement[1]};
                sa.SendPacketsElements[0] = new SendPacketsElement(new byte[1]);

                sa.Completed += (sender, args) =>
                {
                    var response = new byte[128];
                    var ep = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
            
                    var ra = new SocketAsyncEventArgs {RemoteEndPoint = ep};
                    ra.SetBuffer(response, 0, 128);
                    ra.Completed += (o, eventArgs) =>
                    {
                        var theTime = System.Text.Encoding.ASCII.GetString(response);
                        System.Console.WriteLine(theTime);
                    };
                    client.ReceiveMessageFromAsync(ra);


                };
                client.SendPacketsAsync(sa);

            }
            catch (Exception e)
            {
             
                System.Console.WriteLine(e);
                System.Console.WriteLine("Unable to connect to the server");
            }
   
        }
    }

}
