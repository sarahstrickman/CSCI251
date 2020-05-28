
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
            for (var i=1; i<100000; i++)
            {
                myList.Add(i);
            }

            var po = new ParallelOptions {MaxDegreeOfParallelism = 3};
            var source = new CancellationTokenSource();
            po.CancellationToken = source.Token;
            po.CancellationToken.Register(() => {
                Console.WriteLine("Parallel Loop Cancelled");
            });
           
            try {
                var p = Parallel.ForEach(myList, po, (i, state)=>{
                    if (i.isPrime()) 
                    {
                    
                 
                        if (i > 99000) {
                            //state.Break();
                            //state.Stop();
                            source.Cancel();
                            
                        }
                        Console.WriteLine(i + " is prime ("+state.ShouldExitCurrentIteration+")");
                        
                    }
                });
                

                if (p.IsCompleted)
                {
                    Console.WriteLine("done!");
                } else 
                {
                    Console.WriteLine("Aborted");
                }
            } catch {}
            
            
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