
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var n = Int32.Parse(args[0]);
            int shared = 0;
            Console.WriteLine(shared);
            var cu = new CountUp();
            var cd = new CountDown();
            tasks.Add(Task.Run(()=>cu.go(ref shared, n)));
            tasks.Add(Task.Run(()=>cd.go(ref shared, n)));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(shared);

        }
    }

    public class CountUp
    {
        public void go(ref int shared, int n)
        {
            for (var i=0; i<n; i++)
            {
                Interlocked.Increment(ref shared);
            }
        }  
    }

    public class CountDown
    {
        public void go(ref int shared, int n)
        {
            for (var i=0; i<n; i++)
            {
                Interlocked.Decrement(ref shared);
                
            }
        }  
    }
}
