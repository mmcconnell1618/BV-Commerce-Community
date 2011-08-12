using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BVSoftware.Web.Validation
{
    public class EmailValidation
    {
        public static bool MeetsEmailFormatRequirements(string input)
        {
            bool result = false;
            string tester = input.Trim();
            if (tester.Length < 4) return false;

            //string formula = @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"
            string formula = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
            
            result = Regex.IsMatch(tester,formula,RegexOptions.CultureInvariant);
            
            return result;
        }
    }
}
