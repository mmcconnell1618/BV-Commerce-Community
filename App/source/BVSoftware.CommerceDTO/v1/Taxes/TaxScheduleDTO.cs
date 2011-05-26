using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Taxes
{
    [DataContract]
    public class TaxScheduleDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string Name { get; set; }

        public TaxScheduleDTO()
        {
            Id = 0;
            StoreId = 0;
            Name = string.Empty;
        }
    }
}
