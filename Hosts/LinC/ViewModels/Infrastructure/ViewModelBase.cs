using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonServiceLocator;
using Xamarin.Forms;
using Cognizant.Hackathon.Core.Common.Enum;
using Cognizant.Hackathon.Core.Common.Helpers;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Mobile.Core.Validation;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Unity;
using System.ComponentModel;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;

namespace LinC.ViewModels
{
    public abstract partial class ViewModelBase : ValidatableBindableBase, INotifyPropertyChanged
    {
        protected IMainViewModel _mainViewModel;

        private IEnumerable<PropertyInfo> AppNestedViewModelsInfo { get; set; }

        protected static ShellNavigatingEventArgs ShellNavigatingEventArgs { get; set; }
        protected static ShellNavigatedEventArgs ShellNavigatedEventArgs { get; set; }

        internal delegate void InitializedViewModelBase();

        public bool IsInitialized { get; protected set; }

        public Action OnNavigatedComplete { get; set; }

        protected INavigationService AppNavigationService => BootStrapper.Container.Resolve<INavigationService>();

        protected IInitialiserService AppInitialiserService => BootStrapper.Container.Resolve<IInitialiserService>();

        protected IProgressSpinner AppSpinner => BootStrapper.Container.Resolve<IProgressSpinner>();

        public static Type CurrentViewModelType { get; private set; }

        public string CurrentPageType => CurrentViewModelType?.Name.Replace("ViewModel", string.Empty);

        public string CurrentRoute => GetCurrentRoute();

        public Int16 TabCurrentIndex { get; set; }
        public Int16 TabPreviousIndex { get; set; }

        public LinCUserType LinCAppUserType { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the HasNavigationBar.
        /// </summary>
        public bool HasNavigationBar { get; set; }

        /// <summary>
        /// Gets or sets the HasNavigationBarTitleImage.
        /// </summary>
        public bool HasNavigationBarTitleImage { get; set; }

        /// <summary>
        /// Gets or sets the HasBackButton.
        /// </summary>
        public bool HasBackButton { get; set; }

        /// <summary>
        /// Gets or sets the NavigationBarTitleImage.
        /// </summary>
        public ImageSource NavigationBarTitleImage { get; set; } // TODO: should be a string path

        /// <summary>
        /// Gets or sets the BackButtonText.
        /// </summary>
        public string BackButtonTitle { get; set; }

        /// <summary>
        /// Gets or sets the HasLeftCustomButton.
        /// </summary>
        public bool HasLeftCustomButton { get; set; }

        /// <summary>
        /// Gets or Sets the iOS Status bar hidden/visible
        /// </summary>
        public bool IsStatusBarHidden { get; set; }

        /// <summary>
        /// For setting the background color of page
        /// </summary>
        public Color PageBackgroundColor { get; set; }


        public string CompanyCode { get; set; }

        public LinCUser UserDetails { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is current view model.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is current view model; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentViewModel => this.GetType() == CurrentViewModelType;

        public CustomDelegateCommand ShowFlyoutCommand { get; set; }

        public CustomDelegateCommand GoBackCommand { get; set; }
        public CustomDelegateCommand TabbarTappedCommand { get; set; }

        protected ViewModelBase()
        {
            _mainViewModel = BootStrapper.Container.Resolve<IMainViewModel>() ?? ServiceLocator.Current.GetInstance<IMainViewModel>();

            ShowFlyoutCommand = new CustomDelegateCommand(ShowFlyout, () => true);
            GoBackCommand = new CustomDelegateCommand(async () => await GoBack(), () => true);

            // initialize only if has pushed
            Initialize();
        }

        ~ViewModelBase()
        {
            MessagingCenter.Unsubscribe<string, ShellNavigatingEventArgs>(nameof(AppShell), $"{GetCurrentRoute()}{nameof(AppShell.OnShellNavigating)}In");
            MessagingCenter.Unsubscribe<string, ShellNavigatingEventArgs>(nameof(AppShell), $"{GetCurrentRoute()}{nameof(AppShell.OnShellNavigating)}Out");
            MessagingCenter.Unsubscribe<string, ShellNavigatedEventArgs>(nameof(AppShell), GetCurrentRoute() + nameof(AppShell.OnShellNavigated));
            MessagingCenter.Unsubscribe<string, bool>(nameof(AppShell), nameof(Shell.FlyoutIsPresented));
        }

        private void Initialize()
        {
            if (IsInitialized || CurrentRoute == "appshell")
                return;

            //_nestedViewModelsInfo = GetNestedViewModelsInfo();

            MessagingCenter.Subscribe<string, ShellNavigatingEventArgs>(nameof(AppShell), $"{GetCurrentRoute()}{nameof(AppShell.OnShellNavigating)}Out",
                async (sender, args) => await OnShellNavigatingOut(sender, args));

            MessagingCenter.Subscribe<string, ShellNavigatingEventArgs>(nameof(AppShell), $"{GetCurrentRoute()}{nameof(AppShell.OnShellNavigating)}In",
                 async (sender, args) => await OnShellNavigatingIn(sender, args));

            MessagingCenter.Subscribe<string, ShellNavigatedEventArgs>(nameof(AppShell), GetCurrentRoute() + nameof(AppShell.OnShellNavigated),
                 async (sender, args) => await OnShellNavigated(sender, args));

            MessagingCenter.Subscribe<string, bool>(nameof(AppShell), nameof(Shell.FlyoutIsPresented),
                (sender, args) => OnFlyoutClosed(sender, args));

            IsInitialized = true;
        }

        protected virtual void OnFlyoutClosed(string sender, bool isFlyoutClosed)
        {
        }

        protected virtual async Task OnShellNavigatingOut(string sender, ShellNavigatingEventArgs args)
        {
            ShellNavigatingEventArgs = args;

            AppErrorService.ClearInlineValidators();
        }

        protected virtual async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            ShellNavigatingEventArgs = args;

            if (this.GetType() != typeof(AppShellViewModel) && this.GetType() != typeof(MainViewModel))
                CurrentViewModelType = this.GetType();

            AppInitialiserService.Initialise(this);
        }

        protected virtual async Task OnShellNavigated(string sender, ShellNavigatedEventArgs args)
        {
            ShellNavigatedEventArgs = args;

            if (AppNavigationService.NavigationDirection == NavigationDirection.Backwards)
                AppInitialiserService.Initialise(this);

            OnNavigatedComplete?.Invoke();

            // reset navigation direction
            AppNavigationService.NavigationDirection = NavigationDirection.Forward;
        }

        private IEnumerable<PropertyInfo> GetNestedViewModelsInfo()
        {
            return GetType().GetProperties(ReflectionExtensions.BindingFlags.Public | ReflectionExtensions.BindingFlags.Instance)
                .Where(x => x.PropertyType.Name.Contains("ViewModel"));
        }

        protected virtual async Task GoBack()
        {
            await AppNavigationService.GoBackAsync();
        }

        private void ShowFlyout()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        public async void OnResume()
        {

        }

        public async void OnSleep()
        {
            // In the case of Upgrade alert popup, we are closing it down when the app goes to background.
            await AppPopupInputService.CloseLastPopup();
        }

        public async Task<Xamarin.Essentials.Location> GetUserLocation()
        {
            var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var userLocation = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);

            return userLocation;
        }

        public async Task<Xamarin.Essentials.Placemark> GetUserAddressFromLatLong(double latitude, double longitude)
        {
            Xamarin.Essentials.Placemark placemark = null;
            try
            {
                var placemarks = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(latitude, longitude);

                placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";

                    Console.WriteLine(geocodeAddress);
                }

            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
                placemark = null;
            }

            return placemark;
        }

        public async Task<Xamarin.Essentials.Location> GetUserLocationFromAddress(string address)
        {
            //Example: address =  "Microsoft Building 25 Redmond WA USA";

            Xamarin.Essentials.Location location = null;
            try
            {
                var locations = await Xamarin.Essentials.Geocoding.GetLocationsAsync(address);

                location = locations?.FirstOrDefault();
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }

            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
                location = null;
            }

            return location;
        }

        public async Task CheckLocationPermission()
        {
            if (global::Xamarin.Forms.DependencyService.Get<global::LinC.Platforms.ILocationService>().CheckPermission()) //returns true when Location Service disabled
            {
                await global::Xamarin.Forms.DependencyService.Get<global::LinC.Platforms.ILocationService>().OpenSettings();
                await Task.Delay(500);
            }
        }

        public async Task<PermissionStatus> CheckPermission(Permission permission)
        {
            var status = PermissionStatus.Unknown;
            if (Xamarin.Forms.Device.RuntimePlatform == global::Xamarin.Forms.Device.Android)
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    status = await CheckPermissionsAsync(Permission.Location);
                }
                //if (status == PermissionStatus.Granted)
                //{
                //    var location = await GetUserLocation();
                //    if (App.UserDetails == null)
                //    {
                //        App.UserDetails = new LinCUser();
                //    }
                //    App.UserDetails.Latitude = location?.Latitude.ToString();
                //    App.UserDetails.Longitude = location?.Longitude.ToString();
                //}
            }

            return status;
        }

        public static async Task<PermissionStatus> CheckPermissionsAsync(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                // denied
                if (!newStatus.ContainsKey(permission))
                {
                    return permissionStatus;
                }
                permissionStatus = newStatus[permission];

                /*if (newStatus[permission] != PermissionStatus.Granted)
                {
                    permissionStatus = newStatus[permission];
                    var title = $"{permission} Permission";
                    var question = $"LinC requires {permission} permission.";
                    var positive = "Settings";
                    var negative = "Cancel";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                    {
                        return permissionStatus;
                    }

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return permissionStatus;
                }*/
            }
            return permissionStatus;
        }

        private string GetCurrentRoute()
        {
            return this.GetType().Name.Replace("ViewModel", string.Empty).ToLower();
        }
    }
}