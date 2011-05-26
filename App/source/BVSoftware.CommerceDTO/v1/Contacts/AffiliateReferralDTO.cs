using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class AffiliateReferralDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime TimeOfReferralUtc { get; set; }
        [DataMember]
		public long AffiliateId {get;set;}
        [DataMember]
		public string ReferrerUrl {get;set;}

        public AffiliateReferralDTO()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.TimeOfReferralUtc = DateTime.UtcNow;
            this.AffiliateId = 0;
            this.ReferrerUrl = string.Empty;
        }
    }
}
