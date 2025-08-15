using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIImplementByMyself.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }
    }
}