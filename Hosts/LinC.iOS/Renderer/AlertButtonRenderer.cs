using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using CoreAnimation;
using LinC.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AlertButton), typeof(AlertButtonRenderer))]
namespace LinC.iOS.Renderer
{
    public class AlertButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;

            Control.Layer.MaskedCorners = (CACornerMask)12;
            Control.Layer.CornerRadius = 5;

            //0: no rounded corners
            //1: top left
            //2: top right
            //3: top left & right(both top corners)
            //4: bottom left
            //5: top & bottom left(both left corners)
            //6: top right & bottom left
            //7: top left & right, bottom left(all corners except bottom right)
            //8: bottom right
            //9: top left, bottom right
            //10: top & bottom right(both right corners)
            //11: both top corners, bottom right(all corners except bottom left)
            //12: bottom left & right(both bottom corners)
            //13: bottom left & right, top left(all corners except top right)
            //14: bottom left & right, top right(all corners except top left)
            //15: all corners rounded
        }
    }
}
