using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum ProductReviewRatingDTO
    {
        [EnumMember]
        ZeroStars = 0,
        [EnumMember]
        OneStar = 1,
        [EnumMember]
        TwoStars = 2,
        [EnumMember]
        ThreeStars = 3,
        [EnumMember]
        FourStars = 4,
        [EnumMember]
        FiveStars = 5
    }
}
