
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using LinC.Infrastructure;
using Xamarin.Forms.Xaml;

namespace LinC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDashboardPage : BaseContentPage
    {
        public UserDashboardPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains(ExceptionLiteral.StaticResNotFound))
                    throw;
            }
            catch (System.Exception ex)
            {

            }
           
        }
    }
}
