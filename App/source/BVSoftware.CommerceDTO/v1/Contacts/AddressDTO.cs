using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class AddressDTO
    {
        [DataMember]
        public string Bvin { get; set; }
        [DataMember]
        public DateTime LastUpdatedUtc { get; set; }
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
		public string NickName {get;set;}
        [DataMember]
		public string FirstName {get;set;}
        [DataMember]
		public string MiddleInitial {get;set;}
        [DataMember]
		public string LastName {get;set;}
        [DataMember]
		public string Company {get;set;}
        [DataMember]
		public string Line1 {get;set;}
        [DataMember]
		public string Line2 {get;set;}
        [DataMember]
		public string Line3 {get;set;}
        [DataMember]
		public string City {get;set;}
        [DataMember]
		public string RegionName {get;set;}
        [DataMember]
		public string RegionBvin {get;set;}
        [DataMember]
        public string PostalCode {get;set;}
        [DataMember]
		public string CountryName {get;set;}
        [DataMember]
		public string CountryBvin {get;set;}
        [DataMember]
		public string Phone {get;set;}
        [DataMember]
		public string Fax {get;set;}
        [DataMember]
		public string WebSiteUrl {get;set;}
        [DataMember]
		public string CountyName {get;set;}
        [DataMember]
		public string CountyBvin {get;set;}
        [DataMember]
		public string UserBvin {get;set;}
        [DataMember]
        public AddressTypesDTO AddressType {get;set;}

        public AddressDTO()
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
            this.CountryBvin = MerchantTribe.Web.Geography.Country.FindByISOCode("US").Bvin;
            this.CountyBvin = string.Empty;
            this.CountyName = string.Empty;
            this.Phone = string.Empty;
            this.Fax = string.Empty;
            this.WebSiteUrl = string.Empty;
            this.UserBvin = string.Empty;
            this.AddressType = AddressTypesDTO.General;
            this.LastUpdatedUtc = DateTime.UtcNow;
        }
    }
}
