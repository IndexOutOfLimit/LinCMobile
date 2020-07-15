/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Cognizant.Hackathon.Mobile.Core.Controls.AlertViews;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Interfaces;

namespace Cognizant.Hackathon.Mobile.Core.Services
{
     public class PopupInputService : IPopupInputService, IDisposable
    {
        private readonly IPopupNavigation _popupNavigation;

        public string OkButtonText { get; set; }
        public string MessageText { get; set; }
        public string TitleText { get; set; }
        public string PlaceHolderText { get; set; }
        public string ValidationLabelText { get; set; }
        public string CancelButtonText { get; set; }
        public bool IsShowing { get; set; }
    
         public PopupInputService(IPopupNavigation popupNavigation)
        {
            _popupNavigation = popupNavigation;
        }

        public async Task<string> ShowInputTextOkCancelAlertPopup(string titleText, string messageText, string placeHolderText, string okButtonText,
            string cancelButtonText, string validationLabelText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;
            PlaceHolderText = placeHolderText;
            CancelButtonText = cancelButtonText;
            ValidationLabelText = validationLabelText;

            var inputView = new InputTextOkCancelView(
                titleText,
                messageText,
                placeHolderText,
                okButtonText,
                cancelButtonText,
                validationLabelText);

            var page = new CustomAlertPopupPage<string>(inputView);

            bool clicked = false;

            inputView.OkButtonEventHandler += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(((InputTextOkCancelView)sender).TextInputResult))
                {
                    if (clicked) return;
                    clicked = true;

                    ((InputTextOkCancelView)sender).IsValidationLabelVisible = false;
                    page.PageClosingTaskCompletionSource.SetResult(((InputTextOkCancelView)sender).TextInputResult);
                }
                else
                {
                    ((InputTextOkCancelView)sender).IsValidationLabelVisible = true;
                }

                IsShowing = false;
            };

            inputView.CancelButtonEventHandler += (sender, args) =>
            {
                page.PageClosingTaskCompletionSource.SetResult(null);
                IsShowing = false;
            };

            IsShowing = true;

            return await Navigate(page);
        }

        public async Task<bool> ShowMessageOkCancelAlertPopup(
            string titleText, string messageText, 
            string okButtonText, string cancelButtonText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;
            CancelButtonText = cancelButtonText;
            
            if (Device.RuntimePlatform == "Test")
                return true;

            var inputView = new MessageAlertOkCancelView(
                titleText, messageText,
                okButtonText, cancelButtonText);

            var page = new CustomAlertPopupPage<bool>(inputView);

            inputView.OkButtonEventHandler += (sender, args) =>
            {
                page.PageClosingTaskCompletionSource.SetResult(true);
                IsShowing = false;
            };

            inputView.CancelButtonEventHandler += (sender, args) =>
            {
                page.PageClosingTaskCompletionSource.SetResult(false);
                IsShowing = false;
            };

            IsShowing = true;

            return await Navigate(page);
        }
        
        public Task<string> ShowInputSelectionCancelAlertPopup(string titleText, List<string> selectionList, string cancelButtonText)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ShowCustomViewAlertPopup<T>(object viewObject)
        {
            var page = new CustomAlertPopupPage<T>((View)viewObject);
            IsShowing = true;
            return await Navigate(page);
        }

        public async Task CloseLastPopup()
        {
            if (Device.RuntimePlatform == "Test")
            {
                IsShowing = false;
                return;
            }

            if (_popupNavigation.PopupStack.Any())
                await _popupNavigation.PopAsync();

            IsShowing = false;
        }

        public async Task<string> ShowMessageOkAlertPopup(
            string titleText, string messageText,
            string okButtonText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;

            var inputView = new MessageAlertOkView(
                titleText, messageText, okButtonText);

            var page = new CustomAlertPopupPage<string>(inputView);

            bool clicked = false;

            inputView.OkButtonEventHandler += (sender, args) =>
            {
                if (clicked) return;
                clicked = true;
                IsShowing = false;

                page.PageClosingTaskCompletionSource.SetResult(okButtonText);
            };

            IsShowing = true;

            return await Navigate(page);
        }
        
        /// <summary>
        /// Handle popup page Navigation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popup"></param>
        /// <returns></returns>
        private async Task<T> Navigate<T>(CustomAlertPopupPage<T> popup)
        {
            if (Device.RuntimePlatform == "Test")
                return default(T);

            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                await _popupNavigation.PushAsync(popup);
            });

            // await for the user to enter the text input
            var result = await popup.PageClosingTask;

            // Pop the page from Navigation Stack
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                await _popupNavigation.RemovePageAsync(popup);
            });

            return result;
        }

        public void Dispose() { }

        public async Task ShowInfoMarkdownViewPopup(string markDownText, ImageSource imageSource)
        {
            var inputView = new InfoMarkdownView(markDownText, imageSource);
            var page = new CustomAlertPopupPage<string>(inputView);

            inputView.CancelButtonEventHandler += async (sender, args) => { await CloseLastPopup(); };

            await Navigate(page);
            IsShowing = true;

        }
    }
}
*/