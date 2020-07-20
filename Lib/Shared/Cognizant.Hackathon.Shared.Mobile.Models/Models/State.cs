using Cognizant.Hackathon.Mobile.Core.Models;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class State : ModelBase
    {
        private string stateName;

        [JsonProperty(PropertyName = "stateCode")]
        public string StateCode { get; set; }

        [JsonProperty(PropertyName = "stateId")]
        public int StateId { get; set; }

        [JsonProperty(PropertyName = "stateName")]
        public string StateName
        {
            get
            {
                return stateName;
            }
            set
            {
                stateName = System.Web.HttpUtility.HtmlDecode(value);
            }
        }
    }
}
