using System.Diagnostics;
using System.Threading.Tasks;
using LinC.Views;
using Xamarin.Forms;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;

namespace LinC.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;
        private readonly IAppSettings _appSettings;
        private bool _flyOutIsOpen;
        
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }

        public bool IsComissionUpdated { get; set; }
        public string NetSellerCommission { get; set; }

        public CustomDelegateCommand GoToLoginCommand { get; }
        public CustomDelegateCommand HeaderTappedCommand { get; }
        public CustomDelegateCommand LogoutCommand { get; }

        public AppShellViewModel(ILinCApiServices services, IAppSettings appSettings)
        {
            _services = services;
            _appSettings = appSettings;

            GoToLoginCommand = new CustomDelegateCommand(async () => await GoToLogin(), () => true);
            HeaderTappedCommand = new CustomDelegateCommand(async () => await HeaderTapped(), () => true);
            LogoutCommand = new CustomDelegateCommand(async () => await Logout(), () => true);

            new InitializedViewModelBase(DoInitialSetUp).Invoke();

            SetApiEndPoint();
        }

        public bool FlyoutIsOpen
        {
            get => _flyOutIsOpen;
            set
            {
                SetProperty(ref _flyOutIsOpen, value);
            }
        }
        
        public Command<string> OpenMenuCommand => new Command<string>(item =>
        {
            //switch (item)
            //{
            //    case nameof(SomeExternalUrl): Device.OpenUri(new Uri(_services.AppSettings.WebUrl + SomeExternalUrl)); break;
            //}
        });

        public async Task GoToLogin()
        {
            FlyoutIsOpen = false;
            await AppNavigationService.GoToAsync(nameof(LoginPage).ToLower());
        }

        public async Task HeaderTapped()
        {
            Debug.WriteLine("tapped");
        }

        public async Task Logout()
        {
            await _services.AuthService.Logout();
            IsAuthenticated = _services.AppCache.State.IsAuthenticated;

            var currentPageType = AppNavigationService.CurrentPage.GetType();
            //if (currentPageType == typeof(ItemsPage)) //  || ...
            //{
            //    await _navigationService.GoToShellRootAsync();
            //}

            await Task.Delay(1000);//.ContinueWith(task => FlyoutIsOpen = false);
        }

        private void DoInitialSetUp()
        {
           
        }

        private void SetApiEndPoint()
        {
            //string sEndPoint = "Not set";
#if DEBUG
            _appSettings.ApiEndpoint = _appSettings.ApiEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiEndpointUser;
            //sEndPoint = "DEBUG";
#elif AdHoc_QA
            _appSettings.ApiEndpoint = _appSettings.ApiQAEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiQAEndpointUser;
            //sEndPoint = "QA";
#elif AdHoc_PQA
            _appSettings.ApiEndpoint = _appSettings.ApiPQAEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiPQAEndpointUser;
            //sEndPoint = "PQA";
#elif AdHoc_Staging
            _appSettings.ApiEndpoint = _appSettings.ApiStagingEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiStagingEndpointUser;
            //sEndPoint = "staging";
#elif AppStore
            _appSettings.ApiEndpoint = _appSettings.ApiPRODEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiPRODEndpointUser;
            //sEndPoint = "AppStore";
#elif AdHoc_PROD
            _appSettings.ApiEndpoint = _appSettings.ApiPRODEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiPRODEndpointUser;
            //sEndPoint = "PROD";
#else
            _appSettings.ApiEndpoint = _appSettings.ApiPRODEndpoint;
            _appSettings.ApiEndpointUser = _appSettings.ApiPRODEndpointUser;

            //sEndPoint = "All else";
#endif
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    Shell.Current.DisplayAlert("", sEndPoint, "ok");
            //});
        }
    }
}