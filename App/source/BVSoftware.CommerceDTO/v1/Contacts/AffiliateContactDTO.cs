using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class AffiliateContactDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string AffiliateId { get; set; }
        [DataMember]
        public string UserId { get; set; }

        public AffiliateContactDTO()
        {
            Id = 0;
            AffiliateId = string.Empty;
            UserId = string.Empty;
            StoreId = 0;
        }
    }
}
