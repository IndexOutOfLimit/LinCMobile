using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Helpers;
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
            var deserializationSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                ContractResolver = new RestClient.Helpers.ReadOnlyJsonContractResolver(),
                Converters = new List<Newtonsoft.Json.JsonConverter>
                {
                    new RestClient.Helpers.XmlTimeSpanConverter()
                }
            };
            string masterDataJson = GetMasterDataJson();

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterData>(masterDataJson, deserializationSettings)
                                .AsServiceResponse();
            return data;
            /*
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

            */
        }


        private string GetMasterDataJson()
        {
            return "{ \"usrTypeMasterList\": [ { \"usrTypeMasterId\": 1, \"usrTypeName\": \"Supplier\", \"usrTypeCode\": \"SPPLR\" }, { \"usrTypeMasterId\": 2, \"usrTypeName\": \"Consumer\", \"usrTypeCode\": \"CNSMR\" }, { \"usrTypeMasterId\": 3, \"usrTypeName\": \"Volunteer\", \"usrTypeCode\": \"VLNTR\" } ], \"stateList\": [ { \"stateId\": 1, \"stateName\": \"Andhra Pradesh\", \"stateCode\": \"AP\" }, { \"stateId\": 2, \"stateName\": \"Bihar\", \"stateCode\": \"BH\" }, { \"stateId\": 3, \"stateName\": \"Odisha\", \"stateCode\": \"OD\" }, { \"stateId\": 4, \"stateName\": \"Jharkhand\", \"stateCode\": \"JH\" }, { \"stateId\": 5, \"stateName\": \"Maharashtra\", \"stateCode\": \"MH\" }, { \"stateId\": 6, \"stateName\": \"Tamil Nadu\", \"stateCode\": \"TN\" }, { \"stateId\": 7, \"stateName\": \"Uttar Pradesh\", \"stateCode\": \"UP\" }, { \"stateId\": 8, \"stateName\": \"West Bengal\", \"stateCode\": \"WB\" } ], \"countryList\": [ { \"countryId\": 2, \"countryName\": \"China\", \"countryCode\": \"CHN\", \"areaCode\": \"+86\" }, { \"countryId\": 1, \"countryName\": \"India\", \"countryCode\": \"IN\", \"areaCode\": \"+91\" } ], \"productTypes\" : [{\"prdctTypeId\": 1,\"prdctTypeCode\": \"GRCSTP\",\"prdctTypeName\": \"Groceries & Staples\",\"prdctTypeDispSeq\": 1},{\"prdctTypeId\": 2,\"prdctTypeCode\": \"FRTVEG\",\"prdctTypeName\": \"Fruits & Vegetables\",\"prdctTypeDispSeq\": 2} ]}";
        }
    }
}
