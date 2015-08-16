using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdLineTokenizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] tokens = Tokenizer.TokenizeCommandLineToStringArray(Environment.CommandLine);

            Console.WriteLine("Environment: {0}", Environment.CommandLine);
            Console.Write("args: ");
            for (int i = 0; i < args.Length; i++)
            {
                Console.Write(args[i].ToString() + " ");
            }
            Console.Write(Environment.NewLine);

            Console.Write("tokenizer: ");
            for (int i = 0; i < tokens.Length; i++)
            {
                Console.Write(tokens[i].ToString() + " ");
            }
            Console.Write(Environment.NewLine);

        }
    }
}
