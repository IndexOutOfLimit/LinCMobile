using CommonServiceLocator;
using MoreLinq;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using Cognizant.Hackathon.RestClient.Interfaces;
using System;
using Unity;
using Unity.ServiceLocation;
using Cognizant.Hackathon.Mobile.Core.Services;
using Cognizant.Hackathon.RestClient.Services;
using Cognizant.Hackathon.RestClient.Providers;
using Cognizant.Hackathon.Shared.Mobile.Shared.Providers;
using Cognizant.Hackathon.Shared.Mobile.Shared.Services;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services;

namespace Cognizant.Hackathon.Shared.Mobile.Bootstrap
{
    public sealed class BootStrapper
    {
        private readonly IAppSettings _appSettings;
        private static Lazy<IUnityContainer> _container;

        public static IAppSettings AppSettings { get; private set; }
        //public static IUnityContainer Container => _container?.Value;
        public static IUnityContainer Container
        {
            get
            {
                return _container?.Value;
            }
            set
            {
                _container = new Lazy<IUnityContainer>(() =>
                {
                    UnityContainer container = (UnityContainer)value;
                    return container;
                });
            }
        }

        public static void Initialize(IAppSettings appSettings)
        {
            AppSettings = appSettings;
            new BootStrapper(appSettings);
        }

        public static void Dispose()
        {
            if (Container != null)
            {
                Container.Registrations.ForEach(x => x.LifetimeManager.RemoveValue());
                Container.Dispose();
                _container = null;

                ServiceLocator.SetLocatorProvider(null);
            }
        }

        private BootStrapper(IAppSettings appSettings)
        {
            _appSettings = appSettings;

            _container = new Lazy<IUnityContainer>(() =>
            {
                UnityContainer container = new UnityContainer();
                container.RegisterInstance(appSettings);

                RegisterCommonServices(container);
                
                RegisterServices(container);
                
                var locator = new UnityServiceLocator(container);
                ServiceLocator.SetLocatorProvider(() => locator);

                return container;
            });
        }
     
        public void RegisterCommonServices(IUnityContainer container)
        {
            container.RegisterInstance<IAppSettings>(_appSettings);

            var apiConfiguration = new ApiConfiguration(_appSettings.ApiEndpoint, _appSettings.ApiAuthEndpoint, null,
                null, int.Parse(_appSettings.ApiTimeOut), false);
            container.RegisterInstance<IApiConfiguration>(apiConfiguration);

            var idsConfiguration = new IDSConfiguration(_appSettings.IdentityBaseUrl, _appSettings.IdentityClientId, _appSettings.IdentityTenantId, _appSettings.IdentityClientSecret,
                _appSettings.IdentityClientResource, _appSettings.IdentityScope);

            container.RegisterInstance<IIDSConfiguration>(idsConfiguration);

            container.RegisterSingleton<INavigationService, NavigationService>();
            container.RegisterSingleton<IInitialiserService, InitialiserService>();
            container.RegisterSingleton<IServiceBase, ServiceBase>();

            container.RegisterSingleton<IRestClient, RestClientMSAL>();
            container.RegisterSingleton<IRestConfiguration, RestConfiguration>();
            container.RegisterSingleton<IUrlRewriteProvider, UrlRewriteProvider>();

            container.RegisterSingleton<IErrorService, ErrorService>();
            container.RegisterSingleton<IPopupInputService, PopupInputService>();
            container.RegisterSingleton<IProgressSpinner, ProgressSpinner>();
            container.RegisterSingleton<IAppCacheService<ClientState>, AppCacheService>();
            container.RegisterSingleton<ILinCApiServices, LinCApiServices>();
            container.RegisterSingleton<IAuthService, AuthService>();
            container.RegisterSingleton<IAdAuthService, AdAuthService>();
        }

        public void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IMasterDataService, MasterDataService>();
        }
    }
}
