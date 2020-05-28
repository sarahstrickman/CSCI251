
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var totalLines = 0L;
            var totalWords = 0L;
            var totalChars = 0L;

            var files = args;

            var start = DateTime.Now;
            Parallel.ForEach(files, file => {
                    var counter = new Counter(file);
                    counter.count();
                    totalLines += counter.lineCount;
                    totalWords += counter.wordCount;
                    totalChars += counter.letterCount;
            });
            
            Console.WriteLine(totalLines);
            Console.WriteLine(totalWords);
            Console.WriteLine(totalChars);
            var end = DateTime.Now;
            Console.WriteLine((end-start).TotalSeconds + "s");
        }
    }


    public class Counter 
    {
        private static Object mylock = new Object();
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
                lock(mylock)
                {
                    lineCount += 1;
                    Console.Write("{0, 5}", lineCount);
                    Console.Write("{0, 10}", wordCount);
                    Console.Write("{0, 15}", letterCount);
                    Console.WriteLine(); 
                }
            } catch (FileNotFoundException ex) {
                Console.WriteLine(ex.FileName + " not found");
            }
        }
    }
}
