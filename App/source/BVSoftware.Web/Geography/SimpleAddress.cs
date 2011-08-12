using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Geography
{
    public class SimpleAddress: IAddress
    {

        public string Street {get;set;}
        public string Street2 { get; set; }
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

        public SimpleAddress Clone()
        {
            SimpleAddress result = new SimpleAddress();
            result.City = this.City;
            result.CountryData.Bvin = this.CountryData.Bvin;
            result.CountryData.Name = this.CountryData.Name;
            result.PostalCode = this.PostalCode;
            result.RegionData.Abbreviation = this.RegionData.Abbreviation;
            result.RegionData.Name = this.RegionData.Name;
            result.Street = this.Street;
            result.Street2 = this.Street2;
            return result;
        }

        public static SimpleAddress CloneAddress(IAddress source)
        {
            SimpleAddress result = new SimpleAddress();

            result.City = source.City;
            result.CountryData.Bvin = source.CountryData.Bvin;
            result.CountryData.Name = source.CountryData.Name;
            result.PostalCode = source.PostalCode;
            result.RegionData.Abbreviation = source.RegionData.Abbreviation;
            result.RegionData.Name = source.RegionData.Name;
            result.Street = source.Street;
            result.Street2 = source.Street2;

            return result;
        }
		
    }
}
