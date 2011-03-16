using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.TestDomain
{
    public class ExampleSubObjectDb
    {
        public long Id { get; set; }
        public string BaseIdDb { get; set; }
        public string NameDb { get; set; }
        public int SortOrderDb { get; set; }

        public ExampleSubObjectDb()
        {
            Id = 0;
            BaseIdDb = string.Empty;
            NameDb = string.Empty;
            SortOrderDb = 0;
        }
    }
}
