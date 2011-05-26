using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum ProductInventoryModeDTO
    {
        [EnumMember]
        NotSet = -1,
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        AlwayInStock = 100,
        [EnumMember]
        WhenOutOfStockHide = 101,
        [EnumMember]
        WhenOutOfStockShow = 102,
        [EnumMember]
        WhenOutOfStockAllowBackorders = 103
    }
}
