using System;
using Xamarin.Forms;

namespace LinC.Controls
{
    public class CompanyAreaFrame : Frame
    {
        public CompanyAreaFrame()
        {
            //base.CornerRadius = 0;
        }

        public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CompanyAreaFrame), typeof(CornerRadius), typeof(CompanyAreaFrame), defaultBindingMode: BindingMode.TwoWay);

        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
            nameof(IsSelected),
            typeof(bool),
            typeof(CompanyAreaFrame),
            default(bool),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedChanged);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public bool NeedShadow { get; set; }

        private static void OnSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //((CustomFrame)bindable).CallThis();
            if (((CompanyAreaFrame)bindable).IsSelected)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    ((CompanyAreaFrame)bindable).CornerRadius = new CornerRadius(8, 17, 17, 8);
                }
                else
                {
                    ((CompanyAreaFrame)bindable).CornerRadius = new CornerRadius(5, 17, 17, 5);
                }

                ((CompanyAreaFrame)bindable).NeedShadow = true;
            }
            else
            {
                ((CompanyAreaFrame)bindable).CornerRadius = new CornerRadius(8, 8, 8, 8);
                ((CompanyAreaFrame)bindable).NeedShadow = false;
            }
        }
    }
}
