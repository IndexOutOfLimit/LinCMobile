using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Cognizant.Hackathon.Core.Common.Enum;

namespace Cognizant.Hackathon.Mobile.Core.Interfaces
{
    public interface INavigationService
    {
        INavigation Navigation { get; }
        Shell Shell { get; }
        Page CurrentPage { get; }
        NavigationDirection NavigationDirection { get; set; }
        Task GoToAsync<TViewModel>(string route, Action<TViewModel> initialiser = null, bool animate = true);
        Task GoToAsync(string route, bool animate = true);
        Task GoBackAsync<TViewModel>(Action<TViewModel> initialiser, bool animate = true);
        Task GoBackAsync(bool animate = true);
        Task GoToRootAsync(bool animate = true);
        Task GoToShellAsync(string title, bool animate = true);
        Task GoToShellRootAsync(bool animate = true, string passingRoute = "");
    }
}