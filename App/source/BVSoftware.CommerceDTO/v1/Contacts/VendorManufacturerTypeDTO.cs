using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public enum VendorManufacturerTypeDTO
    {
        [EnumMember]
        Unknown = -1,
        [EnumMember]
        Vendor = 0,
        [EnumMember]
        Manufacturer = 1
    }
}
