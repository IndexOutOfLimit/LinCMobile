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
    }
}
