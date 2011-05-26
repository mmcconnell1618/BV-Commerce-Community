using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.CommerceDTO.v1.Content;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class CategoryPageVersionDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string PageId { get; set; }
        [DataMember]
        public string AdminName { get; set; }
        [DataMember]
        public long AvailableScheduleId { get; set; }
        [DataMember]
        public DateTime AvailableStartDateUtc { get; set; }
        [DataMember]
        public DateTime AvailableEndDateUtc { get; set; }
        [DataMember]
        public string SerializedParts { get; set; }
        [DataMember]
        public PublishStatusDTO PublishedStatus { get; set; }

        public CategoryPageVersionDTO()
        {
            Id = 0;
            PageId = string.Empty;
            AdminName = string.Empty;
            AvailableScheduleId = 0;
            AvailableStartDateUtc = DateTime.UtcNow;
            AvailableStartDateUtc = DateTime.UtcNow;
            SerializedParts = string.Empty;
            PublishedStatus = PublishStatusDTO.Draft;
        }
    }
}
