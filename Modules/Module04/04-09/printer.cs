
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{

    public class Printer
    {
        public void printJob(string user, int id, string data)
        {
            lock(this) {
                write("##############################################");
                write(String.Format("User: {0}", user));
                write(String.Format("Job ID: {0}", id));
                write("");
                write(data);
                write("");
            }
        }

        private void write(string text)
        {
            foreach (var c in text) 
            {
                Console.Write(c);
                Thread.Sleep(33);
            }
            Console.WriteLine();
        }

    }
}