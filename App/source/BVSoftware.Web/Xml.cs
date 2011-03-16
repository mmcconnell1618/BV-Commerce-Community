using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BVSoftware.Web
{
    public static class Xml
    {
        public static string Parse(XElement x, string elementName)
        {
            return ParseInnerText(x, elementName);
        }

        public static string ParseInnerText(XmlNode n, string nodeName)
        {
            string result = string.Empty;
            if (n != null)
            {
                if (n.SelectSingleNode(nodeName) != null)
                {
                    result = n.SelectSingleNode(nodeName).InnerText;
                }
            }
            return result;
        }
        public static string ParseInnerText(XElement x, string elementName)
        {
            string result = string.Empty;
            if (x != null)
            {
                if (x.Element(elementName) != null)
                {
                    result = x.Element(elementName).Value;
                }
            }
            return result;
        }

        public static bool ParseBoolean(XmlNode n, string nodeName)
        {
            bool result = false;
            string temp = ParseInnerText(n, nodeName);
            if (temp == "1" || temp.ToLower() == "true")
            {
                result = true;
            }
            return result;
        }
        public static bool ParseBoolean(XElement x, string elementName)
        {
            bool result = false;
            string temp = ParseInnerText(x, elementName);
            if (temp == "1" || temp.ToLower() == "true")
            {
                result = true;
            }
            return result;
        }

        public static decimal ParseDecimal(XmlNode n, string nodeName)
        {
            decimal result = 0m;
            string temp = ParseInnerText(n, nodeName);
            decimal.TryParse(temp, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
            return result;
        }
        public static decimal ParseDecimal(XElement x, string elementName)
        {
            decimal result = 0m;
            string temp = ParseInnerText(x, elementName);
            decimal.TryParse(temp, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static long ParseLong(XElement x, string elementName)
        {
            long result = 0;
            string temp = ParseInnerText(x, elementName);
            long.TryParse(temp, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static int ParseInteger(XmlNode n, string nodeName)
        {
            int result = 0;
            string temp = ParseInnerText(n, nodeName);
            int.TryParse(temp, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
            return result;
        }
        public static int ParseInteger(XElement x, string elementName)
        {
            int result = 0;
            string temp = ParseInnerText(x, elementName);
            int.TryParse(temp, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static void WriteIfNotEmpty(XmlTextWriter xw, string name, string value)
        {
            if (value.Trim().Length > 0)
            {
                xw.WriteElementString(name, value);
            }
        }

        public static void WriteBool(string name, bool value, ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement(name);
            xw.WriteValue(value);
            xw.WriteEndElement();
        }
        public static void WriteDate(string name, DateTime value, ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement(name);
            xw.WriteValue(value);
            xw.WriteEndElement();
        }
        public static void WriteInt(string name, int value, ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement(name);
            xw.WriteValue(value);
            xw.WriteEndElement();
        }
        public static void WriteDecimal(string name, decimal value, ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement(name);
            xw.WriteValue(value);
            xw.WriteEndElement();
        }
        public static void WriteLong(string name, long value, ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement(name);
            xw.WriteValue(value);
            xw.WriteEndElement();
        }

    }
}
