
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        private Queue<PrintJob> jobQueue;
        public Spooler(Printer _printer, Queue<PrintJob> _jobQueue)
        {
            printer = _printer;
            jobQueue = _jobQueue;
        }

        public void spool()
        {
            while (true)
            {
                var job = jobQueue.get();
                printer.printJob(job.user, job.id, job.content);
            }
        }
    }
    public class Generator
    {
        private Queue<PrintJob> jobQueue;
        private String user;
        private int id;


        private Generator(Queue<PrintJob> _jobQueue, String _user)
        {
            jobQueue = _jobQueue;
            user = _user;
        }

        private void print()
        {
            id+=1;
            //System.Console.WriteLine("User {0} job {1}", user, id);
            jobQueue.put(new PrintJob(user, id, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vulputate, sem eget dignissim porttitor, dolor orci sollicitudin purus, eget ornare mauris ligula consectetur leo. Donec maximus egestas massa vitae dignissim. Donec bibendum ornare nisi a bibendum.  "));
            Console.WriteLine("done");
        }

        public static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var printer = new Printer();
            var jobQueue = new Queue<PrintJob>();
            var spooler = new Spooler(printer, jobQueue);
            tasks.Add(Task.Run(()=>spooler.spool()));
            foreach (var user in args)
            {
                var gen = new Generator(jobQueue, user);
                Task.Run(()=>gen.print());
            }
            Task.WaitAll(tasks.ToArray());
        
        }

    }

}