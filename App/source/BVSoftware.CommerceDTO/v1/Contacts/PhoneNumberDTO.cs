using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class PhoneNumberDTO
    {
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Extension { get; set; }

        public PhoneNumberDTO()
        {
            CountryCode = string.Empty;
            Number = string.Empty;
            Extension = string.Empty;
        }
    }
}
