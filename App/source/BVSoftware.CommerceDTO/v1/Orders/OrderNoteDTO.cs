using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public class OrderNoteDTO
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public string OrderID { get; set; }
        [DataMember]
        public DateTime AuditDate { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public bool IsPublic { get; set; }

        public OrderNoteDTO()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.OrderID = string.Empty;
            this.AuditDate = DateTime.UtcNow;
            this.Note = string.Empty;
            this.IsPublic = false;
        }
    }
}
