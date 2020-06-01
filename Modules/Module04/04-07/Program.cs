
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{

    public class Generator
    {
        private Printer printer;
        private String user;
        private int id;


        public Generator(Printer _printer, String _user)
        {
            printer = _printer;
            user = _user;
        }

        public void print()
        {
            id+=1;
            //System.Console.WriteLine("User {0} job {1}", user, id);
            printer.printJob(user, id, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vulputate, sem eget dignissim porttitor, dolor orci sollicitudin purus, eget ornare mauris ligula consectetur leo. Donec maximus egestas massa vitae dignissim. Donec bibendum ornare nisi a bibendum.  ");
            Console.WriteLine("done");
        }

        public static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var printer = new Printer();
            foreach (var user in args)
            {
                var gen = new Generator(printer, user);
                tasks.Add(Task.Run(()=>gen.print()));
            }
            Task.WaitAll(tasks.ToArray());
        
        }

    }

}