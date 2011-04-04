using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration
{
    public class MigrationSettings
    {
        public string DestinationServiceRootUrl {get;set;}
        public string ApiKey { get; set; }
        public MigrationSourceType SourceType { get; set; }

        public string ImagesRootFolder { get; set; }

        public bool ImportCategories {get;set;}
        public bool ImportProducts {get;set;}
        public bool ImportUsers {get;set;}
        public bool ImportAffiliates {get;set;}
        public bool ImportOrders {get;set;}
        public bool ImportOtherSettings {get;set;}

        public bool ClearCategories {get;set;}
        public bool ClearProducts {get;set;}
        public bool ClearOrders {get;set;}
        public bool ClearAffiliates {get;set;}

        public bool UseMetricUnits {get;set;}

        public DateTime IgnoreOrdersOlderThan {get;set;}

        public string SingleOrderImport {get;set;}
        public string SingleSkuImport {get;set;}
        public int ImportProductLimit { get; set; }

        public int UserStartPage { get; set; }

        public string SQLServer { get; set; }
        public string SQLDatabase { get; set; }
        public string SQLUsername { get; set; }
        public string SQLPassword { get; set; }

        public string SourceConnectionString()
        {
            string result = "Data Source=" + SQLServer + ";";
            result += "Initial Catalog=" + SQLDatabase + ";";
            result += "User Id=" + SQLUsername + ";";
            result += "Password=" + SQLPassword + ";";
            return result;
        }

        public MigrationSettings()
        {
            DestinationServiceRootUrl = "http://localhost/bv6/api/v1";
            ImagesRootFolder = string.Empty;
            ApiKey = string.Empty;
            SourceType = MigrationSourceType.BV5;
            SQLServer = string.Empty;
            SQLDatabase = string.Empty;
            SQLUsername = string.Empty;
            SQLPassword = string.Empty;
            ImportCategories = false;
            ImportProducts = false;
            ImportUsers = false;
            ImportAffiliates = false;
            ImportOrders = false;
            ImportOtherSettings = false;
            ClearCategories = false;
            ClearProducts = false;
            ClearOrders = false;
            ClearAffiliates = false;
            UseMetricUnits = false;
            IgnoreOrdersOlderThan = new DateTime(1975, 1, 1);
            SingleOrderImport = string.Empty;
            SingleSkuImport = string.Empty;
            ImportProductLimit = -1;
            UserStartPage = 1;
        }

        public void PrepArgs()
        {
            this.DestinationServiceRootUrl = this.DestinationServiceRootUrl.Trim();

            if (!this.DestinationServiceRootUrl.StartsWith("http://") &&
                !this.DestinationServiceRootUrl.StartsWith("https://"))
            {
                this.DestinationServiceRootUrl = "http://" + this.DestinationServiceRootUrl;
            }
            this.DestinationServiceRootUrl = this.DestinationServiceRootUrl.TrimEnd('/');

            // Prep Image Root Folder
            this.ImagesRootFolder.TrimEnd('\\');
        }
    }
}
