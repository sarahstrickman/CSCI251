/*
 * Main.  This is where the code is run from.
 * 
 * author : Sarah Strickman
 *          sxs4599
 */



using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Numerics;
using System.Threading.Tasks;

namespace primegen
{
    class Program
    {
        internal static object CalculateLock = new object();
        internal static Int32 NumCalculated = 1;
        
        static void Main(string[] args)
        {

            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: primegen bits [count]\n" +
                                  "- bits: the number of bits of " +
                                  "the prime number, this must be a multiple " +
                                  "of 8, and at least 32 bits.\n" +
                                  "- count (optional): the number of prime " +
                                  "numbers to generate. Defaults to 1.");
                Environment.Exit(0);
            }
            
            // get the count of prime numbers
            var count = 1;
            if (args.Length == 2)
            {
                if (Int32.TryParse(args[1], out count))
                {
                    count = Int32.Parse(args[1]);
                    if (count < 1)
                    {
                        Console.WriteLine("Count should be greater than 1.");
                    }
                }
                else
                {
                    Console.WriteLine("Please input a valid value for count.");
                    Environment.Exit(0);
                }
            }

            // get number of bits
            Int32 numBits = 0;
            Int32 bigIntEight = 8;
            try
            {
                numBits = Int32.Parse(args[0]);

                // must be divisible by 8
                if ((numBits % bigIntEight) != 0)
                {
                    Console.WriteLine("Number of bits must be a multiple of 8.");
                    Environment.Exit(0);
                }
                
                // must be greater than 32 bits (4 bytes)
                Int32 minValue = 32;
                if (numBits <= minValue)
                {
                    Console.WriteLine("Number of bits should be greater than 32.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please input a number input for bits.");
                Environment.Exit(0);
            }
            
            Int32 numBytes = numBits / bigIntEight;
            var primeNumbers = new BigInteger[count];
            
            var numCores = Environment.ProcessorCount;    // used for threading
            Console.Write("BitLength: " + numBits + " bits");
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            
            Parallel.For(0, (numCores * numBytes * count), generate =>
            {
                var oper = new Operation();
                oper.CalculateAndPrint(numBytes, count);
            });
            
            // Parallel.ForEach(primeNumbers, number =>
            //     {
            //         var oper = new Operation();
            //         number = oper.Calculate(numBytes, numCores);
            //         lock (CalculateLock)
            //         {
            //             Console.WriteLine("\n" + Program.NumCalculated + 
            //                               ": " + number.ToString());
            //             NumCalculated += 1;
            //         }
            //     }
            // );

            watch.Stop();

            var elapsedtime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000000}",
            watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds,
            watch.Elapsed.Milliseconds);
            Console.WriteLine("Time to Generate: " + elapsedtime);
        }
    }
}