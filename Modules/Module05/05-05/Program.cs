
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace DiningPhilosophers
{

    public class Fork
    {
        
        private Thread heldBy;
        public void PickUp()
        {
            lock(this)
            {
                while (heldBy != null) 
                {
                    Monitor.Wait(this);
                }
                heldBy = Thread.CurrentThread;
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
                System.Console.WriteLine(space + "Hungry");
                leftFork.PickUp();
                System.Console.WriteLine(space +"LeftUp");
                rightFork.PickUp();
                System.Console.WriteLine(space + "RightUp");
                System.Console.WriteLine(space + "Eat!");
                leftFork.PutDown();
                System.Console.WriteLine(space + "LeftDown");
                rightFork.PutDown();
                System.Console.WriteLine(space + "RightDown");
                System.Console.WriteLine(space + "Thinking");
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
            foreach (var p in philosophers) 
            {
                var pThread = new Thread(p.eat);
                pThread.Start();
            }
           
        }

    }

}