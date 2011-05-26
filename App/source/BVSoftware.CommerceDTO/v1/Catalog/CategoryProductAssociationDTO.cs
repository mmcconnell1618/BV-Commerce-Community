using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    public class CategoryProductAssociationDTO
    {
        public long Id { get; set; }
        public string CategoryId { get; set; }
        public string ProductId { get; set; }
        public int SortOrder { get; set; }
        public long StoreId { get; set; }

        public CategoryProductAssociationDTO()
        {
            this.Id = 0;
            this.CategoryId = string.Empty;
            this.ProductId = string.Empty;
            this.SortOrder = 0;
            this.StoreId = 0;
        }

    }
}
