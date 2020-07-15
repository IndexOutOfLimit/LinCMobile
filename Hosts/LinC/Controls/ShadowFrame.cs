using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace LinC.Controls
{
    public class ShadowFrame : Frame
    {
       
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(ShadowFrame),
        default(bool),
        BindingMode.TwoWay);


        public static readonly BindableProperty IsDashboardProperty = BindableProperty.Create(
        "IsDashboard",
        typeof(bool),
        typeof(ShadowFrame),
        false,
        BindingMode.TwoWay);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public bool IsDashboard
        {
            get => (bool)GetValue(IsDashboardProperty);
            set => SetValue(IsDashboardProperty, value);
        }
    }


}
