using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Interfaces
{
    public interface ILinCApiServices
    {
        // main services        
        IAuthService AuthService { get; }
        IMasterDataService MasterDataService { get; }
        IUserService UserService { get; }

        // This is to expose dependency from base to the implementation. Do not remove!
        // And do not implement the properties in ApiServices.cs class.
        IAppSettings AppSettings { get; set; }
        IAppCacheService<ClientState> AppCache { get; set; }
      
    }
} 