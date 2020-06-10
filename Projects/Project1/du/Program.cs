/*
 * Contains main.
 *
 * author :    Sarah Strickman
 *             sxs4599@rit.edu
 */

using System;
using System.Threading;
using System.Collections.Generic;
using du;

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

            // parallel option
            if (args[0] == "-p")
            {
                Console.WriteLine("Directory '" + args[1] +"'\n");

                var parTime = 0.0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
        
                var p = new ParallelOperation();
                p.Calculate(args[1]);
                
                watch.Stop();
                parTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                parTime = Math.Round(parTime, 7);

                Console.WriteLine("Parallel calculated in: " + parTime + "s");
                Console.WriteLine(string.Format("{0:#,##0}", p.NumDirs) + " folders, " + 
                        string.Format("{0:#,##0}", p.NumFiles) + " files, " + 
                        string.Format("{0:#,##0}", p.NumBytes) + " bytes");
            }

            // sequential operation
            if (args[0] == "-s") {
                Console.WriteLine("Directory '" + args[1] +"'\n");

                var seqTime = 0.0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                
                var s = new SequentialOperation();
                s.Calculate(args[1]);

                watch.Stop();
                seqTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                seqTime = Math.Round(seqTime, 7);

                Console.WriteLine("Sequential calculated in: " + seqTime + "s");
                Console.WriteLine(string.Format("{0:#,##0}", s.NumDirs) + " folders, " + 
                        string.Format("{0:#,##0}", s.NumFiles) + " files, " + 
                        string.Format("{0:#,##0}", s.NumBytes) + " bytes");
            }
            if (args[0] == "-b") {
                Console.WriteLine("Directory '" + args[1] +"'\n");

                var seqTime = 0.0;      // length of time for sequential execution
                var parTime = 0.0;      // length of time for parallel execution
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var p = new ParallelOperation();
                p.Calculate(args[1]);
                
                watch.Stop();
                parTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                parTime = Math.Round(parTime, 7);

                // restart the clock
                watch.Restart();

                var s = new SequentialOperation();
                s.Calculate(args[1]);

                watch.Stop();
                seqTime = watch.Elapsed.TotalMilliseconds / 1000.0;
                seqTime = Math.Round(seqTime, 7);

                Console.WriteLine("Parallel calculated in: " + parTime + "s");
                Console.WriteLine(string.Format("{0:#,##0}", p.NumDirs) + " folders, " + 
                        string.Format("{0:#,##0}", p.NumFiles) + " files, " + 
                        string.Format("{0:#,##0}", p.NumBytes) + " bytes\n");

                Console.WriteLine("Sequential calculated in: " + seqTime + "s");
                Console.WriteLine(string.Format("{0:#,##0}", s.NumDirs) + " folders, " + 
                        string.Format("{0:#,##0}", s.NumFiles) + " files, " + 
                        string.Format("{0:#,##0}", s.NumBytes) + " bytes\n");
            }

            else {
                Console.WriteLine("Usage: du [-s] [-p] [-b]\n"+
                "You MUST specify one of the parameters -s, -p, or -b.\n"+
                "-s\tRun in sequential mode\n"+
                "-p\tRun in parallel bode (uses all available processors)\n"+
                "-b\tRuns in parallel, followed by sequential mode");
            }
        }
    }
}
