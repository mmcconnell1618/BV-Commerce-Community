using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Validation
{
    [CLSCompliant(true)]
    public class RuleViolation
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string ControlName { get; set; }
        public string ErrorMessage { get; set; }

        public RuleViolation()
        {
        }

        public RuleViolation(string name, string value, string message)
        {
            PropertyName = name;
            PropertyValue = value;
            ErrorMessage = message;
            ControlName = string.Empty;
        }

        public RuleViolation(string name, string value, string message, string controlName)
        {
            PropertyName = name;
            PropertyValue = value;
            ErrorMessage = message;
            ControlName = controlName;
        }

    }
    
}
