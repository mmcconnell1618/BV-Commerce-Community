using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class VendorManufacturerContactDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string VendorManufacturerId { get; set; }
        [DataMember]
        public string UserId { get; set; }

        public VendorManufacturerContactDTO()
        {
            Id = 0;
            StoreId = 0;
            VendorManufacturerId = string.Empty;
            UserId = string.Empty;
        }
    }
}
