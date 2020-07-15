using System;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Request
{
    public class RequestHeader
    {
        [JsonProperty]
        public object UserCode { get; set; }

        [JsonProperty]
        public object UserId { get; set; }

        [JsonProperty]
        public string AppVersion { get; set; }

        [JsonProperty]
        public string DeviceType { get; set; }

        [JsonProperty]
        public string Manufacturer { get; set; }

        [JsonProperty]
        public string Platform { get; set; }

        [JsonProperty]
        public string Density { get; set; }

        [JsonProperty]
        public string Timestamp { get; set; }

        [JsonProperty]
        public string Model { get; set; }

        [JsonProperty]
        public string OSVersion { get; set; }

        [JsonProperty]
        public string BrowserType { get; set; }

        [JsonProperty]
        public string BrowserVersion { get; set; }
    }
}
