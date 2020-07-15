
using Android.Content;
using LinC.Controls;
using LinC.Droid.Helper;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace LinC.Droid.Renderer
{
    public class CustomDatePickerRenderer : MaterialDatePickerRenderer
    {
        private CustomDatePicker datePicker;

        public CustomDatePickerRenderer(Context context) : base(context)
        {
            MaterialHelper.AndroidContext = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            datePicker = Element as CustomDatePicker;

            datePicker.DateSelected += (sender, arg) => {
                datePicker.DateSelectedCommand.Execute(datePicker.Date.ToShortDateString());
            };
        }
    }
}
