using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
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
    public class PackageItem
    {

        public string ProductBvin { get; set; }
        public long LineItemId { get; set; }
        public int Quantity { get; set; }

        public static Collection<PackageItem> FromXml(string data)
        {
            Collection<PackageItem> result = new Collection<PackageItem>();

            try
            {
                StringReader tr = new StringReader(data);
                XmlSerializer xs = new XmlSerializer(result.GetType());                
                result = (Collection<PackageItem>)xs.Deserialize(tr);
                if (result == null) result = new Collection<PackageItem>();                
            }
            catch
            {
                result = new Collection<PackageItem>();
            }

            return result;
        }
    }
}
