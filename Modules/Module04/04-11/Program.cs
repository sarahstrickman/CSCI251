
using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ssWriter;

namespace ConsoleApplication
{

    public class PrintJob
    {
        public string user { get; set; }
        public int id { get; set; }
        public string content { get; set; }
        public PrintJob(string _user, int _id, string _content)
        {
            user = _user;
            id = _id;
            content = _content;
        }
    }

    public class Spooler
    {
        private Printer printer;
        private ConcurrentQueue<PrintJob> jobQueue;
        public Spooler(Printer _printer, ConcurrentQueue<PrintJob> _jobQueue)
        {
            printer = _printer;
            jobQueue = _jobQueue;
        }

        public void spool()
        {
            while (true)
            {
                PrintJob job;
                jobQueue.TryDequeue(out job);
                if (job != null) {
                    printer.printJob(job.user, job.id, job.content);
                }
            }
        }
    }
    public class Generator
    {
        private ConcurrentQueue<PrintJob> jobQueue;
        private String user;
        private int id;


        public Generator(ConcurrentQueue<PrintJob> _jobQueue, String _user)
        {
            jobQueue = _jobQueue;
            user = _user;
        }

        public void print()
        {
            id+=1;
            //System.Console.WriteLine("User {0} job {1}", user, id);
            jobQueue.Enqueue(new PrintJob(user, id, 
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. \n" +
            "Maecenas vulputate, sem eget dignissim porttitor, dolor orci \n"+
            "sollicitudin purus, eget ornare mauris ligula consectetur leo.\n"+
            " Donec maximus egestas massa vitae dignissim. Donec bibendum \n"+
            "ornare nisi a bibendum.  "));

            id+=1;
            //System.Console.WriteLine("User {0} job {1}", user, id);
            jobQueue.Enqueue(new PrintJob(user, id, 
            "2nd print job.  This one is much shorter, and used for demo purposes only  "));
            //System.Console.WriteLine("done");
        }

        public static void Main(string[] args)
        {
            splitWriter.Init();
            var tasks = new List<Task>();
            var printer = new Printer(Location.top);
            var printer2 = new Printer(Location.bottom);
            var jobQueue = new ConcurrentQueue<PrintJob>();
            var spooler = new Spooler(printer, jobQueue);
            var spooler2 = new Spooler(printer2, jobQueue);
            tasks.Add(Task.Run(()=>spooler.spool()));
            tasks.Add(Task.Run(()=>spooler2.spool()));
            foreach (var user in args)
            {
                Console.WriteLine(user);
                var gen = new Generator(jobQueue, user);
                Task.Run(()=>gen.print());
            }
            Task.WaitAll(tasks.ToArray());
        
        }
    }

}