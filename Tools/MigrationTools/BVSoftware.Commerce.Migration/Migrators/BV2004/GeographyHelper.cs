using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV2004
{
    public class GeographyHelper
    {
        public static MerchantTribe.Web.Geography.Country TranslateCountry(string connectionString, string countryCode)
        {
            MerchantTribe.Web.Geography.Country result = new MerchantTribe.Web.Geography.Country();
            data.bvc2004Entities db = new data.bvc2004Entities(connectionString);
            var old = db.bvc_Country.Where(y => y.Code == countryCode).FirstOrDefault();
            if (old == null) return MerchantTribe.Web.Geography.Country.FindByISOCode("US");
            result = MerchantTribe.Web.Geography.Country.FindByISOCode(old.UPSCode);
            return result;
        }

        public static string TranslateRegionBvinToAbbreviation(string connString, string stateCode)
        {
            string result = string.Empty;
            int stateId = 0;
            int.TryParse(stateCode, out stateId);
            data.bvc2004Entities db = new data.bvc2004Entities(connString);
            var old = db.bvc_Region.Where(y => y.ID == stateId).FirstOrDefault();
            if (old == null) return result;
            result = old.Abbreviation;
            return result;
        }
    }
}
