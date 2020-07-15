using System;
using Xamarin.Forms;

namespace LinC.Converters
{
    public class BoolToExpandIndicatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? "CollapseMinusIcon.png" : "ExpandedPlusIcon.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
