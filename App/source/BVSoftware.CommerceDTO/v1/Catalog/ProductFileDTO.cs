using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductFileDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string ProductId { get; set; }
        [DataMember]
        public int AvailableMinutes { get; set; }
        [DataMember]
        public int MaxDownloads { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }

        public ProductFileDTO()
        {
            Bvin = string.Empty;
            LastUpdated = DateTime.UtcNow;
            StoreId = 0;
            ProductId = string.Empty;
            AvailableMinutes = 0;
            MaxDownloads = 0;
            FileName = string.Empty;
            ShortDescription = string.Empty;
        }
    }
}
