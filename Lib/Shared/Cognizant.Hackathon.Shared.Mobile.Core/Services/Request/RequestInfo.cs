using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Request
{
    public class RequestInfo <T> where T : class
    {
        [JsonProperty]
        public RequestMessege<T> Message { get; set; }
    }
}