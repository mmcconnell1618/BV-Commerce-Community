using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.TestDomain
{
    public class ExampleBase
    {
        public string bvin { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }        
        public DateTime LastUpdatedUtc { get; set; }
        public List<ExampleSubObject> SubObjects { get; set; }

        public ExampleBase()
        {
            bvin = string.Empty;
            Description = string.Empty;
            IsActive = false;
            LastUpdatedUtc = DateTime.UtcNow;
            SubObjects = new List<ExampleSubObject>();
        }
    }
}
