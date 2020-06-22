using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var prog = new Program();
            prog.startClient();
            Console.ReadKey();
        }

        internal async void startClient()

        {
            var client = new TcpClient();
            await client.ConnectAsync("localhost", 3003);
            var sr = new StreamReader(client.GetStream());
            var rit = JsonConvert.DeserializeObject<Stock>(sr.ReadToEnd());
            System.Console.WriteLine(rit);
        }
    }

 

}
