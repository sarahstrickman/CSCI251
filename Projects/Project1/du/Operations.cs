/*
 * Contains the code for the IOperation interface, as well as code for
 * sequential and parallel operations.
 *
 * author :    Sarah Strickman
 *             sxs4599@rit.edu
 */

using System;
using System.IO;
using System.Threading.Tasks;

namespace du
{
    
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
                Console.WriteLine("Please specify a valid directory.");
                Environment.Exit(0);
            }
            catch (IOException) {
                Console.WriteLine("Please specify a valid directory.");
                Environment.Exit(0);
            }
            catch (UnauthorizedAccessException)
            {
                // squash
            }
        }

        public long DirSize(DirectoryInfo d) 
        {    
            long size = 0;    
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis) 
            {      
                size += fi.Length;    
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis) 
            {
                size += DirSize(di);   
            }
            return size;  
        }
    }

    /**
     * This class will calculate usage data by using the Parallel library 
    */
    class ParallelOperation : IOperation
    {
        private static object NumLock = new Object();   // lock for accumulating numbers

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
                Console.WriteLine("Please specify a valid directory.");
                Environment.Exit(0);
            }
            catch (IOException) {
                Console.WriteLine("Please specify a valid directory.");
                Environment.Exit(0);
            }
            catch (UnauthorizedAccessException)
            {
                // squash
            }
        }
    }
}