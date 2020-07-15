using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.View;
using LinC.Controls;
using LinC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FrameRenderer = Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer;

[assembly: ExportRenderer(typeof(CompanyAreaFrame), typeof(CompanyAreaFrameRenderer))]
namespace LinC.Droid.Renderer
{
    public class CompanyAreaFrameRenderer : FrameRenderer
    {
        private bool IsSelected { get; set; }
        private bool IsShadowNeeded { get; set; }

        public CompanyAreaFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null) return;

            try
            {
                if (!(e.NewElement is CompanyAreaFrame stack)) return;

                IsSelected = (e.NewElement as CompanyAreaFrame).IsSelected;

                if (e.NewElement != null && Control != null)
                {
                    UpdateCornerRadius();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CompanyAreaFrame.CornerRadius) || e.PropertyName == nameof(CompanyAreaFrame.IsSelected)
                || e.PropertyName == nameof(CompanyAreaFrame))
            {
                IsSelected = (sender as CompanyAreaFrame).IsSelected;
                UpdateCornerRadius();

                if ((sender as CompanyAreaFrame).FindByName("UpperFrame") != null)
                {
                    IsShadowNeeded = (sender as CompanyAreaFrame).NeedShadow;
                    (sender as CompanyAreaFrame).HasShadow = IsShadowNeeded;
                    UpdateElevation();
                }
            }
        }

        private void UpdateCornerRadius()
        {
            if (Control.Background is GradientDrawable backgroundGradient)
            {
                var cornerRadius = (Element as CompanyAreaFrame)?.CornerRadius;
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
            // we need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            //set the elevation manually
            if (IsSelected)
            {
                ViewCompat.SetElevation(this, 30);
                ViewCompat.SetElevation(Control, 30);
            }
            else
            {
                ViewCompat.SetElevation(this, 10);
                ViewCompat.SetElevation(Control, 10);
            }
        }
    }
}
