using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BVSoftware.CommerceDTO.v1.Contacts;

namespace BVSoftware.Commerce.Migration.Migrators.BV2004
{
    public class BVC2004Address
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebSiteURL { get; set; }

        public BVC2004Address()
        {
            this.ID = 0;
            this.FirstName = string.Empty;
            this.MiddleInitial = string.Empty;
            this.LastName = string.Empty;
            this.CompanyName = string.Empty;
            this.StreetLine1 = string.Empty;
            this.StreetLine2 = string.Empty;
            this.City = string.Empty;
            this.StateName = string.Empty;
            this.StateCode = string.Empty;
            this.PostalCode = string.Empty;
            this.CountryName = string.Empty;
            this.CountryCode = string.Empty;
            this.PhoneNumber = string.Empty;
            this.FaxNumber = string.Empty;
            this.WebSiteURL = string.Empty;
        }

        public bool FromXml(ref XmlReader xr)
        {
            bool results = false;

            try
            {
                while (xr.Read())
                {
                    if (xr.IsStartElement())
                    {
                        if (!xr.IsEmptyElement)
                        {
                            switch (xr.Name)
                            {
                                case "ID":
                                    xr.Read();
                                    this.ID = xr.ReadContentAsInt();
                                    break;
                                case "FirstName":
                                    xr.Read();
                                    this.FirstName = xr.ReadString();
                                    break;
                                case "MiddleInitial":
                                    xr.Read();
                                    this.MiddleInitial = xr.ReadString();
                                    break;
                                case "LastName":
                                    xr.Read();
                                    this.LastName = xr.ReadString();
                                    break;
                                case "CompanyName":
                                    xr.Read();
                                    this.CompanyName = xr.ReadString();
                                    break;
                                case "StreeLine1":
                                    xr.Read();
                                    this.StreetLine1 = xr.ReadString();
                                    break;
                                case "StreetLine2":
                                    xr.Read();
                                    this.StreetLine2 = xr.ReadString();
                                    break;
                                case "City":
                                    xr.Read();
                                    this.City = xr.ReadString();
                                    break;
                                case "StateName":
                                    xr.Read();
                                    this.StateName = xr.ReadString();
                                    break;
                                case "StateCode":
                                    xr.Read();
                                    this.StateCode = xr.ReadString();
                                    break;
                                case "PostalCode":
                                    xr.Read();
                                    this.PostalCode = xr.ReadString();
                                    break;
                                case "CountryName":
                                    xr.Read();
                                    this.CountryName = xr.ReadString();
                                    break;
                                case "CountryCode":
                                    xr.Read();
                                    this.CountryCode = xr.ReadString();
                                    break;
                                case "PhoneNumber":
                                    xr.Read();
                                    this.PhoneNumber = xr.ReadString();
                                    break;
                                case "FaxNumber":
                                    xr.Read();
                                    this.FaxNumber = xr.ReadString();
                                    break;
                                case "WebSiteUrl":
                                    xr.Read();
                                    this.WebSiteURL = xr.ReadString();
                                    break;
                            }
                        }
                    }
                }

                results = true;
            }

            catch (XmlException XmlEx)
            {
                //EventLog.LogEvent(XmlEx);
                results = false;
            }

            return results;
        }


        public static List<BVC2004Address> ReadAddressesFromXml(string xmlData)
        {
            List<BVC2004Address> results = new List<BVC2004Address>();

            try
            {
                if (xmlData.Trim().Length > 0)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xmlData);

                    XmlNodeList addressNodes;
                    addressNodes = xdoc.SelectNodes("/AddressBook/Address");

                    if (addressNodes != null)
                    {
                        for (int i = 0; i <= addressNodes.Count - 1; i++)
                        {
                            BVC2004Address a = new BVC2004Address();
                            if (a.FromXmlString(addressNodes[i].OuterXml) == true)
                            {
                                results.Add(a);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //EventLog.LogEvent(ex);
                results = new List<BVC2004Address>();
            }

            return results;
        }

        public bool CopyTo(AddressDTO destinationAddress, string connString)
        {
            bool result = true;

            try
            {
                destinationAddress.NickName = string.Empty;
                destinationAddress.FirstName = this.FirstName;
                destinationAddress.MiddleInitial = this.MiddleInitial;
                destinationAddress.LastName = this.LastName;
                destinationAddress.Company = this.CompanyName;
                destinationAddress.Line1 = this.StreetLine1;
                destinationAddress.Line2 = this.StreetLine2;
                destinationAddress.Line3 = string.Empty;
                destinationAddress.City = this.City;
                destinationAddress.PostalCode = this.PostalCode;

                MerchantTribe.Web.Geography.Country newCountry = GeographyHelper.TranslateCountry(connString, this.CountryCode);
                destinationAddress.CountryBvin = newCountry.Bvin;
                destinationAddress.CountryName = newCountry.DisplayName;

                string RegionAbbreviation = GeographyHelper.TranslateRegionBvinToAbbreviation(connString, this.StateCode);
                destinationAddress.RegionBvin = RegionAbbreviation;
                destinationAddress.RegionName = RegionAbbreviation;

                destinationAddress.Phone = this.PhoneNumber;
                destinationAddress.Fax = this.FaxNumber;
                destinationAddress.WebSiteUrl = this.WebSiteURL;
                destinationAddress.CountyBvin = string.Empty;
                destinationAddress.CountyName = string.Empty;
                //destinationAddress.Residential = this.Residential;                
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public virtual bool FromXmlString(string x)
        {
            System.IO.StringReader sw = new System.IO.StringReader(x);
            XmlReader xr = XmlReader.Create(sw);
            bool result = FromXml(ref xr);
            sw.Dispose();
            xr.Close();
            return result;
        }                     
    }
}
