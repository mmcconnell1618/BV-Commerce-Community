using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    public class PageOfProducts
    {
        public List<ProductDTO> Products { get; set; }
        public int TotalProductCount { get; set; }

        public PageOfProducts()
        {
            Products = new List<ProductDTO>();
            TotalProductCount = 0;
        }
    }
}
