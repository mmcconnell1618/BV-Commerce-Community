using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum ProductStatusDTO : int
    {
        [EnumMember]
        Active = 1,
        [EnumMember]
        Disabled = 0,
        [EnumMember]
        NotSet = -1
    }
}
