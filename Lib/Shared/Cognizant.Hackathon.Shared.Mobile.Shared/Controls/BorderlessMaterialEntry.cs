using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls
{
    /// <summary>
    /// Material Entry without Border on iOS/Android
    /// </summary>
    public class BorderlessMaterialEntry : Entry
    {
        public static readonly BindableProperty IsNumericKeyboardProperty = BindableProperty.Create(nameof(IsNumericKeyboard), typeof(bool), typeof(BorderlessMaterialEntry), false);

        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(BorderlessMaterialEntry), Color.Transparent);
        
        public bool IsNumericKeyboard
        {
            get => (bool)GetValue(IsNumericKeyboardProperty);
            set => SetValue(IsNumericKeyboardProperty, value);
        }

        public Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }

        public static readonly BindableProperty IsNegativeAllowedProperty = BindableProperty.Create(nameof(IsNegativeAllowed), typeof(bool), typeof(BorderlessMaterialEntry), false);

        public bool IsNegativeAllowed
        {
            get => (bool)GetValue(IsNegativeAllowedProperty);
            set => SetValue(IsNegativeAllowedProperty, value);
        }        
    }
}
