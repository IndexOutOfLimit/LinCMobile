namespace Cognizant.Hackathon.Shared.Mobile.Core.Interfaces
{
    public interface IAppSettings
    {     
        string AppName { get; }
        string AppConfig { get; }
        string LocaleDefault { get; set; }
        string AppStoreiOS { get; }
        string AppStoreDroid { get; }
        string ErrorDescription { get; set; }
        string ErrorTitle { get; set; }
        string ErrorEnabled { get; set; }

        string ApiEndpoint { get; set; }
        string ApiQAEndpoint { get; set; }
        string ApiPQAEndpoint { get; set; }
        string ApiStagingEndpoint { get; set; }
        string ApiPRODEndpoint { get; set; }

        string ApiEndpointUser { get; set; }
        string ApiQAEndpointUser { get; set; }
        string ApiPQAEndpointUser { get; set; }
        string ApiStagingEndpointUser { get; set; }
        string ApiPRODEndpointUser { get; set; }

        string ApiEndpointCompany { get; set; }
        string ApiEndpointCalculator { get; set; }

        string ApiAuthEndpoint { get; set; }
        string ApiTimeOut { get; set; }
        string ApiLoginType { get; set; }
        string AppCenterIosKey { get; set; }
        string AppCenterAndroidKey { get; set; }
        string AppCenterLogTag { get; set; }
  
        // IDentityServer
        string IdentityBaseUrl { get; }
        string IdentityClientId { get; }
        string IdentityTenantId { get; }
        string IdentityClientSecret { get; }
        string IdentityClientResource { get; }
        string IdentityScope { get; }
    }
}