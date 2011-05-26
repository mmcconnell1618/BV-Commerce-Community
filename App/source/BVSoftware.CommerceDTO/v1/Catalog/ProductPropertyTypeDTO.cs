using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum ProductPropertyTypeDTO
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        TextField = 1,
        [EnumMember]
        MultipleChoiceField = 2,
        [EnumMember]
        CurrencyField = 3,
        [EnumMember]
        DateField = 4,
        [EnumMember]
        HyperLink = 7
    }
}
