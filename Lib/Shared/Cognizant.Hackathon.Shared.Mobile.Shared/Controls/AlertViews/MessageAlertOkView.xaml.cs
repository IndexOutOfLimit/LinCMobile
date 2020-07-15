using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cognizant.Hackathon.Mobile.Core.Exceptions;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls.AlertViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageAlertOkView : ContentView
    {
        public EventHandler OkButtonEventHandler { get; set; }

        public MessageAlertOkView(
            string titleText,
            string messageText,
            string okButtonText)
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

            TitleLabel.Text = titleText;
            MessageLabel.Text = messageText;
            OkButton.Text = okButtonText;

            OkButton.Clicked += OkButton_Clicked;
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            OkButtonEventHandler?.Invoke(this, e);
        }
    }
}
