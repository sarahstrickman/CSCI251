using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;


namespace ConsoleApplication
{

    public class InternetWordCount
    {
        public static void Main(string[] args)
        {
            var iwc = new InternetWordCount();
            iwc.writeFile(args);
            Task.WaitAll(iwc.DoAsyncThing(args));  

            System.Console.WriteLine("All Done");
            Console.ReadKey();
        }

        public async Task<bool> DoAsyncThing(string[] args) {
            var client = new HttpClient();
            var myTask = client.GetStringAsync(args[0]);
            System.Console.WriteLine(myTask.Status);
            System.Console.WriteLine("Doing something...");
            System.Console.WriteLine(await(myTask));
            System.Console.WriteLine("Done Waiting");
            return true;
        }
        
        public async void writeFile(string[] args)
        {
            var f = new FileInfo("test.txt");
            var fs = f.OpenWrite(); 
            int size = 1024*1024*500;
            var b = new byte[size];
            var r = new Random();
            r.NextBytes(b);
            await fs.WriteAsync(b, 0, size);
            fs.Dispose();
            System.Console.WriteLine("Line after write");
            
        }
    }

}