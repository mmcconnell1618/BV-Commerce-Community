using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    public class GeographyHelper
    {
        public static MerchantTribe.Web.Geography.Country TranslateCountry(string connectionString, string countryBvin)
        {
            MerchantTribe.Web.Geography.Country result = new MerchantTribe.Web.Geography.Country();            
            data.BV53Entities db = new data.BV53Entities(connectionString);
            var old = db.bvc_Country.Where(y => y.bvin == countryBvin).FirstOrDefault();
            if (old == null) return MerchantTribe.Web.Geography.Country.FindByISOCode("US");
            result = MerchantTribe.Web.Geography.Country.FindByISOCode(old.ISOCode);
            return result;
        }

        public static string TranslateRegionBvinToAbbreviation(string connString, string regionBvin)
        {
            string result = string.Empty;
            data.BV53Entities db = new data.BV53Entities(connString);
            var old = db.bvc_Region.Where(y => y.bvin == regionBvin).FirstOrDefault();
            if (old == null) return result;
            result = old.Abbreviation;
            return result;
        }
    }
}
