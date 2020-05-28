
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

            var handles = new ManualResetEvent[args.Length];
            var start = DateTime.Now;
            var count = 0;
            foreach (var dest in args){

                var sc = new SeqCounter(dest);
                handles[count] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(sc.run, handles[count]);   
                count++;  
            }

            WaitHandle.WaitAll(handles);
            var end = DateTime.Now;
            Console.WriteLine((end-start).TotalSeconds + "s");
        }
    }

    public class SeqCounter
    {
        public static object outputLock = new Object();
        private string path;

        public SeqCounter(string _path)
        {
            path = _path;
        }
        public void run(object handle)
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
            ((ManualResetEvent)handle).Set();
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
