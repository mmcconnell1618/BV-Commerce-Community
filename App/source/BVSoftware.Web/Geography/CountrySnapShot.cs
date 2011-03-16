using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Geography
{
    public class CountrySnapShot
    {
        public string Name { get; set; }
        public string Bvin { get; set; }

        public CountrySnapShot()
        {
            Name = string.Empty;
            Bvin = string.Empty;
        }
    }
}
