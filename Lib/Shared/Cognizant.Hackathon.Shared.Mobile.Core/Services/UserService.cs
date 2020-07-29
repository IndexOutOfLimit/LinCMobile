using System;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.RestClient.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Request;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;
using Cognizant.Hackathon.RestClient.Helpers;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IRestClient _restClient;

        public UserService(IRestClient restClient)
        {
            _restClient = restClient;
        }
        public async Task<ServiceResponse<LinCUser>> GetOrCreateUser(Guid userId, string accessToken = null)
        {
            var response = await RestClient
                .ExecuteAsync<LinCUser, Guid>(
                    HttpVerb.GET,
                    nameof(GetOrCreateUser),
                    isServiceResponse: true,
                    apiRoutePrefix: $"{AppSettings.ApiEndpoint}{nameof(LinCUser)}/");

            return response;
        }

        public async Task<ServiceResponse<LinCUser>> CreateUserAsync(string deviceDensity, string deviceType, LinCUser newUser)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            var response = await _restClient
               .ExecuteAsync<string, LinCUser>(
                   HttpVerb.POST,
                   action: "/user/registration",
                   paramMode: HttpParamMode.BODY,
                   requestBody: newUser,

                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<LinCUser>(ServiceStatus.Error, data: null,errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "User data not saved.");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);

            if(jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<LinCUser>(ServiceStatus.Error, data: null, errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "User data not saved.");
            }

            var userResponse = JsonConvert.DeserializeObject<LinCUser>(jSonResponse);            

            return new ServiceResponse<LinCUser>(ServiceStatus.Success, data: userResponse, errorCode: LinCTrasactionStatus.Success.ToString());

        }

        public async Task<ServiceResponse<(LinCUser, bool)>> GetUserAsync(string deviceDensity, string deviceType, string companyCode, string userId, string UserCode)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            UserLoginReqBody userReq = new UserLoginReqBody
            {
                UserSecret = UserCode,
                UserName = userId
            };
           
            var response = await _restClient
               .ExecuteAsync<string, UserLoginReqBody>(
                   HttpVerb.POST,
                   action: "/user/login",
                   paramMode: HttpParamMode.BODY,
                   requestBody: userReq,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Please enter valid credential.");
            }

            var userResponse = JsonConvert.DeserializeObject<LinCUser>(jSonResponse);
            return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Success, data: (userResponse, true));
        }

        public async Task<ServiceResponse<bool>> SaveProduct(List<Product> productsToAdd , int? userId)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            List<ProductReq> prdsReq = new List<ProductReq>();

            foreach (var item in productsToAdd)
            {
                ProductReq prdReq = new ProductReq();
                prdReq.ProductCatId = item.ProductCategoryId;
                prdReq.ProductDesc = item.Description;
                prdReq.ProductName = item.ProductCategory;
                prdReq.ProductQty = item.Quantity;
                prdReq.ProductRate = item.UnitPrice;

                prdsReq.Add(prdReq);
            }

            AddProductRequest addProdsReq = new AddProductRequest
            {
                UserId = userId.Value,
                Products = prdsReq
            };

            var response = await _restClient
               .ExecuteAsync<string, AddProductRequest>(
                   HttpVerb.POST,
                   action: "/product/saveProductForSupplier",
                   paramMode: HttpParamMode.BODY,
                   requestBody: addProdsReq,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );

            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<bool>(ServiceStatus.Error, data: false, errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in saving product.");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<bool>(ServiceStatus.Error, data: false, errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in saving product.");
            }           

            return new ServiceResponse<bool>(ServiceStatus.Success, data: true);
        }

        public async Task<ServiceResponse<(List<Product>, bool)>> GetUserProducts(int? userId, int? supplierId, int productTypeMasterId)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            GetProductsReqBody getProdsReq = new GetProductsReqBody
            {
                UsrId = userId,
                SupplierId = supplierId,
                ProductTypeMasterId = productTypeMasterId
            };

            var response = await _restClient
               .ExecuteAsync<string, GetProductsReqBody>(
                   HttpVerb.POST,
                   action: "/product/getSupplierProducts",
                   paramMode: HttpParamMode.BODY,
                   requestBody: getProdsReq,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );

            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<(List<Product>, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<(List<Product>, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Please enter valid credential.");
            }

            var prodsResponse = JsonConvert.DeserializeObject<ProductsResponse>(jSonResponse);

            //convert response....
            List<Product> products = ProductDataHelper.ConvertResponseProducts(prodsResponse);

            return new ServiceResponse<(List<Product>, bool)>(ServiceStatus.Success, data: (products, true));
        }

        public async Task<ServiceResponse<(List<LinCUser>, bool)>> GetSuppliers(int userId, int prdTypeId, int searchWithin)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            SuppliersReqBody userReq = new SuppliersReqBody
            {
                UserId = userId,
                ProductTypeMasterId = prdTypeId,
                SearchWithin = searchWithin
            };

            var response = await _restClient
               .ExecuteAsync<string, SuppliersReqBody>(
                   HttpVerb.POST,
                   action: "/product/getSupplier",
                   paramMode: HttpParamMode.BODY,
                   requestBody: userReq,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<(List<LinCUser>, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<(List<LinCUser>, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Please enter valid credential.");
            }

            var userResponse = JsonConvert.DeserializeObject<SuppliersResponse>(jSonResponse);
            //convert supplier list response
            var supplierList = ProductDataHelper.ConvertSuppliersResponse(userResponse);

            return new ServiceResponse<(List<LinCUser>, bool)>(ServiceStatus.Success, data: (supplierList, true));
        }

        public async Task<ServiceResponse<MasterData>> GetProductCategoryByUser(int userId)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();
            //=====
            /*
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
            string masterDataJson = GetProductCategoryDataJson();

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterData>(masterDataJson, deserializationSettings)
                                .AsServiceResponse();
            data.ServiceErrorCode = LinCTrasactionStatus.Success.ToString();
            return data;
            */
            // ======
            var reqBody = new ProdCategoryReq() { UserId = userId };

            var response = await _restClient
                    .ExecuteAsync<MasterData, ProdCategoryReq>(
                        HttpVerb.POST,
                        "/product/getProdCat",
                        headers: headers,
                        paramMode: HttpParamMode.BODY,
                        requestBody: reqBody,
                        apiRoutePrefix: $"{AppSettings.ApiEndpoint}");

            if (!response.IsOK() || (response.Data == null))
                return new ServiceResponse<MasterData>(ServiceStatus.Error, data: null, errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Master data not found.");

            return new ServiceResponse<MasterData>(ServiceStatus.Success, data: response.Data, errorMessage: "Success", errorCode: LinCTrasactionStatus.Success.ToString());
            
        }

        private string GetProductCategoryDataJson()
        {
            return "{\"productCategories\":[{\"prdctTypeId\":1,\"prdctTypeCode\":\"GRCSTP\",\"prdctTypeName\":\"Groceries&Staples\",\"prdctTypeDispSeq\":1,\"prdctCatId\":1,\"prdctCatCode\":\"FLRATA\",\"prdctCatName\":\"Flour|Atta\",\"prdctCatDispSeq\":1},{\"prdctTypeId\":1,\"prdctTypeCode\":\"GRCSTP\",\"prdctTypeName\":\"Groceries&Staples\",\"prdctTypeDispSeq\":1,\"prdctCatId\":2,\"prdctCatCode\":\"RCCRLS\",\"prdctCatName\":\"Rice&Cereals\",\"prdctCatDispSeq\":2},{\"prdctTypeId\":2,\"prdctTypeCode\":\"FRTVEG\",\"prdctTypeName\":\"Fruits&Vegetables\",\"prdctTypeDispSeq\":2,\"prdctCatId\":3,\"prdctCatCode\":\"FRFRTS\",\"prdctCatName\":\"FreshFruits\",\"prdctCatDispSeq\":1},{\"prdctTypeId\":2,\"prdctTypeCode\":\"FRTVEG\",\"prdctTypeName\":\"Fruits&Vegetables\",\"prdctTypeDispSeq\":2,\"prdctCatId\":4,\"prdctCatCode\":\"FRVEGS\",\"prdctCatName\":\"FreshVegetables\",\"prdctCatDispSeq\":2}]}";
        }

    }
}