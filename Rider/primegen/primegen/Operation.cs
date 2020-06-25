/*
 * contains classes that perform operations to calculate Prime numbers
 *
 * author : Sarah Strickman
 *          sxs4599
 */

using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace primegen
{
    /*
     * Class that does operations for this project
     */
    public class Operation
    {
        
        static object IsPrimeGeneratedLock = new object();
        
        /*
         * Calculate a Prime number with the provided byte Length.
         *
         * return a prime BigInteger.
         */
        public BigInteger Calculate(Int32 numBytes, int count)
        {
            var rng = new RNGCryptoServiceProvider();
            var IsPrimeGenerated = false;
            
            BigInteger GeneratedNumber = new BigInteger();
            while (!IsPrimeGenerated)
            {
                var byteArray = new byte[numBytes];
                rng.GetBytes(byteArray);
                
                GeneratedNumber = new BigInteger(byteArray);
                
                // check if the generated number is prime
                IsPrimeGenerated = ExtensionMethods.IsProbablyPrime(GeneratedNumber);
            }
            return GeneratedNumber;
        }

        
        /*
         * void function that finds a prime number and prints it.
         *
         * Does not return the prime number!
         */
        public void CalculateAndPrint(Int32 numBytes, Int32 count)
        {
            var rng = new RNGCryptoServiceProvider();
            BigInteger generatedNumber = new BigInteger();
            
            while (Program.NumCalculated <= count)
            {
                var byteArray = new byte[numBytes];
                rng.GetBytes(byteArray);
                
                generatedNumber = new BigInteger(byteArray);
                
                // check if the generated number is prime
                if (ExtensionMethods.IsProbablyPrime(generatedNumber) 
                    && Program.NumCalculated <= count)
                {
                    lock (Program.CalculateLock)
                    {
                        if (Program.NumCalculated <= count)
                        {
                            Console.WriteLine("\n" + Program.NumCalculated +
                                              ": " + generatedNumber);
                            Program.NumCalculated += 1;
                        }
                    }
                }
            }
        }
    }

    /*
     * Static class that contains any extension methods 
     */
    public static class ExtensionMethods
    {
        
        /*
         * check if a BigInteger probably is prime.  Copied from project
         * document
         */
        public static Boolean IsProbablyPrime(this BigInteger value, int witnesses = 10) {
            if (value <= 1) return false;
            
            if (witnesses <= 0) witnesses = 10;
            
            BigInteger d = value - 1;
            int s = 0;
            
            while (d % 2 == 0) {
                d /= 2;
                s += 1;
            }
            
            Byte[] bytes = new Byte[value.ToByteArray().LongLength];
            BigInteger a;
            
            for (int i = 0; i < witnesses; i++) {
                do {
                    var Gen = new Random();
                    Gen.NextBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= value - 2);
                
                BigInteger x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1) continue;
                
                for (int r = 1; r < s; r++) {
                    x = BigInteger.ModPow(x, 2, value);
                    if (x == 1) return false;
                    if (x == value - 1) break;
                }
                
                if (x != value - 1) return false;
            }
            return true;
        }
    }
}