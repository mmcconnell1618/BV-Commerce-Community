using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class CategorySnapshotDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public string ParentId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public CategorySourceTypeDTO SourceType { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
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
        public string CustomPageId { get; set; }
        [DataMember]
        public CustomPageLayoutTypeDTO CustomPageLayout { get; set; }
        [DataMember]
        public string RewriteUrl { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string MetaTitle { get; set; }

        [DataMember]
        public List<ApiOperation> Operations { get; set; }

        //public List<CategoryPageVersionDTO> Versions { get; set; }

        public CategorySnapshotDTO()
        {
            Bvin = string.Empty;
            StoreId = 0;
            ParentId = string.Empty;
            Name = string.Empty;
            SourceType = CategorySourceTypeDTO.Manual;
            ImageUrl = string.Empty;
            LatestProductCount = 0;
            CustomPageUrl = string.Empty;
            CustomPageOpenInNewWindow = false;
            ShowInTopMenu = true;
            Hidden = false;
            CustomPageId = string.Empty;
            CustomPageLayout = CustomPageLayoutTypeDTO.Empty;
            RewriteUrl = string.Empty;
            SortOrder = 0;
            MetaTitle = string.Empty;
            Operations = new List<ApiOperation>();
        }

    }
}
