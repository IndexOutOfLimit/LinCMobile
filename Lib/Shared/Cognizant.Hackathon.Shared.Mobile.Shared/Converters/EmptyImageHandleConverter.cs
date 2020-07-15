using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Converters
{
    /// <summary>
    /// Handles possibility of empty image source and return the 
    /// You must pass in the App context: Buyer or Seller in the ConverterParameter
    /// </summary>
    public class EmptyImageHandleConverter : IValueConverter
    {
        private string _noImageAvailableIndicator = "Cognizant.Hackathon.{0}.Resources.Images.placeholder.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                _noImageAvailableIndicator = string.Format(_noImageAvailableIndicator, (string)parameter);
                return ImageSource.FromResource(_noImageAvailableIndicator);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
