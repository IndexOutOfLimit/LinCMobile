using System;
using Xamarin.Forms;

namespace LinC.Triggers
{
    public class IsVisibleAnimationTriggerAction : TriggerAction<VisualElement>
    {
        public bool IsMakeVisible { set; get; }
        public double TranslateX { set; get; }
        public double TranslateY { set; get; }

        protected override void Invoke(VisualElement sender)
        {
            if (IsMakeVisible)
            {
                sender.FadeTo(0.98, 150, Easing.SinIn);
            }
            else
            {
                sender.FadeTo(0, 150, Easing.SinIn);
            }

            sender.TranslateTo(TranslateX, TranslateY, Convert.ToUInt32(150), Easing.SinIn);
        }
    }
}
