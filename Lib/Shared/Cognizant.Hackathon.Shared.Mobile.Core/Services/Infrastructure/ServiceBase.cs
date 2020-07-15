using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Unity;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure
{
    public abstract class ServiceBase : IServiceBase
    {        
        [Dependency]
        public IAppSettings AppSettings { get; set; }
        
        [Dependency]
        public IRestClient RestClient { get; set; }
        
        [Dependency]
        public IAppCacheService<ClientState> AppCache { get; set; }
    }
}
