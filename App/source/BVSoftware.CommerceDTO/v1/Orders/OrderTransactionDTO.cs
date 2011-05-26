using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderTransactionDTO
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string OrderId { get; set; }
        [DataMember]
        public string OrderNumber { get; set; }
        [DataMember]
        public DateTime TimeStampUtc { get; set; }
        [DataMember]
        public OrderTransactionActionDTO Action { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public OrderTransactionCardDataDTO CreditCard { get; set; }
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public bool Voided { get; set; }
        [DataMember]
        public string RefNum1 { get; set; }
        [DataMember]
        public string RefNum2 { get; set; }
        [DataMember]
        public string LinkedToTransaction { get; set; }
        [DataMember]
        public string Messages { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public string PurchaseOrderNumber { get; set; }
        [DataMember]
        public string GiftCardNumber { get; set; }
        [DataMember]
        public string CompanyAccountNumber { get; set; }

        public OrderTransactionDTO()
        {
            this.Action = OrderTransactionActionDTO.Uknown;
            this.Amount = 0;
            this.CheckNumber = string.Empty;
            this.CompanyAccountNumber = string.Empty;
            this.CreditCard = new OrderTransactionCardDataDTO();
            this.GiftCardNumber = string.Empty;
            this.Id = new Guid();
            this.LinkedToTransaction = string.Empty;
            this.Messages = string.Empty;
            this.OrderId = string.Empty;
            this.OrderNumber = string.Empty;
            this.PurchaseOrderNumber = string.Empty;
            this.RefNum1 = string.Empty;
            this.RefNum2 = string.Empty;
            this.StoreId = 0;
            this.Success = false;
            this.TimeStampUtc = DateTime.UtcNow;
            this.Voided = false;

        }
    }
}
