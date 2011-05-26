using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public enum OrderPaymentStatusDTO
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Unpaid = 1,
        [EnumMember]
        PartiallyPaid = 2,
        [EnumMember]
        Paid = 3,
        [EnumMember]
        Overpaid = 4
    }
}
