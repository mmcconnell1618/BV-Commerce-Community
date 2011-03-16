using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Logging
{
    public interface ILogger
    {
         void LogMessage(string message);
         void LogException(Exception ex);
    }
}
