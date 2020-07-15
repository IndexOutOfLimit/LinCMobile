using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Extensions;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using Cognizant.Hackathon.Mobile.Core.Extensions;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Interfaces;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Services
{
    public class ProgressSpinner : IProgressSpinner, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly IPopupNavigation _popupNavigation;
        private ProgressSpinnerPage _spinnerPageInstance;
        private bool _isSpinning;

        public ProgressSpinner(INavigationService navigationService, IPopupNavigation popupNavigation)
        {
            _navigationService = navigationService;
            _popupNavigation = popupNavigation;
        }

        public void HideLoading()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                int retry = 0;

                if (_spinnerPageInstance == null)
                    return;

                while (!_isSpinning && retry <= 5)
                {
                    await Task.Delay(200);
                    retry++;
                }

                if (_spinnerPageInstance != null)
                {
                    await _navigationService.Navigation.RemovePopupPageAsync(_spinnerPageInstance, true);
                    //await _navigationService.Navigation.PopPopupAsyncSafe(_popupNavigation, true);
                }

                _spinnerPageInstance = null;
                _isSpinning = false;
            });
        }

        public void ShowLoading(bool isCancellable = true)
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                if (_spinnerPageInstance == null)
                {
                    _spinnerPageInstance = new ProgressSpinnerPage(isCancellable);
                    
                        await _navigationService.Navigation.PushPopupAsyncSafe(_spinnerPageInstance, _popupNavigation, true)
                                .ContinueWith((t) => _isSpinning = true);
                }
            });
        }

        public void Dispose() { }
    }
}