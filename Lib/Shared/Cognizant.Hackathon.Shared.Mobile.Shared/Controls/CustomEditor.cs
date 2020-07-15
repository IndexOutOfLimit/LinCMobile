using System;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls
{
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty BorderColorProperty =
    BindableProperty.Create(nameof(BorderColor),
        typeof(Color), typeof(CustomEditor), Color.Gray);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static int BorderWidthDefaultValue
        {
            get
            {
                if(Device.RuntimePlatform == Device.iOS)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

        public static double CornerRadiusDefaultValue
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    return 6;
                }
                else
                {
                    return 7;
                }
            }
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(int),
            typeof(CustomEditor), BorderWidthDefaultValue);
        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius),
            typeof(double), typeof(CustomEditor), CornerRadiusDefaultValue);
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomEditor), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public static readonly BindableProperty IsBorderEnabledProperty =
        BindableProperty.Create(nameof(IsBorderEnabled),
            typeof(bool), typeof(CustomEditor), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsBorderEnabled
        {
            get => (bool)GetValue(IsBorderEnabledProperty);
            set => SetValue(IsBorderEnabledProperty, value);
        }
    }
}