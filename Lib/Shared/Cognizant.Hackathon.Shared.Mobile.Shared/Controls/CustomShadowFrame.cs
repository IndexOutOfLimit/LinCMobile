using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls
{
    public class CustomShadowFrame : Frame
    {
        public static BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(CustomShadowFrame), 4.0f);

        public static readonly BindableProperty CommandProperty =
               BindableProperty.Create("Command", typeof(ICommand), typeof(Frame), null);


        public float Elevation
        {
            get
            {
                return (float)GetValue(ElevationProperty);
            }
            set
            {
                SetValue(ElevationProperty, value);
            }
        }
    }
}
