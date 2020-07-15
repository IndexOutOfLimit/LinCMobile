using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure
{
    public class LinCApiServices : ServiceBase, ILinCApiServices
    {
        public LinCApiServices(IUserService userService, IAuthService authService, IMasterDataService masterDataService)
        {
            UserService = userService;
            AuthService = authService;
            MasterDataService = masterDataService;
            // ... etc.
        }
                
        public IUserService UserService { get; }
        public IAuthService AuthService { get;  }
        public IMasterDataService MasterDataService { get; }
        // ... etc.

    }
}
