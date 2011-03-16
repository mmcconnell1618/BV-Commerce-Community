using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Validation
{
    public class ValidationHelper
    {
        public static void Required(string propertyName, string propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (propertyValue.Trim().Length < 1)
            {
                violations.Add(new RuleViolation(propertyName, propertyValue, propertyName + " is required", controlName));
            }        
        }

        public static void RequiredMinimum(int minimum, string propertyName, int propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (propertyValue < minimum)
            {
                violations.Add(new RuleViolation(propertyName, propertyValue.ToString(), propertyName + " is required", controlName));
            }        
        }

        public static void RangeCheck(int minimum, int maximum, string propertyName, int propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (propertyValue < minimum || propertyValue > maximum)
            {
                violations.Add(new RuleViolation(propertyName,
                                                 propertyValue.ToString(),
                                                 propertyName + " must be between " + minimum.ToString() + " and " + maximum.ToString(),
                                                 controlName));
            }
        }

        public static void LengthCheck(int minimum, int maximum, string propertyName, string propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (propertyValue.Trim().Length < minimum)
            {
                violations.Add(new RuleViolation(propertyName, 
                                                propertyValue, 
                                                propertyName + " minimum length is " + minimum.ToString(),
                                                controlName));
            }
            else
            {
                if (propertyValue.Trim().Length > maximum)
                {
                    violations.Add(new RuleViolation(propertyName,
                                                propertyValue,
                                                propertyName + " maximum length is " + maximum.ToString(),
                                                controlName));
                }
            }
        }

        public static void MaxLength(int maxmimum, string propertyName, string propertyValue, List<RuleViolation> violations, string controlName)
        {
            LengthCheck(0, maxmimum, propertyName, propertyValue, violations,controlName);
        }

        public static void ValidEmail(string propertyName, string propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (propertyValue.Trim().Length < 6)
            {
                violations.Add(new RuleViolation(propertyName,
                                                propertyValue,
                                                "Email address should be in the format user@domain.com",
                                                controlName));
            }
            else
            {
                if (!propertyValue.Contains("@") || !propertyValue.Contains("."))
                {
                    violations.Add(new RuleViolation(propertyName,
                                                propertyValue,
                                                "Email address should be in the format user@domain.com",
                                                controlName));
                }
            }
        }

        public static void ValidateTrue(bool valueToCheck, string errorMessage, string propertyName, string propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (!valueToCheck)
            {
                violations.Add(new RuleViolation(propertyName,
                                            propertyValue,
                                            errorMessage,
                                            controlName));
            }
        }

        public static void ValidateFalse(bool valueToCheck, string errorMessage, string propertyName, string propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (valueToCheck)
            {
                violations.Add(new RuleViolation(propertyName,
                                            propertyValue,
                                            errorMessage,
                                            controlName));
            }
        }

        public static void GreaterThanZero(string propertyName, decimal propertyValue, List<RuleViolation> violations, string controlName)
        {
            if (propertyValue < 0)
            {
                violations.Add(new RuleViolation(propertyName, propertyValue.ToString(), propertyName + " must be greater than zero", controlName));
            }   
        }

    }
}
