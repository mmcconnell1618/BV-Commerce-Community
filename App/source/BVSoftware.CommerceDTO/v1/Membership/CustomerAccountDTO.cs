using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using BVSoftware.CommerceDTO.v1.Contacts;

namespace BVSoftware.CommerceDTO.v1.Membership
{
    [DataContract]
    public class CustomerAccountDTO
    {        
        [DataMember]
        public string Bvin {get;set;}
        [DataMember]
        public string Email {get;set;}
        [DataMember]
        public string FirstName {get;set;}
        [DataMember]
        public string LastName {get;set;}
        [DataMember]
        public string Password {get;set;}
        [DataMember]
        public string Salt {get;set;}
        [DataMember]
        public List<AddressDTO> Addresses {get;set;}
        [DataMember]
        public bool TaxExempt {get;set;}
        [DataMember]
        public string Notes {get;set;}
        [DataMember]
        public string PricingGroupId {get;set;}
        [DataMember]
        public int FailedLoginCount {get;set;}
        [DataMember]
        public DateTime LastUpdatedUtc {get;set;}
        [DataMember]
		public System.DateTime CreationDateUtc {get;set;}
        [DataMember]
		public System.DateTime LastLoginDateUtc {get;set;}
	        
		// Constructor			
		public CustomerAccountDTO()
		{
            this.Addresses = new List<AddressDTO>();
            this.Bvin = string.Empty;
            this.CreationDateUtc = DateTime.UtcNow;
            this.Email = string.Empty;
            this.FailedLoginCount = 0;
            this.FirstName = string.Empty;
            this.LastLoginDateUtc = DateTime.UtcNow;
            this.LastName = string.Empty;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.Notes = string.Empty;
            this.Password = string.Empty;
            this.PricingGroupId = string.Empty;
            this.Salt = string.Empty;
            this.TaxExempt = false;            
		}	
	
	
    }
}
