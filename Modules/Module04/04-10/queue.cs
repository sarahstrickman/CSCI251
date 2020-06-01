
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Queue<T>
    {
        private int max;
        public Queue(int _max)
        {
            max = _max;
        }
        private LinkedList<T> queue = new LinkedList<T>();

        public void put(T item)
        {
            lock(this) 
            {
                while (queue.Count == max) 
                {
                    Monitor.Wait(this);
                }
                queue.AddLast(item);
                Monitor.PulseAll(this);
            }
        }

        public T get() 
        {
            lock(this)
            {
                while(queue.Count == 0)
                {
                    Monitor.Wait(this);
                }
                var item = queue.First.Value;
                queue.RemoveFirst();
                Monitor.PulseAll(this);
                return item;
            }
        }
    }
}
