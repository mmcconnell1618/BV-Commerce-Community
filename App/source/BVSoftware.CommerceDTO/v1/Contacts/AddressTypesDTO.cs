using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public enum AddressTypesDTO
    {
        [EnumMember]
        General = 0,
        [EnumMember]
        Billing = 1,
        [EnumMember]
        Shipping = 2,
        [EnumMember]
        BillingAndShipping = 3,
        [EnumMember]
        StoreContact = 99
    }
}
