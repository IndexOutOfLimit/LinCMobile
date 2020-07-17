using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces
{
    public interface IMasterDataService
    {
        Task<ServiceResponse<MasterData>> GetMasterData();
        Task<ServiceResponse<MasterData>> GetProductCategoryByUser(string userId);
    }
}
