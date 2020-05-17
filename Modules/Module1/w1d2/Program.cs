using System;
using System.IO;
using System.Linq;
using System.Text;

namespace w1d2
{
    public class DirThing
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var start = DateTime.Now;
            var di = Directory.GetFiles(".");
            Console.WriteLine("Program arguments:\t" + args);
            if(args.Length == 1)
            {
                di = Directory.GetFiles(args[0]);
            }
            

            foreach (var d in di)
            {
                var f = new FileInfo(d);
                var fname = "";
                f.Refresh();
                
                fname = Path.GetFileName(f.FullName);    // get just the name of the file
                
                var fileTime = f.LastWriteTime.ToString("MMM dd HH:mm");
                var ft = f.LastWriteTime;
                var interval = DateTime.Now - ft;
                if (interval.Days > 365)
                {
                    fileTime = f.LastWriteTime.ToString("yyyy");
                }
                Console.WriteLine("{0, 10} {1} {2}", f.Length, fileTime, fname);
            }

            var end = DateTime.Now;
            Console.WriteLine(end-start);
            Console.ReadKey();

        }
    }    
}
