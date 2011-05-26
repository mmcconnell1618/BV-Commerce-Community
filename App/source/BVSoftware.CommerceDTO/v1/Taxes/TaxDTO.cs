using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Taxes
{
    [DataContract]
    public class TaxDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string RegionAbbreviation { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public long TaxScheduleId { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        [DataMember]
        public bool ApplyToShipping { get; set; }

        public TaxDTO()
        {
            Id = 0;
            StoreId = 0;
            CountryName = string.Empty;
            RegionAbbreviation = string.Empty;
            PostalCode = string.Empty;
            TaxScheduleId = 0;
            Rate = 0;
            ApplyToShipping = false;
        }
    }
}
