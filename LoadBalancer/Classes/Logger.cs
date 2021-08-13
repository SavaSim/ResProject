using LoadBalancer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer
{
    public class Logger : ILogger
    {
        public Object obj = new Object();
        string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()))); //pravi novi txt file u folderu gde je projekat
        private string logFilePath;  
        public Logger(string logFilePath = "")
        {
            this.logFilePath = logFilePath;
        }

        public void Log(string message) 
        {
            this.logFilePath = Path.Combine(solutionPath, "LogFile.txt");
            lock (obj)
            {
                if (File.Exists(this.logFilePath)) //refresuje file ukoliko postoji
                {
                    using (StreamWriter sw = File.AppendText(this.logFilePath))
                    {
                        sw.WriteLine(message);
                    }
                }
                else
                {
                    StreamWriter sw = new StreamWriter(this.logFilePath); //pravi novi ukoliko ne postoji
                    sw.WriteLine(message);
                    sw.Close();
                }
            }
        }
    }
}
