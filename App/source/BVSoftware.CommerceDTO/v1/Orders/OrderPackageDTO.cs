using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderPackageDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public System.DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public List<OrderPackageItemDTO> Items { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string OrderId { get; set; }
        [DataMember]
        public decimal Width { get; set; }
        [DataMember]
        public decimal Height { get; set; }
        [DataMember]
        public decimal Length { get; set; }
        [DataMember]
        public Shipping.LengthTypeDTO SizeUnits { get; set; }
        [DataMember]
        public decimal Weight { get; set; }
        [DataMember]
        public Shipping.WeightTypeDTO WeightUnits { get; set; }
        [DataMember]
        public string ShippingProviderId { get; set; }
        [DataMember]
        public string ShippingProviderServiceCode { get; set; }
        [DataMember]
        public string TrackingNumber { get; set; }
        [DataMember]
        public bool HasShipped { get; set; }
        [DataMember]
        public DateTime ShipDateUtc { get; set; }
        [DataMember]
        public decimal EstimatedShippingCost { get; set; }
        [DataMember]
        public string ShippingMethodId { get; set; }
        [DataMember]
        public List<CustomPropertyDTO> CustomProperties { get; set; }

        public OrderPackageDTO()
        {
            this.CustomProperties = new List<CustomPropertyDTO>();
            this.Description = string.Empty;
            this.EstimatedShippingCost = 0;
            this.HasShipped = false;
            this.Height = 0;
            this.Id = 0;
            this.Items = new List<OrderPackageItemDTO>();
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.Length = 0;
            this.OrderId = string.Empty;
            this.ShipDateUtc = DateTime.UtcNow;
            this.ShippingMethodId = string.Empty;
            this.ShippingProviderId = string.Empty;
            this.ShippingProviderServiceCode = string.Empty;
            this.SizeUnits = Shipping.LengthTypeDTO.Inches;
            this.StoreId = 0;
            this.TrackingNumber = string.Empty;
            this.Weight = 0;
            this.WeightUnits = Shipping.WeightTypeDTO.Pounds;
            this.Width = 0;
                
        }
    }
}
