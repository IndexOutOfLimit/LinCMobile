using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Interfaces
{
    public interface IServiceBase
    {
        IAppSettings AppSettings { get; set; }
        IRestClient RestClient { get; set; }
        IAppCacheService<ClientState> AppCache { get; set; }
    }
}