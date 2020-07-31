using System;
using System.Collections.Generic;
using LinC.Infrastructure;
using LinC.ViewModels;
using Xamarin.Forms;

namespace LinC.Views
{
    public partial class ChatPage : BaseContentPage
    {
        public ChatPage()
        {
            try
            {
                InitializeComponent();

                Webview.Source = "https://web-chat.global.assistant.watson.cloud.ibm.com/preview.html?region=eu-gb&integrationID=b097d5fb-d034-40c0-be3b-a61f82f598de&serviceInstanceID=5e250dbe-ef07-4823-96df-3126941ea2a2";

                //BindingContext = vm = new ChatBotViewModel();
                ChatPageViewModel vm = (ViewModels.ChatPageViewModel)this.BindingContext;

                vm.Messages.CollectionChanged += (sender, e) =>
                {
                    var target = vm.Messages[vm.Messages.Count - 1];
                    //MessagesList.ScrollTo(target, ScrollToPosition.End, true);
                };
            }
            catch (Exception ex)
            {

            }            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await progress.ProgressTo(0.9, 900, Easing.SpringIn);
        }

        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }

        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }
    }
}
