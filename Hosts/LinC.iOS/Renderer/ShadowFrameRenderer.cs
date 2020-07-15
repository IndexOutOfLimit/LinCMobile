using System;
using System.ComponentModel;
using CoreGraphics;
using LinC.Controls;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(ShadowFrameRenderer))]
namespace LinC.iOS.Renderer
{
    public class ShadowFrameRenderer : FrameRenderer
    {
        private bool IsShadowNeeded { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                //UpdateLayer();

                //UpdateCornerRadius();

            }
        }

        public ShadowFrameRenderer()
        {

        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if ((Element as ShadowFrame).IsDashboard == true)
            {
                UpdateDashboardLayer();
            }
            else
                 UpdateLayer();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var DashboardFrame = (ShadowFrame)sender;
            if(DashboardFrame.IsDashboard==true)
            {
                UpdateDashboardLayer();
            }
            if (e.PropertyName == nameof(ShadowFrame.IsSelected))
            {
                UpdateLayer((Element as ShadowFrame).IsSelected);
            }
            if(e.PropertyName=="IsDashboard")
            {
                //do something
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdateLayer(bool hasShadow = false)
        {
            
            //Layer.CornerCurve = CoreAnimation.CACornerCurve.Circular;
            Layer.MasksToBounds = false;
            Layer.BorderColor = UIColor.White.CGColor;
            Layer.CornerRadius = 32;// new CornerRadius(5,5,5,15);
            Layer.ShadowOffset = hasShadow ? new CGSize(3, 5) : new CGSize(1, 1);
            Layer.ShadowRadius = 10;
            Layer.ShadowOpacity = hasShadow ? 0.8f : 0.2f;
            //this.ClipsToBounds = true;
        }

        private void UpdateDashboardLayer()
        {
            //Layer.CornerCurve = CoreAnimation.CACornerCurve.Circular;
            Layer.MasksToBounds = false;
            Layer.BorderColor = UIColor.White.CGColor;
            Layer.CornerRadius = 18;
            Layer.ShadowOffset = new CGSize(-2, 2);
            Layer.ShadowRadius = 5;
            Layer.ShadowOpacity =  0.2f;
        }
    }
}
