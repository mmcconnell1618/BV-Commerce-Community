using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Shipping
{
    [DataContract]
    public enum LengthTypeDTO
    {
        [EnumMember]
        Inches = 1,
        [EnumMember]
        Centimeters = 2
    }
}
