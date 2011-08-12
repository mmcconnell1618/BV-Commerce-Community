using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductVolumeDiscountDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
        public string ProductId { get; set; }
        [DataMember]
        public int Qty { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public ProductVolumeDiscountTypeDTO DiscountType { get; set; }

        public ProductVolumeDiscountDTO()
        {
            Bvin = string.Empty;
            StoreId = 0;
            LastUpdated = DateTime.UtcNow;
            ProductId = string.Empty;
            Qty = 0;
            Amount = 0;
            DiscountType = ProductVolumeDiscountTypeDTO.None;
        }
    }
}
