using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MerchantTribe.Web;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class CategoryDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string ParentId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public CategorySortOrderDTO DisplaySortOrder { get; set; }
        [DataMember]
        public CategorySourceTypeDTO SourceType { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string MetaKeywords { get; set; }
        [DataMember]
        public string MetaDescription { get; set; }
        [DataMember]
        public string MetaTitle { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string BannerImageUrl { get; set; }
        [DataMember]
        public int LatestProductCount { get; set; }
        [DataMember]
        public string CustomPageUrl { get; set; }
        [DataMember]
        public bool CustomPageOpenInNewWindow { get; set; }
        [DataMember]
        public bool ShowInTopMenu { get; set; }
        [DataMember]
        public bool Hidden { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string PreContentColumnId { get; set; }
        [DataMember]
        public string PostContentColumnId { get; set; }
        [DataMember]
        public bool ShowTitle { get; set; }
        [DataMember]
        public string Criteria { get; set; }
        [DataMember]
        public string CustomPageId { get; set; }
        [DataMember]
        public string PreTransformDescription { get; set; }
        [DataMember]
        public string Keywords { get; set; }
        [DataMember]
        public bool CustomerChangeableSortOrder { get; set; }
        [DataMember]
        public CustomPageLayoutTypeDTO CustomPageLayout { get; set; }
        [DataMember]
        public string RewriteUrl {get;set;}

        [DataMember]
        public List<ApiOperation> Operations { get; set; }

        //public List<CategoryPageVersionDTO> Versions { get; set; }

        public CategoryDTO()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.ParentId = "0";
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.DisplaySortOrder = CategorySortOrderDTO.ManualOrder;
            this.SourceType = CategorySourceTypeDTO.Manual;
            this.SortOrder = 0;
            this.MetaKeywords = string.Empty;
            this.MetaDescription = string.Empty;
            this.MetaTitle = string.Empty;
            this.ImageUrl = string.Empty;
            this.BannerImageUrl = string.Empty;
            this.LatestProductCount = 0;
            this.CustomPageUrl = string.Empty;
            this.ShowInTopMenu = false;
            this.Hidden = false;
            this.TemplateName = "BV Grid";
            this.PreContentColumnId = string.Empty;
            this.PostContentColumnId = string.Empty;
            this.ShowTitle = true;
            this.Criteria = string.Empty;
            this.CustomPageId = string.Empty;
            this.PreTransformDescription = string.Empty;
            this.Keywords = string.Empty;
            this.CustomerChangeableSortOrder = true;
            this.CustomPageLayout = CustomPageLayoutTypeDTO.Empty;
            this.RewriteUrl = string.Empty;

            //this.Versions = new List<CategoryPageVersionDTO>();

            this.Operations = new List<ApiOperation>();            
        }
              
    }

 
}
