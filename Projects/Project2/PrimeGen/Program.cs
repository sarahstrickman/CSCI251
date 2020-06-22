﻿
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Numerics;

namespace primegen
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: primegen bits [count]\n" +
                                  "- bits: the number of bits of " +
                                  "the prime number, this must be a multiple " +
                                  "of 8, and at least 32 bits.\n" +
                                  "- count (optional): the number of prime " +
                                  "numbers to generate. Defaults to 1.");
                Environment.Exit(0);
            }
            
            // get the count of prime numbers
            var count = 1;
            if (args.Length == 2)
            {
                if (Int32.TryParse(args[1], out count))
                {
                    count = Int32.Parse(args[1]);
                }
                else
                {
                    Console.WriteLine("Please input a valid value for count.");
                    Environment.Exit(0);
                }
            }

            // get number of bits
            var numBits = new BigInteger(0);
            var bigIntEight = new BigInteger(8);
            try
            {
                numBits = BigInteger.Parse(args[0]);
                
                // must be divisible by 8
                if (BigInteger.Remainder(numBits, bigIntEight) != BigInteger.Zero)
                {
                    Console.WriteLine("Number of bits must be a multiple of 8.");
                    Environment.Exit(0);
                }
                
                // must be greater than 32 bits (4 bytes)
                var minValue = new BigInteger(32);
                if (BigInteger.Compare(numBits, minValue) <= 0)
                {
                    Console.WriteLine("Number of bits should be greater than 32.");
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Please input a number input for bits.");
                Environment.Exit(0);
            }
            
            var numBytes = BigInteger.Divide(numBits, new BigInteger(8));
            
            //TODO : do things
        }
    }
}