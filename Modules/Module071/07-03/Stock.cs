using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApplication
{
    internal class Stock
    {
        public string name { get; set; }
        public double price { get; set; }
        public List<StockHistory> history { get; set; }
    }

    internal class StockHistory
    {
        public DateTime when { get; set; }
        public double price { get; set; }
    }
}
