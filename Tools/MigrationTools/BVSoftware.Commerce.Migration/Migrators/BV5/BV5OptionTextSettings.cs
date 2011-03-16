using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    public class BV5OptionTextSettings
    {
        public string DisplayName { get; set; }
        public string Rows { get; set; }
        public string Columns { get; set; }
        public string Required { get; set; }
        public string WrapText { get; set; }

        public BV5OptionTextSettings()
        {
            DisplayName = string.Empty;
            Rows = "1";
            Columns = "10";
            Required = "0";
            WrapText = "0";
        }
    }
}
