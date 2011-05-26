using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderTransactionCardDataDTO
    {
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public bool CardIsEncrypted { get; set; }
        [DataMember]
        public int ExpirationMonth {get;set;}
        [DataMember]
        public int ExpirationYear { get; set; }
        [DataMember]
        public string SecurityCode { get; set; }
        [DataMember]
        public string CardHolderName { get; set; }

        public OrderTransactionCardDataDTO()
        {
            this.CardHolderName = string.Empty;
            this.CardIsEncrypted = false;
            this.CardNumber = string.Empty;
            this.ExpirationMonth = 1;
            this.ExpirationYear = 1900;
            this.SecurityCode = string.Empty;            
        }
    }
}
