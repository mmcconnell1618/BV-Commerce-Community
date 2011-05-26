using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class OptionDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public OptionTypesDTO OptionType { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool NameIsHidden { get; set; }
        [DataMember]
        public bool IsVariant { get; set; }
        [DataMember]
        public bool IsShared { get; set; }
        [DataMember]
        public List<OptionSettingDTO> Settings { get; set; }
        [DataMember]
        public List<OptionItemDTO> Items { get; set; }

        public OptionDTO()
        {
            Bvin = string.Empty;
            StoreId = 0;
            OptionType = OptionTypesDTO.Uknown;
            Name = string.Empty;
            NameIsHidden = false;
            IsVariant = false;
            IsShared = false;
            Settings = new List<OptionSettingDTO>();
            Items = new List<OptionItemDTO>();
        }
    }
}
