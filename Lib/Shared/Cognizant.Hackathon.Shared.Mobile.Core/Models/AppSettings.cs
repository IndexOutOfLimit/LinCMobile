using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using StandardAppConfig;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Models
{
    public class AppSettings : IAppSettings
    {
        public AppSettings()
        {
            //GENERAL
            AppName = ConfigurationManager.AppSettings["app.name"];
            AppConfig = ConfigurationManager.AppSettings["app.config"];
            LocaleDefault = ConfigurationManager.AppSettings["locale.default"];
            AppStoreiOS = ConfigurationManager.AppSettings["app.store.ios"];
            AppStoreDroid = ConfigurationManager.AppSettings["app.store.droid"];

            ApiEndpoint = ConfigurationManager.AppSettings["api.endpoint"];
            ApiQAEndpoint = ConfigurationManager.AppSettings["api.qaendpoint"];
            ApiPQAEndpoint = ConfigurationManager.AppSettings["api.pqaendpoint"];
            ApiStagingEndpoint = ConfigurationManager.AppSettings["api.stagingendpoint"];
            ApiPRODEndpoint = ConfigurationManager.AppSettings["api.prdendpoint"];
            ApiEndpointUser = ConfigurationManager.AppSettings["api.endpoint.user"];
            ApiQAEndpointUser = ConfigurationManager.AppSettings["api.qaendpoint.user"];
            ApiPQAEndpointUser = ConfigurationManager.AppSettings["api.pqaendpoint.user"];
            ApiStagingEndpointUser = ConfigurationManager.AppSettings["api.stagingendpoint.user"];
            ApiPRODEndpointUser = ConfigurationManager.AppSettings["api.endpoint.user"];
            ApiEndpointCompany = ConfigurationManager.AppSettings["api.endpoint.company"];
            ApiEndpointCalculator = ConfigurationManager.AppSettings["api.endpoint.calculator"];

            ApiAuthEndpoint = ConfigurationManager.AppSettings["api.auth"];
            ApiLoginType = ConfigurationManager.AppSettings["api.logintype"];
            ApiTimeOut = ConfigurationManager.AppSettings["api.timeout"];

            // ERROR
            ErrorEnabled = ConfigurationManager.AppSettings["error.enabled"];
            ErrorTitle = ConfigurationManager.AppSettings["error.title"];
            ErrorDescription = ConfigurationManager.AppSettings["error.description"];

            //Identity Server
            IdentityBaseUrl = ConfigurationManager.AppSettings["api.auth"];
            IdentityClientId = ConfigurationManager.AppSettings["identity.client.id"];
            IdentityTenantId = ConfigurationManager.AppSettings["identity.tenant.id"];
            IdentityClientSecret = ConfigurationManager.AppSettings["identity.client.secret"];
            IdentityClientResource = ConfigurationManager.AppSettings["identity.client.resource"];
            IdentityScope = ConfigurationManager.AppSettings["identity.client.scope"];
        }

        public string AppName { get; }
        public string AppConfig { get; }
        public string LocaleDefault { get; set; }
        public string AppStoreiOS { get; }
        public string AppStoreDroid { get; }
        public string ErrorDescription { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorEnabled { get; set; }

        public string ApiEndpoint { get; set; }
        public string ApiQAEndpoint { get; set; }
        public string ApiPQAEndpoint { get; set; }
        public string ApiStagingEndpoint { get; set; }
        public string ApiPRODEndpoint { get; set; }
         
        public string ApiEndpointUser { get; set; }
        public string ApiQAEndpointUser { get; set; }
        public string ApiPQAEndpointUser { get; set; }
        public string ApiStagingEndpointUser { get; set; }
        public string ApiPRODEndpointUser { get; set; }

        public string ApiEndpointCompany { get; set; }
        public string ApiEndpointCalculator { get; set; }
        
        public string ApiAuthEndpoint { get; set; }
        public string ApiTimeOut { get; set; }
        public string ApiLoginType { get; set; }
        public string AppCenterIosKey { get; set; }
        public string AppCenterAndroidKey { get; set; }
        public string AppCenterLogTag { get; set; }
        public string AzureAdClientId { get; set; }
        public string AzureAdTenantId { get; set; }

        // Identity Server
        public string IdentityBaseUrl { get; }
        public string IdentityClientId { get; }
        public string IdentityTenantId { get; }
        public string IdentityClientSecret { get; }
        public string IdentityClientResource { get; }
        public string IdentityScope { get; }        
    }
}