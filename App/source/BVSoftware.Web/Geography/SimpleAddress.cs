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

        public string ToHtmlString()
        {
            StringBuilder sb = new StringBuilder();
            if ((Street ?? string.Empty).Length > 0)
            {
                sb.Append(Street + "<br />");
            }            
            if (RegionData != null)
            {
                sb.Append((City ?? string.Empty) + ", " + RegionData.Abbreviation + " " + (PostalCode ?? string.Empty) + "<br />");
            }
            else
            {
                sb.Append(" ");                
            }
            if (CountryData != null)
            {
                if (CountryData.Name.Trim().Length > 0)
                {
                    sb.Append(CountryData.Name + "<br />");
                }
            }
            return sb.ToString();
        }
		
    }
}
