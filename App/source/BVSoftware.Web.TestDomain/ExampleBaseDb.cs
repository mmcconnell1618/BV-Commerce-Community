using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.TestDomain
{
    public class ExampleBaseDb
    {
        public string bvin { get; set; }
        public string DescriptionDb { get; set; }
        public bool IsActiveDb { get; set; }        
        public DateTime LastUpdatedUtcDb { get; set; }

        public ExampleBaseDb()
        {
            bvin = string.Empty;
            DescriptionDb = string.Empty;
            IsActiveDb = false;
            LastUpdatedUtcDb = DateTime.UtcNow;
        }
    }
}
