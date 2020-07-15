using System;
using System.ComponentModel;
using CoreAnimation;
using CoreGraphics;
using LinC.Controls;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CompanyAreaFrame), typeof(CompanyAreaFrameRenderer))]
namespace LinC.iOS.Renderer
{
    public class CompanyAreaFrameRenderer : FrameRenderer
    {
        private bool IsSelected { get; set; }


        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                IsSelected = (e.NewElement as CompanyAreaFrame).IsSelected;

                UpdateCornerRadius();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            UpdateCornerRadius();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CompanyAreaFrame.CornerRadius) || e.PropertyName == nameof(CompanyAreaFrame.IsSelected)
                || e.PropertyName == nameof(CompanyAreaFrame))
            {
                IsSelected = (sender as CompanyAreaFrame).IsSelected;
                UpdateCornerRadius();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private double RetrieveCommonCornerRadius(CornerRadius cornerRadius)
        {
            var commonCornerRadius = cornerRadius.TopLeft;
            if (commonCornerRadius <= 0)
            {
                commonCornerRadius = cornerRadius.TopRight;
                if (commonCornerRadius <= 0)
                {
                    commonCornerRadius = cornerRadius.BottomLeft;
                    if (commonCornerRadius <= 0)
                    {
                        commonCornerRadius = cornerRadius.BottomRight;
                    }
                }
            }

            return commonCornerRadius;
        }

        private UIRectCorner RetrieveRoundedCorners(CornerRadius cornerRadius)
        {
            var roundedCorners = default(UIRectCorner);

            if (cornerRadius.TopLeft > 0)
            {
                roundedCorners |= UIRectCorner.TopLeft;
            }

            if (cornerRadius.TopRight > 0)
            {
                roundedCorners |= UIRectCorner.TopRight;
            }

            if (cornerRadius.BottomLeft > 0)
            {
                roundedCorners |= UIRectCorner.BottomLeft;
            }

            if (cornerRadius.BottomRight > 0)
            {
                roundedCorners |= UIRectCorner.BottomRight;
            }

            return roundedCorners;
        }

        private void UpdateCornerRadius()
        {

            var cornerRadius = (Element as CompanyAreaFrame)?.CornerRadius;
            if (!cornerRadius.HasValue)
            {
                return;
            }

            UIBezierPath path;
            if (IsSelected)
            {
                path = UIBezierPath.FromRoundedRect(Bounds, UIRectCorner.TopRight | UIRectCorner.BottomLeft, new CGSize(cornerRadius.Value.TopRight, cornerRadius.Value.BottomLeft));
            }
            else
            {
                path = UIBezierPath.FromRoundedRect(Bounds, UIRectCorner.AllCorners, new CGSize(cornerRadius.Value.TopRight, cornerRadius.Value.BottomLeft));
            }

            CAShapeLayer frameLayer = new CAShapeLayer();
            frameLayer.Path = path.CGPath;
            NativeView.Layer.Mask = frameLayer;
            this.ClipsToBounds = true;
        }
    }
}
