using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    public class BV5OptionHtmlSettings
    {
        public string HtmlData { get; set; }

        public BV5OptionHtmlSettings()
        {
            HtmlData = string.Empty;
        }
    }
}
