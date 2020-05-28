
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var start = DateTime.Now;
            Parallel.ForEach(args, dest => {
                var sc = new SeqCounter(dest);
                sc.run();
            });
            var end = DateTime.Now;
            Console.WriteLine((end-start).TotalSeconds + "s");
        }
    }

    public class SeqCounter
    {
        private static object outputLock = new Object();
        private string path;

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
