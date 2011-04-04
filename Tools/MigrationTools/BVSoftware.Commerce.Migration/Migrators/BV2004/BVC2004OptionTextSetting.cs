using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV2004
{
    public class BVC2004OptionTextSetting
    {
        public string DisplayName { get; set; }
        public string Rows { get; set; }
        public string Columns { get; set; }
        public string Required { get; set; }
        public string WrapText { get; set; }

        public BVC2004OptionTextSetting()
        {
            DisplayName = string.Empty;
            Rows = "1";
            Columns = "10";
            Required = "0";
            WrapText = "0";
        }
    }
}
