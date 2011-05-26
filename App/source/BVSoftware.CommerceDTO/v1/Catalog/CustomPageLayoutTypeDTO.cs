using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum CustomPageLayoutTypeDTO
    {
        [EnumMember]
        Empty = 0,
        [EnumMember]
        WithSideBar = 5
    }
}
