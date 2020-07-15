using Xamarin.Forms;

namespace Cognizant.Hackathon.Mobile.Core.Common.Utils
{
    /// <summary>
    /// Contains the collection of public 
    /// bindable Attached Properties
    /// </summary>
    public class PublicAttachedProperties
    {

    }

    /// <summary>
    /// Bindable Attach property to Disable or 
    /// Enable ViewCell Default OnTouch Effect of iOS
    /// </summary>
    public static class ViewCellAttachedProperties
    {
        public static readonly BindableProperty IsDefaultOnTouchEffectEnabledProperty =
            BindableProperty.CreateAttached(
                "IsDefaultOnTouchEffectEnabled",
                typeof(bool),
                typeof(ViewCellAttachedProperties),
                true);

        public static bool GetIsDefaultOnTouchEffectEnabled(BindableObject view)
        {
            return (bool)view.GetValue(IsDefaultOnTouchEffectEnabledProperty);
        }

        public static void SetIsDefaultOnTouchEffectEnabled(BindableObject view, bool value)
        {
            view.SetValue(IsDefaultOnTouchEffectEnabledProperty, value);
        }
    }


    /// <summary>
    /// Bindable Attach property for 
    ///  the default Picker Control
    /// </summary>
    public static class PickerAttachedProperties
    {
        public static readonly BindableProperty PickerBorderColorProperty =
            BindableProperty.CreateAttached(
                "PickerBorderColor",
                typeof(Color),
                typeof(PickerAttachedProperties),
                Color.Default);

        public static Color GetPickerBorderColor(BindableObject view)
        {
            return (Color)view.GetValue(PickerBorderColorProperty);
        }

        public static void SetPickerBorderColor(BindableObject view, Color value)
        {
            view.SetValue(PickerBorderColorProperty, value);
        }



        public static readonly BindableProperty PickerPlaceholderColorProperty =
            BindableProperty.CreateAttached(
                "PickerPlaceholderColor",
                typeof(Color),
                typeof(PickerAttachedProperties),
                Color.Default);

        public static Color GetPickerPlaceholderColor(BindableObject view)
        {
            return (Color)view.GetValue(PickerPlaceholderColorProperty);
        }

        public static void SetPickerPlaceholderColor(BindableObject view, Color value)
        {
            view.SetValue(PickerPlaceholderColorProperty, value);
        }

        /// <summary>
        /// For getting the Selected Value of the Picker after clicking Ok Button on iOS
        /// </summary>
        public static readonly BindableProperty PickerConfirmedSelectedItemProperty =
            BindableProperty.CreateAttached(
                "PickerConfirmedSelectedItem",
                typeof(object),
                typeof(PickerAttachedProperties),
                default(object), BindingMode.TwoWay);

        public static object GetPickerConfirmedSelectedItem(BindableObject view)
        {
            return (object)view.GetValue(PickerConfirmedSelectedItemProperty);
        }

        public static void SetPickerConfirmedSelectedItem(BindableObject view, object value)
        {
            view.SetValue(PickerConfirmedSelectedItemProperty, value);
        }
    }


    /// <summary>
    /// Bindable Attach property to Disable or 
    /// Enable Status Bar's Transparency and
    /// whether it's drawn over the page
    /// or above the page in Android
    /// </summary>
    public static class ContentPageAttachedProperties
    {
        public static readonly BindableProperty IsStatusBarOverPageAndroidProperty =
            BindableProperty.CreateAttached(
                "IsStatusBarOverPageAndroid",
                typeof(bool),
                typeof(ContentPageAttachedProperties),
                false);

        public static bool GetIsStatusBarOverPageAndroid(BindableObject view)
        {
            return (bool)view.GetValue(IsStatusBarOverPageAndroidProperty);
        }

        public static void SetIsStatusBarOverPageAndroid(BindableObject view, bool value)
        {
            view.SetValue(IsStatusBarOverPageAndroidProperty, value);
        }
    }
}
