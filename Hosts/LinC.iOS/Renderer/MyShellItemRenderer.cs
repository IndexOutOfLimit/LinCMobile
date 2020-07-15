/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LinC.iOS.Renderer
{
    /// <summary>
    /// This renderer is necessary to allow us to handle the TabReselected (current tab clicked) event as it is not implemented by default on Android
    /// and the only way is to go through a renderer. 
    /// </summary>
    public class MyShellItemRenderer : ShellItemRenderer
    {
        public MyShellItemRenderer(IShellContext shellContext) : base(shellContext)
        {
        }

        /// <summary>
        /// Pops to root when the selected tab is pressed.
        /// </summary>
        /// <param name="shellSection"></param>
        protected override void OnTabReselected(ShellSection shellSection)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await shellSection?.Navigation.PopToRootAsync();
            });
        }
    }
}
*/