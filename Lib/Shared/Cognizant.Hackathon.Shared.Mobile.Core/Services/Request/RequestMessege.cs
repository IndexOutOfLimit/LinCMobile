using System;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Request
{
    public class RequestMessege<T> where T : class
    {
        [JsonProperty]
        public RequestHeader Header { get; set; }

        [JsonProperty]
        public T Body { get; set; }
    }
}
