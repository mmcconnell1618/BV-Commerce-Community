using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Membership
{
    [DataContract]
    public class UserSearchCriteriaDTO
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }

        public UserSearchCriteriaDTO()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
    }
}
