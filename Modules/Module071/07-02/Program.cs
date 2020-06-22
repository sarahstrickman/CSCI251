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
            var history1 = new StockHistory
            {
                price = 16.44,
                when = DateTime.Today
            };
            var history2 = new StockHistory
            {
                price = 13.44,
                when = DateTime.Today.AddDays(-5)
            };

            var s = new Stock
            {
                name = "RIT",
                price = 37.80,
                history = new List<StockHistory>
                {
                    history1,
                    history2

                }
            };

            string json = JsonConvert.SerializeObject(s, Formatting.Indented);
            var dstock = JsonConvert.DeserializeObject<Stock>(json);
            Console.WriteLine(json);
        }

    }

}
