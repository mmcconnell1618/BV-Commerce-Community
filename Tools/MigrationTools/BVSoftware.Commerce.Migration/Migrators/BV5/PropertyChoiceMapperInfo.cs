using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    public class PropertyChoiceMapperInfo
    {
        public string OldBvin { get; set; }
        public long NewBvin { get; set; }
        public string TextValue { get; set; }
        public int SortOrder { get; set; }

        public PropertyChoiceMapperInfo()
        {
            this.OldBvin = string.Empty;
            this.NewBvin = 0;
            this.TextValue = string.Empty;
            this.SortOrder = -1;
        }
    }
}
