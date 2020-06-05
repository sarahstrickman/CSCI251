/*
 * Contains the code for the IOperation interface, as well as code for
 * sequential and parallel operations.
 *
 * author :    Sarah Strickman
 *             sxs4599@rit.edu
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace du {
    
    /**
     * Interface for operations.
     */
    interface IOperation {
        
        /**
         * Calculate the number of directories and files in the specified location.
         * Also calculate the total size of all items in bytes.
         */
        void Calculate(string home);
    }

    class SequentialOperation : IOperation {
        public long NumDirs {get; private set;}  // number of folders counted
        public long NumFiles {get; private set;} // number of files counted
        public long NumBytes {get; private set;} // total logical size of everything, in bytes

        /**
         * Constructor for SequentialOperation.  sets all properties to 0.
         */
        public SequentialOperation()
        {
            this.NumDirs = 0;
            this.NumFiles = 0;
            this.NumBytes = 0;
        }

        /**
         * Recursively calculates (see interface description) sequentially.
        */
        public void Calculate(string home)
        {
            try
            {
                var files = Directory.GetFiles(home);
                var dirs = Directory.GetDirectories(home);
                foreach (var file in files)
                {
                    NumBytes += file.Length;
                    NumFiles++;
                }

                foreach (var dir in dirs)
                {
                    NumDirs++;
                    NumBytes += dir.Length;
                    Calculate(dir);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid directory specified.");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                // squash
            }
        }

        /**
         * Iteratively calculates  (see interface description) sequentially
         */
        public void IterCalculate(string home)
        {
            // abort if the specified home directory is invalid
            if (!Directory.Exists(home))
            {
                Console.WriteLine("Invalid path. Please specify a valid directory.");
                return;
            }
            
            // make new queue
            var q = new Queue<string>();
            q.Enqueue(home);
            
            while (q.Count > 0)
            {
                var root = q.Dequeue();
                try
                {
                    var files = Directory.GetFiles(root);
                    var dirs = Directory.GetDirectories(root);

                    foreach (var file in files)
                    {
                        NumBytes += file.Length;
                        NumFiles++;
                    }

                    foreach (var dir in dirs)
                    {
                        NumDirs++;
                        NumBytes += dir.Length;
                        q.Enqueue(dir);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // ignore
                }
            }
        }
    }

    class ParallelOperation : IOperation
    {
        private static object QueueLock = new Object();
        private static object NumLock = new Object();

        public long NumDirs {get; private set;}  // number of folders counted
        public long NumFiles {get; private set;} // number of files counted
        public long NumBytes {get; private set;} // total logical size of everything, in bytes

        /**
         * Constructor for SequentialOperation.  sets all properties to 0.
         */
        public ParallelOperation()
        {
            this.NumDirs = 0;
            this.NumFiles = 0;
            this.NumBytes = 0;
        }

        /**
         * Recursively calculates (see interface description) in parallel.
        */
        public void Calculate(string home)
        {
            try
            {
                var files = Directory.GetFiles(home);
                var dirs = Directory.GetDirectories(home);

                Parallel.ForEach(files, file =>
                {
                    lock (NumLock)
                    {
                        NumBytes += file.Length;
                        NumFiles++;
                    }
                });

                Parallel.ForEach(dirs, dir =>
                {
                    lock (NumLock)
                    {
                        NumDirs++;
                        NumBytes += dir.Length;                        
                    }
                    Calculate(dir);
                });
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid directory specified.");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                // squash
            }
        }
        
        /**
         * Iteratively calculates  (see interface description) in parallel
         */
        public void IterCalculate(string home)
        {
            // abort if the specified home directory is invalid
            if (!Directory.Exists(home))
            {
                Console.WriteLine("Invalid path. Please specify a valid directory.");
                return;
            }
            
            // make new queue
            var q = new Queue<string>();
            lock (QueueLock)
            {
                q.Enqueue(home);
            }

            // var root = "";
            
            
            
            //
            // while (q.Count > 0)
            // {
            //     lock (QueueLock)
            //     {
            //         root = q.Dequeue();
            //     }
            //     
            //     try
            //     {
            //         var files = Directory.GetFiles(root);
            //         var dirs = Directory.GetDirectories(root);
            //
            //         Parallel.ForEach(files, file =>
            //         {
            //             lock (NumLock)
            //             {
            //                 NumBytes += file.Length;
            //                 NumFiles++;
            //             }
            //         });
            //
            //         // DirectoryInfo.GetFileSystemInfos("", );
            //         Parallel.ForEach(dirs,dir =>
            //         {
            //             lock (NumLock)
            //             {
            //                 NumDirs++;
            //                 NumBytes += dir.Length;
            //             }
            //
            //             lock (QueueLock)
            //             {
            //                 q.Enqueue(dir);
            //             }
            //         });
            //     }
            //     catch (UnauthorizedAccessException)
            //     {
            //         // ignore
            //     }
            // }
            //
            
        }
    }

    public class ParallelThread
    {
        
    }
}