using System;
using System.IO;
using System.Net.Sockets;

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
                
                var result = await client.ReceiveAsync();
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
