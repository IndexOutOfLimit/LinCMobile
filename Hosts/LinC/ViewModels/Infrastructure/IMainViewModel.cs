using System.Threading.Tasks;

namespace LinC.ViewModels
{
    public interface IMainViewModel
    {
        bool IsAuthenticated { get; }

        Task OnSleep();
        Task OnResume();
        Task OnStart();
    }
}