using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SIMPLC_
{
    class Program
    {
        static void Main(string[] args)
        {
			if ( args.Length == 3 )
			{
				string
					source = args[0],
					output = args[1],
					origin = args[2];

				FileStream fileReader = File.OpenRead(source);
				FileStream fileWriter = File.OpenWrite(output);

				new ASSEMBLER(null, null, null, true);
			}

			else
			{
				Console.WriteLine(
@"
Usage: simplc source output origin
");
			}
        }
    }
}
