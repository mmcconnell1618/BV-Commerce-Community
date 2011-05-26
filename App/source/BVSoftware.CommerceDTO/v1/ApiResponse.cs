using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1
{    
    [DataContract]
    public class ApiResponse<T>: IApiResponse
    {
        [DataMember]
        public List<ApiError> Errors { get; set; }

        [DataMember]
        public T Content { get; set; }

        public ApiResponse()
        {
            Errors = new List<ApiError>();
        }
    }
}
