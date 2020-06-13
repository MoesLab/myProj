using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Common
{
    public static class LoggingService
    {
        public static void WriteToFile(List<ILoggable> itemToLog)
        {
            foreach(var item in itemToLog)
            {
                Console.WriteLine(item.Log());


            }
        }
    }
}
