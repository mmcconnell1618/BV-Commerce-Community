using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Logging
{
    public class TextLogger : ILogger
    {

        public Queue<string> Messages { get; private set; }

        public TextLogger()
        {
            Messages = new Queue<string>();
        }

        public void LogMessage(string message)
        {
            Messages.Enqueue(message);
        }

        public void LogException(Exception ex)
        {
            Messages.Enqueue(ex.Message + " | " + ex.StackTrace);
        }
    }
}
