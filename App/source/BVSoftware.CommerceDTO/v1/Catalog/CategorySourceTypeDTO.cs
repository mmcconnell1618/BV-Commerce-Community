using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum CategorySourceTypeDTO
    {
        [EnumMember]
        Manual = 0,
        [EnumMember]
        ByRules = 1,
        [EnumMember]
        CustomLink = 2,
        [EnumMember]
        CustomPage = 3,
        [EnumMember]
        DrillDown = 4,
        [EnumMember]
        FlexPage = 5
    }
}
