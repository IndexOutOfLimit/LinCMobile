using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Validation;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;

namespace LinC.ViewModels
{

    public class MainViewModel : ValidatableBindableBase, IMainViewModel
    {
        private readonly ILinCApiServices _linCApiServices;

        public MainViewModel(ILinCApiServices linCApiServices) : base()
        {
            _linCApiServices = linCApiServices;
        }

        public bool IsAuthenticated { get; }

        public async Task OnSleep()
        {

        }

        public async Task OnResume()
        {
            
        }

        public async Task OnStart()
        {

        }
    }
}