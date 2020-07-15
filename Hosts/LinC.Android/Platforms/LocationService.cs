using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Locations;
using Android.Runtime;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using LinC.Droid.Platforms;
using LinC.Platforms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(LocationAppService))]
namespace LinC.Droid.Platforms
{
    public class LocationAppService : FormsAppCompatActivity, ILocationService
    {
        private static readonly string[] _initialPermissions ={
              Manifest.Permission.AccessFineLocation,
              Manifest.Permission.AccessCoarseLocation,
            };
        public async Task OpenSettings()
        {
            LocationManager LM = (LocationManager)((CrossCurrentActivity.Current as MainActivity).ApplicationContext.GetSystemService(Context.LocationService));
            if (LM.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                //To check wheather the android version is Marshmallow(Version 6.0) or above to ask permission for using location service

                Context ctx = (CrossCurrentActivity.Current as MainActivity).ApplicationContext;
                ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            }
            await Task.Delay(500);

            //var googleApiClient = new GoogleApiClient.Builder(Android.App.Application.Context).AddApi(LocationServices.API).Build();
            //googleApiClient.Connect();

            //var activity = Forms.Context as Activity;

            //var locationRequest = LocationRequest.Create();
            //locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            //locationRequest.SetInterval(10000);
            //locationRequest.SetFastestInterval(10000 / 2);

            //var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
            //builder.SetAlwaysShow(true);

            //var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
            //result.SetResultCallback((LocationSettingsResult callback) =>
            //{
            //    switch (callback.Status.StatusCode)
            //    {
            //        case CommonStatusCodes.Success:
            //            {
            //                break;
            //            }
            //        case CommonStatusCodes.ResolutionRequired:
            //            {
            //                try
            //                {
            //                    callback.Status.StartResolutionForResult(activity, 1);
            //                }
            //                catch (IntentSender.SendIntentException ex)
            //                {
            //                }
            //                break;
            //            }
            //        default:
            //            {
            //                Forms.Context.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            //                break;
            //            }
            //    }
            //});
        }

        public bool CheckPermission()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) || locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                return false;
            }
            return true;
        }

        public bool CheckVersion()
        {

            if (Android.OS.Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.LollipopMr1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
