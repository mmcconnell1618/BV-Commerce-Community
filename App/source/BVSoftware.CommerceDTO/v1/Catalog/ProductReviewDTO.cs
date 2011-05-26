using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class ProductReviewDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public string UserID {get;set;}
        [DataMember]
        public string ProductBvin {get;set;}
        [DataMember]
        public System.DateTime ReviewDateUtc {get;set;}
        [DataMember]
        public ProductReviewRatingDTO Rating {get;set;}
        [DataMember]
        public int Karma {get;set;}
        [DataMember]
        public string Description {get;set;}
        [DataMember]
        public bool Approved {get;set;}
        [DataMember]
        public string ProductName {get;set;}

        public ProductReviewDTO()
        {
            Bvin = string.Empty;
            UserID = string.Empty;
            ProductBvin = string.Empty;
            ReviewDateUtc = DateTime.UtcNow;
            Rating = ProductReviewRatingDTO.ThreeStars;
            Karma = 0;
            Description = string.Empty;
            Approved = false;
            ProductName = string.Empty;
        }
    }
}
