using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Response
{
    public class ResponseMessage<T> where T : class
    {
        [JsonProperty]
        public T Body { get; set; }

        [JsonProperty]
        public T Error { get; set; }
    }
}
