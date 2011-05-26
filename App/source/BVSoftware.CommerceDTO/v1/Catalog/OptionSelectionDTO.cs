using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class OptionSelectionDTO
    {
        [DataMember]
        public string OptionBvin {get;set;}
        [DataMember]
        public string SelectionData {get;set;}

        public OptionSelectionDTO()
        {
            OptionBvin = string.Empty;
            SelectionData = string.Empty;
        }
    }
}
