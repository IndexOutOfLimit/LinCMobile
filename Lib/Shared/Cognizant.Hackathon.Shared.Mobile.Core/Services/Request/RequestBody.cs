using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Request
{   
    public class UserReqBody
    {
        public LinCUser UserInfo;
    }

    public class UserLoginReqBody
    {
        [JsonProperty(PropertyName = "usrName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "usrPass")]
        public string UserSecret { get; set; }
    }

    public class ProdCategoryReq
    {
        [JsonProperty(PropertyName = "usrId")]
        public int? UserId { get; set; }
    }

    public class DocumentMailDataReq
    {
        public string OperationName { get; set; }
    }


    public class EmailInput
    {
        public string UserCode { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DefaultSub { get; set; }
        public string DocumentName { get; set; }
        public string Email { get; set; }
    }

    public class MailReqBody
    {
        public string OperationName { get; set; }
    }

    public class MailInputDetails
    {
        [JsonProperty]
        public List<CommonDictionary> Fields { get; set; }
        public string PropertyAddress { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string DefaultSub { get; set; }
        public string Subject { get; set; }
    }
}
