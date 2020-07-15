using System;
using System.Collections.Generic;
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls.AlertViews
{
    public partial class MessageAlertOkCancelView : ContentView
    {
        public EventHandler OkButtonEventHandler { get; set; }

        public EventHandler CancelButtonEventHandler { get; set; }

        public MessageAlertOkCancelView(
            string titleText,
            string messageText,
            string okButtonText,
            string cancelButtonText,
            bool isFromDeleteQuote)
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

            if (isFromDeleteQuote)
                OkButton.TextColor = Color.FromHex("#D30808");

            if (string.IsNullOrEmpty(cancelButtonText))
            {
                CancelButton.IsVisible = false;
            }
            else
            {
                CancelButton.Text = cancelButtonText;
            }

            OkButton.Clicked += OkButton_Clicked;
            CancelButton.Clicked += CancelButton_Clicked;

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    TitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            //    MessageLabel.HorizontalTextAlignment = TextAlignment.Center;
            //}
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            OkButtonEventHandler?.Invoke(this, e);
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            CancelButtonEventHandler?.Invoke(this, e);
        }
    }
}
