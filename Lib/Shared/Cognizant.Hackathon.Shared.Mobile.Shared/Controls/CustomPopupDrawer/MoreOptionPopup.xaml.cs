using System;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls.CustomPopupDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoreOptionPopup : ContentView
    {
        public EventHandler OkButtonEventHandler { get; set; }

        public EventHandler CancelButtonEventHandler { get; set; }

        public Task<string> PageClosingTask => PageClosingTaskCompletionSource.Task;

        public TaskCompletionSource<string> PageClosingTaskCompletionSource { get; set; }
        public string PreviousRoute { get; set; }

        public MoreOptionPopup()
        {           
            try
            {
                InitializeComponent();                
            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains(ExceptionLiteral.StaticResNotFound))
                    throw;
            }            
        }

        private async void TapClose(object sender, EventArgs e)
        {
            Shell.SetTabBarIsVisible(this, true);
            //await System.Threading.Tasks.Task.Delay(200);
            CancelButtonEventHandler?.Invoke(this, e);

            //string route = PreviousRoute ?? "//roottab/dashboard/dashboard"; //
            //await Shell.Current.GoToAsync(route, true);
        }

        public void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            var tabCell = (sender as StackLayout);
            var item = (TapGestureRecognizer)tabCell.GestureRecognizers[0];

            string arg = item.CommandParameter.ToString();

            CancelButtonEventHandler?.Invoke(this, args);
        }
    }
}
