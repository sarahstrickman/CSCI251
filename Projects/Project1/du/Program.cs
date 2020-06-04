/*
 * Contains main.
 *
 * author :    Sarah Strickman
 *             sxs4599@rit.edu
 */

using System;
using System.Threading;
using System.Collections.Generic;

namespace du
{
    class Program
    {
        static void Main(string[] args)
        {
            // check args
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: du [-s] [-p] [-b]\n"+
                "You MUST specify one of the parameters -s, -p, or -b.\n"+
                "-s\tRun in sequential mode\n"+
                "-p\tRun in parallel bode (uses all available processors)\n"+
                "-b\tRuns in parallel, followed by sequential mode");
                Environment.Exit(0);
            }

            if (args[0] == "-p")
            {
                var parTime = 0.0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
        
                // run project in parallel
                
                watch.Stop();
                parTime = Math.Round(parTime, 7);
                Console.WriteLine("Parallel calculated in: " + parTime + "s");
            }
            if (args[0] == "-s") {
                Console.WriteLine("Calculating usage data for " + args[1] + "\n");
                var seqTime = 0.0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                
                SequentialOperation s = new SequentialOperation();
                s.Calculate(args[1]);

                watch.Stop();
                seqTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                seqTime = Math.Round(seqTime, 7);
                Console.WriteLine("Sequential calculated in: " + seqTime + "s");
                Console.WriteLine("Number of files:\t" + s.NumFiles);
                Console.WriteLine("Number of directories:\t" + s.NumDirs);
                Console.WriteLine("Total size in bytes:\t" + s.NumBytes);
            }
            if (args[0] == "-b") {
                var seqTime = 0.0;  // length of time for sequential execution
                var parTime = 0.0;  // length of time for parallel execution
                var watch = System.Diagnostics.Stopwatch.StartNew();

                // run project in parallel
                Thread.Sleep(1234);
                
                watch.Stop();
                parTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                parTime = Math.Round(parTime, 7);



                watch.Restart();

                // run project sequentially
                Thread.Sleep(1432);

                watch.Stop();
                seqTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                seqTime = Math.Round(seqTime, 7);



                Console.WriteLine("Parallel calculated in: " + parTime + "s");
                Console.WriteLine("Sequential calculated in: " + seqTime + "s");
            }
        }
    }
}
