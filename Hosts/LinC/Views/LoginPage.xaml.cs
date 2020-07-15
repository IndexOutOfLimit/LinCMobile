using System;
using LinC.Infrastructure;
using LinC.ViewModels;
using Xamarin.Forms.Xaml;

namespace LinC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BaseContentPage
    {
        LoginPageViewModel vm;

        public LoginPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                
            }

            vm = this.BindingContext as LoginPageViewModel;
        }

        protected override void OnAppearing()
        {
            //base.OnAppearing();

            if(vm != null)
            {
                //if(vm.CheckIfComissionUpdated())
                //{
                   // vm.ShowComissionValue();
                //}
                //else
                //{
                    vm.LoginCommand.Execute();
                //}                
            }
        }
    }
}