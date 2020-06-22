
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

        private Philosopher(int id, Fork _left, Fork _right) 
        {
            Console.WriteLine("Philosopher: "+id);
            for (var i=0; i<id; i++)
            {
                space += "\t";
            }
            leftFork = _left;
            rightFork = _right;
        }

        private void eat()
        {
            
            while (true) 
            {
                Console.WriteLine(space + "Hungry");
                leftFork.PickUp();
                Console.WriteLine(space +"LeftUp");
                rightFork.PickUp();
                Console.WriteLine(space + "RightUp");
                Console.WriteLine(space + "Eat!");
                leftFork.PutDown();
                Console.WriteLine(space + "LeftDown");
                rightFork.PutDown();
                Console.WriteLine(space + "RightDown");
                Console.WriteLine(space + "Thinking");
            }
        }

        public static void Main()
        {
            var forks = new Fork[5] 
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

            var tasks = new List<Task>();
            foreach (var p in philosophers) 
            {
                tasks.Add(Task.Run(() => {
                    p.eat();
                }));
                
            }
            Task.WaitAll(tasks.ToArray());
        }

    }

}