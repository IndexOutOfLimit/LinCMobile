using System;
using System.Globalization;
using Xamarin.Forms;

namespace LinC.Converters
{
    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((parameter as string).ToUpper()[0])
            {
                case 'U':
                    return ((string)value).ToUpper();
                case 'L':
                    return ((string)value).ToLower();
                default:
                    return ((string)value);
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}