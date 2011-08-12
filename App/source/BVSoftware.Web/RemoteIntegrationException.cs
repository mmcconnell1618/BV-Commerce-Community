using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class RemoteIntegrationException : Exception
    {
        public RemoteIntegrationException()
            : base()
        {

        }
        public RemoteIntegrationException(string message)
            : base(message)
        {

        }
        public RemoteIntegrationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
