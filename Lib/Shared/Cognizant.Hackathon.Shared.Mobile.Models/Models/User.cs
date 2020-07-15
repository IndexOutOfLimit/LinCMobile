using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Cognizant.Hackathon.Core.Model;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    [Serializable]
    public class LinCUser : MemberBase
    {
        [JsonProperty(PropertyName = "CompanyCode")]
        public string CompanyCode { get; set; }

        [JsonProperty(PropertyName = "UserCode")]
        public string UserCode { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "UseCurrentLocation")]
        public bool UseCurrentLocation { get; set; }

        [JsonProperty(PropertyName = "AddressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "AddressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "Pin")]
        public string Pin { get; set; }

        [JsonProperty(PropertyName = "Latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "State")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "RegisterType")]
        public string RegisterType { get; set; }

        [JsonProperty(PropertyName = "Organization")]
        public string Organization { get; set; }

        [JsonProperty(PropertyName = "UserType")]
        public string UserType { get; set; }

        [JsonProperty(PropertyName = "ServiceType")]
        public string ServiceType { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string UserSecret { get; set; }

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