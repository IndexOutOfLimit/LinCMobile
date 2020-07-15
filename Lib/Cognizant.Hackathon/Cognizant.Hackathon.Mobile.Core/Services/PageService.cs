using System;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Mobile.Core.Services
{
    public class PageService
    {
        public static Page CurrentPage
        {
            get;
            set;
        }

        public static Page SavedStatePage
        {
            get;
            set;
        }
    }
}
