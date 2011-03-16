using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Validation
{
    public interface IValidatable
    {
        bool IsValid();
        List<RuleViolation> GetRuleViolations();
    }
}

