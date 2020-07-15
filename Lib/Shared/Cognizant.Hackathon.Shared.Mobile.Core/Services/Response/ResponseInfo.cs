using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Response
{
    public class ResponseInfo<T> where T : class
    {
        [JsonProperty]
        public ResponseMessage<T> Message { get; set; }
    }
}