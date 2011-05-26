using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductImageDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public string ProductId { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Caption { get; set; }
        [DataMember]
        public string AlternateText { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public long StoreId { get; set; }

        public ProductImageDTO()
        {
            Bvin = string.Empty;
            LastUpdatedUtc = DateTime.UtcNow;
            ProductId = string.Empty;
            FileName = string.Empty;
            Caption = string.Empty;
            AlternateText = string.Empty;
            SortOrder = 0;
            StoreId = 0;
        }
    }
}
