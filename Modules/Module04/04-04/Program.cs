
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var n = Int32.Parse(args[0]);
            var tasks = new List<Task>();
            var channel = new Channel();
            var getter = new Getter(channel);
            var putter = new Putter(channel, n);
            tasks.Add(Task.Run(() => getter.get()));
            tasks.Add(Task.Run(() => putter.put()));
            Task.WaitAll(tasks.ToArray());
        }
    }

    public class Channel
    {
        private int? value;
        private bool valueExists;

        public void put(int? v)
        {
            lock(this) {
                while (valueExists) {
                    Monitor.Wait(this);
                }
                value = v;
                valueExists = true;
                Monitor.PulseAll(this);
            }
        }

        public int? get()
        {
            lock(this) {
                while (!valueExists)
                {
                    Monitor.Wait(this);
                }
                var v = value;
                valueExists = false;
                Monitor.PulseAll(this);     
                return v;
            }
            
        }

    }

    public class Putter {
        private Channel channel;
        private int n;
        public Putter(Channel _channel, int _n)
        {
            channel = _channel;
            n = _n;
        }

        public void put()
        {
            for (var i =1; i<=n; i++)
            {
                Thread.Sleep(1000);
                channel.put(i);
            }
            channel.put(null);
        }
    }

    public class Getter {
        private Channel channel;

        public Getter(Channel _channel)
        {
            channel = _channel;
        
        }

        public void get()
        {
            int? v;
            while ((v = channel.get()) != null) 
            {
                Console.WriteLine(v);
            }
        }
    }

}