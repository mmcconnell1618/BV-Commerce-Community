using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductPropertyChoiceDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public long PropertyId {get;set;}
        [DataMember]
        public string ChoiceName {get;set;}
        [DataMember]
        public int SortOrder {get;set;}
        [DataMember]
        public DateTime LastUpdated {get;set;}

        public ProductPropertyChoiceDTO()
        {
            Id = 0;
            StoreId = 0;
            PropertyId = 0;
            ChoiceName = string.Empty;
            SortOrder = 0;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
