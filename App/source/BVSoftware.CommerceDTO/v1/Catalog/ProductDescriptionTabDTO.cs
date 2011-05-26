using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductDescriptionTabDTO
    {

        [DataMember]
        public string Bvin {get;set;}
        [DataMember]
        public string TabTitle {get;set;}
        [DataMember]
        public string HtmlData {get;set;}
        [DataMember]
        public int SortOrder {get;set;}

        public ProductDescriptionTabDTO()
        {
            Bvin = string.Empty;
            TabTitle = string.Empty;
            HtmlData = string.Empty;
            SortOrder = 0;
        }
    }
}
