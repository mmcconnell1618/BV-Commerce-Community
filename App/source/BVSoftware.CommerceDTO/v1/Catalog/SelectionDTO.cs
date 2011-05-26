using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class SelectionDTO
    {
        [DataMember]
        public string OptionBvin {get;set;}
        [DataMember]
        public string SelectionData {get;set;}

        public SelectionDTO()
        {
            OptionBvin = string.Empty;
            SelectionData = string.Empty;
        }
        public SelectionDTO(string optionBvin, string selectionData)
        {
            OptionBvin = optionBvin;
            SelectionData = selectionData;
        }
    }
}
