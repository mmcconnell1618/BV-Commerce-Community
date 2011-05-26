using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderPackageItemDTO
    {
        [DataMember]
        public string ProductBvin { get; set; }
        [DataMember]
        public long LineItemId { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        public OrderPackageItemDTO()
        {
            this.ProductBvin = string.Empty;
            this.LineItemId = 0;
            this.Quantity = 0;
        }
    }
}
