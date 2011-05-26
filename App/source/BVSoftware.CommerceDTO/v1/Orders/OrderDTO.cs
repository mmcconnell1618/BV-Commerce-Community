using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderDTO
    {
        [DataMember]
        public List<OrderCouponDTO> Coupons { get; set; }
        [DataMember]
        public List<LineItemDTO> Items { get; set; }
        [DataMember]
        public List<OrderNoteDTO> Notes { get; set; }
        [DataMember]
        public List<OrderPackageDTO> Packages { get; set; }

        // Basics
        [DataMember]
        public int Id { get; set; } // not used as primary key, only for insert order in SQL pages
        [DataMember]
        public string Bvin { get; set; } // Primary Key
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public DateTime TimeOfOrderUtc { get; set; }
        [DataMember]
        public string OrderNumber { get; set; }
        [DataMember]
        public string ThirdPartyOrderId { get; set; }
        [DataMember]
        public string UserEmail { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public List<CustomPropertyDTO> CustomProperties { get; set; }

        // Status   
        [DataMember]
        public OrderPaymentStatusDTO PaymentStatus { get; set; }
        [DataMember]
        public OrderShippingStatusDTO ShippingStatus { get; set; }
        [DataMember]
        public bool IsPlaced { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        

        // Addresses
        [DataMember]
        public Contacts.AddressDTO BillingAddress { get; set; }
        [DataMember]
        public Contacts.AddressDTO ShippingAddress { get; set; }

        // Others
        [DataMember]
        public string AffiliateID { get; set; }
        [DataMember]
        public decimal FraudScore { get; set; }
        [DataMember]
        public string Instructions { get; set; }
        [DataMember]
        public string ShippingMethodId { get; set; }
        [DataMember]
        public string ShippingMethodDisplayName { get; set; }
        [DataMember]
        public string ShippingProviderId { get; set; }
        [DataMember]
        public string ShippingProviderServiceCode { get; set; }
        
        // Totals   
        [DataMember]             
        public decimal TotalTax { get; set; }
        [DataMember]
        public decimal TotalTax2 { get; set; }
        [DataMember]
        public decimal TotalShippingBeforeDiscounts { get; set; }
        [DataMember]
        public decimal TotalHandling { get; set; }

        [DataMember]
        public List<Marketing.DiscountDetailDTO> OrderDiscountDetails { get; set; }
        [DataMember]
        public List<Marketing.DiscountDetailDTO> ShippingDiscountDetails { get; set; }

        public OrderDTO()
        {
            this.AffiliateID = string.Empty;
            this.BillingAddress = new Contacts.AddressDTO();
            this.Bvin = string.Empty;
            this.Coupons = new List<OrderCouponDTO>();
            this.CustomProperties = new List<CustomPropertyDTO>();
            this.FraudScore = 0;
            this.Id = 0;
            this.Instructions = string.Empty;
            this.IsPlaced = false;
            this.Items = new List<LineItemDTO>();
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.Notes = new List<OrderNoteDTO>();
            this.OrderDiscountDetails = new List<Marketing.DiscountDetailDTO>();
            this.OrderNumber = string.Empty;
            this.Packages = new List<OrderPackageDTO>();
            this.PaymentStatus = OrderPaymentStatusDTO.Unknown;
            this.ShippingAddress = new Contacts.AddressDTO();
            this.ShippingDiscountDetails = new List<Marketing.DiscountDetailDTO>();
            this.ShippingMethodDisplayName = string.Empty;
            this.ShippingMethodId = string.Empty;
            this.ShippingProviderId = string.Empty;
            this.ShippingProviderServiceCode = string.Empty;
            this.ShippingStatus = OrderShippingStatusDTO.Unknown;
            this.StatusCode = string.Empty;
            this.StatusName = string.Empty;
            this.StoreId = 0;
            this.ThirdPartyOrderId = string.Empty;
            this.TimeOfOrderUtc = DateTime.UtcNow;
            this.TotalHandling = 0;
            this.TotalShippingBeforeDiscounts = 0;
            this.TotalTax = 0;
            this.TotalTax2 = 0;
            this.UserEmail = string.Empty;
            this.UserID = string.Empty;
        }
    }
}
