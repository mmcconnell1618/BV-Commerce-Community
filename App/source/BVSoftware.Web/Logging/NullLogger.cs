using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Logging
{
    public class NullLogger: ILogger
    {
        
        public void LogMessage(string message)
        {            
            Console.WriteLine(message);            
        }

        public void LogException(Exception ex)
        {
            throw ex;
        }

    }
}
