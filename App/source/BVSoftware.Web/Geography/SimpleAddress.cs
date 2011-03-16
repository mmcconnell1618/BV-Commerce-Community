using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Geography
{
    public class SimpleAddress: IAddress
    {

        public string Street {get;set;}
        public string City {get;set;}
        public RegionSnapShot RegionData {get;set;}
        public string PostalCode {get;set;}
        public CountrySnapShot CountryData {get;set;}

        public SimpleAddress()
        {
            RegionData = new RegionSnapShot();
            CountryData = new CountrySnapShot();
        }
    }
}
