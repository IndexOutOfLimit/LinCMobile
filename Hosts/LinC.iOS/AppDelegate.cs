using System;
using System.Threading.Tasks;
using FFImageLoading.Svg.Forms;
using Firebase.Crashlytics;
using Foundation;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using StandardAppConfig;
using StandardAppConfig.FileSystemStream;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using LinC.iOS.Platforms;
using UIKit;
using Unity;
using Xamarin.Forms;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using Cognizant.Hackathon.Shared.Mobile.Core.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Core.Interface;
using Cognizant.Hackathon.Mobile.Shared.Services;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;

namespace LinC.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += UnobservedTaskException;

            //Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");

            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            Xamarin.FormsMaps.Init();

            Rg.Plugins.Popup.Popup.Init();

            #region App Settings and bootstrapper
            try
            {
                ConfigurationManager.Initialise(PortableStream.Current);
            }
            catch (Exception) { }

            BootStrapper.Initialize(new AppSettings());

            RegisterPlatformSpecificTypes();

            #endregion

            Initialize();
            //var formsApp = new App();
            App.DeviceDensity = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density.ToString();
            App.DeviceType = Device.Idiom == TargetIdiom.Phone ? "phone" : "tablet";
            LoadApplication(new App());

            //Firebase.Core.App.Configure();
            //Crashlytics.Configure();
            CrossCurrentActivity.Current = null;

            return base.FinishedLaunching(app, options);
        }

        private void Initialize()
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageSourceHandler();

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
            };
            FFImageLoading.ImageService.Instance.Initialize(config);
            var ignore = typeof(SvgCachedImage);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            //AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }

        private void RegisterPlatformSpecificTypes()
        {
            BootStrapper.Container?.RegisterType<ILogger, MobileAppLogger>();

            BootStrapper.Container?.RegisterInstance<IPopupNavigation>(PopupNavigation.Instance);

            BootStrapper.Container?.RegisterType<IAnalyticsService, AnalyticsService>();
        }

        private void UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
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
    }
}
