using Newtonsoft.Json;
using System;

namespace Cognizant.Hackathon.RestClient.Models
{
    public class AuthToken
    {        
        public const string ApiVersionKey = "api_version";
                
        public const string MinMobileAppVersionKey = "min_mobile_app_version";

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        
        [JsonProperty(".issued")]
        public DateTime IssuedAt { get; set; }
        
        [JsonProperty(".expires")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty(ApiVersionKey)]
        public string ApiVersion { get; set; }

        [JsonProperty(MinMobileAppVersionKey)]
        public string MinMobileAppVersion { get; set; }
    }
}