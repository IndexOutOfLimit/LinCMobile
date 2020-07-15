using System;
using Android.OS;
using Android.Views;
using LinC.Droid.Platforms;
using LinC.Platforms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarColorDroid))]
namespace LinC.Droid.Platforms
{
    public class StatusBarColorDroid : IStatusBarColor
    {

        public void SetStatusBarColor(string color)
        {
            Color barColor = Color.FromHex(color);
            //(Android.App.Application.Context as FormsAppCompatActivity).Window.SetStatusBarColor(barColor.ToAndroid());

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var c = MainActivity.context as FormsAppCompatActivity;
                c?.RunOnUiThread(() => c.Window.SetStatusBarColor(barColor.ToAndroid()));
            }
        }

    }
}
