using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1
{
    [DataContract]
    public class CustomPropertyDTO
    {
         
        [DataMember]
        public string DeveloperId {get;set;}
        [DataMember]
        public string Key {get;set;}
        [DataMember]
        public string Value {get;set;}
        
        public CustomPropertyDTO()
        {
            DeveloperId = string.Empty;
            Key = string.Empty;
            Value = string.Empty;
        }
    }
}
