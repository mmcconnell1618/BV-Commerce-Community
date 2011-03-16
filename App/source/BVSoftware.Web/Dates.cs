using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class Dates
    {
        public static string MonthToString(int m)
        {
            string result = m.ToString();
            switch (m)
            {
                case 1:
                    result = "Jan";
                    break;
                case 2:
                    result = "Feb";
                    break;
                case 3:
                    result = "Mar";
                    break;
                case 4:
                    result = "Apr";
                    break;
                case 5:
                    result = "May";
                    break;
                case 6:
                    result = "Jun";
                    break;
                case 7:
                    result = "Jul";
                    break;
                case 8:
                    result = "Aug";
                    break;
                case 9:
                    result = "Sep";
                    break;
                case 10:
                    result = "Oct";
                    break;
                case 11:
                    result = "Nov";
                    break;
                case 12:
                    result = "Dec";
                    break;
            }
            return result;
        }

        public static string FriendlyLocalDateFromUtc(DateTime dutc)
        {
            return FriendlyShortDate(dutc.ToLocalTime(), DateTime.Now.Year);
        }

        // Takes a date and returns a friendly version
        public static string FriendlyShortDate(DateTime d, int currentYear)
        {
            string result = string.Empty;
            result = MonthToString(d.Month) + "-" + d.Day.ToString();

            if (d.Year < currentYear)
            {
                string fullYear = d.Year.ToString();
                if (fullYear.Length > 2)
                {
                    result = result + " " + fullYear.Substring(fullYear.Length - 2, 2);
                }
            }
            return result;
        }

        public static DateTime ZeroOutTime(DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, 0, 0, 0, 0, input.Kind);
        }
        
        public static DateTime MaxOutTime(DateTime input)
        {
            // Note: Only precise to seconds for SQL compatibility
            DateTime result = new DateTime(input.Year, input.Month, input.Day, 23, 59, 59, 0);            
            return result;
        }

        //// Adds one month to the date but limits the day to 28 or less
        //public static DateTime AddOneMonthAndLimitToDay28(DateTime inputInLocalTime)
        //{
        //    DateTime Result = MaxOutTime(inputInLocalTime);
        //    Result = Result.AddMonths(1);
        //    if (Result.Day > 28)
        //    {
        //        Result = new DateTime(Result.Year, Result.Month, 28, Result.Hour, Result.Minute, Result.Second, Result.Millisecond);
        //    }
        //    return Result;
        //}

    }
}
