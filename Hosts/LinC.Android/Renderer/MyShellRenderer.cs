using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(Shell), typeof(MyShellRenderer))]
namespace LinC.Droid.Renderer
{
   public class MyShellRenderer : ShellRenderer
    {
        private Context androidContext;

        public MyShellRenderer(Context context) : base(context)
        {
            androidContext = context;
        }

        //protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        //{
        //    return new CustomBottomNavAppearance(androidContext);
        //}

        //protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        //{
        //    return new MyShellItemRenderer(this);
        //}
    }

    public class CustomBottomNavAppearance : IShellBottomNavViewAppearanceTracker
    {
        private Context androidContext;
        public CustomBottomNavAppearance(Context context)
        {
            androidContext = context;
        }

        public void Dispose()
        {

        }

        public void ResetAppearance(BottomNavigationView bottomView)
        {
            IMenu myMenu = bottomView.Menu;
            bottomView.ItemIconTintList = null;

            for (int i = 0; i < bottomView.Menu.Size(); i++)
            {
                IMenuItem myItemOne = myMenu.GetItem(i);

                if(!string.IsNullOrWhiteSpace(myItemOne.ToString()))
                    SetTabItemTitleColorAndIcon(myItemOne);
               
            }
        }

        public void SetAppearance(BottomNavigationView bottomView, ShellAppearance appearance)
        {
            IMenu myMenu = bottomView.Menu;
            bottomView.ItemIconTintList = null;
            
            for (int i = 0; i < bottomView.Menu.Size(); i++)
            {
                IMenuItem myItemOne = myMenu.GetItem(i);
                if (!string.IsNullOrWhiteSpace(myItemOne.ToString()))
                    SetTabItemTitleColorAndIcon(myItemOne);
                
            }
            
        }

        public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            //throw new System.NotImplementedException();
        }

        private void SetTabItemTitleColorAndIcon(IMenuItem myItemOne)
        {
            
        }
    }
}