using System;
using System.Globalization;
using Xamarin.Forms;

namespace LinC.Converters
{
    public class LogoImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is null)
                return ImageSource.FromResource("LinC.Resources.Images.Cornerstone_small.png");

            if (value is bool && (bool)value)
			{
                return ImageSource.FromResource("LinC.Resources.Images.Burnett_small.png"); 
            }

            return ImageSource.FromResource("LinC.Resources.Images.Cornerstone_small.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
