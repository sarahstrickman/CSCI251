
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

            var p = Parallel.ForEach(myList, i=>{
                if (i.isPrime()) 
                {
                    Console.WriteLine(i + " is prime");
                }
            });

            

            /*       Parallel.For(0, 1000, i=>{
                       if (i.isPrime()) 
                       {
                           System.Console.WriteLine(i + " is prime");
                       }
                   });
       */
            
        }


    }

    public static class Extensions
    {
        public static bool isPrime(this int number)
        {
            switch (number)
            {
                case 1:
                    return false;
                case 2:
                    return true;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (var i = 2; i <= boundary; ++i)
            {
                if (number % i == 0)  return false;
            }

            return true;        
        }
    }

}