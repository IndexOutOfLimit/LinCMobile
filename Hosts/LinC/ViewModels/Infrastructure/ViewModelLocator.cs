using System;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using LinC.Views;
using Unity;
using Unity.Lifetime;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class ViewModelLocator
    {
        /// <summary>
        /// ViewModelLocator constructor
        /// </summary>
        static ViewModelLocator()
        {
            Init();
        }

        // CORE

        public AppShellViewModel AppShell => BootStrapper.Container.Resolve<AppShellViewModel>();

        public LoginPageViewModel LoginPage => BootStrapper.Container.Resolve<LoginPageViewModel>();                
        //LinC Project 
        public ChatPageViewModel ChatPage => BootStrapper.Container.Resolve<ChatPageViewModel>();
        public RegistrationPageViewModel RegistrationPage => BootStrapper.Container.Resolve<RegistrationPageViewModel>();
        public AddProductPageViewModel AddProductPage => BootStrapper.Container.Resolve<AddProductPageViewModel>();
        public ProductsPageViewModel ProductsPage => BootStrapper.Container.Resolve<ProductsPageViewModel>();
        public ReviewProductsPageViewModel ReviewProductsPage => BootStrapper.Container.Resolve<ReviewProductsPageViewModel>();
        public UserDashboardPageViewModel UserDashboardPage => BootStrapper.Container.Resolve<UserDashboardPageViewModel>();

        public CartPageViewModel CartPage => BootStrapper.Container.Resolve<CartPageViewModel>();

        public SupplierCataloguePageViewModel SupplierCataloguePage => BootStrapper.Container.Resolve<SupplierCataloguePageViewModel>();
        public ProductCataloguePageViewModel ProductCataloguePage => BootStrapper.Container.Resolve<ProductCataloguePageViewModel>();
        public LandingPageViewModel LandingPage=> BootStrapper.Container.Resolve<LandingPageViewModel>();
        public MapPageViewModel MapPage => BootStrapper.Container.Resolve<MapPageViewModel>();

        public static void Init()
        {
            // Register anything related to this shared projects including *ViewModels.
            // After registering the page to Unity container, make sure to define the
            // property of the view model as well. See below part.

            // CORE
            BootStrapper.Container?.RegisterSingleton<AppShellViewModel>();
            BootStrapper.Container?.RegisterSingleton<IMainViewModel, MainViewModel>();

            RegisterViewModel<LoginPageViewModel, LoginPage>();
           
            //LinC Project 
            RegisterViewModel<ChatPageViewModel, ChatPage>();
            RegisterViewModel<RegistrationPageViewModel, RegistrationPage>();
            RegisterViewModel<AddProductPageViewModel, AddProductPage>();
            RegisterViewModel<ProductsPageViewModel, ProductsPage>();
            RegisterViewModel<UserDashboardPageViewModel, UserDashboardPage>();
            RegisterViewModel<ReviewProductsPageViewModel, ReviewProductsPage>();

            RegisterViewModel<CartPageViewModel, CartPage>();

            RegisterViewModel<SupplierCataloguePageViewModel, SupplierCataloguePage>();
            RegisterViewModel<ProductCataloguePageViewModel, ProductCataloguePage>();
            RegisterViewModel<LandingPageViewModel, LandingPage>();
            RegisterViewModel<MapPageViewModel, MapPage>();

            void RegisterViewModel<TViewModel, TPage>()
            {
                BootStrapper.Container?.RegisterType<TViewModel>(new SingletonLifetimeManager());
                BootStrapper.Container?.RegisterType<TPage>();
            }
        }

        public static void RegisterRoutes()
        {
            // REGISTER ROUTES

            RegisterRoute<RegistrationPage>();
            //RegisterRoute<LoginPage>();
            RegisterRoute<ChatPage>();
            RegisterRoute<AddProductPage>();
            RegisterRoute<ReviewProductsPage>();
            RegisterRoute<CartPage>();
            RegisterRoute<UserDashboardPage>();
            RegisterRoute<ProductsPage>();
            RegisterRoute<SupplierCataloguePage>();
            RegisterRoute<ProductCataloguePage>();
            RegisterRoute<MapPage>();
        }

        private static void RegisterRoute<TPage>() where TPage : class
        {            
            var page = typeof(TPage).Name.ToLower();

            Routing.UnRegisterRoute(page);

            var fac = new TypeRouteFactory(typeof(TPage));
            //Routing.RegisterRoute(page, new TypeRouteFactory(typeof(TPage)));
            try
            {
                Routing.RegisterRoute(page, fac);
                fac.GetOrCreate();
            }
            catch (Exception ex)
            {

            }
            
        }

        private class TypeRouteFactory : RouteFactory
        {
            private readonly Type _type;

            public TypeRouteFactory(Type type)
            {
                _type = type;
            }

            public override Element GetOrCreate()
            {
                return (Element) BootStrapper.Container?.Resolve(this._type);
            }
        }
    }
}
