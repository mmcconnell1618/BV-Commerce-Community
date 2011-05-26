using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BVSoftware.CommerceDTO.v1.Catalog;
using BVSoftware.CommerceDTO.v1;

namespace BVSoftware.Commerce.Migration.Migrators.BV2004
{
    [Serializable]
    public class CustomProperty
    {
        public string Key {get;set;}
        public string Value {get;set;}
        public CustomProperty()
        {
            Key = string.Empty;
            Value = string.Empty;
        }

        public CustomProperty(string propertyKey, string propertyValue)
        {            
            Key = propertyKey;
            Value = propertyValue;
        }

        public CustomPropertyDTO ToDto()
        {
            CustomPropertyDTO dto = new CustomPropertyDTO();
            dto.Value = this.Value;
            dto.Key = this.Key;
            dto.DeveloperId = "bvc2004";
            return dto;
        }

        public static List<CustomProperty> ReadExtraInfo(string xmlData)
        {
            List<CustomProperty> results = new List<CustomProperty>();

            try
            {
                if (xmlData.Trim().Length > 0)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xmlData);

                    XmlNodeList keyNodes;
                    keyNodes = xdoc.SelectNodes("/OrderItemExtraInformation/ExtraInformationDictionary");

                    if (keyNodes != null)
                    {
                        for (int i = 0; i <= keyNodes.Count - 1; i++)
                        {
                            XmlNode kNode = keyNodes[i].SelectSingleNode("Key");
                            XmlNode vNode = keyNodes[i].SelectSingleNode("Value");

                            if (kNode != null && vNode != null)
                            {
                                results.Add(new CustomProperty(kNode.Value, vNode.Value));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //EventLog.LogEvent(ex);
                results = new List<CustomProperty>();
            }

            return results;
        }


    }
}

