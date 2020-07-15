using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services
{
    public class MasterDataService : ServiceBase, IMasterDataService
    {
        private readonly IRestClient _restClient;

        public MasterDataService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<ServiceResponse<MasterData>> GetMasterData()
        {
            var qryParams = new Dictionary<string, object>();
            qryParams.Add("requestType", "ALL");

            var response = await _restClient
                .ExecuteAsync<MasterData>(
                    HttpVerb.GET,
                    "product/lookupdata",
                    parameters: qryParams,
                    paramMode: HttpParamMode.QUERYSTRING,
                    apiRoutePrefix: $"{AppSettings.ApiEndpoint}");

            if (!response.IsOK() || (response.Data == null))
                return new ServiceResponse<MasterData>(ServiceStatus.Error, data: null, errorMessage: "Master data not found.");
                        
            return new ServiceResponse<MasterData>(ServiceStatus.Success, data: response.Data, errorMessage: "Success");            
        }
    }
}
