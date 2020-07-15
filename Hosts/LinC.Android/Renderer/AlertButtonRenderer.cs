using Android.Content;
using Android.Graphics.Drawables;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ButtonRenderer = Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer;

[assembly: ExportRenderer(typeof(AlertButton), typeof(AlertButtonRenderer))]
namespace LinC.Droid.Renderer
{
    public class AlertButtonRenderer : ButtonRenderer
    {
        public AlertButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control is null) return;

            Control.SetBackgroundResource(Resource.Drawable.DefaultButtonShape);

            var bg = (GradientDrawable)Control.Background;
            bg.SetColor(e.NewElement.BackgroundColor.ToAndroid());
        }
    }
}
