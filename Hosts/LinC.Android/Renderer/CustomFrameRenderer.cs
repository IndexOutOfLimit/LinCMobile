using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FrameRenderer = Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer;
[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace LinC.Droid.Renderer
{
    public class CustomFrameRenderer : FrameRenderer
    {
        private Color StartColor { get; set; }
        private Color EndColor { get; set; }
        private bool IsSelected { get; set; }

        public CustomFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null) return;
            if (e.OldElement != null || Element == null) return;
            
            try
            {
                if (!(e.NewElement is CustomFrame stack)) return;

                if (e.NewElement != null && Control != null)
                {
                    IsSelected = (e.NewElement as CustomFrame).IsSelected;

                    UpdateElevation();

                    UpdateCornerRadius();
                }

                //StartColor = stack.StartColor;
                //EndColor = stack.EndColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CustomFrame.CornerRadius) ||
                e.PropertyName == nameof(CustomFrame))
            {
                UpdateCornerRadius();
                UpdateElevation();

            }
        }

        protected override void DispatchDraw(Android.Graphics.Canvas canvas)
        {

            var gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height,
                StartColor.ToAndroid(),
                EndColor.ToAndroid(),
                Android.Graphics.Shader.TileMode.Mirror);

            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        private void UpdateCornerRadius()
        {
            if (Control.Background is GradientDrawable backgroundGradient)
            {
                var cornerRadius = (Element as CustomFrame)?.CornerRadius;
                if (!cornerRadius.HasValue)
                {
                    return;
                }

                var topLeftCorner = Context.ToPixels(cornerRadius.Value.TopLeft);
                var topRightCorner = Context.ToPixels(cornerRadius.Value.TopRight);
                var bottomLeftCorner = Context.ToPixels(cornerRadius.Value.BottomLeft);
                var bottomRightCorner = Context.ToPixels(cornerRadius.Value.BottomRight);

                var cornerRadii = new[]
                {
                    topLeftCorner,
                    topLeftCorner,

                    topRightCorner,
                    topRightCorner,

                    bottomRightCorner,
                    bottomRightCorner,

                    bottomLeftCorner,
                    bottomLeftCorner,
                };

                backgroundGradient.SetCornerRadii(cornerRadii);
            }
        }
        private void UpdateElevation()
        {
            var extendedFrame = (CustomFrame)Element;
            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            ViewCompat.SetElevation(this, extendedFrame.Elevation);
            ViewCompat.SetElevation(Control, extendedFrame.Elevation);

            //if (IsSelected)
            //{
            //    ViewCompat.SetElevation(this, 30);
            //    ViewCompat.SetElevation(Control, 30);
            //}
            //else
            //{
            //    ViewCompat.SetElevation(this, 10);
            //    ViewCompat.SetElevation(Control, 10);
            //}

        }


    }
}