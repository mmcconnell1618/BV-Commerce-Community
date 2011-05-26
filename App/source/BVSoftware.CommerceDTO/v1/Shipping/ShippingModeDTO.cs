using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Shipping
{
    [DataContract]
    public enum ShippingModeDTO
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        ShipFromSite = 1,
        [EnumMember]
        ShipFromVendor = 2,
        [EnumMember]
        ShipFromManufacturer = 3
    }
}
