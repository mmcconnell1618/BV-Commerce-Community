using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Logging
{
    public interface ILoggable
    {
        ILogger Logger { get; set; }
    }
}
