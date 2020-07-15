using System;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class CommonDictionary
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("isMandatory")]
        public bool IsMandatory { get; set; }

        [JsonProperty("showInOutput")]
        public bool ShowInOutput { get; set; }

        public string Title { get; set; }


    }
}
