﻿
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;


namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var threads = new List<Thread>();
            var start = DateTime.Now;
            foreach (var dest in args){

                var sc = new SeqCounter(dest);
                // this is a longer way of accomplishing the same thing
                //Thread pThread = new Thread(new ThreadStart(sc.run));
                var pThread = new Thread(sc.run);
                pThread.Start();
                threads.Add(pThread);
            }
            foreach(var th in threads)
            {
                th.Join();
            }
            var end = DateTime.Now;
            Console.WriteLine((end-start).TotalSeconds + "s");
        }
    }

    public class SeqCounter
    {
        private static readonly object outputLock = new Object();
        private readonly string path;

        public SeqCounter(string _path)
        {
            path = _path;
        }
        public void run()
        {

            var c = new Counter(path);
            c.count();
            lock(outputLock) {
                Console.WriteLine(path);
                Console.Write("{0, 5}", c.lineCount);
                Console.Write("{0, 10}", c.wordCount);
                Console.Write("{0, 15}", c.letterCount);
                Console.WriteLine();
            }
        }
    }

    public class Counter 
    {
        private string path;
        public int lineCount { get; set; } = 0;
        public int wordCount { get; set; } = 0;
        public int letterCount { get; set; } = 0;
        public Counter(string _path)
        {
            path = _path;
        }
        public void count()
        {
            try {
                var contents = File.ReadAllLines(path);
                lineCount = contents.Length;
                foreach (var line in contents) {
                    var words = line.Split();
                    wordCount += words.Length;
                    letterCount += line.Length;
                }

            } catch (FileNotFoundException ex) {
                Console.WriteLine(ex.FileName + " not found");
            }
        }
    }
}
