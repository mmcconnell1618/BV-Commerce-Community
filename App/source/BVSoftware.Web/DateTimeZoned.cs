using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class DateTimeZoned
    {
        private DateTime _dateUtc = DateTime.UtcNow;

        public DateTime AsUtc
        {
            get { return _dateUtc; }
            set { _dateUtc = value; }
        }

        public DateTime AsLocal
        {
            get { return _dateUtc.ToLocalTime(); }
            set { _dateUtc = value.ToUniversalTime(); }
        }

        public DateTime AsTimeZone(TimeZoneInfo tz)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(_dateUtc, tz);
        }

        public DateTimeZoned()
        {
        }

        public DateTimeZoned(DateTime utc)
        {
            AsUtc = utc;
        }

    }
}
