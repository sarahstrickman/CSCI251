using System;

namespace Project3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: project3 <option> [Optional: " +
                                  "other arguments]\nAvailable options:");
                Console.WriteLine("keyGen [keysize]: Generate a keypair of " +
                                  "[keysize] bits.");
                Console.WriteLine("sendKey [email]: Sends the public key " +
                                  "associated with this [email] to the server.");
                Console.WriteLine("getKey [email]: Retrieve the public key " +
                                  "of a user with [email] account.");
                Console.WriteLine("sendMsg [email] [plaintext]: Encrypts the " +
                                  "[plaintext] with the [email] user's public " +
                                  "key, then sends the message to the user.");
                Console.WriteLine("getMsg [email]: Retrieve the message for " +
                                  "a particular user with [email] account.");
                
                Environment.Exit(0);
            }

            switch (args[0].ToLowerInvariant())
            {
                case "keygen":
                    //
                    break;
                case "sendkey":
                    //
                    break;
                case "getKey":
                    //
                    break;
                case "sendmsg":
                    //
                    break;
                case "getmsg":
                    //
                    break;
            }
        }
    }
}