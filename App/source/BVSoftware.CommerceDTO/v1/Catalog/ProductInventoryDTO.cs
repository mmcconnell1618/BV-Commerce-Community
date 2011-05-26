using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductInventoryDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
        public string ProductBvin {get;set;}
        [DataMember]
        public string VariantId {get;set;}
        [DataMember]
        public int QuantityOnHand {get;set;} // The total physical count of items on hand
        [DataMember]
        public int QuantityReserved {get;set;} // Count of items in stock but reserved for carts or orders		
        [DataMember]
        public int LowStockPoint {get;set;} // When does the Low Stock warning go out to merchant?

        public ProductInventoryDTO()
        {
            Bvin = string.Empty;
            LastUpdated = DateTime.UtcNow;
            ProductBvin = string.Empty;
            VariantId = string.Empty;
            QuantityOnHand = 0;
            QuantityReserved = 0;
            LowStockPoint = 0;
        }
    }
}
