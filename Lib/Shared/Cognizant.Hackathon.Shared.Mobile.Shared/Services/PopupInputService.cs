using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls.AlertViews;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls.CustomPopupDrawer;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Enums;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Services
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

            var page = new CustomGeneralPopupView<string>(inputView);

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

        public async Task<string> ShowMessageOkCancelAlertPopup(
            string titleText, string messageText,
            string okButtonText, string cancelButtonText, bool isFromDeleteQuote = false)
        {

            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;
            CancelButtonText = cancelButtonText;

            var inputView = new MessageAlertOkCancelView(titleText, messageText, okButtonText, cancelButtonText, isFromDeleteQuote);

            var page = new CustomGeneralPopupView<string>(inputView);

            bool clicked = false;

            inputView.OkButtonEventHandler += (sender, args) =>
            {
                if (clicked) return;
                clicked = true;
                IsShowing = false;

                page.PageClosingTaskCompletionSource.SetResult(okButtonText);
            };

            inputView.CancelButtonEventHandler += (sender, args) =>
            {
                if (clicked) return;
                clicked = true;
                IsShowing = false;

                page.PageClosingTaskCompletionSource.SetResult(cancelButtonText);
            };

            IsShowing = true;

            return await Navigate(page);
        }

        public async Task<object> ShowInputSelectionPopup(string titleText, List<object> selectionList, string cancelButtonText)
        {
            TitleText = titleText;
            CancelButtonText = cancelButtonText;

            var inputView = new MultipleInputSectionPopup(selectionList, titleText);

            var page = new CustomGeneralPopupView<object>(inputView);

            inputView.CancelButtonEventHandler += (sender, args) =>
            {
                page.PageClosingTaskCompletionSource.SetResult("Close");
            };

            bool clicked = false;

            inputView.InputSelectionEventHandler += async (sender, args) =>
            {
                if (clicked) return;
                clicked = true;

                page.PageClosingTaskCompletionSource.SetResult(((MultipleInputSectionPopup)sender).InputSelectionStringResult);
            };

            return await Navigate(page);
        }
        
        public Task<string> ShowInputSelectionCancelAlertPopup(string titleText, List<string> selectionList, string cancelButtonText)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ShowCustomViewAlertPopup<T>(object viewObject)
        {
            var page = new CustomGeneralPopupView<T>((View)viewObject);
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

        public async Task<string> ShowMoreOptionPopupView(string previousRoute)
        {
            var inputView = new MoreOptionPopup();
            inputView.PreviousRoute = previousRoute;

            var page = new CustomGeneralPopupView<string>(inputView);

            bool clicked = false;

            inputView.CancelButtonEventHandler += (sender, args) =>
            {
                if (clicked) return;
                clicked = true;
                IsShowing = false;

                page.PageClosingTaskCompletionSource.SetResult("Close");
            };

            IsShowing = true;

            return await Navigate(page);
        }

        public async Task<string> ShowMessageOkAlertPopup(
            string titleText, string messageText,
            string okButtonText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;

            var inputView = new MessageAlertOkView(titleText, messageText, okButtonText);

            var page = new CustomGeneralPopupView<string>(inputView);

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
        private async Task<T> Navigate<T>(CustomGeneralPopupView<T> popup)
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
                try
                {
                    await _popupNavigation.RemovePageAsync(popup);
                }
                catch
                {
                    try
                    {
                        await _popupNavigation.PopAsync();
                    }
                    catch
                    {

                    }
                    
                }               
            });

            return result;
        }

        public void Dispose() { }
    }
}
