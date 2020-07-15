using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Material.iOS;
using Xamarin.Forms.Platform.iOS;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(BorderlessMaterialEntry), typeof(BorderlessMaterialEntryRenderer))]
namespace LinC.iOS.Renderer
{
    public class BorderlessMaterialEntryRenderer : MaterialEntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement != null)
            {
                SetControl();
            }

            try
            {
                SetCustomToolbar("Done", "Cancel", string.Empty);
            }
            catch (System.Exception)
            {

            }

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
            {
                return;
            }

            SetControl();

            if (e?.PropertyName == nameof(BorderlessMaterialEntry.TintColor))
            {
                Control.TintColor = (Element as BorderlessMaterialEntry)?.TintColor.ToUIColor();
            }

            if (e?.PropertyName == nameof(BorderlessMaterialEntry.IsNumericKeyboard))
            {
                var isNegativeAllowed = (Element as BorderlessMaterialEntry).IsNegativeAllowed;
                AddRemoveReturnKeyToNumericInput((Element as BorderlessMaterialEntry).IsNumericKeyboard, isNegativeAllowed);
            }

            if (e?.PropertyName == nameof(BorderlessMaterialEntry.IsNegativeAllowed))
            {
                var isNegativeAllowed = (Element as BorderlessMaterialEntry).IsNegativeAllowed;
                AddRemoveReturnKeyToNumericInput((Element as BorderlessMaterialEntry).IsNumericKeyboard, isNegativeAllowed);
            }

        }

        private void SetControl()
        {
            if (Control == null)
            {
                return;
            }

            Control.ActiveTextInputController.UnderlineHeightActive = 0;
            Control.ActiveTextInputController.UnderlineHeightNormal = 0;
            Control.ActiveTextInputController.BorderFillColor = UIColor.White;
            Control.ActiveTextInputController.UnderlineViewMode = UITextFieldViewMode.Never;
            Control.ActiveTextInputController.TextInput.TextColor = UIColor.Black;

            Control.TintColor = (Element as BorderlessMaterialEntry)?.TintColor.ToUIColor();
           
            var isNegativeAllowed = (Element as BorderlessMaterialEntry).IsNegativeAllowed;
            AddRemoveReturnKeyToNumericInput((Element as BorderlessMaterialEntry).IsNumericKeyboard, isNegativeAllowed);
           
        }

        private void SetCustomToolbar(string doneButtonText, string cancelButtonText, string titleText)
        {
            if (Control == null)
            {
                return;
            }

            UIToolbar toolbar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true,
                BackgroundColor = Color.FromHex("#252D3C").ToUIColor(),
                BarTintColor = Color.FromHex("#252D3C").ToUIColor()
            };
            toolbar.SizeToFit();

            UIBarButtonItem cancelButton = new UIBarButtonItem(cancelButtonText, UIBarButtonItemStyle.Plain, (sender, e) =>
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
                Control.ResignFirstResponder();
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

        private void AddRemoveReturnKeyToNumericInput(bool isNumeric, bool isNegativeAllowed)
        {
            if (isNumeric)
            {
                if(isNegativeAllowed)
                {
                    Control.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
                }
                else
                {
                    Control.KeyboardType = UIKeyboardType.DecimalPad;
                }
            }
            else
            {
                //Control.KeyboardType = UIKeyboardType.Default;
                Control.InputAccessoryView = null;
            }
        }

      

        //private UIView GetAccessoryButtons()
        //{
        //    var view = new UIView(frame: new CGRect(x: 0, y: 0, width: Control.Superview.Frame.Size.Width, height: 44));
        //    view.BackgroundColor = UIColor.LightGray;

        //    var minusButton = new UIButton(type: UIButtonType.Custom);
        //    var doneButton = new UIButton(type: UIButtonType.Custom);
        //    minusButton.SetTitle("-", forState: UIControlState.Normal);
        //    doneButton.SetTitle("Done", forState: UIControlState.Normal);
        //    var buttonWidth = view.Frame.Size.Width / 3;
        //    minusButton.Frame = new CGRect(x: 0, y: 0, width: buttonWidth, height: 44);
        //    doneButton.Frame = new CGRect(x: view.Frame.Size.Width - buttonWidth, y: 0, width: buttonWidth, height: 44);

        //    minusButton.TouchUpInside += MinusButton_TouchUpInside;
        //    doneButton.TouchUpInside += DoneButton_TouchUpInside;

        //    view.AddSubview(minusButton);
        //    view.AddSubview(doneButton);

        //    return view;
        //}

        //private void DoneButton_TouchUpInside(object sender, System.EventArgs e)
        //{
        //    Control.ResignFirstResponder();
        //    Element.SendCompleted();
        //}

        //private void MinusButton_TouchUpInside(object sender, System.EventArgs e)
        //{
        //    var text = Control.Text;
        //    if (text.Count() > 0)
        //    {
        //        var index = text.IndexOf("-", System.StringComparison.InvariantCulture);
        //        if (index == -1)
        //        {
        //            Control.Text = "-" + text;
        //            return;
        //        }

        //        var minusArr = text.Split("-");

        //        if (text.StartsWith("-", System.StringComparison.InvariantCulture) && minusArr.Length == 1)
        //        {
        //            return;
        //        }

        //        if (minusArr.Length > 1)
        //        {
        //            var replaceWithText = text.Replace("-", string.Empty);
        //            Control.Text = "-" + replaceWithText;
        //        }
        //    }
        //}
    }
}
