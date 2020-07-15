using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Cognizant.Hackathon.Core.Common.Enum;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Interfaces;

namespace Cognizant.Hackathon.Mobile.Core.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IInitialiserService _initialiserService;
        public INavigation Navigation => GetCurrentPage()?.Navigation;
        public Shell Shell => Application.Current.MainPage as Shell;
        public Page CurrentPage => GetCurrentPage();
        public NavigationDirection NavigationDirection { get; set; }
        public string Root { get; set; } = "root";

        private Stack<string> _navigationStack;

        public NavigationService(IInitialiserService initialiserService)
        {
            _initialiserService = initialiserService;
            _navigationStack = new Stack<string>();
        }

        private Page GetCurrentPage()
        {
            var mainPage = GetShellPage(Application.Current.MainPage);

            if (mainPage?.Navigation?.NavigationStack?.Count() > 1)
                return mainPage?.Navigation?.NavigationStack.LastOrDefault();

            switch (mainPage)
            {
                case MasterDetailPage page:
                    return page.Detail;
                case TabbedPage _:
                case CarouselPage _:
                    return ((MultiPage<Page>)mainPage).CurrentPage;
                default:
                    return mainPage;
            }
        }

        private Page GetShellPage(object item)
        {
            if (item is null)
                return null;

            if (item is ShellContent content)
            {
                if (content.Content != null)
                    return content.Content as Page;
                else if (PageService.CurrentPage != null)
                    return PageService.CurrentPage;
            }


            if (item is Shell shell)
                return GetShellPage(shell.CurrentItem);

            if (item is ShellItem shellItem)
                return GetShellPage(shellItem.CurrentItem);

            if (item is ShellSection shellSection)
                return GetShellPage(shellSection.CurrentItem);

            return null;
        }

        public async Task GoToAsync<TViewModel>(string route, Action<TViewModel> initialiser = null, bool animate = true)
        {
            if (initialiser != null)
                _initialiserService.SetInitialiser(initialiser);

            await GoToAsync(route, animate);
        }

        public async Task GoToAsync(string route, bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Forward;

            if (AddToNavigationStack(route))
                await Shell.Current.GoToAsync(route, animate);
        }

        public async Task GoBackAsync<TViewModel>(Action<TViewModel> initialiser = null, bool animate = true)
        {
            if (initialiser != null)
                _initialiserService.SetInitialiser(initialiser);

            await GoBackAsync(animate);
        }

        public async Task GoBackAsync(bool animate = true)
        {
            bool result = false;

            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            if (_navigationStack.Any())
                _navigationStack.Pop();

            if (Navigation.NavigationStack.Any())
                result = Shell.SendBackButtonPressed();
            else
                await GoToRootAsync();
        }

        public async Task GoToRootAsync(bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            // clear
            _navigationStack.Clear();

            await Navigation.PopToRootAsync(true);
        }

        public async Task GoToShellRootAsync(bool animate = true, string passingRoute = "")
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            if (Shell.Current != null && Shell.Current.Items != null && Shell.Current.Items.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(passingRoute))
                {
                    Shell.Current.CurrentItem = Shell.Current.Items.First(x => x.Route == Root);
                }
                else
                {
                    if(passingRoute.IndexOf("/") == -1)
                    {
                        Shell.Current.CurrentItem = Shell.Current.Items.Where(l => l.Route.Contains(passingRoute)).FirstOrDefault();
                    }
                    else
                    {
                        var strRouteArr = passingRoute.Split('/');
                        strRouteArr = strRouteArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (strRouteArr.Length == 1)
                        {
                            Shell.Current.CurrentItem = Shell.Current.Items.Where(l => l.Route.Contains(passingRoute)).FirstOrDefault();
                        }
                        else
                        {
                            Shell.Current.CurrentItem = Shell.Current.Items.Where(l => l.Route.Contains(strRouteArr[0])).FirstOrDefault();

                            Shell.Current.CurrentItem.CurrentItem = Shell.Current.CurrentItem.Items.Where(n => n.Route.Contains(strRouteArr[1])).FirstOrDefault();
                        }
                    }
                }
            }
        }

        public bool AddToNavigationStack(string route)
        {
            if (NavigationDirection == NavigationDirection.Backwards)
                return false;

            _navigationStack.Push(route.Replace("/", ""));
            NavigationDirection = NavigationDirection.Forward;

            return true;
        }

        public async Task GoToShellAsync(string title, bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            // clear
            _navigationStack.Clear();

            await Navigation.PopToRootAsync(true);

            Shell.Current.CurrentItem = Shell.Current.Items.FirstOrDefault(x => x.Title == title);
        }
    }
}