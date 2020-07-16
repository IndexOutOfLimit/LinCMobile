using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.OS;
using Android.Content;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using StandardAppConfig;
using Unity;
using Xamarin.Forms;
using FFImageLoading.Svg.Forms;
using LinC.Droid.Platforms;
using StandardAppConfig.FileSystemStream;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using Cognizant.Hackathon.Shared.Mobile.Core.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Core.Interface;
using Cognizant.Hackathon.Mobile.Shared.Services;
using Cognizant.Hackathon.Mobile.Core.Services;
using Plugin.Permissions;
using Android;

namespace LinC.Droid
{
    [Activity(Label = "LinC", Icon = "@drawable/appicon", LaunchMode=LaunchMode.SingleInstance, Theme = "@style/MainTheme", MainLauncher = true,  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static App _app;
        private static IUnityContainer _container;

        private static readonly string[] _initialPermissions ={
             Manifest.Permission.AccessFineLocation,
              Manifest.Permission.AccessCoarseLocation,
              Manifest.Permission.AccessNotificationPolicy,
              Manifest.Permission.Camera,
              Manifest.Permission.ReadContacts,
              Manifest.Permission.WriteContacts,
              Manifest.Permission.CallPhone,
              Manifest.Permission.RecordAudio,
              Manifest.Permission.ReadExternalStorage
            };

        public static Context context;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            //base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);

            //To check wheather the android version is Marshmallow(Version 6.0) or above to ask permission for using location service
            if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.LollipopMr1)
            {
                RequestPermissions(_initialPermissions, 1337);
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentOnUnhandledException;
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);

            global::Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            #region App Settings and bootstrapper
            try
            {
                ConfigurationManager.Initialise(PortableStream.Current);
                //Crashlytics.Crashlytics.HandleManagedExceptions();
            }
            catch (Exception)
            {
            }

            //BootStrapper.Initialize(new AppSettings());
            if (_container == null)
            {
                BootStrapper.Initialize(new AppSettings());
            }
            else
            {
                BootStrapper.Container = _container;
            }

            RegisterPlatformSpecificTypes();

            #endregion

            Initialize();

            App.DeviceDensity = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density.ToString();
            App.DeviceType = Device.Idiom == TargetIdiom.Phone ? "phone" : "tablet";
            CrossCurrentActivity.Current = this;

            if(_app == null)
            {
                _app = new App();
            }
            LoadApplication(_app);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnPause()
        {
            base.OnPause();
            //save the current page before app pauses, because Xamarin doesn't always call OnAppearing on the correct page when resume happens
            PageService.SavedStatePage = PageService.CurrentPage;

            _container = BootStrapper.Container;
        }

        private void Initialize()
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
        }

        private void AndroidEnvironmentOnUnhandledException(object sender, RaiseThrowableEventArgs e)
        {
            Exception ex = e.Exception;
            HandleException(ex);
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            HandleException(ex);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            HandleException(ex, e.IsTerminating);
        }

        private void HandleException(Exception ex, bool isTerminating = false)
        {
            if (isTerminating)
                Task.Run(() => BootStrapper.Container?.Resolve<IAppCacheService<ClientState>>().Save());

            var innerEx = Cognizant.Hackathon.Core.Common.Helpers.ExceptionHelpers.GetInnermostException(ex);
            BootStrapper.Container?.Resolve<IErrorService>().AddError(innerEx.Message,
                ViewModelError.ErrorAction.Log,
                ViewModelError.ErrorSeverity.Error,
                ex: innerEx);
            BootStrapper.Container?.Resolve<IErrorService>().ProcessErrors();
        }

        private void RegisterPlatformSpecificTypes()
        {
            BootStrapper.Container?.RegisterType<ILogger, MobileAppLogger>();
            BootStrapper.Container?.RegisterInstance<IPopupNavigation>(PopupNavigation.Instance);
            BootStrapper.Container?.RegisterType<IAnalyticsService, AnalyticsService>();
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ContactServiceDroid.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            context = this;
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();   
        }
    }
}