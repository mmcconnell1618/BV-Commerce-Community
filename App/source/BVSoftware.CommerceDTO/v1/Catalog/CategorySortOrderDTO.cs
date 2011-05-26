using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum CategorySortOrderDTO
    {
        [EnumMember()]
        None = 0,
        [EnumMember()]
        ManualOrder = 1,
        [EnumMember()]
        ProductName = 2,
        [EnumMember()]
        ProductPriceAscending = 3,
        [EnumMember()]
        ProductPriceDescending = 4,
        [EnumMember()]
        ManufacturerName = 5
    }
}
