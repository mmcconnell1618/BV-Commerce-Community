using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class PriceGroupDTO
    {
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
		public string Name {get;set;}
        [DataMember]
		public PricingTypesDTO PricingType {get;set;}
        [DataMember]
		public decimal AdjustmentAmount {get;set;}

        public PriceGroupDTO()
        {
            this.StoreId = 0;
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.Name = string.Empty;
            this.PricingType = PricingTypesDTO.PercentageOffListPrice;
            this.AdjustmentAmount = 0m;
        }
    }
}
