using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "password";
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0} :: {1}", input, Crypto_New.Encrypt(input));
            }

            Console.WriteLine();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0} :: {1}", input, Crypto.Encrypt(input, "blah"));
            }

            Console.ReadKey();
        }
    }
}
