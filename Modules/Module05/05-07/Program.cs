
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace DiningPhilosophers
{


    public class Fork
    {
        
        private Thread heldBy;
        private Queue<Thread> wantsThisFork = new Queue<Thread>();
        public void PickUp()
        {
            lock(this)
            {
                wantsThisFork.Enqueue(Thread.CurrentThread);
                while (heldBy != null || wantsThisFork.Peek() != Thread.CurrentThread) 
                {
                    Monitor.Wait(this);
                }
                heldBy = wantsThisFork.Dequeue();
                
            }
        }

        public void PutDown()
        {
            lock(this)
            {
                if (heldBy != Thread.CurrentThread) 
                {
                    throw new Exception("Invalid Fork!");
                }
                heldBy = null;
                Monitor.PulseAll(this);

            }
        }
    }

    public class Philosopher 
    {
        public static object philLock = new Object();
        public static int eatingCount = 0;
        public static int maxPhilosophers;
        private string space;
        private Fork leftFork;
        private Fork rightFork;

        public Philosopher(int id, Fork _left, Fork _right) 
        {
            System.Console.WriteLine("Philosopher: "+id);
            for (var i=0; i<id; i++)
            {
                space += "\t";
            }
            leftFork = _left;
            rightFork = _right;
        }

        public void eat()
        {
          
            while (true) 
            {
                lock(philLock) {
                    System.Console.WriteLine(space + "Hungry");
                    while (eatingCount == maxPhilosophers - 1) {
                        Thread.Sleep(100);
                    }
                    Interlocked.Increment(ref eatingCount);
                    leftFork.PickUp();
                    System.Console.WriteLine(space +"LeftUp");
                    rightFork.PickUp();
                    System.Console.WriteLine(space + "RightUp");
                    System.Console.WriteLine(space + "Eat!");
                    leftFork.PutDown();
                    System.Console.WriteLine(space + "LeftDown");
                    rightFork.PutDown();
                    System.Console.WriteLine(space + "RightDown");
                    Interlocked.Decrement(ref eatingCount);   
                    Monitor.PulseAll(philLock);
                    System.Console.WriteLine(space + "Thinking");
                }
            }

        }

        public static void Main()
        {
            Fork[] forks = new Fork[5] 
            {
                new Fork(),
                new Fork(),
                new Fork(),
                new Fork(),
                new Fork()
            };

            var philosophers = new Philosopher[5]
            {
                new Philosopher(0, forks[0], forks[1]),
                new Philosopher(1, forks[1], forks[2]),
                new Philosopher(2, forks[2], forks[3]),
                new Philosopher(3, forks[3], forks[4]),
                new Philosopher(4, forks[4], forks[0])
            };
            Philosopher.maxPhilosophers = philosophers.Length;
            foreach (var p in philosophers) 
            {
                var pThread = new Thread(p.eat);
                pThread.Start();
 
            }
     
        }

    }

}