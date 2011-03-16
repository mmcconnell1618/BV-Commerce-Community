using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    class BV5Address
    {
            private XmlWriterSettings _BVXmlWriterSettings = new XmlWriterSettings();
            private XmlReaderSettings _BVXmlReaderSettings = new XmlReaderSettings();

            public string Bvin { get; set; }
            public DateTime LastUpdatedUtc { get; set; }
            public long StoreId { get; set; }
            public string NickName { get; set; }
            public string FirstName { get; set; }
            public string MiddleInitial { get; set; }
            public string LastName { get; set; }
            public string Company { get; set; }
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Line3 { get; set; }
            public string City { get; set; }
            public string RegionName { get; set; }
            public string RegionBvin { get; set; }
            private string _PostalCode = string.Empty;
            public string PostalCode
            {
                get { return _PostalCode; }
                set { _PostalCode = value.Replace("-", ""); }
            }
            public string CountryName { get; set; }
            public string CountryBvin { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string WebSiteUrl { get; set; }
            public string CountyName { get; set; }
            public string CountyBvin { get; set; }
            public string UserBvin { get; set; }                       

            public BV5Address()
            {
                this.Init();
            }

            private void Init()
            {
                this.StoreId = 0;
                this.NickName = string.Empty;
                this.FirstName = string.Empty;
                this.MiddleInitial = string.Empty;
                this.LastName = string.Empty;
                this.Company = string.Empty;
                this.Line1 = string.Empty;
                this.Line2 = string.Empty;
                this.Line3 = string.Empty;
                this.City = string.Empty;
                this.RegionName = string.Empty;
                this.RegionBvin = string.Empty;
                this.PostalCode = string.Empty;
                this.CountryName = "US";
                this.CountryBvin = string.Empty;
                this.CountyBvin = string.Empty;
                this.CountyName = string.Empty;
                this.Phone = string.Empty;
                this.Fax = string.Empty;
                this.WebSiteUrl = string.Empty;
                this.UserBvin = string.Empty;
                this.LastUpdatedUtc = DateTime.UtcNow;
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
                                    case "Bvin":
                                        xr.Read();
                                        Bvin = xr.ReadString();
                                        break;
                                    case "NickName":
                                        xr.Read();
                                        NickName = xr.ReadString();
                                        break;
                                    case "FirstName":
                                        xr.Read();
                                        FirstName = xr.ReadString();
                                        break;
                                    case "MiddleInitial":
                                        xr.Read();
                                        MiddleInitial = xr.ReadString();
                                        break;
                                    case "LastName":
                                        xr.Read();
                                        LastName = xr.ReadString();
                                        break;
                                    case "Company":
                                        xr.Read();
                                        Company = xr.ReadString();
                                        break;
                                    case "Line1":
                                        xr.Read();
                                        Line1 = xr.ReadString();
                                        break;
                                    case "Line2":
                                        xr.Read();
                                        Line2 = xr.ReadString();
                                        break;
                                    case "Line3":
                                        xr.Read();
                                        Line3 = xr.ReadString();
                                        break;
                                    case "City":
                                        xr.Read();
                                        City = xr.ReadString();
                                        break;
                                    case "RegionName":
                                        xr.Read();
                                        RegionName = xr.ReadString();
                                        break;
                                    case "RegionBvin":
                                        xr.Read();
                                        RegionBvin = xr.ReadString();
                                        break;
                                    case "PostalCode":
                                        xr.Read();
                                        _PostalCode = xr.ReadString();
                                        break;
                                    case "CountryName":
                                        xr.Read();
                                        CountryName = xr.ReadString();
                                        break;
                                    case "CountryBvin":
                                        xr.Read();
                                        CountryBvin = xr.ReadString();
                                        break;
                                    case "Phone":
                                        xr.Read();
                                        Phone = xr.ReadString();
                                        break;
                                    case "Fax":
                                        xr.Read();
                                        Fax = xr.ReadString();
                                        break;
                                    case "WebSiteUrl":
                                        xr.Read();
                                        WebSiteUrl = xr.ReadString();
                                        break;
                                    case "LastUpdated":
                                        xr.Read();
                                        LastUpdatedUtc = DateTime.Parse(xr.ReadString());
                                        break;
                                    case "CountyName":
                                        xr.Read();
                                        CountyName = xr.ReadString();
                                        break;
                                    case "CountyBvin":
                                        xr.Read();
                                        CountyBvin = xr.ReadString();
                                        break;
                                    case "UserBvin":
                                        xr.Read();
                                        UserBvin = xr.ReadString();
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


            public static List<BV5Address> ReadAddressesFromXml(string xmlData)
            {
                List<BV5Address> results = new List<BV5Address>();

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
                                BV5Address a = new BV5Address();
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
                    results = new List<BV5Address>();
                }

                return results;
            }

            public bool CopyTo(MigrationServices.AddressDTO destinationAddress, string connString)
            {
                bool result = true;

                try
                {
                    destinationAddress.NickName = this.NickName;
                    destinationAddress.FirstName = this.FirstName;
                    destinationAddress.MiddleInitial = this.MiddleInitial;
                    destinationAddress.LastName = this.LastName;
                    destinationAddress.Company = this.Company;
                    destinationAddress.Line1 = this.Line1;
                    destinationAddress.Line2 = this.Line2;
                    destinationAddress.Line3 = this.Line3;
                    destinationAddress.City = this.City;
                    destinationAddress.PostalCode = this.PostalCode;
                                        
                    BVSoftware.Web.Geography.Country newCountry = GeographyHelper.TranslateCountry(connString, this.CountryBvin);
                    destinationAddress.CountryBvin = newCountry.Bvin;
                    destinationAddress.CountryName = newCountry.DisplayName;

                    string RegionAbbreviation = GeographyHelper.TranslateRegionBvinToAbbreviation(connString, this.RegionBvin);
                    destinationAddress.RegionBvin = RegionAbbreviation;
                    destinationAddress.RegionName = RegionAbbreviation;

                    
                    destinationAddress.Phone = this.Phone;
                    destinationAddress.Fax = this.Fax;
                    destinationAddress.WebSiteUrl = this.WebSiteUrl;
                    destinationAddress.CountyBvin = this.CountyBvin;
                    destinationAddress.CountyName = this.CountyName;
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
