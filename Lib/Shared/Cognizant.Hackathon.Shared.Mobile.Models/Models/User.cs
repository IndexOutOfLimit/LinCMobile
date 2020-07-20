using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Cognizant.Hackathon.Core.Model;
using System.Collections.Generic;
using System.Globalization;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    [Serializable]
    public class LinCUser : MemberBase
    {
        [JsonProperty(PropertyName = "usrId")]
        public int? UserId { get; set; }

        [JsonProperty(PropertyName = "usrTypeMasterId")]
        public int UserTypeId { get; set; }

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

        [JsonConverter(typeof(JsonExponentialConverter))]
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonConverter(typeof(JsonExponentialConverter))]
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "addr1")]
        public string AddressLine1 { get; set; }

        [JsonProperty(PropertyName = "addr2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "pin")]
        public int Pin { get; set; }

        [JsonProperty(PropertyName = "stateId")]
        public int StateId { get; set; }

        [JsonProperty(PropertyName = "countryId")]
        public int CountryId { get; set; }

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
        public List<int> ProductTypeIds { get; set; }

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


    public class JsonExponentialConverter : JsonConverter
    {
        public override bool CanRead { get { return true; } }
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            double amount = 0;
            if (double.TryParse(reader.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out amount))
            {
                return amount;
            }
            return amount;
        }
    }
}