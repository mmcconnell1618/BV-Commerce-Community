using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{    
    [DataContract]
    public enum PricingTypesDTO
    {
        [EnumMember]
        PercentageOffListPrice = 0,
        [EnumMember]
        AmountOffListPrice = 1,
        [EnumMember]
        PercentageAboveCost = 2,
        [EnumMember]
        AmountAboveCost = 3,
        [EnumMember]
        PercentageOffSitePrice = 4,
        [EnumMember]
        AmountOffSitePrice = 5
    }
}
