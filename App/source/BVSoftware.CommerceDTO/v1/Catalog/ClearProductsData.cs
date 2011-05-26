using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    public class ClearProductsData
    {
        public int ProductsCleared { get; set; }
        public int ProductsRemaining { get; set; }

        public ClearProductsData()
        {
            ProductsCleared = 0;
            ProductsRemaining = 0;
        }
    }
}
