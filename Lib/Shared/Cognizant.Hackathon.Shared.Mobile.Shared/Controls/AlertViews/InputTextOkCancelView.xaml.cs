using System;
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls.AlertViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputTextOkCancelView : ContentView
    {
        public static readonly BindableProperty IsValidationLabelVisibleProperty =
            BindableProperty.Create(
                nameof(IsValidationLabelVisible),
                typeof(bool),
                typeof(InputTextOkCancelView),
                false, BindingMode.OneWay, null,
                (bindable, value, newValue) =>
                {
                    if ((bool)newValue)
                    {
                        ((InputTextOkCancelView)bindable).ValidationLabel.IsVisible = true;
                    }
                    else
                    {
                        ((InputTextOkCancelView)bindable).ValidationLabel.IsVisible = false;
                    }
                });

        /// <summary>
        /// Gets or Sets if the ValidationLabel is visible
        /// </summary>
        public bool IsValidationLabelVisible
        {
            get
            {
                return (bool)GetValue(IsValidationLabelVisibleProperty);
            }
            set
            {
                SetValue(IsValidationLabelVisibleProperty, value);
            }
        }


        public static readonly BindableProperty TextInputResultProperty =
            BindableProperty.Create(
                nameof(TextInputResult),
                typeof(string),
                typeof(InputTextOkCancelView),
                string.Empty, BindingMode.OneWay, null,
                (bindable, value, newValue) =>
                {
                    ((InputTextOkCancelView)bindable).InputTextEntry.Text = (string)newValue;
                });

        /// <summary>
        /// Gets or Sets the TextInputResult
        /// </summary>
        public string TextInputResult
        {
            get
            {
                return (string)GetValue(TextInputResultProperty);
            }
            set
            {
                SetValue(TextInputResultProperty, value);
            }
        }


        public EventHandler OkButtonEventHandler { get; set; }

        public EventHandler CancelButtonEventHandler { get; set; }

        public InputTextOkCancelView(
            string titleText,
            string messageText,
            string placeHolderText,
            string okButtonText,
            string cancelButtonText,
            string validationLabelText)
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
            InputTextEntry.Placeholder = placeHolderText;
            OkButton.Text = okButtonText;
            CancelButton.Text = cancelButtonText;
            ValidationLabel.Text = validationLabelText;

            OkButton.Clicked += OkButton_Clicked;
            CancelButton.Clicked += CancelButton_Clicked;
            InputTextEntry.TextChanged += InputTextEntry_TextChanged;

            if (Device.RuntimePlatform == Device.Android)
            {
                FrameView.CornerRadius = 2;
                //TitleLabel.Text = TitleLabel.Text.ToUpper();
            }
            else
            {
                FrameView.CornerRadius = 8;
                //TitleLabel.HorizontalTextAlignment = TextAlignment.Center;
            }
        }

        private void InputTextEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextInputResult = ((Entry)sender).Text;

            if (!string.IsNullOrEmpty(TextInputResult))
                IsValidationLabelVisible = false;
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
