using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum OptionTypesDTO
    {
        [EnumMember]
        Uknown = 0,
        [EnumMember]
        DropDownList = 100,
        [EnumMember]
        RadioButtonList = 200,
        [EnumMember]
        CheckBoxes = 300,
        [EnumMember]
        Html = 400,
        [EnumMember]
        TextInput = 500,
        [EnumMember]
        FileUpload = 600
    }
}
