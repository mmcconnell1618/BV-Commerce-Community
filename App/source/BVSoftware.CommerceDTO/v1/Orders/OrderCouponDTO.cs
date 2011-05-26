using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderCouponDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public string OrderBvin { get; set; }
        [DataMember]
        public string CouponCode { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public string UserId { get; set; }

        public OrderCouponDTO()
        {
            Id = 0;
            StoreId = 0;
            LastUpdatedUtc = DateTime.UtcNow;
            OrderBvin = string.Empty;
            CouponCode = string.Empty;
            IsUsed = false;
            UserId = string.Empty;
        }
    }
}
