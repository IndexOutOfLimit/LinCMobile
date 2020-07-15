using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using LinC.Droid.Helper;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessMaterialEntry), typeof(BorderlessMaterialEntryRenderer))]
namespace LinC.Droid.Renderer
{
    public class BorderlessMaterialEntryRenderer : MaterialEntryRenderer
    {
        private BorderlessMaterialEntry _materialEntry;

        public BorderlessMaterialEntryRenderer(Context context) : base(context)
        {
            MaterialHelper.AndroidContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e?.NewElement != null)
            {
                //ChangeCursorColor();
                _materialEntry = Element as BorderlessMaterialEntry;
                SetControl();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e?.PropertyName == nameof(BorderlessMaterialEntry.TintColor))
            {
                //ChangeCursorColor();
            }
        }

        private void SetControl()
        {
            if (Control == null)
            {
                return;
            }
            
            Control.EditText.SetTextColor(global::Android.Graphics.Color.Black);
            Control.EditText.PaintFlags = Android.Graphics.PaintFlags.LinearText;

            Control.EditText.Background = null;
            Control.EditText.SetBackgroundColor(Android.Graphics.Color.Transparent);

            //Control.EditText.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);
            //if ((Element as BorderlessMaterialEntry).IsNumericKeyboard)
            //{
            //    Control.EditText.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagSigned | Android.Text.InputTypes.NumberFlagDecimal;
            //}            

            //// DEV HINT: This will be used for the future control `MaterialTextArea`.
            //// This removes the 'Next' button and shows a 'Done' button when the device's orientation is in landscape.
            //// This prevents the crash that is caused by a `java.lang.IllegalStateException'.
            //// Reported here https://github.com/xamarin/Xamarin.Forms/issues/4832.
            //this.Control.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            var currentFocus = (Context as Activity).CurrentFocus;

            if (currentFocus != null)
            {
                var inputMethodManager = (InputMethodManager)(Context as Activity).GetSystemService(Context.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }

            _materialEntry.ReturnCommand?.Execute(_materialEntry.ReturnCommandParameter);

            return false;
        }
    }
}
