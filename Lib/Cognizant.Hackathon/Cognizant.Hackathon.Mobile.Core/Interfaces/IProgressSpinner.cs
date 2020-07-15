using System.Threading.Tasks;

namespace Cognizant.Hackathon.Mobile.Core.Interfaces
{
    public interface IProgressSpinner
    {
        void HideLoading();

        void ShowLoading(bool isCancellable = true);
    }
}