using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class VendorManufacturerDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public AddressDTO Address { get; set; }
        [DataMember]
        public string DropShipEmailTemplateId { get; set; }
        [DataMember]
        public List<VendorManufacturerContactDTO> Contacts { get; set; }
        [DataMember]
        public VendorManufacturerTypeDTO ContactType { get; set; }

        public VendorManufacturerDTO()
        {
            Bvin = string.Empty;
            StoreId = 0;
            LastUpdated = DateTime.UtcNow;
            DisplayName = string.Empty;
            EmailAddress = string.Empty;
            Address = new AddressDTO();
            DropShipEmailTemplateId = string.Empty;
            Contacts = new List<VendorManufacturerContactDTO>();
            ContactType = VendorManufacturerTypeDTO.Vendor;
        }
    }
}
