using System;
using System.Windows.Input;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using LinC.Views;
using Unity;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Mobile.Core.Services;

namespace LinC.Infrastructure
{
    public class BaseContentPage : ContentPage
    {
        public static readonly BindableProperty ToolbarItemCommandProperty = BindableProperty.Create(
            nameof(ToolbarItemCommand),
            typeof(ICommand),
            typeof(BaseContentPage));

        public ICommand ToolbarItemCommand
        {
            get => (ICommand)GetValue(ToolbarItemCommandProperty);
            set => SetValue(ToolbarItemCommandProperty, value);
        }

        public BaseContentPage() : base()
        {
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();

            //Mainly for Android, restore the current page to the last saved page when the app paused
            if (PageService.SavedStatePage != null)
            {
                PageService.CurrentPage = PageService.SavedStatePage;
                PageService.SavedStatePage = null;
            }
            else
            {
                //default behavior. Set the current page to the one currently appearing.
                PageService.CurrentPage = this;
            }
        }
                
        protected virtual async Task ToolbarItemTapped(string navParam)
        {
            // TODO: treat as hamburger, ie navigate on same top level           
            var currentPage = BootStrapper.Container.Resolve<INavigationService>().CurrentPage.GetType().Name;

            //if (!string.Equals(currentPage, navParam, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    if(navParam == nameof(ItemDetailPage))
            //        await BootStrapper.Container.Resolve<INavigationService>().GoBackAsync();
            //    else
            //        await BootStrapper.Container.Resolve<INavigationService>().GoToAsync(navParam.ToLower());
            //}
        }
    }

    public class DisplayPage
    {
        public string Title { get; set; }
        public string NavParam { get; set; }
    }
}