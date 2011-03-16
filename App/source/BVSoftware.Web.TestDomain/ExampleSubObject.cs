using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.TestDomain
{
    public class ExampleSubObject
    {
        public long Id { get; set; }
        public string BaseId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public ExampleSubObject()
        {
            Id = 0;
            BaseId = string.Empty;
            Name = string.Empty;
            SortOrder = 0;
        }

    }
}
