using System.Collections.Generic;
using LinC.Controls;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Material.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace LinC.iOS.Renderer
{
    public class CustomDatePickerRenderer : MaterialDatePickerRenderer, IMaterialEntryRenderer
    {
        CustomDatePicker datePicker;

        public CustomDatePickerRenderer()
        {
            datePicker = (CustomDatePicker)this.Element;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement != null)
            {
                datePicker = (CustomDatePicker)this.Element;

                //var toolBar = (UIToolbar)Control.InputAccessoryView;
                //var doneButton = toolBar.Items[1];
                //var cancelButton = toolBar.Items[0];
                //doneButton.Clicked += SaveButton_TouchUpInside;
                //cancelButton.Clicked += CancelButton_TouchUpInside;

                //var originalToolbar = this.Control.InputAccessoryView as UIToolbar;

                //var clearButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                //{
                //    Control.ResignFirstResponder();
                //}));

                //var titleButton = new UIBarButtonItem("Title", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                //{

                //}));


                //var newItems = new List<UIBarButtonItem>();

                //foreach (var item in originalToolbar.Items)
                //{
                //    newItems.Add(item);
                //}

                //newItems.Insert(0, clearButton);
                //newItems.Insert(0, titleButton);
                //originalToolbar.Items = newItems.ToArray();
                //originalToolbar.SetNeedsDisplay();
                SetCustomToolbar(this.datePicker.FinishTitle, this.datePicker.CancelTitle, this.datePicker.CenterTitle);
            }
        }

        //private void SaveButton_TouchUpInside(object sender, System.EventArgs e)
        //{
        //    Control.Text = this.Element.Date.ToString("MM/dd/yyyy");
        //    datePicker.DateSelectedCommand.Execute(Control.Text);
        //    Control.ResignFirstResponder();
        //}

        //private void CancelButton_TouchUpInside(object sender, System.EventArgs e)
        //{
        //    Control.ResignFirstResponder();
        //}

        private void SetCustomToolbar(string doneButtonText, string cancelButtonText, string titleText)
        {
            UIToolbar toolbar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true,
                BackgroundColor = Color.FromHex("#252D3C").ToUIColor(),
                BarTintColor = Color.FromHex("#252D3C").ToUIColor()
            };
            toolbar.SizeToFit();

            UIBarButtonItem cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                Control.ResignFirstResponder();
            });
            cancelButton.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = UIColor.White,
                    Font = UIFont.BoldSystemFontOfSize(16)
                }, UIControlState.Normal);

            cancelButton.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = UIColor.White

                }, UIControlState.Selected);

            UIBarButtonItem flexible = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

            UILabel titleView = new UILabel(new CoreGraphics.CGRect(0, 0, 200, 50))
            {
                Text = titleText,
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White

            };

            UIBarButtonItem title = new UIBarButtonItem(titleView);

            titleView.Font = UIFont.BoldSystemFontOfSize(16);

            title.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = UIColor.White
                }, UIControlState.Normal);

            title.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = UIColor.White,
                    Font = UIFont.BoldSystemFontOfSize(16)

                }, UIControlState.Selected);

            UIBarButtonItem doneButton = new UIBarButtonItem(doneButtonText, UIBarButtonItemStyle.Done, (s, ev) =>
            {
                if (this.datePicker != null)
                {
                    Control.TextColor = Color.FromHex("#2E2E2E").ToUIColor();
                    Control.Text = this.Element.Date.ToString("MM/dd/yyyy");
                    datePicker.DateSelectedCommand.Execute(Control.Text);
                    Control.ResignFirstResponder();
                }
            });

            doneButton.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = UIColor.White,
                    Font = UIFont.BoldSystemFontOfSize(16)
                }, UIControlState.Normal);

            doneButton.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = UIColor.White,
                    Font = UIFont.BoldSystemFontOfSize(16)

                }, UIControlState.Selected);

            toolbar.SetItems(new UIBarButtonItem[] { cancelButton, flexible, title, flexible, doneButton }, true);
            Control.InputAccessoryView = toolbar;

        }
    }
}
