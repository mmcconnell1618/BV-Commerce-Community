using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Logging
{
    public class SupressLogger : ILogger
    {
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void LogException(Exception ex)
        {            
            Console.WriteLine(ex.Message + " | " + ex.StackTrace);
        }
    }
}
