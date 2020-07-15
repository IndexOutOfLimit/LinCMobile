using System;
using System.ComponentModel;
using Cognizant.Hackathon.Shared.Mobile.Shared.Controls;
using LinC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Material.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace LinC.iOS.Renderer
{
    public class CustomEditorRenderer : MaterialEditorRenderer
    {
        public CustomEditorRenderer()
        {
           
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.ActiveTextInputController.UnderlineHeightActive = 0;
                Control.ActiveTextInputController.UnderlineHeightNormal = 0;
                Control.ActiveTextInputController.BorderFillColor = UIColor.White;
                Control.ActiveTextInputController.UnderlineViewMode = UITextFieldViewMode.Never;
                Control.TextColor = UIColor.Black;
              
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
            {
                return;
            }
            else
            {
                Control.ActiveTextInputController.UnderlineHeightActive = 0;
                Control.ActiveTextInputController.UnderlineHeightNormal = 0;
                Control.ActiveTextInputController.BorderFillColor = UIColor.White;
                Control.ActiveTextInputController.UnderlineViewMode = UITextFieldViewMode.Never;
                Control.TextColor = UIColor.Black;
            }

           
        }

    }
}
