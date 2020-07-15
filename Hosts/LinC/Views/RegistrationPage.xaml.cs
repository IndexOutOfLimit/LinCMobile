using LinC.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : BaseContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            //Shell.SetTabBarIsVisible(this, false);            
        }
    }
}
