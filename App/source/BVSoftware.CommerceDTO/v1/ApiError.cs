using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1
{

    [DataContract]
    public class ApiError
    {        
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }

        public ApiError()
        {
        }
        public ApiError(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }
    }
}
