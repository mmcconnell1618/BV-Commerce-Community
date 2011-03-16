using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Geography
{
    public class RegionSnapShot
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public RegionSnapShot()
        {
            Name = string.Empty;
            Abbreviation = string.Empty;
        }
    }
}
