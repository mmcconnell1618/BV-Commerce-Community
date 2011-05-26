using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{    

    [Serializable()]    
    [XmlInclude(typeof(BVSoftware.CommerceDTO.v1.CustomPropertyDTO))]
    [XmlInclude(typeof(BVSoftware.CommerceDTO.v1.Shipping.ShippableItemDTO))]
    [XmlInclude(typeof(BVSoftware.CommerceDTO.v1.Shipping.ShippingModeDTO))]
    [XmlInclude(typeof(BVSoftware.CommerceDTO.v1.Catalog.ProductStatusDTO))]
    [XmlInclude(typeof(BVSoftware.CommerceDTO.v1.Catalog.ProductInventoryModeDTO))]
    [XmlInclude(typeof(BVSoftware.CommerceDTO.v1.Catalog.ProductDescriptionTabDTO))]
    public class ProductDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public string Sku { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductTypeId { get; set; }
        [DataMember]
        public List<CustomPropertyDTO> CustomProperties { get; set; }
        [DataMember]
        public decimal ListPrice { get; set; }
        [DataMember]
        public decimal SitePrice { get; set; }
        [DataMember]
        public string SitePriceOverrideText { get; set; }
        [DataMember]
        public decimal SiteCost { get; set; }
        [DataMember]
        public string MetaKeywords { get; set; }
        [DataMember]
        public string MetaDescription { get; set; }
        [DataMember]
        public string MetaTitle { get; set; }
        [DataMember]
        public bool TaxExempt { get; set; }
        [DataMember]
        public long TaxSchedule { get; set; }
        [DataMember]
        public Shipping.ShippableItemDTO ShippingDetails { get; set; }
        [DataMember]
        public Shipping.ShippingModeDTO ShippingMode {get;set;}
        [DataMember]
        public ProductStatusDTO Status { get; set; }
        [DataMember]
        public string ImageFileSmall { get; set; }
        [DataMember]
        public string ImageFileSmallAlternateText { get; set; }
        [DataMember]
        public string ImageFileMedium { get; set; }
        [DataMember]
        public string ImageFileMediumAlternateText { get; set; }
        [DataMember]
        public System.DateTime CreationDateUtc { get; set; }
        [DataMember]
        public int MinimumQty { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }
        [DataMember]
        public string LongDescription { get; set; }
        [DataMember]
        public string ManufacturerId { get; set; }
        [DataMember]
        public string VendorId { get; set; }
        [DataMember]
        public bool GiftWrapAllowed { get; set; }
        [DataMember]
        public decimal GiftWrapPrice { get; set; }
        [DataMember]
        public string Keywords { get; set; }
        [DataMember]
        public string PreContentColumnId { get; set; }
        [DataMember]
        public string PostContentColumnId { get; set; }
        [DataMember]
        public string UrlSlug { get; set; }        
        [DataMember]
        public string PreTransformLongDescription { get; set; }
        [DataMember]
        public ProductInventoryModeDTO InventoryMode { get; set; }
        [DataMember]
        public bool IsAvailableForSale { get; set; }
        //[DataMember]
        //public List<OptionDTO> Options { get; set; }
        //[DataMember]
        //public List<VariantDTO> Variants { get; set; }        
        [DataMember]
        public bool Featured { get; set; }
        [DataMember]
        public bool AllowReviews { get; set; }
        [DataMember]
        public List<ProductDescriptionTabDTO> Tabs { get; set; }        
        [DataMember]
        public long StoreId { get; set; }
                
        public ProductDTO()
        {
            this.Bvin = string.Empty;
            this.Sku = string.Empty;
            this.ProductName = string.Empty;
            this.ProductTypeId = string.Empty;
            this.CustomProperties = new List<CustomPropertyDTO>();
            this.ListPrice = 0m;
            this.SitePrice = 0m;
            this.SitePriceOverrideText = string.Empty;
            this.SiteCost = 0m;
            this.MetaKeywords = string.Empty;
            this.MetaDescription = string.Empty;
            this.MetaTitle = string.Empty;
            this.TaxExempt = false;
            this.TaxSchedule = -1;
            this.Status = ProductStatusDTO.Active;
            this.ImageFileSmall = string.Empty;
            this.ImageFileMedium = string.Empty;
            this.ImageFileSmallAlternateText = string.Empty;
            this.ImageFileMediumAlternateText = string.Empty;
            this.CreationDateUtc = DateTime.UtcNow;
            this.MinimumQty = 1;
            this.ShortDescription = string.Empty;
            this.LongDescription = string.Empty;
            this.ManufacturerId = string.Empty;
            this.VendorId = string.Empty;
            this.GiftWrapAllowed = false;
            this.Keywords = string.Empty;
            this.PreContentColumnId = string.Empty;
            this.PostContentColumnId = string.Empty;
            this.UrlSlug = string.Empty;
            this.PreTransformLongDescription = string.Empty;
            this.GiftWrapPrice = 0m;
            //this.Options = new List<OptionDTO>();
            //this.Variants = new List<VariantDTO>();
            this.ShippingDetails = new Shipping.ShippableItemDTO();
            this.Featured = false;
            this.AllowReviews = false;
            this.Tabs = new List<ProductDescriptionTabDTO>();
            this.StoreId = 0;
            this.IsAvailableForSale = true;
        }
    }
}
