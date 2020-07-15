using Xamarin.Forms;

namespace LinC.Helpers
{
    public static class FontSizeHelpers
    {
        public static double TitleFontSize
        {
            get
            {
                var fontSize = 20;
                
                if (Device.RuntimePlatform == "Test") return fontSize;

                var density = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
               
                if (Device.RuntimePlatform == Device.iOS)
                {
                    switch (density)
                    {
                        case 2:
                            fontSize = 30;
                            break;
                        case 3:
                            fontSize = 38;
                            break;
                    }
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    switch (density)
                    {
                        case 2:
                            fontSize = 36;
                            break;
                        case 3:
                            fontSize = 40;
                            break;
                        case 3.5:
                            fontSize = 44;
                            break;
                    }
                }

                return fontSize;
            }
        }
    }
}