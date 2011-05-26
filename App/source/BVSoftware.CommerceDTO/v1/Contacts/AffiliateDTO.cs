using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Contacts
{
    [DataContract]
    public class AffiliateDTO
    {
        [DataMember]
        public long Id {get;set;}
        [DataMember]
        public long StoreId { get; set; }
        [DataMember]
        public System.DateTime LastUpdatedUtc {get;set;}
        [DataMember]
		public bool Enabled {get;set;}
        [DataMember]
		public string ReferralId {get;set;}
        [DataMember]
		public string DisplayName {get;set;}
        [DataMember]
		public AddressDTO Address {get;set;}
        [DataMember]
		public decimal CommissionAmount {get;set;}
        [DataMember]
		public AffiliateCommissionTypeDTO CommissionType {get;set;}
        [DataMember]
		public int ReferralDays {get;set;}
        [DataMember]
		public string TaxId {get;set;}
        [DataMember]
		public string DriversLicenseNumber {get;set;}
        [DataMember]
		public string WebSiteUrl {get;set;}
        [DataMember]
		public string CustomThemeName {get;set;}
        [DataMember]
		public string Notes {get;set;}
        [DataMember]
        public List<AffiliateContactDTO> Contacts { get; set; }

		public AffiliateDTO()
		{
            this.Id = 0;
            this.StoreId = 0;
            this.Enabled = false;
		    this.ReferralId = string.Empty;
		    this.DisplayName = string.Empty;
		    this.Address = new AddressDTO();
		    this.CommissionAmount = 0;
		    this.CommissionType = AffiliateCommissionTypeDTO.PercentageCommission;
		    this.ReferralDays = 30;
		    this.TaxId = string.Empty;
		    this.DriversLicenseNumber = string.Empty;
		    this.WebSiteUrl = string.Empty;
		    this.CustomThemeName = string.Empty;
		    this.Notes = string.Empty;
            this.LastUpdatedUtc = System.DateTime.UtcNow;
            this.Contacts = new List<AffiliateContactDTO>();
		}
    }
}
