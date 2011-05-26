using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BVSoftware.CommerceDTO.v1;
using BVSoftware.CommerceDTO.v1.Catalog;
using BVSoftware.CommerceDTO.v1.Client;
using BVSoftware.CommerceDTO.v1.Contacts;
using BVSoftware.CommerceDTO.v1.Content;
using BVSoftware.CommerceDTO.v1.Marketing;
using BVSoftware.CommerceDTO.v1.Membership;
using BVSoftware.CommerceDTO.v1.Orders;
using BVSoftware.CommerceDTO.v1.Shipping;
using BVSoftware.CommerceDTO.v1.Taxes;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    [Serializable]
    public class CustomProperty
    {

        private string _DeveloperId = string.Empty;
        private string _Key = string.Empty;
        private string _Value = string.Empty;

        public string DeveloperId
        {
            get { return _DeveloperId; }
            set { _DeveloperId = value; }
        }
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public CustomProperty()
        {

        }

        public CustomProperty(string devId, string propertyKey, string propertyValue)
        {
            _DeveloperId = devId;
            _Key = propertyKey;
            _Value = propertyValue;
        }
     
        public CustomPropertyDTO ToDto()
        {
            CustomPropertyDTO dto = new CustomPropertyDTO();
            dto.Value = this.Value;
            dto.Key = this.Key;
            dto.DeveloperId = this.DeveloperId;

            return dto;
        }


    }
}
