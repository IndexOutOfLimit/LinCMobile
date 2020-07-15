using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressSpinnerPage : PopupPage
    {
        private readonly bool _isCancellable;

        private const string SpinnerAnimation = "SpinnerPage";
        private readonly Animation _animation;

        public ProgressSpinnerPage(bool isCancellable = true)
        {
            InitializeComponent();

            _isCancellable = isCancellable;
            BackgroundColor = Color.FromHex("#50152744");

            _animation = new Animation(v => spinnerImage.Rotation = v, 0, 360);
        }

        protected override void OnAppearing()
        {
            RunAnimation();
            base.OnAppearing();
        }

        private void RunAnimation()
        {
            _animation.Commit(this, SpinnerAnimation, 16, 800, Easing.Linear, null, () => true);
        }

        protected override void OnDisappearing()
        {
            this.AbortAnimation(SpinnerAnimation);
            base.OnDisappearing();
        }

        // Invoked when back button on Android is clicked
        protected override bool OnBackButtonPressed()
        {
            // Return default value - OnBackButtonPressed
            if (_isCancellable)
                return false;

            return true;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            if (_isCancellable)
                return true;

            return false;
        }

    }
}