using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    public class OrderSnapshotDTO
    {

        // Basics
        public int Id { get; set; } // not used as primary key, only for insert order in SQL pages
        public string bvin { get; set; } // Primary Key
        public long StoreId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public DateTime TimeOfOrderUtc { get; set; }
        public string OrderNumber { get; set; }
        public string ThirdPartyOrderId { get; set; }
        public string UserEmail { get; set; }
        public string UserID { get; set; }
        public List<CustomPropertyDTO> CustomProperties { get; set; }

        // Status        
        public OrderPaymentStatusDTO PaymentStatus { get; set; }
        public OrderShippingStatusDTO ShippingStatus { get; set; }
        public bool IsPlaced { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        // Addresses
        public Contacts.AddressDTO BillingAddress { get; set; }
        public Contacts.AddressDTO ShippingAddress { get; set; }

        // Totals                        
        public decimal TotalTax { get; set; }
        public decimal TotalTax2 { get; set; }
        public decimal TotalOrderBeforeDiscounts { get; set; }
        public decimal TotalShippingBeforeDiscounts { get; set; }
        public decimal TotalShippingDiscounts { get; set; }
        public decimal TotalOrderDiscounts { get; set; }
        public decimal TotalHandling { get; set; }
        public decimal TotalGrand { get; set; }

        // Others
        public string AffiliateID { get; set; }
        public decimal FraudScore { get; set; }
        public string Instructions { get; set; }
        public string ShippingMethodId { get; set; }
        public string ShippingMethodDisplayName { get; set; }
        public string ShippingProviderId { get; set; }
        public string ShippingProviderServiceCode { get; set; }

        public OrderSnapshotDTO()
        {
            this.bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.TimeOfOrderUtc = DateTime.MinValue;
            this.OrderNumber = string.Empty;
            this.ThirdPartyOrderId = string.Empty;
            this.UserEmail = string.Empty;
            this.UserID = string.Empty;
            this.CustomProperties = new List<CustomPropertyDTO>();

            this.PaymentStatus = OrderPaymentStatusDTO.Unknown;
            this.ShippingStatus = OrderShippingStatusDTO.Unknown;
            this.IsPlaced = false;
            this.StatusCode = string.Empty;
            this.StatusName = string.Empty;

            this.BillingAddress = new Contacts.AddressDTO();
            this.ShippingAddress = new Contacts.AddressDTO();

            this.TotalTax = 0m;
            this.TotalTax2 = 0m;
            this.TotalOrderBeforeDiscounts = 0m;
            this.TotalShippingBeforeDiscounts = 0m;
            this.TotalShippingDiscounts = 0m;
            this.TotalOrderDiscounts = 0m;
            this.TotalHandling = 0m;
            this.TotalGrand = 0m;

            this.AffiliateID = string.Empty;
            this.FraudScore = -1m;
            this.Instructions = string.Empty;
            this.ShippingMethodId = string.Empty;
            this.ShippingMethodDisplayName = string.Empty;
            this.ShippingProviderId = string.Empty;
            this.ShippingProviderServiceCode = string.Empty;

        }
    }
}
