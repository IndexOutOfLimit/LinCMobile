﻿/*using LinC;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Shell), typeof(MyShellRenderer))]
namespace LinC.iOS.Renderer
{
    public class MyShellRenderer : ShellRenderer
    {
        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            var renderer = base.CreateShellSectionRenderer(shellSection);
            if (renderer != null)
            {

            }
            return renderer;
        }

        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new CustomTabbarAppearance();
        }
    }

    public class CustomTabbarAppearance : IShellTabBarAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(UITabBarController controller)
        {

        }

        public void SetAppearance(UITabBarController controller, ShellAppearance appearance)
        {
            UITabBar myTabBar = controller.TabBar;

            if (myTabBar.Items != null)
            {
                //UITabBarItem itemOne = myTabBar.Items[0];

                //itemOne.Image = UIImage.FromBundle("tab_about.png");
                //itemOne.SelectedImage = UIImage.FromBundle("tab_feed.png");


                //UITabBarItem itemTwo = myTabBar.Items[1];

                //itemTwo.Image = UIImage.FromBundle("tab_feed.png");
                //itemTwo.SelectedImage = UIImage.FromBundle("tab_about.png");

                //The same logic if you have itemThree, itemFour....
            }

        }

        public void UpdateLayout(UITabBarController controller)
        {

        }
    }
}

*/
