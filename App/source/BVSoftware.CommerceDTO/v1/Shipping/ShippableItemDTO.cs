using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Shipping
{
    [DataContract]
    public class ShippableItemDTO
    {
        [DataMember]
        public bool IsNonShipping { get; set; }
        [DataMember]
        public decimal ExtraShipFee { get; set; }
        [DataMember]
        public decimal Weight { get; set; }
        [DataMember]
        public decimal Length { get; set; }
        [DataMember]
        public decimal Width { get; set; }
        [DataMember]
        public decimal Height { get; set; }
        [DataMember]
        public long ShippingScheduleId { get; set; }
        [DataMember]
        public ShippingModeDTO ShippingSource { get; set; }
        [DataMember]
        public string ShippingSourceId { get; set; }

        [DataMember]
        public bool ShipSeparately { get; set; }

        public ShippableItemDTO()
        {
            IsNonShipping = false;
            ExtraShipFee = 0m;
            Weight = 0m;
            Length = 0m;
            Width = 0m;
            Height = 0m;
            ShippingScheduleId = 0;
            ShippingSource = ShippingModeDTO.ShipFromSite;
            ShippingSourceId = string.Empty;
            ShipSeparately = false;
        }
       
    }
}
