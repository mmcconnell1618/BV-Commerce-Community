using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductPropertyDTO
    {
        [DataMember]
        public long Id {get;set;}
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string PropertyName {get;set;}
        [DataMember]
        public string DisplayName {get;set;}
        [DataMember]
        public bool DisplayOnSite {get;set;}
        [DataMember]
        public bool DisplayToDropShipper {get;set;}
        [DataMember]
        public ProductPropertyTypeDTO TypeCode {get;set;}
        [DataMember]
        public string DefaultValue {get;set;}
        [DataMember]
        public string CultureCode {get;set;}
        [DataMember]
        public List<ProductPropertyChoiceDTO> Choices { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }

        public ProductPropertyDTO()
        {
            Id = 0;
            this.StoreId = 0;
            PropertyName = string.Empty;
            DisplayName = string.Empty;
            DisplayOnSite = true;
            DisplayToDropShipper = true;
            TypeCode = ProductPropertyTypeDTO.None;
            DefaultValue = string.Empty;
            CultureCode = "en-US";
            Choices = new List<ProductPropertyChoiceDTO>();
            //SortOrder = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;

        }
    }
}
