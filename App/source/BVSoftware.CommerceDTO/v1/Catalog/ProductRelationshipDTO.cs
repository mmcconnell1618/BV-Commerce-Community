using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    public class ProductRelationshipDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string ProductId { get; set; }
        public string RelatedProductId { get; set; }
        public bool IsSubstitute { get; set; }
        public int SortOrder { get; set; }
        public string MarketingDescription { get; set; }

        public ProductRelationshipDTO()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.ProductId = string.Empty;
            this.RelatedProductId = string.Empty;
            this.IsSubstitute = false;
            this.SortOrder = 0;
            this.MarketingDescription = string.Empty;
        }
    }
}
