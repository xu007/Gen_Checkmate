using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen_Checkmate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checkmate valuator is started...");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please type in the input path and/or filename, or type 'exit' to end.");
                var filename = Console.ReadLine();
                if (string.Compare(filename, "exit", StringComparison.OrdinalIgnoreCase) == 0) break;

                if (string.IsNullOrWhiteSpace(filename)) continue;

                string[] lines = {};
                try
                {
                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("Supplied filename: " + filename);
                        Console.WriteLine("Unable to locate target input file, please try again.");
                        continue;
                    }

                    lines = System.IO.File.ReadAllLines(filename);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Supplied filename: " + filename);
                    Console.WriteLine("Unable to read target input file, please try again..." + e.Message);

                    continue;
                }

                // Display the file contents by using a foreach loop.
                Console.WriteLine("Contents of the input file:  lines = " + lines.Length);
                Console.WriteLine();//extra line
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    Console.WriteLine("\t" + line);
                }
                Console.WriteLine();//extra line

                Console.WriteLine("type 'ok' to get result, or type 'exit' to end, others to read file");
                var command = Console.ReadLine();
                if (string.Compare(command, "exit", StringComparison.OrdinalIgnoreCase) == 0) break;
                if (string.Compare(command, "ok", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    try
                    {
                        var isChecked = Checkmate.IsCheckmate(lines);
                        Console.WriteLine(isChecked
                            ? "Yes, the black king IS in checkmate."
                            : "No, the black king is NOT in checkmate.");
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error occours... " + e.Message);
                    }
                    
                }
            }

            Console.WriteLine("Have a good day!  Press any key to exit.");
            Console.ReadKey();
        }
    }
}
