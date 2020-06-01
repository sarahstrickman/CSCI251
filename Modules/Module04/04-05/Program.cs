
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
            var tasks = new List<Task>();
            var ac = new AlarmClock();
            tasks.Add(Task.Run(()=>{
                while (true) {
                    Thread.Sleep(5000);
                    Console.WriteLine(ac);
                }
            }));
            tasks.Add(Task.Run(() => {
                ac.setAlarm (Int32.Parse(args[0]), Int32.Parse(args[1]), Int32.Parse(args[2]));
				Console.WriteLine("Alarm at "+ ac);
            }));
            Task.WaitAll(tasks.ToArray());
        }
    }

    public class AlarmClock
    {
        public AlarmClock()
        {
            Task.Run(() => {
                while (true)
                {
                   Thread.Sleep(1000);
                   tick();
                }
            });
        }
        private int hour;
        private int minute;
        private int second;
        public void tick()
        {
            lock(this) {
                second+=1;
                if (second == 60)
                {
                    second = 0;
                    minute+=1;
                    if (minute == 60)
                    {
                        minute = 0;
                        hour+=1;
                    }
                }  
                Monitor.PulseAll(this);
            }   
        }

        public void setAlarm(int h, int m, int s)
        {
            lock(this) {
                while ((h > hour) ||
                (h == hour && m > minute) ||
                (h == hour && m == minute && s > second))
                    Monitor.Wait(this);
		    }
        }

        public int[] getTime() {
            return new int[] {hour, minute, second};
        }

        public override String ToString()
		{
		    return String.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
		}
    }
}