using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer
{
	public class Reader : IReader
	{
        string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
        public void ReadFromDB(string file)
		{
            file = Path.Combine(solutionPath, file + ".txt");
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(file))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    Console.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
	}
}
