using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class LineItemDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public System.DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public decimal BasePricePerItem { get; set; }
        [DataMember]
        public List<Marketing.DiscountDetailDTO> DiscountDetails { get; set; }
        [DataMember]
        public string OrderBvin { get; set; }
        [DataMember]
        public string ProductId { get; set; }
        [DataMember]
        public string VariantId { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductSku { get; set; }
        [DataMember]
        public string ProductShortDescription { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int QuantityReturned { get; set; }
        [DataMember]
        public int QuantityShipped { get; set; }
        [DataMember]
        public decimal ShippingPortion { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public decimal TaxPortion { get; set; }
        [DataMember]
        public List<Catalog.OptionSelectionDTO> SelectionData { get; set; }
        [DataMember]
        public long ShippingSchedule { get; set; }
        [DataMember]
        public long TaxSchedule { get; set; }
        [DataMember]
        public decimal ProductShippingWeight { get; set; }
        [DataMember]
        public decimal ProductShippingLength { get; set; }
        [DataMember]
        public decimal ProductShippingWidth { get; set; }
        [DataMember]
        public decimal ProductShippingHeight { get; set; }
        [DataMember]
        public List<CustomPropertyDTO> CustomProperties { get; set; }
        [DataMember]
        public Shipping.ShippingModeDTO ShipFromMode { get; set; }
        [DataMember]
        public string ShipFromNotificationId { get; set; }
        [DataMember]
        public Contacts.AddressDTO ShipFromAddress { get; set; }
        [DataMember]
        public bool ShipSeparately { get; set; }
        [DataMember]
        public decimal ExtraShipCharge { get; set; }

        public LineItemDTO()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.BasePricePerItem = 0;
            this.DiscountDetails = new List<Marketing.DiscountDetailDTO>();
            this.OrderBvin = string.Empty;
            this.ProductId = string.Empty;
            this.VariantId = string.Empty;
            this.ProductName = string.Empty;
            this.ProductSku = string.Empty;
            this.ProductShortDescription = string.Empty;
            this.Quantity = 0;
            this.QuantityReturned = 0;
            this.QuantityShipped = 0;
            this.ShippingPortion = 0;
            this.StatusCode = string.Empty;
            this.StatusName = string.Empty;
            this.TaxPortion = 0;
            this.SelectionData = new List<Catalog.OptionSelectionDTO>();
            this.ShippingSchedule = 0;
            this.TaxSchedule = 0;
            this.ProductShippingWeight = 0;
            this.ProductShippingLength = 0;
            this.ProductShippingWidth = 0;
            this.ProductShippingHeight = 0;
            this.CustomProperties = new List<CustomPropertyDTO>();
            this.ShipFromMode = Shipping.ShippingModeDTO.None;
            this.ShipFromNotificationId = string.Empty;
            this.ShipFromAddress = new Contacts.AddressDTO();
            this.ShipSeparately = false;
            this.ExtraShipCharge = 0;
        }
    }
}
