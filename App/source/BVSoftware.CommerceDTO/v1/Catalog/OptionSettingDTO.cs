using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class OptionSettingDTO
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }

        public OptionSettingDTO()
        {
            Key = string.Empty;
            Value = string.Empty;
        }
    }
}
