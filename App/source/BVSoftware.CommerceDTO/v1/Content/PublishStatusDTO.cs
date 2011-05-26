using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Content
{
    [DataContract]
    public enum PublishStatusDTO
    {
        [EnumMember]
        Draft = 0,
        [EnumMember]
        Published = 1
    }
}
