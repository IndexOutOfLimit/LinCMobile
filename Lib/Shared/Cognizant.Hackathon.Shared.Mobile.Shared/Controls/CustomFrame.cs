using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls
{
    public class CustomFrame : Frame
    {
        
            public new static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CustomFrame), typeof(CornerRadius), typeof(CustomFrame));

            public CustomFrame()
            {
                base.CornerRadius = 0;
            }

            public new CornerRadius CornerRadius
            {
                get => (CornerRadius)GetValue(CornerRadiusProperty);
                set => SetValue(CornerRadiusProperty, value);
            }

            public static BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(CustomFrame), 4.0f);

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

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
            nameof(IsSelected),
            typeof(bool),
            typeof(CustomFrame),
            default(bool),
            BindingMode.TwoWay,
            propertyChanged: OnSelectedChanged);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        private static void OnSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //((CustomFrame)bindable).CallThis();
            if (((CustomFrame)bindable).IsSelected)
            {
                ((CustomFrame)bindable).Elevation = 20;             
            }
            else
            {
                ((CustomFrame)bindable).Elevation = 5;                
            }
        }
    }    
}


