using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1
{    
    [DataContract]
    public class ApiOperation
    {
        [DataMember]
        public string Uri { get; set; }
        [DataMember]
        public string Rel { get; set; }
        [DataMember]
        public string Description { get; set; }

        public ApiOperation()
        {
            Uri = string.Empty;
            Rel = string.Empty;
            Description = string.Empty;
        }
    }
}
