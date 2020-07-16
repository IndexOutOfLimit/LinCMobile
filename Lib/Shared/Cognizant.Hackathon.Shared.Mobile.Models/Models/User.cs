using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Cognizant.Hackathon.Core.Model;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    [Serializable]
    public class LinCUser : MemberBase
    {
        [JsonProperty(PropertyName = "usrId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "usrTypeMasterId")]
        public string UserTypeId { get; set; }

        //[JsonProperty(PropertyName = "CompanyCode")]
        //public string CompanyCode { get; set; }

        [JsonProperty(PropertyName = "usrName")]
        public override string UserName { get; set; }

        [JsonProperty(PropertyName = "usrPassword")]
        public string UserSecret { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public override string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public override string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phoneNum")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "addr1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addr2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "pin")]
        public string Pin { get; set; }

        [JsonProperty(PropertyName = "stateId")]
        public string StateId { get; set; }

        [JsonProperty(PropertyName = "countryId")]
        public string CountryId { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "UseCurrentLocation")]
        public bool UseCurrentLocation { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "UserCode")]
        public string UserCode { get; set; }

        //[JsonProperty(PropertyName = "RegisterType")]
        //public string RegisterType { get; set; }

        //[JsonProperty(PropertyName = "Organization")]
        //public string Organization { get; set; }

        [JsonProperty(PropertyName = "productTypeId")]
        public string ProductTypeIds { get; set; }

        [JsonIgnore]
        public string Area
        {
            get
            {
                return $"{AddressLine1};{AddressLine2};\nPin: {Pin}";
            }
        }

        public LinCUser ShallowCopy()
        {
            return (LinCUser)this.MemberwiseClone();
        }
    }
}