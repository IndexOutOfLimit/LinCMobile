using LinC.Infrastructure;
using Xamarin.Forms.Xaml;

namespace LinC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : BaseContentPage
    {
        ViewModels.LandingPageViewModel vm;
        public LandingPage()
        {
            InitializeComponent();

            if(this.BindingContext != null)
            {
                vm = this.BindingContext as ViewModels.LandingPageViewModel;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(vm != null)
            {
                await vm.GetMasterData();
            }
        }
    }
}
