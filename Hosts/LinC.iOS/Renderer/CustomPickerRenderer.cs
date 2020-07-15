using System;
using System.ComponentModel;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Foundation;
using LinC.Controls;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Material.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace LinC.iOS.Renderer
{
    public class CustomPickerRenderer : MaterialPickerRenderer, IUIPickerViewDelegate
    {
        CustomPicker element;
        private bool doneActivated;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
                return;

            SetControl();

            this.element = (CustomPicker)this.Element;
            if (this.Control != null && this.Element != null)
            {

                Control.RightViewMode = UITextFieldViewMode.Always;
                UIImage uIImage = UIImage.FromBundle("DropdownArrow"); //new UIImage("DropdownArrow");
                Control.RightView = new UIImageView(uIImage);
                UIPickerView picker = (UIPickerView)this.Control.InputView;
                picker.WeakDelegate = this;
                picker.BackgroundColor = Color.FromHex("#CACACA").ToUIColor();
                Control.TextColor = Color.FromHex("#333333").ToUIColor();
                Control.InputAssistantItem.LeadingBarButtonGroups = null;
                Control.InputAssistantItem.TrailingBarButtonGroups = null;
            }

            if (e.OldElement != null && this.element != null)
            {
                this.element.Unfocused -= this.Handler;
            }

            if (e.NewElement != null && this.element != null)
            {
                SetCustomToolbar(this.element.FinishTitle, this.element.CancelTitle, this.element.CenterTitle);
                this.element.Unfocused += this.Handler;
            }


        }

        private void Handler(object sender, FocusEventArgs e)
        {
            try
            {
                if (!doneActivated)
                {
                    if (this.element.SelectedIndex >= 0 && this.element.SelectedIndex < this.element.Items.Count)
                    {
                        ((UIPickerView)Control.InputView).Select(this.element.SelectedIndex, 0, false);
                    }
                }

                doneActivated = false;
            }
            catch (Exception)
            {
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName && Element.IsEnabled)
            {
                Control.AttributedPlaceholder = new NSAttributedString(Control.AttributedPlaceholder.Value, foregroundColor: Color.FromHex("#5B7183").ToUIColor());
            }

            SetControl();

            base.OnElementPropertyChanged(sender, e);
        }

        private void SetControl()
        {
            if (Control == null)
            {
                return;
            }

            Control.BackgroundColor = UIColor.White;

            Control.TintColor = UIColor.Black;
            Control.ClipsToBounds = true;
            Control.TextInput.CursorColor = UIColor.Black;
            Control.TextInput.TextColor = UIColor.Black;
        }

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
                if (this.element != null)
                {
                    doneActivated = true;
                    this.element.SelectedIndex = (int)(Control.InputView as UIPickerView).SelectedRowInComponent(0);
                    if (this.element.SelectedIndex >= 0)
                    {
                        this.element.SelectedItem = this.element.ItemsSource[this.element.SelectedIndex];
                        Control.ResignFirstResponder();
                        Control.TextColor = Color.FromHex("#2E2E2E").ToUIColor();
                        //this.element.Command.Execute(this.element.SelectedItem as UiControl);
                    }
                    else
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

        [Export("pickerView:viewForRow:forComponent:reusingView:")]
        public UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            view = new UIView();
            var product = Element.ItemsSource[(int)row] as string;
            //string description = product.Name;

            UILabel label = new UILabel(new CoreGraphics.CGRect(50, 0, pickerView.Bounds.Width - 100, 45))
            {
                TextColor = Color.FromHex("#252D3C").ToUIColor(),
                Text = Element.ItemsSource[(int)row] as string,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.BoldSystemFontOfSize(20)
            };
            view.AddSubview(label);
            return view;
        }
    }
}
