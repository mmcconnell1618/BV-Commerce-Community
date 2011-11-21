using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    public class PropertyMapperInfo
    {
        public string OldBvin { get; set; }
        public long NewBvin { get; set; }
        public CommerceDTO.v1.Catalog.ProductPropertyTypeDTO PropertyType { get; set; }
        public List<PropertyChoiceMapperInfo> Choices { get; set; }

        public PropertyMapperInfo()
        {
            this.OldBvin = string.Empty;
            this.NewBvin = 0;
            this.PropertyType = CommerceDTO.v1.Catalog.ProductPropertyTypeDTO.None;
            this.Choices = new List<PropertyChoiceMapperInfo>();
        }
    }
}
