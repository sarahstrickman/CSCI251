
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleApplication
{

    public class ParallelStuff
    {
        public static void Main() 
        {
            var myList = new List<int>();
            for (var i=1; i<10000; i++)
            {
                myList.Add(i);
            }


            var p = Parallel.ForEach(myList, (i, state)=>{
                if (!i.isPrime()) return;
                if (i > 1000) {
                    //state.Break();
                    state.Stop();
                }
                Console.WriteLine(i + " is prime ("+state.ShouldExitCurrentIteration+")");
            });
            

            if (p.IsCompleted)
            {
                Console.WriteLine("done!");
            } else 
            {
                Console.WriteLine("Aborted");
            }
            


            
        }


    }

    public static class Extensions
    {
        public static bool isPrime(this int number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0)  return false;
            }

            return true;        
        }
    }

}