
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
            var shared = new sharedNumber(0);
            Console.WriteLine(shared.Val);
            var cu = new CountUp();
            var cd = new CountDown();
            tasks.Add(Task.Run(()=>cu.go(shared, n)));
            tasks.Add(Task.Run(()=>cd.go(shared, n)));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(shared.Val);

        }
    }

    public class sharedNumber
    {
        private int val;
        private Object myLock = new Object();

        public sharedNumber(int _val)
        {
            val = _val;
        }
        public void up()
        {   
            lock(this) {
                val += 1;
            }
        }

        public void down()
        {
            
            lock(this) {
                val -= 1;
            }
            
        }

        public int Val {
            get {
                return val;
            }
        }
    }

    public class CountUp
    {
        public void go(sharedNumber shared, int n)
        {
            for (int i=0; i<n; i++)
            {
            
                shared.up();
                
            }
        }  
    }

    public class CountDown
    {
        public void go(sharedNumber shared, int n)
        {
            for (int i=0; i<n; i++)
            {
                shared.down();
            }
        }  
    }
}
