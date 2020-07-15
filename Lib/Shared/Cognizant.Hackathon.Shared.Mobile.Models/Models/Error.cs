using System;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class Error
    {
        [JsonProperty]
        public string ErrorCode { get; set; }

        [JsonProperty]
        public string ErrorDescription { get; set; }

        [JsonProperty]
        public string HiddenButtons { get; set; }
    }
}
