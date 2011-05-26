using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class VariantDTO
    {
        [DataMember]              
        public string Bvin {get;set;}
        [DataMember]
        public string ProductId {get;set;}
        [DataMember]
        public string Sku {get;set;}
        [DataMember]
        public decimal Price {get;set;}
        [DataMember]
        public List<SelectionDTO> Selections {get;set;}
        [DataMember]
        public string UniqueKey {get;set;}
      
        public VariantDTO()
        {
            Bvin = string.Empty;
            ProductId = string.Empty;
            Selections = new List<SelectionDTO>();
            Sku = string.Empty;
            Price = -1;
            UniqueKey = string.Empty;
        }
    }
}
