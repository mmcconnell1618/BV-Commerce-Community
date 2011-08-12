using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Geography
{
    public interface IAddress
    {
        string Street { get; set; }
        string Street2 { get; set; }
        string City { get; set; }
        RegionSnapShot RegionData { get; set; }
        string PostalCode { get; set; }
        CountrySnapShot CountryData { get; set; }
    }
}
