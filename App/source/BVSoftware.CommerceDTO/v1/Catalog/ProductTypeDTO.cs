using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductTypeDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
        public bool IsPermanent { get; set; }
        [DataMember]
        public string ProductTypeName { get; set; }

        public ProductTypeDTO()
        {
            Bvin = string.Empty;
            this.StoreId = 0;
            LastUpdated = DateTime.UtcNow;
            IsPermanent = false;
            ProductTypeName = string.Empty;
        }
    }
}
