using System;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using Xamarin.Forms;
using UIKit;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using LinC.iOS.Renderer;

[assembly: ExportRenderer(typeof(CustomShadowFrame), typeof(CustomShadowFrameRenderer))]
namespace LinC.iOS.Renderer
{
    public class CustomShadowFrameRenderer : FrameRenderer
    {
        public static void Initialize()
        {
            // empty, but used for beating the linker
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;
            UpdateShadow();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateShadow();
            }
        }

        private void UpdateShadow()
        {
            var extendedFrame = (CustomShadowFrame)Element;

            Layer.ShadowRadius = extendedFrame.Elevation;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(0, 1);
            Layer.ShadowOpacity = 0.20f;
           
            Layer.MasksToBounds = false;
        }
    }
}