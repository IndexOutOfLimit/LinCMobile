using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls
{
    public class TouchAnimatedViewCell : ViewCell
    {
        public static readonly BindableProperty SelectedItemBackgroundColorProperty = BindableProperty.Create("SelectedItemBackgroundColor", typeof(Color), typeof(TouchAnimatedViewCell), Color.Default);
        public Color SelectedItemBackgroundColor
        {
            get
            {
                return (Color)GetValue(SelectedItemBackgroundColorProperty);
            }
            set
            {
                SetValue(SelectedItemBackgroundColorProperty, value);
            }
        }
        protected override async void OnTapped()
        {
            base.OnTapped();

            if (this.View != null)
            {
                //await (View.FadeTo(0.4, 50).ContinueWith((result) =>
                //{
                //    View.FadeTo(1, 500);
                //}));
            }
        }

    }
}
