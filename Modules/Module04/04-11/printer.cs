
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using ssWriter;

namespace ConsoleApplication
{

    public class Printer
    {
        private Location printerID;
        private FileStream stream;
        public Printer(Location _printerID)
        {
            printerID = _printerID;
            stream = File.OpenWrite(printerID+".txt");
        }
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

        public void write(string text)
        {
            foreach (var c in text) 
            {
                splitWriter.Write(printerID, c);
                //System.Console.Write(c);
                Thread.Sleep(33);
            }
            splitWriter.Write(printerID, "\n");
            
        }

    }
}