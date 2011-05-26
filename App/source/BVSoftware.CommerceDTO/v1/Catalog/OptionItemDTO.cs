using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class OptionItemDTO
    {        
        
        [DataMember]
        public string Bvin {get;set;}
        [DataMember]
        public long StoreId {get;set;}
        [DataMember]
        public string OptionBvin {get;set;}
        [DataMember]
        public string Name {get;set;}
        [DataMember]
        public decimal PriceAdjustment {get;set;}
        [DataMember]
        public decimal WeightAdjustment {get;set;}
        [DataMember]
        public bool IsLabel {get;set;}
        [DataMember]
        public int SortOrder {get;set;}

        public OptionItemDTO()
        {
            Bvin = string.Empty;
            StoreId = 0;
            OptionBvin = string.Empty;
            Name = string.Empty;
            PriceAdjustment = 0;
            WeightAdjustment = 0;
            IsLabel = false;
            SortOrder = 0;
        }
          
    }
}
